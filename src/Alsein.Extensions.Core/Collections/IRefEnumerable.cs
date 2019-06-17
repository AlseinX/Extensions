using System.Collections.Generic;

namespace Alsein.Extensions.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRefEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new IRefEnumerator<T> GetEnumerator();
    }
}