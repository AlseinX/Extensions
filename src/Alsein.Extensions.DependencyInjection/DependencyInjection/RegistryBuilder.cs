using System;
using System.Linq;
using System.Reflection;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class RegistryBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRegistryBuilder To<T>(this IRegistryBuilder builder) => builder.To(typeof(T));

        private static ConstructorInfo GetLongestConstructor(Type type)
        => type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Max((a, b) => a.GetParameters().Length - b.GetParameters().Length).First();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IConstructorRegistryBuilder UseType(this IConstructorRegistryBuilder builder, Type type)
        => builder.UseConstructor(GetLongestConstructor(type));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IConstructorRegistryBuilder UseType<T>(this IConstructorRegistryBuilder builder)
        => builder.UseConstructor(GetLongestConstructor(typeof(T)));
    }
}