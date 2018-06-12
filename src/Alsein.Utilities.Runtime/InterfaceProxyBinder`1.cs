using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alsein.Utilities.Runtime
{
    internal class InterfaceProxyBinder<T> : InterfaceProxyBinder, IProxyBinder<T>
    {
        public new T GetProxy()
        {
            return (T)base.GetProxy();
        }

        public InterfaceProxyBinder(IEnumerable<KeyValuePair<MethodInfo, Delegate>> implements) : base(typeof(T), implements) { }
    }
}