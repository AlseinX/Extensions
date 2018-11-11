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
    public static class Proxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IProxyBinder<TInterface> CreateProxyBinder<TInterface>() =>
            new InterfaceProxyBinder<TInterface>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IProxyBinder CreateProxyBinder(Type type) =>
            new InterfaceProxyBinder(type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="implement"></param>
        /// <returns></returns>
        public static object GetProxy(this IProxyBinder binder, object implement) => binder.GetProxy(new ObjectDynamicInvoker(implement));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="implements"></param>
        /// <returns></returns>
        public static object GetProxy(this IProxyBinder binder, IEnumerable<KeyValuePair<MethodInfo, Delegate>> implements) => binder.GetProxy(new DelegateDynamicInvoker(implements));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="implements"></param>
        /// <returns></returns>
        public static object GetProxy(this IProxyBinder binder, params KeyValuePair<MethodInfo, Delegate>[] implements) => binder.GetProxy(new DelegateDynamicInvoker(implements));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="implement"></param>
        /// <returns></returns>
        public static T GetProxy<T>(this IProxyBinder<T> binder, object implement) => binder.GetProxy(new ObjectDynamicInvoker(implement));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="implements"></param>
        /// <returns></returns>
        public static T GetProxy<T>(this IProxyBinder<T> binder, IEnumerable<KeyValuePair<MethodInfo, Delegate>> implements) => binder.GetProxy(new DelegateDynamicInvoker(implements));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="implements"></param>
        /// <returns></returns>
        public static T GetProxy<T>(this IProxyBinder<T> binder, params KeyValuePair<MethodInfo, Delegate>[] implements) => binder.GetProxy(new DelegateDynamicInvoker(implements));
    }
}