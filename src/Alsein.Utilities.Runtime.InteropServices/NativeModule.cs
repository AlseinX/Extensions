using Alsein.Utilities.Runtime.ReflectionInvokers;
using System;
using System.Reflection;

namespace Alsein.Utilities.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    public static class NativeModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static T GetFunction<T>(this INativeModule assembly, string functionName) where T : Delegate => (T)assembly.GetFunction(functionName, typeof(T));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static INativeModule LoadModule(string filename) => NativeModuleFactory.LoadAssembly(filename);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="replacementOptions"></param>
        /// <returns></returns>
        public static T LoadModule<T>(NativeModuleAttribute replacementOptions = null)
        {
            var options = typeof(T).GetCustomAttribute<NativeModuleAttribute>();
            return typeof(WrapperReflectionInvoker).GetImplementationOf(typeof(T)).New<T>(new Internal.NativeModuleDispatcher(NativeModuleFactory.LoadAssembly(replacementOptions.Path ?? options.Path ?? typeof(T).Name), replacementOptions));
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
            return typeof(WrapperReflectionInvoker).GetImplementationOf(typeof(T)).New<T>(new Internal.NativeModuleDispatcher(factory.LoadModule(replacementOptions.Path ?? options.Path ?? typeof(T).Name), replacementOptions));
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
                case WrapperReflectionInvoker wrapper
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
