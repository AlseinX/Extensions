using System;

namespace Alsein.Utilities.Curses.Internal
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning disable IDE1006
    public interface ILibCurses
    {
        void add_wch(ref AttrChar wch);
        void endwin();
        int getch();
        void initscr();
        void move(int y, int x);
        IntPtr newwin(int nlines, int ncols, int y0, int x0);
        void printw(string fmt);
        void printw(string fmt, string arg0);
        void refresh();
        void scrollok(IntPtr win, bool bf);
        IntPtr stdscr { get; set; }
        int wgetch(IntPtr win);
        void wmove(IntPtr win, int y, int x);
        void wprintw(IntPtr win, string fmt);
        void wprintw(IntPtr win, string fmt, string arg0);
        void wrefresh(IntPtr win);
    }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning restore IDE1006
}
