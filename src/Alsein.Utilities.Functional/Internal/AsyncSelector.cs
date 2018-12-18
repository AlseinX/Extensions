using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alsein.Utilities.RuntimeInternal
{
    internal class AsyncSelector<TSource, TResult> : IAsyncEnumerable<TResult>
    {
        private IAsyncEnumerable<TSource> _sources;

        private Func<TSource, Task<TResult>> _selector;

        public AsyncSelector(IAsyncEnumerable<TSource> sources, Func<TSource, Task<TResult>> selector)
        {
            _sources = sources;
            _selector = selector;
        }

        private class Enumerator : IAsyncEnumerator<TResult>
        {
            private AsyncSelector<TSource, TResult> _target;

            private IAsyncEnumerator<TSource> _enumerator = null;

            public Enumerator(AsyncSelector<TSource, TResult> target) => _target = target;

            public TResult Current { get; private set; }

            public void Dispose() => _enumerator.Dispose();

            public async Task<bool> MoveNext(CancellationToken cancellationToken)
            {
                if (_enumerator == null)
                {
                    _enumerator = _target._sources.GetEnumerator();
                }
                if (!await _enumerator.MoveNext())
                {
                    return false;
                }
                Current = await _target._selector(_enumerator.Current);
                return true;
            }
        }

        public IAsyncEnumerator<TResult> GetEnumerator() => new Enumerator(this);
    }
}