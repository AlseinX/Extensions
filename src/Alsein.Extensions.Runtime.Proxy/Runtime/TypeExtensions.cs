using Alsein.Extensions.Runtime.Internal;
using System;

namespace Alsein.Extensions.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Type GetImplementationOf(this Type parent, Type target) => ImplementationGenerator.GetImplemention(target, parent);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Type GetImplementationOf<TTarget>(this Type parent) => ImplementationGenerator.GetImplemention(typeof(TTarget), parent);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object New(this Type type, params object[] args) => Activator.CreateInstance(type, args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T New<T>(this Type type, params object[] args) => (T)Activator.CreateInstance(type, args);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type MakeGenericType<T1>(this Type type) => type.MakeGenericType(typeof(T1));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type MakeGenericType<T1, T2>(this Type type) => type.MakeGenericType(typeof(T1), typeof(T2));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type MakeGenericType<T1, T2, T3>(this Type type) => type.MakeGenericType(typeof(T1), typeof(T2), typeof(T3));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type MakeGenericType<T1, T2, T3, T4>(this Type type) => type.MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(T4));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type MakeGenericType<T1, T2, T3, T4, T5>(this Type type) => type.MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type MakeGenericType<T1, T2, T3, T4, T5, T6>(this Type type) => type.MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));

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
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type MakeGenericType<T1, T2, T3, T4, T5, T6, T7>(this Type type) => type.MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));

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
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type MakeGenericType<T1, T2, T3, T4, T5, T6, T7, T8>(this Type type) => type.MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
    }
}