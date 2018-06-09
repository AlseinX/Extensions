using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace Alsein.Utilities
{
    public static class AutofacExtensions
    {
        public const string ServiceSuffix = "Service";

        public const string ControllerSuffix = "Controller";

        public static ContainerBuilder RegisterAll(this ContainerBuilder builder, Func<Type, bool> filter = null, Assembly entry = null, bool recursive = false)
        {
            entry = entry ?? Assembly.GetEntryAssembly();
            var types = AssemblyLoader.LoadAssemblies(entry, recursive)
                .Where(AssemblyLoader.IsSharingRootName[entry])
                .SelectMany(t => t.DefinedTypes).ToArray()
                .Where(filter ?? (t => true))
                .ToArray();
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> register() => builder.RegisterTypes(types)
                .AsSelf()
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .PreserveExistingDefaults();
            register().Where(IoCExtensions.IsSingletonService).SingleInstance();
            register().Where(IoCExtensions.IsScopedService).InstancePerLifetimeScope();
            register().Where(t => !t.IsSingletonService() && !t.IsScopedService()).InstancePerDependency();
            return builder;
        }

        public static ContainerBuilder RegisterAll(this ContainerBuilder builder, string suffix, Assembly entry = null, bool recursive = false) =>
            builder.RegisterAll(t => t.Name.EndsWith(suffix), entry, recursive);

        public static ContainerBuilder RegisterAllServices(this ContainerBuilder builder, Assembly entry = null, bool recursive = false) => builder.RegisterAll(ServiceSuffix, entry, recursive);

        public static ContainerBuilder RegisterAllControllers(this ContainerBuilder builder, Assembly entry = null, bool recursive = false) => builder.RegisterAll(ControllerSuffix, entry, recursive);
    }
}