using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal class NestingContainer : INestingContainer, IServiceScope
    {
        private ServiceProvider _provider;

        private readonly NestingContainer _parent;

        private readonly IServiceScope _parentScope;

        private readonly INestedContainerFactory _nestedContainerFactory;

        public NestingContainer(IEnumerable<ServiceDescriptor> services, NestingContainer parent = null, IServiceScope parentScope = null)
        {
            _provider = services == null ? null : HookServices(services).BuildServiceProvider();
            _parent = parent;
            _parentScope = parentScope;
            _nestedContainerFactory = new NestedContainerFactory(this);
        }

        private IServiceCollection HookServices(IEnumerable<ServiceDescriptor> services)
        {
            IServiceCollection newServices = new ServiceCollection();
            foreach (var descriptor in services)
            {
                if (descriptor.ImplementationType != null)
                {
                    if (!descriptor.ImplementationType.IsConstructedGenericType)
                    {
                    }
                    newServices.Add(new ServiceDescriptor(descriptor.ServiceType, provider => ActivatorUtilities.CreateInstance(this, descriptor.ImplementationType)
                    , descriptor.Lifetime));
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    newServices.Add(new ServiceDescriptor(descriptor.ServiceType, provider => descriptor.ImplementationFactory(this), descriptor.Lifetime));
                }
                else
                {
                    newServices.Add(descriptor);
                }
            }
            return newServices;
        }

        private object GetService(Type serviceType, IServiceScope scope)
        {
            if (serviceType == typeof(IServiceScopeFactory) || serviceType == typeof(INestedContainerFactory))
            {
                return _nestedContainerFactory;
            }

            if (serviceType.IsGenericType && !serviceType.IsGenericTypeDefinition && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return _getServicesList.MakeGenericMethod(serviceType.GenericTypeArguments[0]).Invoke(this, new object[] { scope });
            }

            var result = (scope?.ServiceProvider ?? _provider)?.GetService(serviceType);
            return result ?? _parent?.GetService(serviceType, _parentScope);
        }

        private readonly MethodInfo _getServicesList = typeof(NestingContainer).GetMethod(nameof(GetServicesList), BindingFlags.Instance | BindingFlags.NonPublic);

        private IEnumerable<TService> GetServicesList<TService>(IServiceScope scope) => GetServices<TService>(scope);

        private IEnumerable<TService> GetServices<TService>(IServiceScope scope)
        {
            var results = (scope?.ServiceProvider ?? _provider)?.GetServices<TService>() ?? Enumerable.Empty<TService>();
            foreach (var result in results)
            {
                yield return result;
            }
            results = _parent?.GetServices<TService>(scope) ?? Enumerable.Empty<TService>();
            foreach (var result in results)
            {
                yield return result;
            }
        }

        public object GetService(Type serviceType) => GetService(serviceType, null);

        private class NestedContainerFactory : INestedContainerFactory, IServiceScopeFactory
        {
            private NestingContainer _target;

            public NestedContainerFactory(NestingContainer target) => _target = target;

            public IServiceScope CreateScope() => new NestingContainer(null, _target, _target._provider.CreateScope());

            public INestingContainer CreateNestedContainer(IEnumerable<ServiceDescriptor> services, bool asScope) => new NestingContainer(services, _target, asScope ? _target._provider.CreateScope() : null);
        }

        IServiceProvider IServiceScope.ServiceProvider => this;

        public void Dispose()
        {
            _provider?.Dispose();
            _parentScope?.Dispose();
        }
    }
}