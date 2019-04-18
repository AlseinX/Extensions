using Xunit;
using Alsein.Extensions;
using Alsein.Extensions.Modulization;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Alsein.Extensions.LifetimeAnnotations;

namespace Alsein.Extensions.Test.TypeScanning
{
    public class Tester
    {
        [Fact]
        public void TypeScanningTest()
        {
            var manager = AssemblyManagerBuilder.CreateDefault().WithProjectAssemblyFilter(asm => asm.IsSharingRootNamespace(GetType().Assembly)).Build();
            manager.LoadExternalAssemblies();
            var services = new ServiceCollection();
            manager.AddServicesTo(services);
            var container = services.BuildServiceProvider();
            var a = container.GetRequiredService<ITestService>();
            var b = container.GetRequiredService<TestService>();
            a.Name += 2;
        }
    }

    public interface ITestService
    {
        string Name { get; set; }
    }

    [Singleton]
    public class TestService : ITestService
    {
        public string Name { get; set; } = nameof(TestService);
    }
}