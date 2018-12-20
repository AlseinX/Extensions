using Alsein.Utilities.Curses.Internal;
using Alsein.Utilities.Runtime.InteropServices;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Alsein.Utilities.Curses
{
    /// <summary>
    /// 
    /// </summary>
    public static class Curses
    {
        internal static ILibCurses LibCurses { get; }
        private static ILibC LibC { get; }

        static Curses()
        {
            var pathCurses = default(string);
            var pathC = default(string);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                pathCurses = "libncursesw6.dll";
                pathC = "msvcrt.dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                pathCurses = Directory.EnumerateFiles("/lib", "libncursesw.so.6", SearchOption.AllDirectories).First();
                pathC = Directory.EnumerateFiles("/lib", "libc.so.6", SearchOption.AllDirectories).First();
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            LibCurses = NativeModule.LoadModule<ILibCurses>(new NativeModuleAttribute
            {
                Path = pathCurses,
                CharSet = CharSet.Ansi
            });

            LibC = NativeModule.LoadModule<ILibC>(new NativeModuleAttribute
            {
                Path = pathC,
                CharSet = CharSet.Unicode
            });

            LibC.setlocale(0, "");

            NativeModule.Parse(LibCurses).Disposing += (s, e) => ((ILibCurses)s).endwin();
            LibCurses.initscr();

            StandardScreen = new Window(LibCurses.stdscr);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Window CreateWindow(int x, int y, int width, int height) =>
            new Window(LibCurses.newwin(height, width, y, x));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Window CreateWindow(Point location, Size size) =>
            new Window(LibCurses.newwin(size.Height, size.Width, location.Y, location.X));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Window CreateWindow(Rectangle rect) =>
            new Window(LibCurses.newwin(rect.Height, rect.Width, rect.Y, rect.X));


        /// <summary>
        /// 
        /// </summary>
        public static Window StandardScreen { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public static void Move(int x, int y) => LibCurses.move(y, x);

        /// <summary>
        /// 
        /// </summary>
        public static int GetChar() => LibCurses.getch();

        /// <summary>
        /// 
        /// </summary>
        public static void Refresh() => LibCurses.refresh();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void Write(string value) =>
            LibCurses.printw("%s", value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        public static void Write<TValue>(TValue value) =>
            LibCurses.printw("%s", value.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void Write(string format, params object[] arg) =>
            LibCurses.printw("%s", string.Format(format, arg));

        /// <summary>
        /// 
        /// </summary>
        public static void WriteLine() =>
            LibCurses.printw("\n");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void WriteLine(string value) =>
            LibCurses.printw("%s\n", value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        public static void WriteLine<TValue>(TValue value) =>
            LibCurses.printw("%s\n", value.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void WriteLine(string format, params object[] arg) =>
            LibCurses.printw("%s\n", string.Format(format, arg));
    }
}
