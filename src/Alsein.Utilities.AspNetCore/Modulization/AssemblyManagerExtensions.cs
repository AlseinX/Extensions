using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.SignalR;
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
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAssemblyManagerBuilder WithHubs(this IAssemblyManagerBuilder builder) =>
            builder.WithFeatureFilters(filters => filters.Add("Hub", type =>
                type.IsInstantiableClass() &&
                typeof(Hub).IsAssignableFrom(type) &&
                type.IsDefined(typeof(RouteAttribute), false)
            ));

#if NETSTANDARD2_0

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IAssemblyManager AddAssemblyManager(this IServiceCollection services, Action<IAssemblyManagerBuilder> configure = null)
        {
            var builder = AssemblyManagerBuilder.CreateDefault().WithHubs();
            configure?.Invoke(builder);
            var manager = builder.Build();
            services.AddSingleton(manager);
            return manager;
        }

#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <param name="assemblyManager"></param>
        /// <returns></returns>
        public static IMvcBuilder AddAssemblyManager(this IMvcBuilder mvcBuilder, IAssemblyManager assemblyManager)
        {
            var existings = mvcBuilder.PartManager.ApplicationParts.OfType<AssemblyPart>().Select(p => p.Assembly).ToArray();
            var assemblies = assemblyManager.ProjectAssemblies;
            assemblies.Where(a => !existings.Contains(a)).Select(a => new AssemblyPart(a)).ForAll(mvcBuilder.PartManager.ApplicationParts.Add);
            return mvcBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSignalRWithHubs(this IApplicationBuilder app, Action<HubRouteBuilder> configure = null)
        {
            var hubs = app.ApplicationServices.GetRequiredService<IAssemblyManager>().Features["Hub"].ToArray();
            var mapHub = typeof(HubRouteBuilder).GetMethod("MapHub", new[] { typeof(PathString), typeof(Action<HttpConnectionDispatcherOptions>) });
            return app.UseSignalR(builder =>
            {
                foreach (var hub in hubs)
                {
                    var method = mapHub.MakeGenericMethod(hub);
                    var option = hub.GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .Where(m => m.GetParameters().SingleOrDefault()?.ParameterType == typeof(HttpConnectionDispatcherOptions))
                        .Where(m => m.ReturnType == typeof(void))
                        .Select(m => m.CreateDelegate(typeof(Action<HttpConnectionDispatcherOptions>)))
                        .OfType<Action<HttpConnectionDispatcherOptions>>().ToArray();
                    foreach (var route in hub.GetCustomAttributes(false).OfType<RouteAttribute>())
                    {
                        method.Invoke(builder, new object[]
                        {
                            new PathString(route.Template),
                            new Action<HttpConnectionDispatcherOptions>(x => option.ForAll(o => o(x)))
                        });
                    }
                }
                configure?.Invoke(builder);
            });
        }
    }
}