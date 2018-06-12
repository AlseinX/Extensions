using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Alsein.Utilities.Test
{
    public class UtilitiesTest
    {
        [Fact]
        public void Message() => Console.WriteLine("Testing for Alsein.Utilities");

        [Theory]
        [InlineData("../../../../../LICENSE", "MIT License")]
        public void UsingTest(string path, string result) => Assert.Equal
        (path
            .Using(() => File.OpenRead(path))
            .Using(stream => new StreamReader(stream))
            .Return(reader => reader.ReadLine()), result
        );

        [Theory]
        [InlineData(2, 2, 1, new[] { 2 })]
        [InlineData(1, 5, 1, new[] { 1, 2, 3, 4, 5 })]
        [InlineData(6, 0, 1, new[] { 6, 5, 4, 3, 2, 1, 0 })]
        [InlineData(1, 10, 2, new[] { 1, 3, 5, 7, 9 })]
        [InlineData(10, 1, 2, new[] { 10, 8, 6, 4, 2 })]
        [InlineData(10, 1, -2, new[] { 10, 8, 6, 4, 2 })]
        public void ToTest(int form, int to, int step, IEnumerable<int> result)
        {
            Assert.Equal(result, form.To(to, step));
        }

        [Fact]
        public void AssemblyTest()
        {
            var e = AssemblyLoader.LoadAssemblies(Assembly.GetExecutingAssembly(), true)
                .Where(AssemblyLoader.IsSharingRootName[Assembly.GetExecutingAssembly()]).ToArray();
        }

        public interface ITest
        {
            void Out(Action<int> action);
            ITest Mul(int value);
        }
        public class Test : ITest
        {
            private int _value;

            public Test(int value) => _value = value;

            public ITest Mul(int value) => new Test(_value * value);

            public void Out(Action<int> action) => action(_value);
        }
    }
}
