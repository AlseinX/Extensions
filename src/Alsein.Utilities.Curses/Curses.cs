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
        private const string _linuxLibPath = "/usr/lib";

        private static bool _updating = true;

        static Curses()
        {
            var (c, curses, panel) =
            (
                (
                    windows: "msvcrt.dll",
                    linux: "libc.so.6"
                ),
                (
                    windows: "libncursesw6.dll",
                    linux: "libncursesw.so.6"
                ),
                (
                    windows: "libpanelw6.dll",
                    linux: "libpanelw.so.6"
                )
            );

            var pathCurses = default(string);
            var pathPanel = default(string);
            var pathC = default(string);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                pathC = c.windows;
                pathCurses = curses.windows;
                pathPanel = panel.windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                string findPath(string name) =>
                    Directory.EnumerateFiles(_linuxLibPath, name, SearchOption.AllDirectories).First();
                pathC = findPath(c.linux);
                pathCurses = findPath(curses.linux);
                pathPanel = findPath(panel.linux);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            P.LoadModule<LibCurses>(pathCurses).Disposing += (s, e) => P<endwin>.Invoke();
            P.LoadModule<LibPanel>(pathPanel);
            P.LoadModule<LibC>(pathC);

            P<setlocale>.Invoke(0, "");

            Screen = new Screen(P<initscr>.Invoke());
        }

        internal static void Update()
        {
            if (_updating)
            {
                P<doupdate>.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool Updating
        {
            get => _updating;
            set
            {
                _updating = value;
                Update();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Panel CreatePanel(int x, int y, int width, int height) =>
            new Panel(x, y, width, height);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Panel CreatePanel(Point location, Size size) =>
            new Panel(location.X, location.Y, size.Width, size.Height);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Panel CreatePanel(Rectangle rect) =>
            new Panel(rect.X, rect.Y, rect.Width, rect.Height);


        /// <summary>
        /// 
        /// </summary>
        public static Screen Screen { get; }
    }
}
