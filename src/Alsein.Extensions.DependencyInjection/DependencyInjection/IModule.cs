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
        /// <param name="resolver"></param>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryResolve(IResolver resolver, object key, out object result);
    }
}