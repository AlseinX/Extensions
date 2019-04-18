using System;
using System.Collections.Generic;
using System.Text;

namespace Alsein.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ContextNotPresetException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ContextNotPresetException() : base("Context must be set before instantiation.") { }
    }
}
