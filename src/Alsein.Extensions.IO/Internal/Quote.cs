using System;
using System.Threading.Tasks;

namespace Alsein.Extensions.IO.Internal
{
    internal struct Quote
    {
        public TaskCompletionSource<IEventContext<object>> Source { get; private set; }

        public Type Type { get; private set; }

        public Quote(TaskCompletionSource<IEventContext<object>> source, Type type)
        {
            Source = source;
            Type = type;
        }
    }
}