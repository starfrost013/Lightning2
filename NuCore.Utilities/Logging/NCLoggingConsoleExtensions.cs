using System;
using System.Runtime.Versioning;

namespace NuCore.Utilities
{
    /// <summary>
    /// This is a wrapper class for Console that implements additional functionality.
    /// It appears that using partial class Console is bad practice for numerous reasons, so we are using a wrapper class
    /// </summary>
    public static class NCConsole
    {
        public static ConsoleColor ForegroundColor => Console.ForegroundColor;
        public static ConsoleColor BackgroundColor => Console.BackgroundColor;

        [SupportedOSPlatform("windows")]
        public static bool CapsLock => Console.CapsLock;

        public static int CursorSize => Console.CursorSize;
 
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

        
        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
        public static string ReadLine() => Console.ReadLine();
        public static (int Left, int Top) GetConsolePosition() => Console.GetCursorPosition();
        public static void SetConsolePosition(int left, int top) => Console.SetCursorPosition(left, top);

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

        public static void Write(string str) => Console.Write(str);
        public static void WriteLine(string str) => Console.WriteLine(str);
        public static void ClearCurrentLine() => Console.Write("\x1b[2K");

    }
}
