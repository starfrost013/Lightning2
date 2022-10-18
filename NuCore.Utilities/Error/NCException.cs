using System;
using System.Text;
using System.Reflection;

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

        /// <summary>
        /// Throw an error.
        /// </summary>
        /// <param name="nDescription">A description of the error.</param>
        /// <param name="nId">The ID of the error.</param>
        /// <param name="nCause">An optional more detailed error cause - usually the code condition that caused it</param>
        /// <param name="nExceptionSeverity">The severity of the exception - see <see cref="NCExceptionSeverity"/></param>
        /// <param name="nBaseException">The .NET exception that caused the error, if present.</param>
        /// <param name="dontShowMessageBox">Determines if a message box was shown or not</param>
        public NCException(string nDescription, int nId, string nCause = null, NCExceptionSeverity nExceptionSeverity = NCExceptionSeverity.Message, Exception nBaseException = null, bool dontShowMessageBox = false) : base(nDescription)
        {
            Description = nDescription;
            Id = nId;
            Cause = nCause;
            ExceptionSeverity = nExceptionSeverity;
            BaseException = nBaseException;

            StringBuilder stringBuilder = new StringBuilder();

            if (Cause != null) stringBuilder.Append($"{Cause}: ");
            if (Description != null) stringBuilder.Append(Description);
            if (ExceptionSeverity > NCExceptionSeverity.Message) stringBuilder.Append($" [{Id}]");

            if (BaseException != null) stringBuilder.Append($"\n\nError Information:\n{BaseException}");

            string errorString = stringBuilder.ToString();

            NCLogging.Log($"{ExceptionSeverity}:\n{errorString}", nExceptionSeverity);

            // determine if the Lightning utilities are loaded.
            // this is all kludge until we get a better msgbox api

            Assembly assembly = NCAssembly.GetLoadedAssembly(NCAssembly.LIGHTNING_UTILITIES_NAME);

            // display message box
            if (assembly != null
                && !dontShowMessageBox)
            {
                MethodBase msgBoxOk = assembly.GetType(NCAssembly.LIGHTNING_UTILITIES_PRESET_NAME).GetMethod("MessageBoxOK");

                switch (ExceptionSeverity)
                {
                    case NCExceptionSeverity.Message:
                        msgBoxOk.Invoke(null, new object[] 
                        { "Information", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION } );
                        break;
                    case NCExceptionSeverity.Warning:
                        msgBoxOk.Invoke(null, new object[] 
                        { "Warning", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING } );
                        break;
                    case NCExceptionSeverity.Error:
                        msgBoxOk.Invoke(null, new object[] 
                        { "Error", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR });
                        break;
                    case NCExceptionSeverity.FatalError:
                        msgBoxOk.Invoke(null, new object[]
                            { "Fatal Error", $"A fatal error has occurred:\n\n{errorString}\n\n" +
                            $"The program must exit. We are sorry for the inconvenience.", SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR });
                        break;

                }
            }

            if (ExceptionSeverity == NCExceptionSeverity.FatalError) Environment.Exit(Id);
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