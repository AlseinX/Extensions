using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alsein.Utilities.Runtime.InteropServices.Internal
{
    internal class NativeModuleDispatcher : INativeModule, IReflectionInvoker
    {
        private readonly INativeModule _nativeModule;

        private readonly NativeModuleAttribute _replacementOptions;

        public NativeModuleDispatcher(INativeModule nativeAssembly, NativeModuleAttribute replacementOptions)
        {
            _nativeModule = nativeAssembly;
            _replacementOptions = replacementOptions;
            _funcs = new Dictionary<MethodInfo, Delegate>();
        }

        void IDisposable.Dispose() => _nativeModule.Dispose();

        Delegate INativeModule.GetFunction(string functionName, Type delegateType) => _nativeModule.GetFunction(functionName, delegateType);

        private readonly IDictionary<MethodInfo, Delegate> _funcs;

        object IReflectionInvoker.Invoke(MethodInfo method, params object[] args) => _funcs.GetOrCreate(method, () =>
            {
                var entryPoint = method.GetCustomAttribute<NativeFunctionAttribute>()?.EntryPoint ?? method.Name;
                return _nativeModule.GetFunction(entryPoint, NativeFunctionDelegates.GetDelegateTypeFor(method, _replacementOptions));
            }).DynamicInvoke(args);
    }
}
