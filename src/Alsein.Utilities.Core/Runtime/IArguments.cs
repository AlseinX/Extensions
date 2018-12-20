using System;
using System.Collections.Generic;

namespace Alsein.Utilities.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public interface IArguments : IReadOnlyList<object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        ref T At<T>(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        IArguments Invoke(Delegate @delegate);
    }
}
