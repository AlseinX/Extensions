using System;

namespace Alsein.Utilities
{
    public static class IoCExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider) => (T)serviceProvider.GetService(typeof(T));

        public static bool IsSingletonService(this Type type) => type.IsDefined(typeof(LifetimeAnnotations.SingletonAttribute), false);

        public static bool IsTransientService(this Type type) => type.IsDefined(typeof(LifetimeAnnotations.TransientAttribute), false);

        public static bool IsScopedService(this Type type) => type.IsDefined(typeof(LifetimeAnnotations.ScopedAttribute), false);
    }
}