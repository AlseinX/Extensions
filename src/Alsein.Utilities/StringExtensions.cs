using System;
using System.Collections.Generic;
using System.Text;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitByCamel(this string source)
        {
            var chars = source.ToCharArray();
            var sb = new StringBuilder();
            for (var i = 0; i < chars.Length; i++)
            {
                sb.Append(chars[i]);
                if (i + 1 == chars.Length || 'A' <= chars[i + 1] && chars[i + 1] <= 'Z')
                {
                    yield return sb.ToString();
                    sb.Clear();
                }
            }
        }
    }
}