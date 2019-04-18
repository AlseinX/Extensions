using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Alsein.Extensions.Runtime.Internal
{
    internal class Arguments : IArguments
    {
        private readonly Argument[] _values;

        internal Arguments(Argument[] values) => _values = values;

        public object this[int index]
        {
            get => _values[index].Value;
            set => _values[index].Value = value;
        }

        public ref T At<T>(int index)
        {
            switch (_values[index])
            {
                case Argument<T> arg:
                    return ref arg.RefValue;

                case VoidArgument _:
                    throw new NotSupportedException();

                default:
                    throw new ArgumentException();
            }
        }

        public IEnumerator<object> GetEnumerator() =>
            _values.Select(arg => arg.Value).GetEnumerator();

        int IReadOnlyCollection<object>.Count => _values.Length;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IArguments Invoke(Delegate @delegate) => ArgumentsDynamicInvoker.Invoke(this, @delegate);
    }
}
