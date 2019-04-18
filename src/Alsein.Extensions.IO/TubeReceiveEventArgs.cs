using System;

namespace Alsein.Extensions.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class TubeReceiveEventArgs : EventArgs
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
        public TubeReceiveEventArgs(object result) => Result = result;
    }
}
