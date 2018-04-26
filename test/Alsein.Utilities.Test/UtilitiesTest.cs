using System;
using Xunit;
using Alsein.Utilities;
using System.IO;

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
    }
}
