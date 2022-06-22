using System;
using System.IO;
using System.Runtime.Versioning;

namespace NuCore.Utilities
{
    /// <summary>
    /// This is a wrapper class for Console that implements additional functionality.
    /// It appears that using partial class Console is bad practice for numerous reasons, so we are using a wrapper class
    /// </summary>
    public static class NCConsole
    {
        /// <summary>
        /// See <see cref="Console.ForegroundColor"/>
        /// </summary>
        public static ConsoleColor ForegroundColor => Console.ForegroundColor;

        /// <summary>
        /// See <see cref="Console.BackgroundColor"/>
        /// </summary>
        public static ConsoleColor BackgroundColor => Console.BackgroundColor;

        /// <summary>
        /// See <see cref="Console.CapsLock"/> (only supported on Windows!)
        /// </summary>
        [SupportedOSPlatform("windows")]
        public static bool CapsLock => Console.CapsLock;

        /// <summary>
        /// See <see cref="Console.CursorSize"/>
        /// </summary>
        public static int CursorSize => Console.CursorSize;

        /// <summary>
        /// See <see cref="Console.CursorVisible"/> (only supported on Windows!)
        /// </summary>
        [SupportedOSPlatform("windows")]
        public static bool CursorVisible => Console.CursorVisible;

        /// <summary>
        /// See <see cref="Console.In"/>.
        /// </summary>
        public static TextReader In => Console.In;

        /// <summary>
        /// See <see cref="Console.Out"/>.
        /// </summary>
        public static TextWriter Out => Console.Out;

        /// <summary>
        /// See <see cref="Console.Error"/>.
        /// </summary>
        public static TextWriter Error => Console.Error;

        /// <summary>
        /// See <see cref="Console.BufferWidth"/>.
        /// </summary>
        public static int BufferWidth => Console.BufferWidth;

        /// <summary>
        /// See <see cref="Console.BufferHeight"/>.
        /// </summary>
        public static int BufferHeight => Console.BufferHeight;

        /// <summary>
        /// See <see cref="Console.WindowWidth"/>.
        /// </summary>
        public static int WindowWidth => Console.WindowWidth;

        /// <summary>
        /// See <see cref="Console.WindowHeight/>.
        /// </summary>
        public static int WindowHeight => Console.WindowHeight;

        /// <summary>
        /// See <see cref="Console.LargestWindowWidth"/>
        /// </summary>
        public static int LargestWindowWidth => Console.LargestWindowWidth;

        /// <summary>
        /// See <see cref="Console.LargestWindowHeight"/>
        /// </summary>
        public static int LargestWindowHeight => Console.LargestWindowHeight;

        /// <summary>
        /// See <see cref="Console.WindowTop"/>
        /// </summary>
        public static int WindowTop => Console.WindowTop;


        /// <summary>
        /// See <see cref="Console.CursorLeft"/>
        /// </summary>
        public static int CursorLeft
        {
            get
            {
                return Console.CursorLeft;
            }
            set
            {
                Console.SetCursorPosition(value, CursorTop);
                Console.CursorLeft = value;
            }
        }

        /// <summary>
        /// See <see cref="Console.CursorTop"/>
        /// </summary>
        public static int CursorTop
        {
            get
            {
                return Console.CursorTop;
            }
            set
            {
                Console.SetCursorPosition(CursorLeft, value);
                Console.CursorTop = value;
            }
        }

        /// <summary>
        /// See <see cref="Console.Clear"/>
        /// </summary>
        /// <param name="clearEntirely">Clears the entire console instead of just the visible area (on Windows Terminal)</param>
        public static void Clear(bool clearEntirely)
        {
            if (clearEntirely)
            {
                Console.Clear();
            }
            else
            {
                Console.Write("\x1b[3J");
            }
        }

        /// <summary>
        /// See <see cref="Console.ReadKey"/>
        /// </summary>
        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();

        /// <summary>
        /// See <see cref="Console.ReadKey(bool)"/>
        /// </summary>
        public static ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(a);

        /// <summary>
        /// See <see cref="Console.ReadLine"/>
        /// </summary>
        public static string ReadLine() => Console.ReadLine();

        /// <summary>
        /// See <see cref="Console.GetCursorPosition"/>
        /// </summary>
        public static (int Left, int Top) GetCursorPosition() => Console.GetCursorPosition();

        /// <summary>
        /// See <see cref="Console.SetCursorPosition"/>
        /// </summary>
        public static void SetConsolePosition(int left, int top) => Console.SetCursorPosition(left, top);

        /// <summary>
        /// See <see cref="Console.Write(string?)"/>
        /// </summary>
        public static void Write(string str) => Console.Write(str);

        /// <summary>
        /// See <see cref="Console.WriteLine(string?)"/>
        /// </summary>
        public static void WriteLine(string str) => Console.WriteLine(str);

        /// <summary>
        /// Clears the current line from the console.
        /// </summary>
        public static void ClearCurrentLine() => Console.Write("\x1b[2K");

        /// <summary>
        /// See <see cref="IsInputRedirected"/>
        /// </summary>
        public static bool IsInputRedirected => Console.IsInputRedirected;

        /// <summary>
        /// See <see cref="IsOutputRedirected"/>
        /// </summary>
        public static bool IsOutputRedirected => Console.IsOutputRedirected;

        /// <summary>
        /// See <see cref="IsErrorRedirected"/>
        /// </summary>
        public static bool IsErrorRedirected => Console.IsErrorRedirected;

    }
}
