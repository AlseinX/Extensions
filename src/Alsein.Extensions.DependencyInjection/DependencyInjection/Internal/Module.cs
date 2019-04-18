using System;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal class Module : IModule
    {
        public bool TryResolve(IResolver resolver, object key, out object result)
        {
            throw new System.NotImplementedException();
        }
    }
}