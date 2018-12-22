using System;

namespace Alsein.Utilities.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class EntryPoint : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public EntryPoint(string value) => Value = value;
    }
}
