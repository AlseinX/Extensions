using Alsein.Utilities.Runtime.InteropServices;
using System;
using static Alsein.Utilities.Curses.Internal.LibCurses;

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
                P<scrollok>.Invoke(_window, _allowScroll);
                return _allowScroll;
            }

            set
            {
                P<scrollok>.Invoke(_window, value);
                _allowScroll = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetChar() => P<wgetch>.Invoke(_window);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public void Move(int y, int x) => P<wmove>.Invoke(_window, y, x);

        /// <summary>
        /// 
        /// </summary>
        public void Refresh() => P<wrefresh>.Invoke(_window);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(string value) =>
            P<wprintw>.Invoke(_window, "%s", value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        public void Write<TValue>(TValue value) =>
            P<wprintw>.Invoke(_window, "%s", value.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public void Write(string format, params object[] arg) =>
            P<wprintw>.Invoke(_window, "%s", string.Format(format, arg));

        /// <summary>
        /// 
        /// </summary>
        public void WriteLine() =>
            P<wprintw>.Invoke(_window, "%s", "\n");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteLine(string value) =>
            P<wprintw>.Invoke(_window, "%s\n", value);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        public void WriteLine<TValue>(TValue value) =>
            P<wprintw>.Invoke(_window, "%s\n", value.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        public void WriteLine(string format, params object[] arg) =>
            P<wprintw>.Invoke(_window, "%s\n", string.Format(format, arg));
    }
}
