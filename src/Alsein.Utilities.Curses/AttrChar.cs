using System.Runtime.InteropServices;

namespace Alsein.Utilities.Curses
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AttrChar
    {
        /// <summary>
        /// 
        /// </summary>
        public CharAttributes Attributes { get; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        private readonly int[] _chars;

        /// <summary>
        /// 
        /// </summary>
        public char Value => (char)(_chars?[0] ?? 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="attributes"></param>
        public AttrChar(char value, CharAttributes attributes = CharAttributes.Normal)
        {
            Attributes = attributes;
            _chars = new[] { value, 0, 0, 0, 0 };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator AttrChar(char value) => new AttrChar(value);
    }
}
