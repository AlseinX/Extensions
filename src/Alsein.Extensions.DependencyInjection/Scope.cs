using System;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class Scope
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="action"></param>
        public static IModule AddModule(this IScope scope, Action<IModuleBuilder> action)
        {
            var builder = new Internal.ModuleBuilder();
            action(builder);
            var result = builder.Build();
            scope.AddModule(result);
            return result;
        }
    }
}