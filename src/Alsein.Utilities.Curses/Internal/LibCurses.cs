using System;

namespace Alsein.Utilities.Curses.Internal
{
    internal class LibCurses
    {
        public delegate void add_wch(ref AttrChar wch);
        public delegate void endwin();
        public delegate int getch();
        public delegate IntPtr initscr();
        public delegate void move(int y, int x);
        public delegate IntPtr newwin(int nlines, int ncols, int y0, int x0);
        public delegate void printw(string fmt, string arg0);
        public delegate void refresh();
        public delegate void scrollok(IntPtr win, bool bf);
        public delegate int wgetch(IntPtr win);
        public delegate void wmove(IntPtr win, int y, int x);
        public delegate void wprintw(IntPtr win, string fmt, string arg0);
        public delegate void wrefresh(IntPtr win);
    }
}
