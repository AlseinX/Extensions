using System;
using System.Linq;
using System.Reflection;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class Resolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryResolve<T>(this IResolver container, object key, out T result)
        where T : class
        {
            if (container.TryResolve(key, out var _result))
            {
                result = (T)_result;
                return true;
            }
            result = default;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryResolve<T>(this IResolver container, out T result)
        where T : class
        {
            if (container.TryResolve(typeof(T), out var _result))
            {
                result = (T)_result;
                return true;
            }
            result = default;
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Resolve(this IResolver container, object key)
        => container.TryResolve(key, out var result) ? result : throw new Exception();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>(this IResolver container, object key)
        where T : class
        => container.TryResolve(key, out var result) ? (T)result : throw new Exception();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>(this IResolver container)
        where T : class
        => container.TryResolve(typeof(T), out var result) ? (T)result : throw new Exception();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object ResolveOrDefault(this IResolver container, object key)
        => container.TryResolve(key, out var result) ? result : default;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ResolveOrDefault<T>(this IResolver container, object key)
        where T : class
        => container.TryResolve(key, out var result) ? (T)result : default;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ResolveOrDefault<T>(this IResolver container)
        where T : class
        => container.TryResolve(typeof(T), out var result) ? (T)result : default;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T InjectProperties<T>(this IResolver container, T obj)
        {
            var properties = obj.GetType()
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<InjectedAttribute>() is InjectedAttribute attr)
                {
                    property.SetValue(obj, container.Resolve(attr.Key ?? property.PropertyType));
                }
            }

            return obj;
        }
    }
}