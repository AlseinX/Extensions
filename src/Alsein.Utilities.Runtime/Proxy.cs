using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alsein.Utilities.Runtime
{
    public static class Proxy
    {
        public static IProxyBinder<TInterface> GetProxyBinder<TInterface>(IEnumerable<KeyValuePair<MethodInfo, Delegate>> implements) =>
            new InterfaceProxyBinder<TInterface>(implements);


        public static IProxyBinder<TInterface> GetProxyBinder<TInterface>(params (MethodInfo Key, Delegate Value)[] implements) =>
            new InterfaceProxyBinder<TInterface>(implements.Select(imp => new KeyValuePair<MethodInfo, Delegate>(imp.Key, imp.Value)));
    }
}