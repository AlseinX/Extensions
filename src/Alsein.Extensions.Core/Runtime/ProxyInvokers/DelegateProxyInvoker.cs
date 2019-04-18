using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alsein.Extensions.Runtime.ProxyInvokers
{
    /// <summary>
    /// 
    /// </summary>
    public class DelegateProxyInvoker : IProxyInvoker
    {
        private readonly IReadOnlyDictionary<MethodInfo, Delegate> _implements;

        /// <summary>
        /// 
        /// </summary>
        public DelegateProxyInvoker(IEnumerable<KeyValuePair<MethodInfo, Delegate>> implements)
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
        public IArguments Invoke(MethodInfo method, IArguments valueArgs)
        {
            switch (_implements[method])
            {
                case VariableArgsHandler handler:
                    return handler(valueArgs);
                case Delegate handler:
                    return valueArgs.Invoke(handler);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}