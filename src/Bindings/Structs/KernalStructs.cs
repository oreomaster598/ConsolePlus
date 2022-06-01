using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Windows.Kernal32
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FontInfo
    {
        internal int cbSize;
        internal int FontIndex;
        internal short FontWidth;
        /// <summary>
        /// 
        /// </summary>
        public short FontSize;
        /// <summary>
        /// 
        /// </summary>
        public int FontFamily;
        /// <summary>
        /// 
        /// </summary>
        public int FontWeight;
        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FontName;
    }
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        internal short X;
        internal short Y;

        internal COORD(short x, short y)
        {
            X = x;
            Y = y;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SMALL_RECT
    {
        internal short Left;
        internal short Top;
        internal short Right;
        internal short Bottom;
    }
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct COLORREF
    {
        internal COLORREF(byte r, byte g, byte b)
        {
            this.Value = 0;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        internal COLORREF(uint value)
        {
            this.R = 0;
            this.G = 0;
            this.B = 0;
            this.Value = value & 0x00FFFFFF;
        }

        [FieldOffset(0)]
        internal byte R;
        [FieldOffset(1)]
        internal byte G;
        [FieldOffset(2)]
        internal byte B;

        [FieldOffset(0)]
        internal uint Value;
    }
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_SCREEN_BUFFER_INFO_EX
    {
        internal int cbSize;
        internal COORD dwSize;
        internal COORD dwCursorPosition;
        internal ushort wAttributes;
        internal SMALL_RECT srWindow;
        internal COORD dwMaximumWindowSize;
        internal ushort wPopupAttributes;
        internal bool bFullscreenSupported;
        internal COLORREF black;
        internal COLORREF darkBlue;
        internal COLORREF darkGreen;
        internal COLORREF darkCyan;
        internal COLORREF darkRed;
        internal COLORREF darkMagenta;
        internal COLORREF darkYellow;
        internal COLORREF gray;
        internal COLORREF darkGray;
        internal COLORREF blue;
        internal COLORREF green;
        internal COLORREF cyan;
        internal COLORREF red;
        internal COLORREF magenta;
        internal COLORREF yellow;
        internal COLORREF white;
    }
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CONSOLE_FONT_INFO_EX
    {
        internal uint cbSize;
        internal uint nFont;
        internal COORD dwFontSize;
        internal int FontFamily;
        internal int FontWeight;
        internal fixed char FaceName[32];
    }
}
