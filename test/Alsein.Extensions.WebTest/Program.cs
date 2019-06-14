﻿using Alsein.Extensions.DependencyInjection;
using Alsein.Extensions.Modulization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Alsein.Extensions.WebTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddAssemblyManager(a => a.WithEntryAssembly(Assembly.GetExecutingAssembly()));
            var container = services.BuildNestingContainer();
            CreateWebHostBuilder(args).ConfigureServices((s) =>
            {
                s.AddSingleton(container);
            }).UseStartup<Startup>().Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args);
    }
}