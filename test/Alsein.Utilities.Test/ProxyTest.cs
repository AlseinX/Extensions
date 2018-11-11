using System;
using System.Reflection;
using Alsein.Utilities.Runtime;
using Xunit;

namespace Alsein.Utilities.Test
{
    public interface IProxyTest
    {
        int Value { get; }

        (int x, int y) Rebel((int x, int y) value);

        string Hello(string name);

        bool Yeah { get; }
    }

    public class ProxyTest
    {
        public int Value => 23;

        public (int x, int y) Rebel((int x, int y) value) => (value.y, value.x);

        public string Hello(string name) => $"Hello, {name}!";
    }

    public class ProxyTestRunner
    {
        [Fact]
        public void Test()
        {
            var a = new ProxyTest();
            var b = Proxy.CreateProxyBinder<IProxyTest>().GetProxy(a);
            Assert.Equal(23, b.Value);
            Assert.Equal((3, 2), b.Rebel((2, 3)));
            Assert.Equal("Hello, Adam!", b.Hello("Adam"));
            Assert.Throws<NotImplementedException>(() =>
            {
                _ = b.Yeah;
            });
        }
    }
}