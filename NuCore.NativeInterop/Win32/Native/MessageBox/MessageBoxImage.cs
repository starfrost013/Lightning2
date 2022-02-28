#if WINDOWS
using System;
using System.Collections.Generic;
using System.Text;

namespace NuCore.NativeInterop.Win32
{
    /// <summary>
    /// Icons used in messageboxes.
    /// 
    /// 2020-03-06  Moved from Emerald to Lightning.
    /// </summary>
    public enum MessageBoxImage
    {
        /// <summary>
        /// No icon.
        /// </summary>
        None = 0,

        /// <summary>
        /// Same as Error.
        /// </summary>
        Hand = 16,

        /// <summary>
        /// An error icon is shown.
        /// </summary>
        Error = 16,

        /// <summary>
        /// A question icon is shown.
        /// </summary>
        Question = 32,

        /// <summary>
        /// Same as warning.
        /// </summary>
        Exclamation = 48,

        /// <summary>
        /// A warning icon is shown.
        /// </summary>
        Warning = 48,

        /// <summary>
        /// Identical to information.
        /// </summary>
        Asterisk = 64,

        /// <summary>
        /// Display an informational icon in the message box.
        /// </summary>
        Information = 64,


    }
}
#endif