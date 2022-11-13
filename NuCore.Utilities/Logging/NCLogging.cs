using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
        public static StreamWriter LogStream { get; private set; }

        /// <summary>
        /// The settings for the NuCore logger.
        /// </summary>
        public static NCLoggingSettings Settings { get; set; }

        /// <summary>
        /// Determines if logging is initialised.
        /// </summary>
        public static bool Initialised { get; set; }

        /// <summary>
        /// The date string to use when creating log files.
        /// </summary>
        public static string DateString { get; set; }

        /// <summary>
        /// A constant default value for <see cref="DateString"/>
        /// </summary>
        private const string DEFAULT_DATE_STRING = "yyyyMMdd_HHmmss";

        /// <summary>
        /// Private static constructor for initialising the NuCore logging system
        /// </summary>
        static NCLogging()
        {
            Settings = new NCLoggingSettings();
            DateString = DEFAULT_DATE_STRING;
            AppDomain.CurrentDomain.ProcessExit += Exit;
        }

        public static void Init()
        {
            NCAssembly.Init();

            if (Settings.LogFileName == null) Settings.LogFileName = $"NuCore_{DateTime.Now.ToString(DateString)}.log";

            // delete all old log files
            if (!Settings.KeepOldLogs)
            {
                // delete all .log files
                foreach (string fileName in Directory.GetFiles(Directory.GetCurrentDirectory()))
                {
                    if (fileName.Contains(".log"))
                    {
                        File.Delete(fileName);  
                    }
                }
            }

            if (Settings.WriteToLog)
            {
                if (Settings.LogFileName == null) _ = new NCException("Passed null file name to NCLogging::Init!", 6, 
                    "NCLogging::Init passed with Settings.LogFileName = NULL", NCExceptionSeverity.FatalError);

                if (File.Exists(Settings.LogFileName)) File.Delete(Settings.LogFileName);

                LogStream = new StreamWriter(new FileStream(Settings.LogFileName, FileMode.OpenOrCreate));
            }

            Initialised = true;
        }

        internal static void Log(string information, NCExceptionSeverity severity, bool printMetadata = true, bool logToFile = true)
        {
            if (!Initialised) return;

            switch (severity)
            {
                case NCExceptionSeverity.Message:
                    Log(information, ConsoleColor.White, logToFile, printMetadata);
                    return;
                case NCExceptionSeverity.Warning:
                    Log(information, ConsoleColor.Yellow, logToFile, printMetadata);
                    return;
                case NCExceptionSeverity.Error:
                    Log(information, ConsoleColor.Red, logToFile, printMetadata);
                    return;
                case NCExceptionSeverity.FatalError:
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

                MethodBase method = stackTrace.GetFrame(1).GetMethod();

                string methodName = method.Name;
                string className = method.ReflectedType.Name;
                stringBuilder.Append($"{className}::{methodName}");

                stringBuilder.Append("]: ");
            }

            stringBuilder.Append($"{information}\n");

            string finalLogText = stringBuilder.ToString();

            if (Settings.WriteToLog 
                && logToFile) LogStream.Write(finalLogText);

            NCConsole.ForegroundColor = color;

            NCConsole.Write(finalLogText);

            NCConsole.ForegroundColor = ConsoleColor.White;
        }

        public static void Log(string information, string prefix, ConsoleColor color = ConsoleColor.White, bool printMetadata = true, bool logToFile = true)
        {
            Log($"[{prefix}] {information}", color, printMetadata, logToFile);
        }


        public static void Exit(object Sender, EventArgs e)
        {
            if (Settings.WriteToLog 
                && Initialised) LogStream.Close();
        }
    }
}
