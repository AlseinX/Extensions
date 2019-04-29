using System.Reflection;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConstructorRegistryBuilder : IRegistryBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
        IConstructorRegistryBuilder UseConstructor(ConstructorInfo constructor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providedArgs"></param>
        /// <returns></returns>
        IConstructorRegistryBuilder UseProvidedArgs(params object[] providedArgs);
    }
}