using ConsolePlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Windows.Kernal32
{
    internal static class Kernal
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, ConsoleFlag mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out ConsoleFlag mode);
        [DllImport("kernel32")]
        public static extern bool SetConsoleIcon(IntPtr hIcon);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);
    }
}
