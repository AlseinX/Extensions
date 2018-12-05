using Microsoft.Extensions.DependencyInjection;
using System;

namespace Alsein.Utilities.DependencyInjection.Internal
{
    internal class NestingContainerFactory : IServiceProviderFactory<IServiceCollection>
    {
        public IServiceCollection CreateBuilder(IServiceCollection services) => services;

        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) => new NestingContainer(containerBuilder);
    }
}