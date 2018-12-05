using Alsein.Utilities.DependencyInjection;
using Alsein.Utilities.Modulization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Alsein.Utilities.WebTest
{
    public class Startup : IStartup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment, INestingContainer globalProvider)
        {
            Configuration = configuration;
            GlobalContainer = globalProvider;
            HostingEnvironment = hostingEnvironment;
        }

        public IServiceProvider GlobalContainer { get; }

        public IServiceProvider ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            var manager = GlobalContainer.GetRequiredService<IAssemblyManager>();
            manager.LoadExternalAssemblies();
            services.AddMvc().AddAssemblyManager(manager).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();
            services.AddNestingContainer();
            return ApplicationContainer = GlobalContainer.CreateNestedContainer(services, false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSignalRWithHubs();
        }
    }
}
