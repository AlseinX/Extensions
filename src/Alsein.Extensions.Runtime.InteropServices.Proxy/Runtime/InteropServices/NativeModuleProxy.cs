using Alsein.Extensions.Runtime.ProxyInvokers;
using System;
using System.Reflection;

namespace Alsein.Extensions.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    public static class NativeModuleProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="replacementOptions"></param>
        /// <returns></returns>
        public static T LoadModule<T>(NativeModuleAttribute replacementOptions = null)
        {
            var options = typeof(T).GetCustomAttribute<NativeModuleAttribute>();
            return typeof(WrapperProxyInvoker).GetImplementationOf(typeof(T)).New<T>(new Internal.NativeModuleDispatcher(NativeModuleFactory.LoadModule(replacementOptions.Path ?? options.Path ?? typeof(T).Name), replacementOptions));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T LoadModule<T>(string filename) => LoadModule<T>(new NativeModuleAttribute { Path = filename });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="replacementOptions"></param>
        /// <returns></returns>
        public static T LoadModule<T>(this INativeModuleFactory factory, NativeModuleAttribute replacementOptions = null)
        {
            var options = typeof(T).GetCustomAttribute<NativeModuleAttribute>();
            return typeof(WrapperProxyInvoker).GetImplementationOf(typeof(T)).New<T>(new Internal.NativeModuleDispatcher(factory.LoadModule(replacementOptions.Path ?? options.Path ?? typeof(T).Name), replacementOptions));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T LoadModule<T>(this INativeModuleFactory factory, string filename) => factory.LoadModule<T>(new NativeModuleAttribute { Path = filename });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static INativeModule Parse(object module)
        {
            switch (module)
            {
                case WrapperProxyInvoker wrapper
                    when wrapper.Target is INativeModule target:
                    return target;
                case INativeModule result:
                    return result;
                default:
                    throw new InvalidCastException("The object is not an INativeModule");
            }
        }
    }
}
