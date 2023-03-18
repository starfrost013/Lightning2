﻿namespace LightningUtil
{
    /// <summary>
    /// NCLogging
    /// 
    /// Provides NuCore logging capabilities
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Private: Holds stream used for logging
        /// </summary>
        public static StreamWriter? LogStream { get; private set; }

        /// <summary>
        /// The settings for the NuCore logger.
        /// </summary>
        public static LoggerSettings Settings { get; set; }

        /// <summary>
        /// Determines if logging is initialised.
        /// </summary>
        public static bool Initialised { get; set; }

        /// <summary>
        /// Static constructor for initialising the NuCore logging system
        /// </summary>
        static Logger()
        {
            Settings = new();
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
                if (Settings.LogFileName == string.Empty)
                {
                    Logger.LogError("Passed empty file name to Logger::Init!", 6, LoggerSeverity.FatalError);
                    return;
                }

                if (File.Exists(Settings.LogFileName)) File.Delete(Settings.LogFileName);

                LogStream = new StreamWriter(new FileStream(Settings.LogFileName, FileMode.OpenOrCreate));
            }

            Initialised = true;

        }

        internal static void Log(string information, LoggerSeverity severity, bool printMetadata = true, bool logToFile = true)
        {
            if (!Initialised) return;

            switch (severity)
            {
#if !FINAL
                case LoggerSeverity.Message:
                    Log(information, ConsoleColor.White, logToFile, printMetadata);
                    return;
#endif
                case LoggerSeverity.Warning:
                    Log(information, ConsoleColor.Yellow, logToFile, printMetadata);
                    return;
                case LoggerSeverity.Error:
                    Log(information, ConsoleColor.Red, logToFile, printMetadata);
                    return;
                case LoggerSeverity.FatalError:
                    Log(information, ConsoleColor.DarkRed, logToFile, printMetadata);
                    return;
            }
        }

        public static void Log(string information, ConsoleColor color = ConsoleColor.White, bool printMetadata = true, bool logToFile = true)
        {
            if (!Initialised)
            {
                Console.WriteLine("Logger not initialised, not logging anything!");
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
                    Logger.LogError("Failed to get stack frame for logging, ignoring", 302, LoggerSeverity.Error, null, true);
                    return; 
                }

                MethodBase? method = stackFrame.GetMethod();

                // as far as i know this shouldn't happen but ignore if it does
                if (method == null
                    || method.ReflectedType == null)
                {
                    Logger.LogError("Failed to get stack frame for logging, ignoring", 303, LoggerSeverity.Error, null, true);
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
#if !FINAL && !PROFILING // final build turns off all non-error and server console logging, profiling does it for perf

            Console.ForegroundColor = color;

            Console.Write(finalLogText);

            Console.ForegroundColor = ConsoleColor.White;
#endif
        }

        public static void Log(string prefix, string information, ConsoleColor color = ConsoleColor.White, bool printMetadata = true, bool logToFile = true)
        {
            Log($"[{prefix}]: {information}", color, printMetadata, logToFile);
        }

        /// <summary>
        /// Log and display an error message.
        /// </summary>
        /// <param name="description">A description of the error.</param>
        /// <param name="id">The ID of the error.</param>
        /// <param name="exceptionSeverity">The severity of the exception - see <see cref="LoggerSeverity"/></param>
        /// <param name="baseException">The .NET exception that caused the error, if present.</param>
        /// <param name="dontShowMessageBox">Determines if a message box was shown or not</param>
        public static void LogError(string description, int id, LoggerSeverity exceptionSeverity = LoggerSeverity.Message,
            Exception? baseException = null, bool dontShowMessageBox = false)
        {
            StringBuilder stringBuilder = new();

            if (description != null) stringBuilder.Append($"{description}");
            if (exceptionSeverity > LoggerSeverity.Message) stringBuilder.Append($" [{id}]");

            if (baseException != null) stringBuilder.Append($"\n\nError Information:\n{baseException}");

            string errorString = stringBuilder.ToString();

            Logger.Log($"{exceptionSeverity}:\n{errorString}", exceptionSeverity);

            // display message box
            if (AssemblyUtils.NCLightningExists
                && !dontShowMessageBox)
            {
                Debug.Assert(AssemblyUtils.NCLightningAssembly != null);
                Type? lightningUtilName = AssemblyUtils.NCLightningAssembly.GetType(AssemblyUtils.LIGHTNING_UTILITIES_PRESET_NAME, false, true);

                if (lightningUtilName == null)
                {
                    Logger.Log("Failed to load NCMessageBox type through reflection (ignoring)", ConsoleColor.Yellow);
                    return;
                }

                MethodBase? msgBoxOk = lightningUtilName.GetMethod("MessageBoxOK");

                if (msgBoxOk == null)
                {
                    Logger.Log("Failed to display error box (ignoring)", ConsoleColor.Yellow);
                    return;
                }

                switch (exceptionSeverity)
                {
                    case LoggerSeverity.Message:
                        msgBoxOk.Invoke(null, new object[]
                        { "Information", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION });
                        break;
                    case LoggerSeverity.Warning:
                        msgBoxOk.Invoke(null, new object[]
                        { "Warning", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING });
                        break;
                    case LoggerSeverity.Error:
                        msgBoxOk.Invoke(null, new object[]
                        { "Error", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR });
                        break;
                    case LoggerSeverity.FatalError:
                        msgBoxOk.Invoke(null, new object[]
                            { "Fatal Error", $"A fatal error has occurred:\n\n{errorString}\n\n" +
                                $"The Lightning Game Engine-based application you are running must exit. We are sorry for the inconvenience.",
                                SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR });
                        break;

                }
            }

            if (exceptionSeverity == LoggerSeverity.FatalError) Environment.Exit(id);
        }

        /// <summary>
        /// Log and display an error message.
        /// </summary>
        /// <param name="description">A description of the error.</param>
        /// <param name="prefix">A prefix to display before the error message determining the erroring component.</param>
        /// <param name="id">The ID of the error.</param>
        /// <param name="exceptionSeverity">The severity of the exception - see <see cref="LoggerSeverity"/></param>
        /// <param name="baseException">The .NET exception that caused the error, if present.</param>
        /// <param name="dontShowMessageBox">Determines if a message box was shown or not</param>
        public static void LogError(string description, string prefix, int id, LoggerSeverity exceptionSeverity = LoggerSeverity.Message,
            Exception? baseException = null, bool dontShowMessageBox = false)
        {
            LogError($"[{prefix}]: {description}", id, exceptionSeverity, baseException, dontShowMessageBox);
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