using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal class OpenGenericTypeProxy<TService> : DispatchProxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args) => targetMethod.Invoke(_service, args);

        private readonly TService _service;

        public OpenGenericTypeProxy(INestingContainer container) => _service = ActivatorUtilities.CreateInstance<TService>(container);
    }
}
