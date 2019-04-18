using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Alsein.Extensions.UnitTest
{
    public class ContexedTest
    {
        public class A : ContextedBase<A>
        {
            private string Value { get; set; }

            public string GetValue() => Value;
        }

        public class B : IServiceProvider
        {
            private readonly int _index;

            public B(int index) => _index = index;

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(string))
                {
                    return "Hello World!" + _index;
                }

                return null;
            }
        }
        [Fact]
        public void Test()
        {
            var result = 1.To(1000).AsParallel().Select(i =>
            {
                using var locker = A.WithContext(new B(i));
                var a = new A();
                return a.GetValue();
            }).Distinct().Count();

            Assert.Equal(1000, result);
        }
    }
}
