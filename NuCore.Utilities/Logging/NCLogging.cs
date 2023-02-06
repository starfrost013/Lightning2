﻿using System.Text;

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
                    NCLogging.LogError("Passed null file name to NCLogging::Init!", 6, NCLoggingSeverity.FatalError);
                    return;
                }

                if (File.Exists(Settings.LogFileName)) File.Delete(Settings.LogFileName);

                LogStream = new StreamWriter(new FileStream(Settings.LogFileName, FileMode.OpenOrCreate));
            }

            Initialised = true;

        }

        internal static void Log(string information, NCLoggingSeverity severity, bool printMetadata = true, bool logToFile = true)
        {
            if (!Initialised) return;

            switch (severity)
            {
#if !FINAL
                case NCLoggingSeverity.Message:
                    Log(information, ConsoleColor.White, logToFile, printMetadata);
                    return;
#endif
                case NCLoggingSeverity.Warning:
                    Log(information, ConsoleColor.Yellow, logToFile, printMetadata);
                    return;
                case NCLoggingSeverity.Error:
                    Log(information, ConsoleColor.Red, logToFile, printMetadata);
                    return;
                case NCLoggingSeverity.FatalError:
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
                    NCLogging.LogError("Failed to get stack frame for logging, ignoring", 302, NCLoggingSeverity.Error, null, true);
                    return; 
                }

                MethodBase? method = stackFrame.GetMethod();

                // as far as i know this shouldn't happen but ignore if it does
                if (method == null
                    || method.ReflectedType == null)
                {
                    NCLogging.LogError("Failed to get stack frame for logging, ignoring", 303, NCLoggingSeverity.Error, null, true);
                    return;
                }

                // go back by one to get the real method if we called this throug the prefix override for example
                if (method.Name == "Log")
                {
                    // get past cs8602 warning
                    StackFrame originalStackFrame = stackFrame;

                    stackFrame = stackTrace.GetFrame(2);

                    // fail gracefully (revert to original)
                    stackFrame ??= originalStackFrame;

                    if (stackFrame != null)
                    {
                        // gracefully fail
                        MethodBase originalMethod = method;

                        method = stackFrame.GetMethod();

                        if (method == null) method = originalMethod;
                    }

                }

                string methodName = method.Name;

                Type? methodType = method.ReflectedType;

                string className = (methodType == null) ? "<Unknown Class>" : methodType.Name;

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

        /// <summary>
        /// Display an error box.
        /// </summary>
        /// <param name="description">A description of the error.</param>
        /// <param name="id">The ID of the error.</param>
        /// <param name="cause">An optional more detailed error cause - usually the code condition that caused it</param>
        /// <param name="exceptionSeverity">The severity of the exception - see <see cref="NCLoggingSeverity"/></param>
        /// <param name="baseException">The .NET exception that caused the error, if present.</param>
        /// <param name="dontShowMessageBox">Determines if a message box was shown or not</param>
        public static void LogError(string description, int id, NCLoggingSeverity exceptionSeverity = NCLoggingSeverity.Message,
            Exception? baseException = null, bool dontShowMessageBox = false)
        {
            StringBuilder stringBuilder = new();

            if (description != null) stringBuilder.Append($"{description}");
            if (exceptionSeverity > NCLoggingSeverity.Message) stringBuilder.Append($" [{id}]");

            if (baseException != null) stringBuilder.Append($"\n\nError Information:\n{baseException}");

            string errorString = stringBuilder.ToString();

            NCLogging.Log($"{exceptionSeverity}:\n{errorString}", exceptionSeverity);

            // display message box
            if (NCAssembly.NCLightningExists
                && !dontShowMessageBox)
            {
                Debug.Assert(NCAssembly.NCLightningAssembly != null);
                Type? lightningUtilName = NCAssembly.NCLightningAssembly.GetType(NCAssembly.LIGHTNING_UTILITIES_PRESET_NAME, false, true);

                if (lightningUtilName == null)
                {
                    NCLogging.Log("Failed to load NCMessageBox type through reflection (ignoring)", ConsoleColor.Yellow);
                    return;
                }

                MethodBase? msgBoxOk = lightningUtilName.GetMethod("MessageBoxOK");

                if (msgBoxOk == null)
                {
                    NCLogging.Log("Failed to display error box (ignoring)", ConsoleColor.Yellow);
                    return;
                }

                switch (exceptionSeverity)
                {
                    case NCLoggingSeverity.Message:
                        msgBoxOk.Invoke(null, new object[]
                        { "Information", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION });
                        break;
                    case NCLoggingSeverity.Warning:
                        msgBoxOk.Invoke(null, new object[]
                        { "Warning", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING });
                        break;
                    case NCLoggingSeverity.Error:
                        msgBoxOk.Invoke(null, new object[]
                        { "Error", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR });
                        break;
                    case NCLoggingSeverity.FatalError:
                        msgBoxOk.Invoke(null, new object[]
                            { "Fatal Error", $"A fatal error has occurred:\n\n{errorString}\n\n" +
                                $"The Lightning Game Engine-based application you are running must exit. We are sorry for the inconvenience.",
                                SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR });
                        break;

                }
            }

            if (exceptionSeverity == NCLoggingSeverity.FatalError) Environment.Exit(id);
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

/// <summary>
/// Define this here.
/// 
/// This is to reduce time spent in reflection which is very slow.
/// </summary>
[Flags]
internal enum SDL_MessageBoxFlags : uint
{
    SDL_MESSAGEBOX_ERROR = 0x00000010,

    SDL_MESSAGEBOX_WARNING = 0x00000020,

    SDL_MESSAGEBOX_INFORMATION = 0x00000040
}
