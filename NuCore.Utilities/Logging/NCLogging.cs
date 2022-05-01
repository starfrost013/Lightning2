using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static StreamWriter LogStream { get; set; }

        public static NCLoggingSettings Settings { get; set; }

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
            if (Settings == null)
            {
                throw new NCException("You must set up NCLoggingSettings before initialising NCLogging!", 5, "NCLogging.Init", NCExceptionSeverity.FatalError);
            }

            if (Settings.LogFileName == null) Settings.LogFileName = $"NuCore_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.log";

            if (Settings.WriteToLog)
            {
                if (Settings.LogFileName == null) throw new NCException("Passed null file name to NCLogging.Init()!", 6, "NCLogging.Init", NCExceptionSeverity.FatalError);

                if (File.Exists(Settings.LogFileName)) File.Delete(Settings.LogFileName);

                LogStream = new StreamWriter(new FileStream(Settings.LogFileName, FileMode.OpenOrCreate));
            }

        }

        public static void Log(string Information, NCExceptionSeverity Severity = NCExceptionSeverity.Message)
        {
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            StringBuilder sb = new StringBuilder();

            sb.Append($"[{now} - ");
            
            StackTrace st = new StackTrace();

            // get the last called method
            // stack frame 0 is always current 
            // stack frame 1 is therefore previous (hard to imagine a situation where this would be called with 0 stack frames)
            sb.Append(st.GetFrame(1).GetMethod().Name);

            sb.Append("]: ");

            sb.Append($"{Information}\n");

            string final_log = sb.ToString();

            if (Settings.WriteToLog) LogStream.Write(final_log);
    
            switch (Severity)
            {
                case NCExceptionSeverity.Message:
                    Console.Write(final_log);
                    return;
                case NCExceptionSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(final_log);
                    return;
                case NCExceptionSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(final_log);
                    return;
                case NCExceptionSeverity.FatalError:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(final_log);
                    return;
            }

        }

        public static void Exit(object Sender, EventArgs e)
        {
            LogStream.Close();
        }
    }
}
