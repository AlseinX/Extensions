using System;
using System.Collections.Generic;

namespace Alsein.Utilities.Runtime.InteropServices.Internal
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

        protected abstract Delegate GetFunction(string functionName, Type delegateType);

        #region IDisposable Support
        private bool _disposedValue = false;

        protected abstract void Dispose();

        ~NativeModule() => ((IDisposable)this).Dispose();

        void IDisposable.Dispose()
        {
            if (!_disposedValue)
            {
                Dispose();
                _disposedValue = true;
            }
        }

        #endregion
    }
}
