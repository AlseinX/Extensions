using System.Threading.Tasks;

namespace System.Threading
{
    /// <summary>
    /// A async lock for using statement.
    /// </summary>
    public class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Must 'using' the await result of this method, or It may cause problems.
        /// </summary>
        /// <returns>The System.IDisposible object that releases the lock when disposed.</returns>
        public async Task<IDisposable> WaitAsync()
        {
            await _semaphore.WaitAsync();
            return new Disposer(this);
        }

        private class Disposer : IDisposable
        {
            private AsyncLock _target;

            public Disposer(AsyncLock target) => _target = target;

            public void Dispose()
            {
                if (_target == null)
                {
                    return;
                }
                var temp = _target;
                _target = null;
                temp._semaphore.Release();
            }

            ~Disposer() => Dispose();
        }
    }
}