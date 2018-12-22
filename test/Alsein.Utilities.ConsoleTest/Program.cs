using Alsein.Utilities.Extensions;
using static Alsein.Utilities.Curses.Curses;

namespace Alsein.Utilities.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var win = CreateWindow(2, 2, 20, 10);
            win.WriteLine("我爱你");
            win.Refresh();
            win.GetChar();
        }
    }
}
