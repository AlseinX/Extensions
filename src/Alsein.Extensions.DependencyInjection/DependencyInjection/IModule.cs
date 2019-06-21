using System;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryResolve(IResolvingContext context, object key, out object result);
    }
}