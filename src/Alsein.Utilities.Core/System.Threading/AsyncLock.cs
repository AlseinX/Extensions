using System.Threading.Tasks;

namespace System.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                var temp = _target;
                _target = null;
                temp?._semaphore?.Release();
            }
        }
    }
}