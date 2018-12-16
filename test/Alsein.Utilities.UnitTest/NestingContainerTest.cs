using Alsein.Utilities.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Alsein.Utilities.Test
{
    internal class A
    {
        private B _b;
        private C _c;
        public string Name => $"{_b.Name} {_c.Name}";
        public A(B b, C c) => (_b, _c) = (b, c);
    }

    internal class B
    {
        public string Name { get; }
        public B(string name) => Name = name;
    }

    internal class C
    {
        public string Name { get; }
        public C(string name) => Name = name;
    }

    public class NestingContainerTest
    {
        [Fact]
        public void TestName()
        {
            var services1 = new ServiceCollection();
            services1.AddSingleton(new B("X"));
            services1.AddSingleton(new C("Y"));
            var services2 = new ServiceCollection();
            services2.AddTransient<A>();
            services2.AddSingleton(new B("Z"));
            var container1 = services1.BuildNestingContainer();
            var container2 = container1.CreateNestedContainer(services2, true);
            var result = container2.GetRequiredService<A>().Name;
            Assert.Equal("Z Y", result);
            var result2 = container2.GetServices<B>().Select(x => x.Name);
            Assert.Equal(new[] { "Z", "X" }, result2);
        }
    }
}