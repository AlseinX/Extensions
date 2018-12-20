using Alsein.Utilities.Extensions;
using static Alsein.Utilities.Curses.Curses;

namespace Alsein.Utilities.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var win = CreateWindow(0, 0, 20, 10);
            WriteLine("我爱你");
            Refresh();
            GetChar();
        }
    }
}
