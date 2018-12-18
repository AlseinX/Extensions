using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alsein.Utilities.Runtime.InteropServices.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public class NativeModuleDispatcher : INativeModule, IReflectionInvoker
    {
        private readonly INativeModule _nativeAssembly;

        private readonly NativeModuleAttribute _replacementOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nativeAssembly"></param>
        /// <param name="replacementOptions"></param>
        public NativeModuleDispatcher(INativeModule nativeAssembly, NativeModuleAttribute replacementOptions)
        {
            _nativeAssembly = nativeAssembly;
            _replacementOptions = replacementOptions;
            _funcs = new Dictionary<MethodInfo, Delegate>();
        }

        void IDisposable.Dispose() => _nativeAssembly.Dispose();

        Delegate INativeModule.GetFunction(string functionName, Type delegateType) => _nativeAssembly.GetFunction(functionName, delegateType);

        private readonly IDictionary<MethodInfo, Delegate> _funcs;

        object IReflectionInvoker.Invoke(MethodInfo method, params object[] args) => _funcs.GetOrCreate(method, () =>
            {
                var entryPoint = method.GetCustomAttribute<NativeFunctionAttribute>()?.EntryPoint ?? method.Name;
                return _nativeAssembly.GetFunction(entryPoint, NativeFunctionDelegates.GetDelegateTypeFor(method, _replacementOptions));
            }).DynamicInvoke(args);
    }
}
