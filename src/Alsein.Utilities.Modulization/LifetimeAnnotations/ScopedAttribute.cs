using System;

namespace Alsein.Utilities.LifetimeAnnotations
{
    [AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ScopedAttribute : Attribute { }
}