using Alsein.Utilities.Modulization;
using Autofac;
using System;
using System.Linq;
using System.Reflection;
using RegistrationBuilder =
    Autofac.Builder.IRegistrationBuilder<
        System.Object,
        Autofac.Features.Scanning.ScanningActivatorData,
        Autofac.Builder.DynamicRegistrationStyle
    >;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutofacExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ServiceSuffix = "Service";

        /// <summary>
        /// 
        /// </summary>
        public const string ControllerSuffix = "Controller";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="filter"></param>
        /// <param name="config"></param>
        /// <param name="entry"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAll(this ContainerBuilder builder,
            Func<Type, bool> filter = null,
            Action<RegistrationBuilder> config = null,
            Assembly entry = null,
            bool recursive = false)
        {
            entry = entry ?? Assembly.GetEntryAssembly();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(asm => asm.IsSharingRootNamespace(entry))
                .SelectMany(t => t.DefinedTypes).ToArray()
                .Where(filter ?? (t => true))
                .ToArray();
            RegistrationBuilder register() => builder.RegisterTypes(types)
                .AsSelf()
                .AsImplementedInterfaces()
                .PropertiesAutowired();
            config = config ?? (x => { });
            config(register().Where(IoCExtensions.IsSingletonService).SingleInstance());
            config(register().Where(IoCExtensions.IsScopedService).InstancePerLifetimeScope());
            config(register().Where(t => !t.IsSingletonService() && !t.IsScopedService()).InstancePerDependency());
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="suffix"></param>
        /// <param name="config"></param>
        /// <param name="entry"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAll(this ContainerBuilder builder, string suffix,
            Action<RegistrationBuilder> config = null,
            Assembly entry = null,
            bool recursive = false)
            => builder.RegisterAll(t => t.Name.EndsWith(suffix), config, entry, recursive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="config"></param>
        /// <param name="entry"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAllServices(this ContainerBuilder builder,
            Action<RegistrationBuilder> config = null,
            Assembly entry = null,
            bool recursive = false)
            => builder.RegisterAll(ServiceSuffix, config, entry, recursive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="config"></param>
        /// <param name="entry"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAllControllers(this ContainerBuilder builder,
            Action<RegistrationBuilder> config = null,
            Assembly entry = null,
            bool recursive = false)
            => builder.RegisterAll(ControllerSuffix, config, entry, recursive);
    }
}