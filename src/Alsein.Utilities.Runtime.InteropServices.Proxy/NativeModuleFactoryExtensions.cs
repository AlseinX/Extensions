using System.Reflection;

namespace Alsein.Utilities.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    public static class NativeModuleFactoryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="replacementOptions"></param>
        /// <returns></returns>
        public static T LoadAssembly<T>(this INativeModuleFactory factory, NativeModuleAttribute replacementOptions = null)
        {
            var options = typeof(T).GetCustomAttribute<NativeModuleAttribute>();
            return typeof(ReflectionInvokerWrapper).GetImplementationOf(typeof(T)).New<T>(new Internal.NativeModuleDispatcher(factory.LoadAssembly(replacementOptions.Path ?? options.Path ?? typeof(T).Name), replacementOptions));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T LoadAssembly<T>(this INativeModuleFactory factory, string filename) => factory.LoadAssembly<T>(new NativeModuleAttribute { Path = filename });
    }
}
