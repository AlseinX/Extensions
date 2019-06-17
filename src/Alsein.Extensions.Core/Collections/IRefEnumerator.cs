using System.Collections.Generic;

namespace Alsein.Extensions.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRefEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        new ref T Current { get; }
    }
}