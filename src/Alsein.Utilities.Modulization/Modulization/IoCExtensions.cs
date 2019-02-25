using System;
using System.Reflection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public static class IoCExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInstantiableClass(this Type type) =>
            type.IsClass &&
            !type.IsAbstract &&
            !type.IsGenericTypeDefinition &&
            !type.IsNested;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNonService(this Type type) => type.IsDefined(typeof(LifetimeAnnotations.NonServiceAttribute), false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool IsSharingRootNamespace(this Assembly source, Assembly destination) =>
            source.FullName.StartsWith(destination.FullName.Split(',')[0].Split('.')[0]);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetRegisteredEntry(this Type type) => type.GetCustomAttribute<LifetimeAnnotations.AsAttribute>(false)?.Interface ?? type;
    }
}