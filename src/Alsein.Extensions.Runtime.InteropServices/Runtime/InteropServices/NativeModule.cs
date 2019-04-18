using System;

namespace Alsein.Extensions.Runtime.InteropServices
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
        public static INativeModule LoadModule(string filename) => NativeModuleFactory.LoadModule(filename);
    }
}
