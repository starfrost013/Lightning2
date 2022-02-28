#if WINDOWS
using System;
using System.Collections.Generic;
using System.Text;

namespace NuCore.NativeInterop.Win32
{
    /// <summary>
    /// Results returned from a message box; dependent on the button pressed by the user.
    /// 
    /// 2020-03-08  Moved from Emerald to Lightning.
    /// </summary>
    public enum MessageBoxResult
    {
        /// <summary>
        /// Reserved
        /// </summary>
        None = 0,

        /// <summary>
        /// The user pressed the OK button.
        /// </summary>
        OK = 1,

        /// <summary>
        /// The user pressed the Cancel button.
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The user pressed the Abort button.
        /// </summary>
        Abort = 3,

        /// <summary>
        /// The user pressed the Retry button.
        /// </summary>
        Retry = 4,

        /// <summary>
        /// The user pressed the Ignore button.
        /// </summary>
        Ignore = 5,

        /// <summary>
        /// The user pressed the Yes button.
        /// </summary>
        Yes = 6,

        /// <summary>
        /// The user pressed the No button.
        /// </summary>
        No = 7,

        /// <summary>
        /// The user pressed the Try Again button.
        /// </summary>
        TryAgain = 10,

        /// <summary>
        /// The user pressed the Continue button.
        /// </summary>
        Continue = 11

    }
}
#endif