using System;

namespace NuCore.Utilities
{
    /// <summary>
    /// This is a wrapper class for Console that implements additional functionality.
    /// It appears that using partial class System is bad practice for numerous reasons so 
    /// </summary>
    public partial class NCConsole
    {
        public static (int Left, int Top) GetConsolePosition() => Console.GetCursorPosition();

        public ConsoleColor ForegroundColor => Console.ForegroundColor;
        public ConsoleColor BackgroundColor => Console.BackgroundColor;

        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
        public static string ReadLine() => Console.ReadLine();

        public static void Write(string str) => Console.Write(str);
        public static void WriteLine(string str) => Console.WriteLine(str);
        public static void ClearCurrentLine() => Console.Write("\x1b[2K");

    }
}
