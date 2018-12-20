using System;

namespace Alsein.Utilities.Runtime.Internal
{
    internal class ByRefArgument<TValue> : Argument<TValue>
    {
        private readonly IntPtr _pointer;

        public ByRefArgument(ref TValue value) =>
            _pointer = RefPtrConverter.RefToPtr(ref value);

        public override ref TValue RefValue =>
            ref RefPtrConverter.PtrToRef<TValue>(_pointer);
    }
}
