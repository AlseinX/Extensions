using Alsein.Utilities.Runtime;
using System;
using System.Reflection;
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

        void Run();

        void Add(ref int a);

        ref int GetFirst(int[] arr);

    }

    public class ProxyTest<T> : IProxyInvoker where T : struct
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

        public IArguments Invoke(MethodInfo method, IArguments args)
        {
            if (method.Name == "Add")
            {
                args.At<int>(0)++;
            }
            return null;
        }

        public ref int GetFirst(int[] arr) => ref arr[0];
    }

    public class ProxyTestRunner
    {
        [Fact]
        public void Test()
        {
            var args = new object[] { 23, null };
            var b = typeof(ProxyTest<>).GetImplementationOf(typeof(IProxyTest<>)).MakeGenericType<int>().New<IProxyTest<int>>(args);
            b.Run();
            var x = 1;
            b.Add(ref x);
            var y = new[] { 5 };
            b.GetFirst(y) = 6;
            Assert.Equal(2, x);
            Assert.Equal(new[] { 6 }, y);
            Assert.Equal("bbb", args[1]);
            Assert.Equal((3, 2), b.Rebel((2, 3)));
            Assert.Equal(23, b.Value);
            Assert.Equal("Hello, Adam!", b.Hello("Adam"));
            Assert.Equal("String", b.GetName<string>());
            Assert.Throws<NullReferenceException>(() =>
            {
                _ = b.Yeah;
            });
        }
    }
}