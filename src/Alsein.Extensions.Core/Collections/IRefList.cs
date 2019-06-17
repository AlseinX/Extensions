using System.Collections.Generic;

namespace Alsein.Extensions.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRefList<T> : IRefEnumerable<T>, IList<T>, IReadOnlyList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        new ref T this[int index] { get; }
    }
}