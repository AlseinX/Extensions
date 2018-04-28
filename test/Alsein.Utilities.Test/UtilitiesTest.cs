using System;
using Xunit;
using Alsein.Utilities;
using System.IO;
using System.Collections.Generic;

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
        [InlineData(1, 5, new[] { 1, 2, 3, 4 })]
        public void ToTest(int form, int to, IEnumerable<int> result)
        {
            Assert.Equal(result, form.To(to));
        }
    }
}
