using System;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class IoCExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceProvider serviceProvider) => (T)serviceProvider.GetService(typeof(T));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSingletonService(this Type type) => type.IsDefined(typeof(LifetimeAnnotations.SingletonAttribute), false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTransientService(this Type type) => type.IsDefined(typeof(LifetimeAnnotations.TransientAttribute), false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsScopedService(this Type type) => type.IsDefined(typeof(LifetimeAnnotations.ScopedAttribute), false);
    }
}