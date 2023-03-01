namespace LightningUtil
{
    /// <summary>
    /// NCLoggingSettings
    /// 
    /// February 4, 2022
    /// 
    /// Defines NuCore logging settings
    /// </summary>
    public class LoggerSettings
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

        /// <summary>
        /// Determines if old logs will be kept or not.
        /// </summary>
        public bool KeepOldLogs { get; set; }

        /// <summary>
        /// The date string to use when creating log files.
        /// </summary>
        public string DateString { get; set; }

        /// <summary>
        /// A constant default value for <see cref="DateString"/>
        /// </summary>
        private const string DEFAULT_DATE_STRING = "yyyyMMdd_HHmmss";

        public LoggerSettings()
        {
            DateString = DEFAULT_DATE_STRING;
        }
    }
}
