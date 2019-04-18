using System;

namespace Alsein.Extensions.Curses.Internal
{
    internal class LibPanel
    {
        public delegate int move_panel(IntPtr pan, int starty, int startx);

        public delegate IntPtr new_panel(IntPtr win);

        public delegate int update_panels();
    }
}
