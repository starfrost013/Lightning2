using System;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCLoggingSettings
    /// 
    /// February 4, 2022
    /// 
    /// Defines NuCore logging settings
    /// </summary>
    public class NCLoggingSettings
    {
        public ConsoleColor Foreground => Console.ForegroundColor;

        public ConsoleColor Background => Console.BackgroundColor;

        /// <summary>
        /// The file name of the log.
        /// </summary>
        public string LogFileName { get; set; }

        /// <summary>
        /// Determines if the log is to be written to.
        /// </summary>
        public bool WriteToLog { get; set; }
    }
}
