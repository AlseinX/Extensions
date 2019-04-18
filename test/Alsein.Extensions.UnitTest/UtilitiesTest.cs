using Alsein.Extensions.IO;
using Alsein.Extensions.Modulization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Alsein.Extensions.Test
{
    public class UtilitiesTest
    {
        [Fact]
        public void Message() => Console.WriteLine("Testing for Alsein.Extensions");

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
            var asms = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.StartsWith("Alsein")).ToArray();
            var manager = AssemblyManagerBuilder.CreateDefault().WithDirectories(o =>
            {
                o.Clear();
                o.Add(new AssemblyDirectory("../../../../../src/Alsein.Extensions.Essentials/bin/Debug/netstandard2.0/", false, x => true));
            }).Build();
            manager.LoadExternalAssemblies();
            var asms2 = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.StartsWith("Alsein")).ToArray();
        }

        [Fact]
        public void TypeScanTest()
        {

        }

        [Fact]
        public void IOTest()
        {
            var (endPoint1, endPoint2) = Tube.CreateDuplex();
            var record = new List<string>();
            endPoint1.Receive += async (value) => await Task.Run(() => record.Add("ep1 event receiped: " + value));
            endPoint2.Receive += async (value) => await Task.Run(() => record.Add("ep2 event receiped: " + value));
            async Task fun1()
            {
                record.Add("sending a via ep1");
                await endPoint1.SendAsync("a");
                await Task.Delay(500);
                record.Add("ep1 received: " + await endPoint1.ReceiveAsync<string>());
            }
            async Task fun2()
            {
                record.Add("sending x via ep2");
                await endPoint2.SendAsync("b");
                await Task.Delay(100);
                record.Add("ep2 received: " + await endPoint2.ReceiveAsync<string>());
            }
            var task1 = fun1();
            var task2 = fun2();
            Task.WaitAll(task1, task2);
            return;
        }

        [Fact]
        public async Task AsyncSelectTest()
        {
            var result = new List<int>();
            var a = new[] { 1, 2, 3, 4, 5 };
            var b = a.SelectAwait(async x =>
            {
                await Task.Delay(10);
                return x;
            });
            await b.ToListAsync();
        }
    }
}
