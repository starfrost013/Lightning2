#if WINDOWS
using NuCore.NativeInterop.Win32;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        public NCException(string NDescription, int NId, string NCause = null, NCExceptionSeverity NExceptionSeverity = NCExceptionSeverity.Message, Exception NBaseException = null) : base(NDescription)
        {
            Description = NDescription;
            Id = NId;
            Cause = NCause;

            ExceptionSeverity = NExceptionSeverity;
            BaseException = NBaseException;

#if WINDOWS

            StringBuilder sb = new StringBuilder();

            NCLogging.Log($"ERROR:\n\nID: {NId}\nSeverity: {NExceptionSeverity}\nCause: {NCause}, Description: {NDescription}", NExceptionSeverity);

            if (Cause != null) sb.Append($"{Cause}: ");
            if (Description != null) sb.Append(Description);
            if (ExceptionSeverity > NCExceptionSeverity.Message) sb.Append($" [{Id}]");

            if (BaseException != null) sb.Append($"\n\nBase Exception:\n{BaseException}");

            string err_string = sb.ToString();

            switch (ExceptionSeverity)
            {
                case NCExceptionSeverity.Message:
                    MessageBox.Show(IntPtr.Zero, err_string, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                case NCExceptionSeverity.Warning:
                    MessageBox.Show(IntPtr.Zero, err_string, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                case NCExceptionSeverity.Error:
                    MessageBox.Show(IntPtr.Zero, err_string, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                case NCExceptionSeverity.FatalError:
                    MessageBox.Show(IntPtr.Zero, $"A fatal error has occurred." +
                        $"The program must exit. We are sorry for any inconvenience.\n\n{err_string}", "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(Id);
                    return;

            }
            
          
        }
        
    }
}
#endif
