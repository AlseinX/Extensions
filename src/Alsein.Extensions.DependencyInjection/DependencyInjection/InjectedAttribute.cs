using System;

namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(System.AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class InjectedAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public object Key { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public InjectedAttribute(object key = default)
        {
            Key = key;
        }
    }
}