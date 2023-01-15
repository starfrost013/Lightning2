using System.Text;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCLogging
    /// 
    /// February 4, 2022
    /// 
    /// Provides NuCore logging capabilities
    /// </summary>
    public static class NCLogging
    {
        /// <summary>
        /// Private: Holds stream used for logging
        /// </summary>
        public static StreamWriter? LogStream { get; private set; }

        /// <summary>
        /// The settings for the NuCore logger.
        /// </summary>
        public static NCLoggingSettings Settings { get; set; }

        /// <summary>
        /// Determines if logging is initialised.
        /// </summary>
        public static bool Initialised { get; set; }

        /// <summary>
        /// Static constructor for initialising the NuCore logging system
        /// </summary>
        static NCLogging()
        {
            Settings = new();
            Settings.LogFileName = string.Empty;
            AppDomain.CurrentDomain.ProcessExit += Exit;
        }

        public static void Init()
        {
            if (string.IsNullOrWhiteSpace(Settings.LogFileName)) Settings.LogFileName = $"NuCore_{DateTime.Now.ToString(Settings.DateString)}.log";

            // delete all old log files
            if (!Settings.KeepOldLogs)
            {
                // delete all .log files
                foreach (string fileName in Directory.GetFiles(Directory.GetCurrentDirectory()))
                {
                    if (fileName.Contains(".log")) File.Delete(fileName);
                }
            }

            if (Settings.WriteToLog)
            {
                if (Settings.LogFileName == null)
                {
                    NCError.ShowErrorBox("Passed null file name to NCLogging::Init!", 6,
                    NCErrorSeverity.FatalError);
                    return;
                }

                if (File.Exists(Settings.LogFileName)) File.Delete(Settings.LogFileName);

                LogStream = new StreamWriter(new FileStream(Settings.LogFileName, FileMode.OpenOrCreate));
            }

            Initialised = true;

        }

        internal static void Log(string information, NCErrorSeverity severity, bool printMetadata = true, bool logToFile = true)
        {
            if (!Initialised) return;

            switch (severity)
            {
#if !FINAL
                case NCErrorSeverity.Message:
                    Log(information, ConsoleColor.White, logToFile, printMetadata);
                    return;
#endif
                case NCErrorSeverity.Warning:
                    Log(information, ConsoleColor.Yellow, logToFile, printMetadata);
                    return;
                case NCErrorSeverity.Error:
                    Log(information, ConsoleColor.Red, logToFile, printMetadata);
                    return;
                case NCErrorSeverity.FatalError:
                    Log(information, ConsoleColor.DarkRed, logToFile, printMetadata);
                    return;
            }
        }

        public static void Log(string information, ConsoleColor color = ConsoleColor.White, bool printMetadata = true, bool logToFile = true)
        {

            if (!Initialised)
            {
                Console.WriteLine("NCLogging not initialised, not logging anything!");
                return;
            }

            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            StringBuilder stringBuilder = new();

            // If the method specifies it print the date and time, as well as the currently current method
            if (printMetadata)
            {
                stringBuilder.Append($"[{now} - ");

                StackTrace stackTrace = new();

                // get the last called method
                // stack frame 1 is the previously executing method (before Log was called)

                StackFrame? stackFrame = stackTrace.GetFrame(1);

                // as far as i know this shouldn't happen but ignore if it does
                if (stackFrame == null)
                {
                    NCError.ShowErrorBox("Failed to get stack frame for logging, ignoring", 302, NCErrorSeverity.Error, null, true);
                    return; 
                }

                MethodBase? method = stackFrame.GetMethod();

                // as far as i know this shouldn't happen but ignore if it does
                if (method == null
                    || method.ReflectedType == null)
                {
                    NCError.ShowErrorBox("Failed to get stack frame for logging, ignoring", 303, NCErrorSeverity.Error, null, true);
                    return;
                }

                string methodName = method.Name;
                string className = method.ReflectedType.Name;
                stringBuilder.Append($"{className}::{methodName}");

                stringBuilder.Append("]: ");
            }

            stringBuilder.Append($"{information}\n");

            string finalLogText = stringBuilder.ToString();

            if (Settings.WriteToLog
                && logToFile)
            {
                Debug.Assert(LogStream != null);
                LogStream.Write(finalLogText);
            }
#if !FINAL // final build turns off all non-error and server console logging

            Console.ForegroundColor = color;

            Console.Write(finalLogText);

            Console.ForegroundColor = ConsoleColor.White;
#endif
        }

        public static void Log(string information, string prefix, ConsoleColor color = ConsoleColor.White, bool printMetadata = true, bool logToFile = true)
        {
            Log($"[{prefix}]: {information}", color, printMetadata, logToFile);
        }

        public static void Exit(object? Sender, EventArgs e)
        {
            if (Settings.WriteToLog
                && Initialised)
            {
                Debug.Assert(LogStream != null);
                LogStream.Close();
            }
        }
    }
}
