using Alsein.Extensions.Runtime.Internal;
using System;
using System.Collections.Generic;

namespace Alsein.Extensions.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class ArgumentsBuilder
    {
        private readonly List<Argument> _values;

        /// <summary>
        /// 
        /// </summary>
        public ArgumentsBuilder() => _values = new List<Argument>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        public ArgumentsBuilder(int length) => _values = new List<Argument>(length);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public ArgumentsBuilder SetByVal<TValue>(int index, TValue value)
        {
            _values.Preserve(index)[index] = new ByValArgument<TValue>(value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ArgumentsBuilder SetByVal(int index, Type type, object value)
        {

            _values.Preserve(index)[index] =
                (Argument)Activator.CreateInstance(typeof(ByValArgument<>).MakeGenericType(type), value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public ArgumentsBuilder SetByRef<TValue>(int index, ref TValue value)
        {
            _values.Preserve(index)[index] = new ByRefArgument<TValue>(ref value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public ArgumentsBuilder SetVoid(int index)
        {
            _values.Preserve(index)[index] = VoidArgument.Instance;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IArguments Build() => new Arguments(_values.ToArray());

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IArguments Return<TValue>(ref TValue value) =>
            new ArgumentsBuilder(1).SetByRef(0, ref value).Build();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IArguments Return<TValue>(TValue value) =>
            new ArgumentsBuilder(1).SetByVal(0, value).Build();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IArguments Return(Type type, object value) =>
            new ArgumentsBuilder(1).SetByVal(0, type, value).Build();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IArguments Return() =>
            new ArgumentsBuilder(1).SetVoid(0).Build();
    }
}
