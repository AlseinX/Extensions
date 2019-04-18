using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alsein.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="predicate"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static MethodInfo GetMethod(this Type type, string name, Func<MethodInfo, bool> predicate, BindingFlags bindingFlags = BindingFlags.Default)
        {
            var result = default(IEnumerable<MethodInfo>);
            if (bindingFlags == BindingFlags.Default)
            {
                result = type.GetMethods();
            }
            else
            {
                result = type.GetMethods(bindingFlags);
            }
            return result.Where(m => m.Name == name).Where(predicate).Single();
        }
    }
}
