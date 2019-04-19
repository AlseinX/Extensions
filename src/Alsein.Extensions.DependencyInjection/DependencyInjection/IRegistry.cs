using System;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRegistry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        ILifetime Lifetime { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        object CreateInstance(IResolver resolver);
    }
}