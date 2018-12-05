using Alsein.Utilities.Runtime.DynamicInvokers;
using Alsein.Utilities.Runtime.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alsein.Utilities.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Type GetImplementationOf(this Type parent, Type target) => InterfaceProxyBinder.GetImplemention(target, parent);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object New(this Type type, params object[] args) => Activator.CreateInstance(type, args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T New<T>(this Type type, params object[] args) => (T)Activator.CreateInstance(type, args);
        /* 
        public static object CreateMulticastProxy(Type type, IEnumerable<object> targets) =>
            CreateProxyBinder(type).GetProxy(new DelegateDynamicInvoker(type.GetMethods().Select(m => new KeyValuePair<MethodInfo, Delegate>(m, new VariableArgsHandler((ta, va) =>
                m.ReturnType.IsInterface ? CreateMulticastProxy(m.ReturnType, targets.Select(target =>
                {
                    return (m.IsGenericMethodDefinition ? m.MakeGenericMethod(ta) : m).Invoke(target, va);
                }).ToList()) : null
            )))));
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="targets"></param>
        /// <returns></returns>
        public static TInterface CreateMulticastProxy<TInterface>(IEnumerable<TInterface> targets) => (TInterface)CreateMulticastProxy(typeof(TInterface), targets.Select(x => (object)x));
        */

    }
}