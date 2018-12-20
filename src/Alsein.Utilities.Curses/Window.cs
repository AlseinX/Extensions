using System;
using static Alsein.Utilities.Curses.Curses;

namespace Alsein.Utilities.Curses
{
    /// <summary>
    /// 
    /// </summary>
    public class Window
    {
        private readonly IntPtr _window;
        private bool _allowScroll = false;

        internal Window(IntPtr window) => _window = window;

        /// <summary>
        /// 
        /// </summary>
        public bool AllowScroll
        {
            get
            {
                LibCurses.scrollok(_window, _allowScroll);
                return _allowScroll;
            }

            set
            {
                LibCurses.scrollok(_window, value);
                _allowScroll = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetChar() => LibCurses.wgetch(_window);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public void Move(int y, int x) => LibCurses.wmove(_window, y, x);

        /// <summary>
        /// 
        /// </summary>
        public void Refresh() => LibCurses.wrefresh(_window);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(string value) =>
            LibCurses.wprintw(_window, "%s", value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        public void Write<TValue>(TValue value) =>
            LibCurses.wprintw(_window, "%s", value.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public void Write(string format, params object[] arg) =>
            LibCurses.wprintw(_window, "%s", string.Format(format, arg));

        /// <summary>
        /// 
        /// </summary>
        public void WriteLine() =>
            LibCurses.wprintw(_window, "\n");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteLine(string value) =>
            LibCurses.wprintw(_window, "%s\n", value);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        public void WriteLine<TValue>(TValue value) =>
            LibCurses.wprintw(_window, "%s\n", value.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public void WriteLine(string format, params object[] arg) =>
            LibCurses.wprintw(_window, "%s\n", string.Format(format, arg));
    }
}
