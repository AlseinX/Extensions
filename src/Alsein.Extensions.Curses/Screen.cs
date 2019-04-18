using Alsein.Extensions.Runtime.InteropServices;
using System;
using static Alsein.Extensions.Curses.Internal.LibCurses;

namespace Alsein.Extensions.Curses
{
    /// <summary>
    /// 
    /// </summary>
    public class Screen : Window
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public Screen(IntPtr window) : base(window) { }

        internal override void Refresh()
        {
            P<wnoutrefresh>.Invoke(_window);
            Curses.Update();
        }
    }
}
