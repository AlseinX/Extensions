using Xunit;
using Alsein.Utilities;
using Alsein.Utilities.Modulization;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Alsein.Utilities.LifetimeAnnotations;

namespace Alsein.Utilities.Test.TypeScanning
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