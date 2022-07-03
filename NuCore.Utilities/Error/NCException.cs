using System;
using System.Text;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCException
    /// 
    /// February 1, 2022
    /// 
    /// Defines a NuCore exception
    /// </summary>
    public class NCException : Exception
    {
        /// <summary>
        /// The description of this exception
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The ID number of this exception.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Optional cause of the exception.
        /// </summary>
        public string Cause { get; set; }

        /// <summary>
        /// The severity of this exception.
        /// </summary>
        public NCExceptionSeverity ExceptionSeverity { get; set; }

        /// <summary>
        /// An optional base exception that caused this exception.
        /// Used for outputting base exception information, not a replacement for exception.baseexception!
        /// </summary>
        public Exception BaseException { get; set; }

        /// <summary>
        /// If true, the message box of this error will be suppressed and not shown.
        /// </summary>
        public bool DontShowMessageBox { get; set; }

        public NCException(string nDescription, int nId, string nCause = null, NCExceptionSeverity nExceptionSeverity = NCExceptionSeverity.Message, Exception nBaseException = null, bool DontShowMessageBox = false) : base(nDescription)
        {
            Description = nDescription;
            Id = nId;
            Cause = nCause;
            ExceptionSeverity = nExceptionSeverity;
            BaseException = nBaseException;

            StringBuilder sb = new StringBuilder();

            if (Cause != null) sb.Append($"{Cause}: ");
            if (Description != null) sb.Append(Description);
            if (ExceptionSeverity > NCExceptionSeverity.Message) sb.Append($" [{Id}]");

            if (BaseException != null) sb.Append($"\n\nError Information:\n{BaseException}");

            string err_string = sb.ToString();

            NCLogging.Log($"{ExceptionSeverity}:\n{err_string}", nExceptionSeverity);

            if (!DontShowMessageBox)
            {
                switch (ExceptionSeverity)
                {
                    case NCExceptionSeverity.Message:
                        NCMessageBoxPresets.MessageBoxOK("Information", err_string, SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION);
                        return;
                    case NCExceptionSeverity.Warning:
                        NCMessageBoxPresets.MessageBoxOK("Warning", err_string, SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING);
                        return;
                    case NCExceptionSeverity.Error:
                        NCMessageBoxPresets.MessageBoxOK("Error", err_string, SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR);
                        return;
                    case NCExceptionSeverity.FatalError:
                        NCMessageBoxPresets.MessageBoxOK("Fatal Error", $"A fatal error has occurred:\n\n{err_string}\n\n" +
                            $"The program must exit. We are sorry for the inconvenience.", SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR);
                        Environment.Exit(Id);
                        return;

                }
            }
        }
    }
}