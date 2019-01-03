using Alsein.Utilities.Runtime.InteropServices;
using System;

using static Alsein.Utilities.Curses.Internal.LibCurses;

namespace Alsein.Utilities.Curses
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Window : IStringWriter
    {
        private protected readonly IntPtr _window;

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
        internal abstract void Refresh();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(string value)
        {
            P<waddstr>.Invoke(_window, value);
            Refresh();
        }
    }
}
