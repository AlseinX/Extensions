using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyManagerBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IAssemblyManagerBuilder ConfigureDirectories(this IAssemblyManagerBuilder builder, Action<IList<AssemblyDirectory>> action)
        {
            builder.Services.Configure<AssemblyManagerOptions>(option => action(option.Directories));
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAssemblyManager Build(this IAssemblyManagerBuilder builder) => builder.Services.BuildServiceProvider().GetRequiredService<IAssemblyManager>();
    }
}