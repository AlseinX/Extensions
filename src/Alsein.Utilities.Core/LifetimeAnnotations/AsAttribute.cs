using System;
using System.Collections.Generic;
using System.Text;

namespace Alsein.Utilities.LifetimeAnnotations
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AsAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public Type Interface { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interface"></param>
        public AsAttribute(Type @interface) => Interface = @interface;
    }
}
