using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alsein.Utilities.Runtime.Internal
{
    internal class InterfaceProxyBinder<T> : InterfaceProxyBinder, IProxyBinder<T>
    {
        public new T GetProxy(IDynamicInvoker invoker)
        {
            return (T)base.GetProxy(invoker);
        }

        public InterfaceProxyBinder() : base(typeof(T)) { }
    }
}