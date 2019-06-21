using System;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFactoryRegistryBuilder : IRegistryBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        IFactoryRegistryBuilder UseFactory<TImplementation>(Func<IResolver, TImplementation> factory);
    }
}