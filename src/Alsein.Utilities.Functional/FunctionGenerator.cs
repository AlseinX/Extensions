using System;

namespace Alsein.Utilities
{
    public abstract class FunctionGenerator<TFunc> where TFunc : Delegate
    {
        public abstract TFunc Default { get; }

        public static implicit operator TFunc(FunctionGenerator<TFunc> value) => value.Default;
    }

    public sealed class FunctionGenerator<T, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T, TFunc> _generator;

        private T _defaultValue;

        public FunctionGenerator(Func<T, TFunc> generator, T defaultValue)
        {
            _generator = generator;
            _defaultValue = defaultValue;
        }

        public TFunc this[T value] => _generator(value);

        public override TFunc Default => _generator(_defaultValue);
    }

    public sealed class FunctionGenerator<T1, T2, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, TFunc> _generator;

        private (T1, T2) _defaultValues;

        public FunctionGenerator(Func<T1, T2, TFunc> generator, (T1, T2) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        public TFunc this[T1 value1, T2 value2] => _generator(value1, value2);

        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2);
    }

    public sealed class FunctionGenerator<T1, T2, T3, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, TFunc> _generator;

        private (T1, T2, T3) _defaultValues;

        public FunctionGenerator(Func<T1, T2, T3, TFunc> generator, (T1, T2, T3) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        public TFunc this[T1 value1, T2 value2, T3 value3] => _generator(value1, value2, value3);

        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3);
    }

    public sealed class FunctionGenerator<T1, T2, T3, T4, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, TFunc> _generator;

        private (T1, T2, T3, T4) _defaultValues;

        public FunctionGenerator(Func<T1, T2, T3, T4, TFunc> generator, (T1, T2, T3, T4) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4] => _generator(value1, value2, value3, value4);

        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4);
    }

    public sealed class FunctionGenerator<T1, T2, T3, T4, T5, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, T5, TFunc> _generator;

        private (T1, T2, T3, T4, T5) _defaultValues;

        public FunctionGenerator(Func<T1, T2, T3, T4, T5, TFunc> generator, (T1, T2, T3, T4, T5) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4, T5 value5] => _generator(value1, value2, value3, value4, value5);

        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4, _defaultValues.Item5);
    }

    public sealed class FunctionGenerator<T1, T2, T3, T4, T5, T6, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, T5, T6, TFunc> _generator;

        private (T1, T2, T3, T4, T5, T6) _defaultValues;

        public FunctionGenerator(Func<T1, T2, T3, T4, T5, T6, TFunc> generator, (T1, T2, T3, T4, T5, T6) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6] => _generator(value1, value2, value3, value4, value5, value6);

        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4, _defaultValues.Item5, _defaultValues.Item6);
    }

    public sealed class FunctionGenerator<T1, T2, T3, T4, T5, T6, T7, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, T5, T6, T7, TFunc> _generator;

        private (T1, T2, T3, T4, T5, T6, T7) _defaultValues;

        public FunctionGenerator(Func<T1, T2, T3, T4, T5, T6, T7, TFunc> generator, (T1, T2, T3, T4, T5, T6, T7) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7] => _generator(value1, value2, value3, value4, value5, value6, value7);

        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4, _defaultValues.Item5, _defaultValues.Item6, _defaultValues.Item7);
    }

    public sealed class FunctionGenerator<T1, T2, T3, T4, T5, T6, T7, T8, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, T5, T6, T7, T8, TFunc> _generator;

        private (T1, T2, T3, T4, T5, T6, T7, T8) _defaultValues;

        public FunctionGenerator(Func<T1, T2, T3, T4, T5, T6, T7, T8, TFunc> generator, (T1, T2, T3, T4, T5, T6, T7, T8) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8] => _generator(value1, value2, value3, value4, value5, value6, value7, value8);

        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4, _defaultValues.Item5, _defaultValues.Item6, _defaultValues.Item7, _defaultValues.Item8);
    }
}