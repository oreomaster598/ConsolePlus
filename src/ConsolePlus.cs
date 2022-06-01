using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Windows.Kernal32;

namespace ConsolePlus
{
    /// <summary>
    /// Utils form changing fonts.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ConsoleFonts
    {
        internal static string[] ValidFonts = new string[] { "Courier New", "Lucida Console", "NSimSun", "Consolas", "Cascadia Mono", "Cascadia Code", "Unispace", "SimSun-ExtB", "Segoe UI Mono" };

        /// <summary>
        /// 
        /// </summary>
        public static readonly ConsoleFont CourierNew = new ConsoleFont("Courier New");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ConsoleFont LucidaConsole = new ConsoleFont("Lucida Console");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ConsoleFont NSimSun = new ConsoleFont("NSimSun");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ConsoleFont Consolas = new ConsoleFont("Consolas");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ConsoleFont CascadiaMono = new ConsoleFont("Cascadia Mono");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ConsoleFont CascadiaCode = new ConsoleFont("Cascadia Code");
        /// <summary>
        /// 
        /// </summary>
        //Raster Fonts
        public static readonly ConsoleFont Unispace = new ConsoleFont("Unispace");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ConsoleFont SimSunExtB = new ConsoleFont("SimSun-ExtB");
        /// <summary>
        /// 
        /// </summary>
        public static readonly ConsoleFont SegoeUIMono = new ConsoleFont("Segoe UI Mono");

        /// <summary>
        /// Checks if font is valid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidFont(string name) => ValidFonts.Contains(name);
        /// <summary>
        /// Gets font from name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A new console font.</returns>
        /// <exception cref="Exception"></exception>
        public static ConsoleFont FromName(string name)
        {
            if (!ValidFonts.Contains(name))
                throw new Exception($"Invalid font \"{name}\".");
            return new ConsoleFont(name);
        }
    }
    /// <summary>
    /// Contains data to change console font.
    /// </summary>
    public struct ConsoleFont
    {
        internal string name;
        internal ConsoleFont(string name)
        {
            this.name = name;
        }
    }
    internal static class ColorMapper
    {


       
        const int STD_OUTPUT_HANDLE = -11;                                        // per WinBase.h
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);    // per WinBase.h

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        // Set a specific console color to an RGB color
        // The default console colors used are gray (foreground) and black (background)

        static COLORREF[] DefaultMappings = new COLORREF[] { new COLORREF(0,0,0), new COLORREF(0,0,128), new COLORREF(0, 128, 0),
            new COLORREF(0, 128, 128), new COLORREF(128, 0, 0), new COLORREF(128, 0, 128), new COLORREF(128, 128, 0), new COLORREF(192, 192, 192),
            new COLORREF(128, 128, 128), new COLORREF(0, 0, 255), new COLORREF(0, 255, 0), new COLORREF(0, 255, 255), new COLORREF(255, 0, 0),
            new COLORREF(255, 0, 255), new COLORREF(255, 255, 0), new COLORREF(255, 255, 255), };
        static COLORREF[] Mappings = DefaultMappings;
        public static void MapColor(ConsoleColor target, Color color)
        {
            Mappings[(uint)target] = new COLORREF((byte)color.R, (byte)color.G, (byte)color.B);
        }

        public static int UpdateMappings(bool reset)
        {
            CONSOLE_SCREEN_BUFFER_INFO_EX csbe = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            csbe.cbSize = (int)Marshal.SizeOf(csbe);                    // 96 = 0x60
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            if (hConsoleOutput == INVALID_HANDLE_VALUE)
            {
                return Marshal.GetLastWin32Error();
            }
            bool brc = GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }
            if (reset)
                Mappings = DefaultMappings;
            csbe.black = Mappings[0];
            csbe.darkBlue = Mappings[1];
            csbe.darkGreen = Mappings[2];
            csbe.darkCyan = Mappings[3];
            csbe.darkRed = Mappings[4];
            csbe.darkMagenta = Mappings[5];
            csbe.darkYellow = Mappings[6];
            csbe.gray = Mappings[7];
            csbe.darkGray = Mappings[8];
            csbe.blue = Mappings[9];
            csbe.green = Mappings[10];
            csbe.cyan = Mappings[11];
            csbe.red = Mappings[12];
            csbe.magenta = Mappings[13];
            csbe.yellow = Mappings[14];
            csbe.white = Mappings[15];

            ++csbe.srWindow.Bottom;
            ++csbe.srWindow.Right;
            brc = SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }
            return 0;
        }
    }
    /// <summary>
    /// Flags to set console mode
    /// </summary>
    [Flags]
    public enum ConsoleFlag
    {
        /// <summary>
        /// 
        /// </summary>
        ENABLE_PROCESSED_OUTPUT = 0x0001,
        /// <summary>
        /// 
        /// </summary>
        ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
        /// <summary>
        /// 
        /// </summary>
        VIRTUAL_TERMINAL_PROCESSING = 0x0004,
        /// <summary>
        /// 
        /// </summary>
        DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
        /// <summary>
        /// 
        /// </summary>
        ENABLE_LVB_GRID_WORLDWIDE = 0x0010
    }
    internal class ConsoleRestorePoint
    {
        internal FontInfo font;
        internal ConsoleFlag mode;
    }
    #pragma warning disable CS8618
    /// <summary>
    /// 
    /// </summary>
    public static class Console
    {
        /// <summary>
        /// Changes output background color.
        /// </summary>
        public static Color Background
        {
            get => internBack;
            set
            {
                if (!ColorMapping)
                    System.Console.BackgroundColor = value.GetConsoleColor();
                else
                {
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    ColorMapper.MapColor(ConsoleColor.Black, value);
                    ColorMapper.UpdateMappings(false);
                }
                internBack = value;
            }
        }
        /// <summary>
        /// Chnages output foreground color.
        /// </summary>
        public static Color Foreground
        {
            get => internFore;
            set
            {
                if (!ColorMapping)
                    System.Console.ForegroundColor = value.GetConsoleColor();
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.White;
                    ColorMapper.MapColor(ConsoleColor.White, value);
                    ColorMapper.UpdateMappings(false);
                }
                internFore = value;
            }
        }

        internal static Color internFore = new Color(0xFFFFFF);
        internal static Color internBack = new Color(0x0C0C0C);
        //public static bool UseAlpha;
        /// <summary>
        /// If true, it remaps console colors specified by Foreground/Background properties.
        /// </summary>
        public static bool ColorMapping = false;
        internal static ConsoleRestorePoint restorePoint;
        /// <summary>
        /// Creates restore point.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void CreateRestorePoint()
        {
            IntPtr handle = Kernal.GetStdHandle(-11);

            restorePoint = new ConsoleRestorePoint();
            restorePoint.font = new FontInfo{ cbSize = Marshal.SizeOf<FontInfo>() };

            if(!Kernal.GetCurrentConsoleFontEx(handle, false, ref restorePoint.font))
                throw new Exception("Error creating restore point.");
            if(!Kernal.GetConsoleMode(handle, out restorePoint.mode))
                throw new Exception("Error creating restore point.");
        }
        /// <summary>
        /// Restores the console from restore point.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void Restore()
        {
            IntPtr handle = Kernal.GetStdHandle(-11);
            if (restorePoint != null)
            {
                Kernal.SetConsoleMode(handle, restorePoint.mode);
                Kernal.SetCurrentConsoleFontEx(handle, false, ref restorePoint.font);
                ColorMapper.UpdateMappings(true);
            }
            else
                throw new Exception("No restore point to revert to.");
        }
        /// <summary>
        /// Sets current font.
        /// </summary>
        /// <param name="font"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        public static FontInfo[] SetCurrentFont(ConsoleFont font, short fontSize = 0)
        {
            IntPtr handle = Kernal.GetStdHandle(-11);

            FontInfo before = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            if (Kernal.GetCurrentConsoleFontEx(handle, false, ref before))
            {

                FontInfo set = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = 54,
                    FontName = font.name,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // Get some settings from current font.
                if (!Kernal.SetCurrentConsoleFontEx(handle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    WriteLine("Set error " + ex);
                    throw new System.ComponentModel.Win32Exception(ex);
                }

                FontInfo after = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                Kernal.GetCurrentConsoleFontEx(handle, false, ref after);

                return new[] { before, set, after };
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                WriteLine("Get error " + er);
                throw new System.ComponentModel.Win32Exception(er);
            }
        }
        /// <summary>
        /// Resets console mode flags.
        /// </summary>
        public static void ResetConsoleFlags()
        {
            IntPtr handle = Kernal.GetStdHandle(-11);

            Kernal.SetConsoleMode(handle, (ConsoleFlag.ENABLE_PROCESSED_OUTPUT | ConsoleFlag.ENABLE_WRAP_AT_EOL_OUTPUT));
        }
        /// <summary>
        /// Sets console mode flags.
        /// </summary>
        /// <param name="flag"></param>
        public static void SetConsoleFlags(ConsoleFlag flag)
        {
            IntPtr handle = Kernal.GetStdHandle(-11);

            Kernal.SetConsoleMode(handle, flag);
        }
        /// <summary>
        /// Adds console mode flag.
        /// </summary>
        /// <param name="flag"></param>
        public static void AddConsoleFlag(ConsoleFlag flag)
        {
            IntPtr handle = Kernal.GetStdHandle(-11);

            ConsoleFlag flags;
            Kernal.GetConsoleMode(handle, out flags);

            Kernal.SetConsoleMode(handle, flags | flag);
        }
        /// <summary>
        /// Removes console mode flag.
        /// </summary>
        /// <param name="flag"></param>
        public static void RemoveConsoleFlag(ConsoleFlag flag)
        {
            IntPtr handle = Kernal.GetStdHandle(-11);
            
            ConsoleFlag flags;
            Kernal.GetConsoleMode(handle, out flags);

            flags &= ~flag;

            Kernal.SetConsoleMode(handle, flags);
        }
        /// <summary>
        /// Gets the current console mode.
        /// </summary>
        /// <param name="flag"></param>
        public static void GetConsoleMode(out ConsoleFlag flag)
        {
            IntPtr handle = Kernal.GetStdHandle(-11);

            Kernal.GetConsoleMode(handle, out flag);
        }
        internal static void SendColor(Color c)
        {
            System.Console.Write($"\x1b[38;2;{c.R};{c.G};{c.B}m");
        }
        /// <summary>
        /// Writes the specifiedcolored string value to the standard output stream.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="c"></param>
        public static void Write(object obj, Color c)
        {
            SendColor(c);
            System.Console.Write(obj);
        }
        /// <summary>
        /// Writes the specified string value to the standard output stream.
        /// </summary>
        /// <param name="obj"></param>
        public static void Write(object obj)
        {
            System.Console.Write(obj.ToString());
        }
        /// <summary>
        /// Writes the specified colored string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        public static void WriteLine(object obj, Color c)
        {
            SendColor(c);
            System.Console.WriteLine(obj);
        }
        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="obj"></param>
        public static void WriteLine(object obj)
        {
            System.Console.WriteLine(obj.ToString());
        }
        /// <summary>
        /// Reads the next line of characters from the standard input stream, and prints a colored message.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        public static string ReadLine(object msg, Color c)
        {
            System.Console.Write($"\x1b[38;2;{c.R};{c.G};{c.B}m{msg}");
            return System.Console.ReadLine();
        }
        /// <summary>
        /// Reads the next line of characters from the standard input stream, and prints a message.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        public static string ReadLine(object msg)
        {
            SendColor(Foreground);
            System.Console.Write($"{msg}");
            return System.Console.ReadLine();
        }
        /// <summary>
        /// Reads the next line of characters from the standard input stream.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        public static string ReadLine()
        {
            SendColor(Foreground);
            return System.Console.ReadLine();
        }
        /// <summary>
        /// Clears the console
        /// </summary>
        public static void Clear()
        {
            System.Console.Clear();
        }
    }
    /// <summary>
    /// General purpose structure for color
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Red color channel
        /// </summary>
        public int R;
        /// <summary>
        /// Green color channel
        /// </summary>
        public int G;
        /// <summary>
        /// Blue color channel
        /// </summary>
        public int B;
        /// <summary>
        /// Alpha color channel
        /// </summary>
        public int A;

        /// <summary>
        /// Applys alpha based on background color
        /// </summary>
        /// <param name="back"></param>
        public void CalcAlpha(Color back)
        {
            float a = this.A / 255;
            float a2 = 1 - a;
            Color c = this * a + back * a2;
            R = c.R;
            G = c.G;
            B = c.B;
        }

        /// <summary>
        /// Applys alpha based on background color
        /// </summary>
        public void CalcAlpha()
        {
            float a = this.A / 255;
            float a2 = 1 - a;
            Color c = this * a + Console.Background * a2;
            R = c.R;
            G = c.G;
            B = c.B;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>Color from argb</returns>
        public static Color FromArgb(int a, int r, int g, int b) => new Color(r, g, b, a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>Color from argb</returns>
        public static Color FromArgb(int r, int g, int b) => new Color(r, g, b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public Color(int r, int g, int b, int a = 255)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rgb"></param>
        public Color(uint rgb)
        {
            A = 255;
            R = (int)(rgb >> 16) & 0xff;
            G = (int)(rgb >> 08) & 0xff;
            B = (int)(rgb >> 00) & 0xff;
        }
        /// <summary>
        /// Converts color to closest console color
        /// </summary>
        /// <returns>Console color</returns>
        public ConsoleColor GetConsoleColor()
        {
            int index = (R > 128 | G > 128 | B > 128) ? 8 : 0; // Bright bit
            index |= (R > 64) ? 4 : 0; // Red bit
            index |= (G > 64) ? 2 : 0; // Green bit
            index |= (B > 64) ? 1 : 0; // Blue bit
            return (ConsoleColor)index;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color operator *(Color a, Color b)
        {
            return new Color(a.R * b.R, a.G * b.G, a.B * b.B);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color operator *(Color a, float b)
        {
            return new Color((int)(a.R * b), (int)(a.G * b), (int)(a.B * b)); ;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color operator +(Color a, Color b)
        {
            return new Color(a.R + b.R, a.G + b.G, a.B + b.B);
        }
    }
}
