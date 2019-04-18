#if NETCOREAPP3_0

using Alsein.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Alsein.Extensions.AspNetCore.Internal
{
    internal class StartupLoader<TStartup>
        : IServiceProviderFactory<IServiceProvider>
        where TStartup : class, IStartup
    {
        private Func<HostBuilderContext, IServiceProvider> _hostServiceProviderFactory;
        private TStartup _startup;

        public StartupLoader(Func<HostBuilderContext, IServiceProvider> hostServiceProviderFactory) =>
            _hostServiceProviderFactory = hostServiceProviderFactory;

        public void Apply(IHostBuilder hostBuilder) =>
            hostBuilder
                .ConfigureServices((context, services) =>
                {
                    _startup = ActivatorUtilities.CreateInstance<TStartup>(_hostServiceProviderFactory(context));
                })
                .UseServiceProviderFactory(this)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseStartup<FakeStartup>().Configure(app => _startup.Configure(app)));

        IServiceProvider IServiceProviderFactory<IServiceProvider>.CreateBuilder(IServiceCollection services) =>
            _startup.ConfigureServices(services);

        IServiceProvider IServiceProviderFactory<IServiceProvider>.CreateServiceProvider(IServiceProvider containerBuilder) =>
            containerBuilder;

        private class FakeStartup
        {
            public FakeStartup() { }

            public void ConfigureServices(IServiceCollection services) { }

            public void Configure(IApplicationBuilder app) { }
        }
    }
}

#endif
