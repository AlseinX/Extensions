using System;
using System.Runtime.InteropServices;

namespace Alsein.Extensions.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class NativeModuleAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public NativeModuleAttribute() => DefaultFunctionOptions = new NativeFunctionAttribute();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public NativeModuleAttribute(string path) : this() => Path = path;

        /// <summary>
        /// 
        /// </summary>
        public NativeFunctionAttribute DefaultFunctionOptions { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CallingConvention? CallingConvention
        {
            get => DefaultFunctionOptions.CallingConvention;
            set => DefaultFunctionOptions.CallingConvention = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public CharSet? CharSet
        {
            get => DefaultFunctionOptions.CharSet;
            set => DefaultFunctionOptions.CharSet = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool? BestFitMapping
        {
            get => DefaultFunctionOptions.BestFitMapping;
            set => DefaultFunctionOptions.BestFitMapping = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool? SetLastError
        {
            get => DefaultFunctionOptions.SetLastError;
            set => DefaultFunctionOptions.SetLastError = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool? ThrowOnUnmappableChar
        {
            get => DefaultFunctionOptions.ThrowOnUnmappableChar;
            set => DefaultFunctionOptions.ThrowOnUnmappableChar = value;
        }
    }
}