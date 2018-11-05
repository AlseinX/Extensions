using System;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFunc"></typeparam>
    public abstract class FunctionGenerator<TFunc> where TFunc : Delegate
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract TFunc Default { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator TFunc(FunctionGenerator<TFunc> value) => value.Default;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TFunc"></typeparam>
    public sealed class FunctionGenerator<T, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T, TFunc> _generator;

        private T _defaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="defaultValue"></param>
        public FunctionGenerator(Func<T, TFunc> generator, T defaultValue)
        {
            _generator = generator;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TFunc this[T value] => _generator(value);

        /// <summary>
        /// 
        /// </summary>
        public override TFunc Default => _generator(_defaultValue);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TFunc"></typeparam>
    public sealed class FunctionGenerator<T1, T2, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, TFunc> _generator;

        private (T1, T2) _defaultValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="defaultValues"></param>
        public FunctionGenerator(Func<T1, T2, TFunc> generator, (T1, T2) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public TFunc this[T1 value1, T2 value2] => _generator(value1, value2);

        /// <summary>
        /// 
        /// </summary>
        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TFunc"></typeparam>
    public sealed class FunctionGenerator<T1, T2, T3, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, TFunc> _generator;

        private (T1, T2, T3) _defaultValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="defaultValues"></param>
        public FunctionGenerator(Func<T1, T2, T3, TFunc> generator, (T1, T2, T3) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <returns></returns>
        public TFunc this[T1 value1, T2 value2, T3 value3] => _generator(value1, value2, value3);

        /// <summary>
        /// 
        /// </summary>
        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="TFunc"></typeparam>
    public sealed class FunctionGenerator<T1, T2, T3, T4, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, TFunc> _generator;

        private (T1, T2, T3, T4) _defaultValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="defaultValues"></param>
        public FunctionGenerator(Func<T1, T2, T3, T4, TFunc> generator, (T1, T2, T3, T4) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="value4"></param>
        /// <returns></returns>
        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4] => _generator(value1, value2, value3, value4);

        /// <summary>
        /// 
        /// </summary>
        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="TFunc"></typeparam>
    public sealed class FunctionGenerator<T1, T2, T3, T4, T5, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, T5, TFunc> _generator;

        private (T1, T2, T3, T4, T5) _defaultValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="defaultValues"></param>
        public FunctionGenerator(Func<T1, T2, T3, T4, T5, TFunc> generator, (T1, T2, T3, T4, T5) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="value4"></param>
        /// <param name="value5"></param>
        /// <returns></returns>
        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4, T5 value5] => _generator(value1, value2, value3, value4, value5);

        /// <summary>
        /// 
        /// </summary>
        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4, _defaultValues.Item5);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="TFunc"></typeparam>
    public sealed class FunctionGenerator<T1, T2, T3, T4, T5, T6, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, T5, T6, TFunc> _generator;

        private (T1, T2, T3, T4, T5, T6) _defaultValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="defaultValues"></param>
        public FunctionGenerator(Func<T1, T2, T3, T4, T5, T6, TFunc> generator, (T1, T2, T3, T4, T5, T6) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="value4"></param>
        /// <param name="value5"></param>
        /// <param name="value6"></param>
        /// <returns></returns>
        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6] => _generator(value1, value2, value3, value4, value5, value6);

        /// <summary>
        /// 
        /// </summary>
        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4, _defaultValues.Item5, _defaultValues.Item6);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="TFunc"></typeparam>
    public sealed class FunctionGenerator<T1, T2, T3, T4, T5, T6, T7, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, T5, T6, T7, TFunc> _generator;

        private (T1, T2, T3, T4, T5, T6, T7) _defaultValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="defaultValues"></param>
        public FunctionGenerator(Func<T1, T2, T3, T4, T5, T6, T7, TFunc> generator, (T1, T2, T3, T4, T5, T6, T7) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="value4"></param>
        /// <param name="value5"></param>
        /// <param name="value6"></param>
        /// <param name="value7"></param>
        /// <returns></returns>
        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7] => _generator(value1, value2, value3, value4, value5, value6, value7);

        /// <summary>
        /// 
        /// </summary>
        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4, _defaultValues.Item5, _defaultValues.Item6, _defaultValues.Item7);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="TFunc"></typeparam>
    public sealed class FunctionGenerator<T1, T2, T3, T4, T5, T6, T7, T8, TFunc> : FunctionGenerator<TFunc> where TFunc : Delegate
    {
        private Func<T1, T2, T3, T4, T5, T6, T7, T8, TFunc> _generator;

        private (T1, T2, T3, T4, T5, T6, T7, T8) _defaultValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="defaultValues"></param>
        public FunctionGenerator(Func<T1, T2, T3, T4, T5, T6, T7, T8, TFunc> generator, (T1, T2, T3, T4, T5, T6, T7, T8) defaultValues)
        {
            _generator = generator;
            _defaultValues = defaultValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="value4"></param>
        /// <param name="value5"></param>
        /// <param name="value6"></param>
        /// <param name="value7"></param>
        /// <param name="value8"></param>
        /// <returns></returns>
        public TFunc this[T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8] => _generator(value1, value2, value3, value4, value5, value6, value7, value8);

        /// <summary>
        /// 
        /// </summary>
        public override TFunc Default => _generator(_defaultValues.Item1, _defaultValues.Item2, _defaultValues.Item3, _defaultValues.Item4, _defaultValues.Item5, _defaultValues.Item6, _defaultValues.Item7, _defaultValues.Item8);
    }
}