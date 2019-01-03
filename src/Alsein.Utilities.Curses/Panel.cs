using Alsein.Utilities.Runtime.InteropServices;
using System;
using System.Drawing;

using static Alsein.Utilities.Curses.Internal.LibCurses;
using static Alsein.Utilities.Curses.Internal.LibPanel;

namespace Alsein.Utilities.Curses
{
    /// <summary>
    /// 
    /// </summary>
    public class Panel : Window
    {
        private readonly IntPtr _panel;

        private int _x;

        private int _y;

        private readonly int _width;

        private readonly int _height;

        internal Panel(int x, int y, int width, int height) : this(P<newwin>.Invoke(height, width, x, y))
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        private Panel(IntPtr window) : base(window) => _panel = P<new_panel>.Invoke(window);

        internal override void Refresh()
        {
            P<update_panels>.Invoke();
            Curses.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public Point Location
        {
            get => new Point(_x, _y);
            set
            {
                P<move_panel>.Invoke(_panel, _y, _x);
                Refresh();
                _x = value.X;
                _y = value.Y;
            }
        }
    }
}
