using Alsein.Extensions.DependencyInjection;
using Xunit;

namespace Alsein.Extensions.UnitTest
{
    public class TestService
    {
        public int Value { get; set; } = 10;
    }

    public class Test2Service
    {
        [Injected]
        private TestService TestService { get; set; }

        public int Value => TestService.Value;
    }

    public class DITest
    {
        [Fact]
        public void Test()
        {
            var container = Container.Create();

            container.AddModule(module => module
                .Register(reg => reg.UseFactory(r => new TestService()))
                .Register(reg => reg.UseType<Test2Service>().To(1))
            );

            Assert.Equal(10, container.Resolve<Test2Service>(1).Value);
        }
    }
}