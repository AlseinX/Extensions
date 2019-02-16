namespace Alsein.Utilities.Runtime.Internal
{
    internal abstract class Argument<TValue> : Argument
    {
        public abstract ref TValue RefValue { get; }

        public sealed override object Value
        {
            get => RefValue;
            set => RefValue = (TValue)value;
        }
    }
}
