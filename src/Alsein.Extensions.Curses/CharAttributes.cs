using System;

namespace Alsein.Extensions.Curses
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum CharAttributes : uint
    {
        /// <summary>
        /// 
        /// </summary>
        Normal = 0x00000000,

        /// <summary>
        /// 
        /// </summary>
        Standout = 0x00010000,

        /// <summary>
        /// 
        /// </summary>
        Underline = 0x00020000,

        /// <summary>
        /// 
        /// </summary>
        Reverse = 0x00040000,

        /// <summary>
        /// 
        /// </summary>
        Blink = 0x00080000,

        /// <summary>
        /// 
        /// </summary>
        Dim = 0x00100000,

        /// <summary>
        /// 
        /// </summary>
        Bold = 0x00200000,

        /// <summary>
        /// 
        /// </summary>
        AlternateCharSet = 0x00400000,

        /// <summary>
        /// 
        /// </summary>
        Invisible = 0x00800000,

        /// <summary>
        /// 
        /// </summary>
        Protect = 0x01000000,

        /// <summary>
        /// 
        /// </summary>
        Horizontal = 0x02000000,

        /// <summary>
        /// 
        /// </summary>
        Low = 0x08000000,

        /// <summary>
        /// 
        /// </summary>
        Right = 0x10000000,

        /// <summary>
        /// 
        /// </summary>
        Top = 0x20000000,

        /// <summary>
        /// 
        /// </summary>
        Vertical = 0x40000000,

        /// <summary>
        /// 
        /// </summary>
        Italic = 0x80000000,
    }
}