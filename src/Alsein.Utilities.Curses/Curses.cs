using Alsein.Utilities.Curses.Internal;
using Alsein.Utilities.Runtime.InteropServices;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using static Alsein.Utilities.Curses.Internal.LibC;
using static Alsein.Utilities.Curses.Internal.LibCurses;

namespace Alsein.Utilities.Curses
{
    /// <summary>
    /// 
    /// </summary>
    public static class Curses
    {
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

            P.LoadModule<LibCurses>(pathCurses).Disposing += (s, e) => P<endwin>.Invoke();
            P.LoadModule<LibC>(pathC);

            P<setlocale>.Invoke(0, "");

            StandardScreen = new Window(P<initscr>.Invoke());
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
            new Window(P<newwin>.Invoke(height, width, y, x));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Window CreateWindow(Point location, Size size) =>
            new Window(P<newwin>.Invoke(size.Height, size.Width, location.Y, location.X));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Window CreateWindow(Rectangle rect) =>
            new Window(P<newwin>.Invoke(rect.Height, rect.Width, rect.Y, rect.X));


        /// <summary>
        /// 
        /// </summary>
        public static Window StandardScreen { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public static void Move(int x, int y) => P<move>.Invoke(y, x);

        /// <summary>
        /// 
        /// </summary>
        public static int GetChar() => P<getch>.Invoke();

        /// <summary>
        /// 
        /// </summary>
        public static void Refresh() => P<refresh>.Invoke();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void Write(string value) =>
            P<printw>.Invoke("%s", value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        public static void Write<TValue>(TValue value) =>
            P<printw>.Invoke("%s", value.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void Write(string format, params object[] arg) =>
            P<printw>.Invoke("%s", string.Format(format, arg));

        /// <summary>
        /// 
        /// </summary>
        public static void WriteLine() =>
            P<printw>.Invoke("%s", "\n");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void WriteLine(string value) =>
            P<printw>.Invoke("%s\n", value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        public static void WriteLine<TValue>(TValue value) =>
            P<printw>.Invoke("%s\n", value.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public static void WriteLine(string format, params object[] arg) =>
            P<printw>.Invoke("%s\n", string.Format(format, arg));
    }
}
