using System;
using System.Runtime.InteropServices;

namespace Alsein.Utilities.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NativeFunctionAttribute : Attribute
    {
        /*
        /// <summary>
        /// 
        /// </summary>
        public NativeFunctionAttribute()
        {
            EntryPoint = string.Empty;
            CallingConvention = CallingConvention.Cdecl;
            CharSet = CharSet.Ansi;
            BestFitMapping = true;
            SetLastError = true;
            ThrowOnUnmappableChar = false;
        }*/

        /// <summary>
        /// 
        /// </summary>
        public string EntryPoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CallingConvention? CallingConvention { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CharSet? CharSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? BestFitMapping { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? SetLastError { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? ThrowOnUnmappableChar { get; set; }
    }
}
