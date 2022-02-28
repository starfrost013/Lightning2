#if WINDOWS
using System;
using System.Collections.Generic;
using System.Text;

namespace NuCore.NativeInterop.Win32
{
    /// <summary>
    /// 2021-03-05
    /// 
    /// Lightning Native Interop
    /// 
    /// 2020-02-27          Created for Emerald v4.0.1546.0
    /// 2021-03-05          Moved from Emerald to Lightning [v1.0.10].
    /// </summary>
    public enum MessageBoxType
    {
        /// <summary>
        /// OK button. 
        /// </summary>
        MB_OK = 0,

        /// <summary>
        /// OK/Cancel button.
        /// </summary>
        MB_OKCANCEL = 0x1,

        /// <summary>
        /// Abort/Retry/Ignore buttons
        /// </summary>
        MB_ABORTRETRYIGNORE = 0x2,

        /// <summary>
        /// Yes/No/Cancel buttons.
        /// </summary>

        MB_YESNOCANCEL = 0x3,

        /// <summary>
        /// Yes/No buttons.
        /// </summary>

        MB_YESNO = 0x4,

        /// <summary>
        /// Retry/Cancel buttons.
        /// </summary>

        MB_RETRYCANCEL = 0x5,

        /// <summary>
        /// Cancel/Try/Continue buttons.
        /// </summary>

        MB_CANCELTRYCONTINUE = 0x6,

        /// <summary>
        /// Error icon used as default
        /// </summary>

        MB_ICONERROR = 0x10,

        /// <summary>
        /// Question icon used as default
        /// </summary>

        MB_ICONQUESTION = 0x20,

        /// <summary>
        /// Warning icon used as default
        /// </summary>

        MB_ICONWARNING = 0x30,

        /// <summary>
        /// Asterisk icon used as default
        /// </summary>

        MB_ICONASTERISK = 0x40,

        /// <summary>
        /// Default button set to the second button. 0x0 = 1
        /// </summary>

        MB_DEFBUTTON2 = 0x100,

        /// <summary>
        /// Default button set to the third button.
        /// </summary>

        MB_DEFBUTTON3 = 0x200,

        /// <summary>
        /// Default button set to the fourth button.
        /// </summary>

        MB_DEFBUTTON4 = 0x300,

        /// <summary>
        /// Force the window to the top; use for extremely critical errors only. Blocks input to 
        /// all windows with the current HWnd. 
        /// </summary>

        MB_SYSTEMMODAL = 0x1000,

        /// <summary>
        /// APPLMODAL (0x0) for services
        /// </summary>

        MB_TASKMODAL = 0x2000,

        /// <summary>
        /// Adds a help message. You will have to respond to the WM_HELP message if you use this.
        /// </summary>

        MB_HELP = 0x4000,

        /// <summary>
        /// Force the message box to become the foreground window.
        /// </summary>

        MB_SETFOREGROUND = 0x10000,

        /// <summary>
        /// Do not diisplay the message box until the user returns to the default desktop.
        /// </summary>

        MB_DEFAULT_DESKTOP_ONLY = 0x20000,

        /// <summary>
        /// Force the window to be topmost.
        /// </summary>

        MB_TOPMOST = 0x40000,

        /// <summary>
        /// Right-justify the message box text.
        /// </summary>

        MB_RIGHT = 0x80000,

        /// <summary>
        /// Use right to left reading order for Hebrew and other languages.
        /// </summary>

        MB_RTLREADING = 0x100000,

        /// <summary>
        /// (copied from MSDN)
        /// 
        /// The caller is a service notifying the user of an event. The function displays a message box on the current active desktop, even if there is no user logged on to the computer.
        ///
        /// Terminal Services: If the calling thread has an impersonation token, the function directs the message box to the session specified in the impersonation token.
        ///
        /// If this flag is set, the hWnd parameter must be NULL. 
        /// This is so that the message box can appear on a desktop other than the desktop corresponding to the hWnd.
        ///
        /// For information on security considerations in regard to using this flag, see Interactive Services.In particular, be aware that this flag can produce interactive content on a locked desktop and should therefore be used for only a very limited set of scenarios, such as resource exhaustion.
        /// </summary>

        MB_SERVICE_NOTIFICATION = 0x200000

    }
}
#endif