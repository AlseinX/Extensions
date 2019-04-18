using System;

namespace Alsein.Extensions.LifetimeAnnotations
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ScopedAttribute : Attribute { }
}