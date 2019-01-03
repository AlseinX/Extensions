using System;

namespace Alsein.Utilities.Curses.Internal
{
    internal class LibCurses
    {
        public delegate void doupdate();
        public delegate void endwin();
        public delegate IntPtr initscr();
        public delegate IntPtr newwin(int nlines, int ncols, int y0, int x0);
        public delegate void scrollok(IntPtr win, bool bf);
        public delegate int wgetch(IntPtr win);
        public delegate void wmove(IntPtr win, int y, int x);
        public delegate int wnoutrefresh(IntPtr win);
        public delegate void waddstr(IntPtr win, string wstr);
    }
}
