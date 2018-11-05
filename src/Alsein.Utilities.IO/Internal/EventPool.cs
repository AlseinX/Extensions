using System;
using System.Collections.Generic;

namespace Alsein.Utilities.IO.Internal
{
    internal partial class EventPool : IEventPool
    {
        private Dictionary<object, IEvents<object>> _eventsList;

        public EventPool()
        {
            _eventsList = new Dictionary<object, IEvents<object>>();
        }

        public void Reset()
        {
            lock (_eventsList)
            {
                foreach (var item in _eventsList)
                {
                    (item.Value as IChainDisposable)?.DisposeSelf();
                }
                _eventsList.Clear();
            }
        }

        public void Reset(object target)
        {
            _eventsList.Remove(target);
        }

        public IEvents<TTarget> GetEvents<TTarget>(TTarget target) where TTarget : class
        {
            if (target == null)
            {
                return null;
            }
            if (_eventsList.TryGetValue(target, out var value))
            {
                return (IEvents<TTarget>)value;
            }
            var events = (IEvents<TTarget>)Activator.CreateInstance(typeof(Events<>).MakeGenericType(target.GetType()), this, target);
            lock (_eventsList)
            {
                _eventsList.Add(target, events);
            }
            return events;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Reset();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EventPool() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}