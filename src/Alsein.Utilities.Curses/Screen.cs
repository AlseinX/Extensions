using Alsein.Utilities.Runtime.InteropServices;
using System;
using static Alsein.Utilities.Curses.Internal.LibCurses;

namespace Alsein.Utilities.Curses
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
