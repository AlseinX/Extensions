using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyManagerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IAssemblyManager AddServicesTo(this IAssemblyManager manager, IServiceCollection services)
        {
            void register(Type type, ServiceLifetime lifetime)
            {
                services.Add(new ServiceDescriptor(type, type, lifetime));
                var lastName = type.Name.SplitByCamel().Last();
                var interfaces = type.GetInterfaces().Where(i => i.Name.SplitByCamel().Last() == lastName);
                foreach (var i in interfaces)
                {
                    services.Add(new ServiceDescriptor(i, provider => provider.GetService(type), lifetime));
                }
            }
            manager.Features["Service"].ForAll(type =>
            {
                if (type.IsSingletonService())
                {
                    register(type, ServiceLifetime.Singleton);
                }
                else if (type.IsScopedService())
                {
                    register(type, ServiceLifetime.Scoped);
                }
                else
                {
                    register(type, ServiceLifetime.Transient);
                }
            });
            return manager;
        }
    }
}