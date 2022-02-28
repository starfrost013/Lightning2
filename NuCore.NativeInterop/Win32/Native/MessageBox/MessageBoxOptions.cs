#if WINDOWS
using System;
using System.Collections.Generic;
using System.Text;

namespace NuCore.NativeInterop.Win32
{
    /// <summary>
    /// Options used for configuring messageboxes.
    /// 
    /// 2020-03-06  Moved from Emerald to Lightning.
    /// </summary>
    public enum MessageBoxOptions
    {
        None = 0,

        DefaultDesktopOnly = 131072,

        RightAlign = 524288,

        RtlReading = 1048576,

        ServiceNotification = 2097152
    }
}
#endif