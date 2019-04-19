using Alsein.Extensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsein.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> BinarySearch<TSource>(this IReadOnlyList<TSource> list, Func<TSource, int> comparer)
        {
            if (list.Count == 0)
            {
                return Enumerable.Empty<TSource>();
            }

            var min = 0;
            var max = list.Count;

            while (min < max)
            {
                var mid = min + ((max - min) / 2);
                var midItem = list[mid];
                var comp = comparer(midItem);

                if (comp < 0)
                {
                    min = mid + 1;
                }
                else if (comp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    return midItem.Plural();
                }
            }

            return min == max && min < list.Count && comparer(list[min]) == 0 ?
                list[min].Plural() :
                Enumerable.Empty<TSource>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="list"></param>
        /// <param name="keySelector"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> BinarySearch<TSource, TKey>(this IReadOnlyList<TSource> list, Func<TSource, TKey> keySelector, TKey key)
            where TKey : IComparable<TKey> =>
            list.BinarySearch(item => keySelector(item).CompareTo(key));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        public static ICollection<TSource> RemoveWhere<TSource>(this ICollection<TSource> source, Func<TSource, bool> predicate)
        {
            source.Where(predicate).ForAll(source.Remove);
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static T[] Resize<T>(this T[] source, int size)
        {
            var result = new T[size];
            Array.Copy(source, result, Math.Min(size, source.Length));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> creator)
        {
            lock (source)
            {
                if (!source.TryGetValue(key, out var result))
                {
                    source.Add(key, result = creator());
                }
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static TValue GetOrCreate<TValue>(this ICollection<TValue> source, Func<TValue, bool> predicate, Func<TValue> creator)
        {
            lock (source)
            {
                var result = source.FirstOrDefault(predicate);
                if (result == default)
                {
                    source.Add(result = creator());
                }
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static IList<TSource> Preserve<TSource>(this IList<TSource> source, int index)
        {
            for (var diff = index + 1 - source.Count; diff > 0; diff--)
            {
                source.Add(default);
            }
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        public delegate void ByRefAction<T>(ref T obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public delegate void ByRefAction<T1, T2>(ref T1 obj1, ref T2 obj2);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="obj3"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <returns></returns>
        public delegate void ByRefAction<T1, T2, T3>(ref T1 obj1, ref T2 obj2, ref T3 obj3);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] ForArray<T>(this T[] source, ByRefAction<T> action)
        {
            for (var i = 0; i < source.Length; i++)
            {
                action(ref source[i]);
            }
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="action"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public static T1[] ForArray<T1, T2>(this T1[] source1, T2[] source2, ByRefAction<T1, T2> action)
        {
            var default2 = default(T2);
            for (var i = 0; i < source1.Length; i++)
            {
                action(
                    ref source1[i],
                    ref (i < source2.Length ? ref source2[i] : ref default2)
                );
            }
            return source1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="source3"></param>
        /// <param name="action"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <returns></returns>
        public static T1[] ForArray<T1, T2, T3>(this T1[] source1, T2[] source2, T3[] source3, ByRefAction<T1, T2, T3> action)
        {
            var default2 = default(T2);
            var default3 = default(T3);
            for (var i = 0; i < source1.Length; i++)
            {
                action(
                    ref source1[i],
                    ref (i < source2.Length ? ref source2[i] : ref default2),
                    ref (i < source3.Length ? ref source3[i] : ref default3)
                );
            }
            return source1;
        }
    }
}