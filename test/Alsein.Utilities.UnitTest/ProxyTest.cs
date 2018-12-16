using Alsein.Utilities.Runtime;
using System;
using Xunit;

namespace Alsein.Utilities.Test
{
    public interface IProxyTest<T> where T : struct
    {
        T Value { get; }

        (int x, int y) Rebel((int x, int y) value);

        string Hello(string name);

        string GetName<T2>();

        bool Yeah { get; }
    }

    public class ProxyTest<T> where T : struct
    {
        public ProxyTest(T value, out string aaa)
        {
            Value = value;
            aaa = "bbb";
        }

        public T Value { get; }

        public (int x, int y) Rebel((int x, int y) value) => (value.y, value.x);

        public string Hello(string name) => $"Hello, {name}!";

        public string GetName<TT>() => typeof(TT).Name;
    }

    public class ProxyTestRunner
    {
        [Fact]
        public void Test()
        {
            var args = new object[] { 23, null };
            var b = typeof(ProxyTest<>).GetImplementationOf(typeof(IProxyTest<>)).MakeGenericType<int>().New<IProxyTest<int>>(args);
            Assert.Equal("bbb", args[1]);
            Assert.Equal((3, 2), b.Rebel((2, 3)));
            Assert.Equal(23, b.Value);
            Assert.Equal("Hello, Adam!", b.Hello("Adam"));
            Assert.Equal("String", b.GetName<string>());
            Assert.Throws<NotImplementedException>(() =>
            {
                _ = b.Yeah;
            });
        }
    }
}