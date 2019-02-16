using System;

namespace Alsein.Utilities.Runtime.Internal
{
    internal class VoidArgument : Argument
    {
        public override object Value
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        private VoidArgument() { }

        public static VoidArgument Instance { get; } = new VoidArgument();
    }
}
