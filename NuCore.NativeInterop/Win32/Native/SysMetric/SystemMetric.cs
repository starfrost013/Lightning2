#if WINDOWS
using System;
using System.Collections.Generic;
using System.Text;

namespace NuCore.NativeInterop
{
    /// <summary>
    /// Win32 System Metrics (8/26/20)
    /// Written for Tiralen     August 26, 2020
    /// Ported to Lightning     January 3, 2022
    /// Angry rant removed      March 5, 2022
    /// 
    /// Used for first-time setup.
    /// Only define the stuff we need. 
    /// </summary>
    public enum SystemMetric
    {
        SM_CXSCREEN = 0,
        SM_CYSCREEN = 1,
        SM_CXVIRTUALSCREEN = 78,
        SM_CYVIRTUALSCREEN = 79,

    }
}
#endif