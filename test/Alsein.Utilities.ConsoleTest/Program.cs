using Alsein.Utilities.Curses;
using System;
using System.Drawing;

namespace Alsein.Utilities.ConsoleTest
{
    internal class Program
    {
        private delegate int ParseInt(string value);
        private static void Main(string[] args)
        {
            var panel = Curses.Curses.CreatePanel(2, 2, 20, 10);
            panel.WriteLine("abcde12345");
        x:
            panel.GetChar();
            panel.Location = new Point(panel.Location.X + 1, panel.Location.Y + 1);
            goto x;
        }
    }
}
