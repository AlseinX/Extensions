using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alsein.Utilities.Runtime.DynamicInvokers
{
    /// <summary>
    /// 
    /// </summary>
    public class DelegateDynamicInvoker : IDynamicInvoker
    {
        private readonly IReadOnlyDictionary<MethodInfo, Delegate> _implements;

        /// <summary>
        /// 
        /// </summary>
        public DelegateDynamicInvoker(IEnumerable<KeyValuePair<MethodInfo, Delegate>> implements)
        {
            if (implements is IReadOnlyDictionary<MethodInfo, Delegate> value)
            {
                _implements = value;
            }
            else
            {
                var dict = new Dictionary<MethodInfo, Delegate>();
                foreach (var pair in implements)
                {
                    dict.Add(pair.Key, pair.Value);
                }
                _implements = dict;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="valueArgs"></param>
        /// <returns></returns>
        public object Invoke(MethodInfo method, object[] valueArgs)
        {
            switch (_implements[method])
            {
                case VariableArgsHandler handler:
                    return handler(valueArgs);
                case Delegate handler:
                    return handler.DynamicInvoke(valueArgs);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}