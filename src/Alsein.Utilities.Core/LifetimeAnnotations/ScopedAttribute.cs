using System;

namespace Alsein.Utilities.LifetimeAnnotations
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ScopedAttribute : Attribute { }
}