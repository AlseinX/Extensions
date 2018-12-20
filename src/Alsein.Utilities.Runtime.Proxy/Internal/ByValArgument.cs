namespace Alsein.Utilities.Runtime.Internal
{
    internal class ByValArgument<TValue> : Argument<TValue>
    {
        private TValue _value;

        public ByValArgument(TValue value) => _value = value;

        public override ref TValue RefValue => ref _value;
    }
}
