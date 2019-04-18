using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Alsein.Extensions.Runtime.InteropServices.Internal
{
    internal abstract class NativeModule : INativeModule
    {
        private readonly IDictionary<string, IList<Delegate>> _functions;

        public NativeModule() => _functions = new Dictionary<string, IList<Delegate>>();

        Delegate INativeModule.GetFunction(string functionName, Type delegateType)
        {
            var delegates = _functions.GetOrCreate(functionName, () => new List<Delegate>());
            return delegates.GetOrCreate(d => d.GetType() == delegateType, () => GetFunction(functionName, delegateType));
        }

        object INativeModule.GetGlobalVariable(string variableName, Type valueType)
        {
            if (!valueType.IsValueType)
            {
                throw new ArgumentException("The type must be a value type.");
            }
            return Marshal.PtrToStructure(GetGlobalVariable(variableName), valueType);
        }

        void INativeModule.SetGlobalVariable(string variableName, object value)
        {
            if (!value.GetType().IsValueType)
            {
                throw new ArgumentException("The type must be a value type.");
            }
            Marshal.StructureToPtr(value, GetGlobalVariable(variableName), false);
        }

        protected abstract Delegate GetFunction(string functionName, Type delegateType);

        protected abstract IntPtr GetGlobalVariable(string variableName);

        #region IDisposable Support
        private bool _disposedValue = false;

        public event Action<INativeModule, EventArgs> Disposing;

        public event Action<INativeModule, EventArgs> Disposed;

        protected abstract void Dispose();

        ~NativeModule() => ((IDisposable)this).Dispose();

        void IDisposable.Dispose()
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
                Disposing?.Invoke(this, new EventArgs { });
                Dispose();
                Disposed?.Invoke(this, new EventArgs { });
            }
        }
        #endregion
    }
}
