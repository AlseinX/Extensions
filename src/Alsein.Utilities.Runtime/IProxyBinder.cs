using System;

namespace Alsein.Utilities.Runtime
{
    public interface IProxyBinder
    {
        Type Target { get; }

        object GetProxy();
    }
}