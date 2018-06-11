using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Alsein.Utilities.Runtime
{
    internal class InterfaceProxyBinder<T> : InterfaceProxyBinder, IProxyBinder<T>
    {
        public override Type Target => typeof(T);

        public T GetProxy()
        {
            return (T)Activator.CreateInstance(_implement, this);
        }

        public InterfaceProxyBinder(IEnumerable<KeyValuePair<MethodInfo, Delegate>> implements) : base(implements) { }
    }
}