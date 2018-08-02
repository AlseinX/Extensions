using System;

namespace Alsein.Utilities.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class ReceiveEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public object Result { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMonopolied { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsAsyncReceived { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public ReceiveEventArgs(object result) => Result = result;
    }
}
