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
        /// Private static constructor for initialising the NuCore logging system
        /// </summary>
        static NCLogging()
        {
            Settings = new NCLoggingSettings();
            AppDomain.CurrentDomain.ProcessExit += Exit;
        }

        public static void Init()
        {
            if (Settings == null) throw new NCException("You must set up NCLoggingSettings before initialising NCLogging!", 5, "NCLogging.Init", NCExceptionSeverity.FatalError);

            if (Settings.LogFileName == null) Settings.LogFileName = $"NuCore_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.log";

            if (Settings.WriteToLog)
            {
                if (Settings.LogFileName == null) throw new NCException("Passed null file name to NCLogging.Init()!", 6, "NCLogging.Init", NCExceptionSeverity.FatalError);

                if (File.Exists(Settings.LogFileName)) File.Delete(Settings.LogFileName);

                LogStream = new StreamWriter(new FileStream(Settings.LogFileName, FileMode.OpenOrCreate));
            }

            Initialised = true; 
        }

        public static void Log(string information, NCExceptionSeverity severity = NCExceptionSeverity.Message)
        {
            if (!Initialised) return;

            switch (severity)
            {
                case NCExceptionSeverity.Message:
                    Log(information, ConsoleColor.White);
                    return;
                case NCExceptionSeverity.Warning:
                    Log(information, ConsoleColor.Yellow);
                    return;
                case NCExceptionSeverity.Error:
                    Log(information, ConsoleColor.Red);
                    return;
                case NCExceptionSeverity.FatalError:
                    Log(information, ConsoleColor.DarkRed);
                    return;
            }
        }

        public static void Log(string Information, ConsoleColor color)
        {
            if (!Initialised) return;

            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            StringBuilder sb = new StringBuilder();

            sb.Append($"[{now} - ");
            
            StackTrace st = new StackTrace();

            // get the last called method
            // stack frame 0 is always current 
            // stack frame 1 is therefore previous (likely impossible to have a situation where this would be called with 0 stack frames)

            MethodBase method = st.GetFrame(2).GetMethod();

            string methodName = method.Name;
            string className = method.ReflectedType.Name;   
            sb.Append($"{className}::{methodName}");

            sb.Append("]: ");

            sb.Append($"{Information}\n");

            string final_log = sb.ToString();

            if (Settings.WriteToLog) LogStream.Write(final_log);

            Console.ForegroundColor = color;

            Console.Write(final_log);

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Exit(object Sender, EventArgs e)
        {
            if (Settings.WriteToLog && Initialised) LogStream.Close();
        }
    }
}
