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

        event Action<INativeModule, EventArgs> INativeModule.Disposing
        {
            add => _nativeModule.Disposing += value;

            remove => _nativeModule.Disposing -= value;
        }

        event Action<INativeModule, EventArgs> INativeModule.Disposed
        {
            add => _nativeModule.Disposed += value;

            remove => _nativeModule.Disposed -= value;
        }

        void IDisposable.Dispose() => _nativeModule.Dispose();

        Delegate INativeModule.GetFunction(string functionName, Type delegateType) => _nativeModule.GetFunction(functionName, delegateType);

        private readonly IDictionary<MethodInfo, Delegate> _funcs;

        IArguments IReflectionInvoker.Invoke(MethodInfo method, IArguments args)
        {
            var entryPoint = method.GetCustomAttribute<NativeFunctionAttribute>()?.EntryPoint ?? method.Name;
            if (method.IsSpecialName)
            {
                if (method.Name.StartsWith("get_"))
                {
                    var name = method.Name.Substring(4);
                    return ArgumentsBuilder.Return(method.ReturnType, _nativeModule.GetGlobalVariable(name, method.ReturnType));
                }
                else if (method.Name.StartsWith("set_"))
                {
                    var name = method.Name.Substring(4);
                    _nativeModule.SetGlobalVariable(name, args[0]);
                    return ArgumentsBuilder.Return();
                }
            }
            return args.Invoke(_funcs.GetOrCreate(method, () =>
                 _nativeModule.GetFunction(entryPoint, NativeFunctionDelegates.GetDelegateTypeFor(method, _replacementOptions))
            ));
        }

        object INativeModule.GetGlobalVariable(string variableName, Type valueType) => _nativeModule.GetGlobalVariable(variableName, valueType);
        void INativeModule.SetGlobalVariable(string variableName, object value) => _nativeModule.SetGlobalVariable(variableName, value);
    }
}
