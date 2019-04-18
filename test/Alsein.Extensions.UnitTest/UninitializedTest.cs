using System;
using System.Runtime.Serialization;
using Xunit;

namespace Alsein.Extensions.UnitTest
{
    public class A
    {
        public int Value { get; set; }

        public A()
        {
            Value = 10;
        }
    }
    public class UninitializedTest
    {
        [Fact]
        public void Test()
        {
            var a = (A)FormatterServices.GetUninitializedObject(typeof(A));
            Assert.Equal(a.Value, default);
            typeof(A).GetConstructor(new Type[] { }).Invoke(a, new object[] { });
            Assert.Equal(a.Value, 10);
            var b = (string)FormatterServices.GetUninitializedObject(typeof(string));
            typeof(string).GetConstructor(new[] { typeof(char[]) }).Invoke(b, new object[] { "eee".ToCharArray() });
            Assert.Equal(b, "eee");
        }
    }
}