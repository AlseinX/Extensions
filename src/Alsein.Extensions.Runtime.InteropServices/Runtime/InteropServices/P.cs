using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alsein.Extensions.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    public class P
    {
        /// <summary>
        /// 
        /// </summary>
        public static IDictionary<Type, INativeModule> Modules { get; }
            = new Dictionary<Type, INativeModule>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static INativeModule GetModule(Type type) => Modules.GetOrCreate(type, () => NativeModuleFactory.LoadModule(type.GetCustomAttribute<LibraryPathAttribute>().Value));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="path"></param>
        public static INativeModule LoadModule<TModule>(string path)
        {
            var result = NativeModule.LoadModule(path);
            Modules.Add(typeof(TModule), result);
            return result;
        }
    }
}
