using System.Collections;
using System.Collections.Generic;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class DeconstructExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> source, out TKey key, out TValue value) => (key, value) = (source.Key, source.Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Deconstruct<TKey, TValue>(this DictionaryEntry source, out object key, out object value) => (key, value) = (source.Key, source.Value);
    }
}