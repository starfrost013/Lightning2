#if WINDOWS
using System;
using System.Collections.Generic;
using System.Text;

namespace NuCore.NativeInterop
{
    /// <summary>
    /// Win32 System Metrics (8/26/20)
    /// Written for Tiralen August 26, 2020
    /// Ported to Lightning January 3, 2022
    /// 
    /// Used for first-time setup.
    /// 
    /// <rant>
    ///     Backwards compatibility, what the fuck?
    ///     This enum has some great values:
    ///     System metric 0x19 - is a mouse installed? Oh I don't fucking know, is there a mouse installed? I don't think so!
    ///     System metric 0x41 - is Microsoft Windows for Pen Computing installed? What the fuck? It's not 1994!
    ///     System metric 0x71 - are we running a slow processor? WHAT THE FUCK DOES THIS MEAN BILLY GATES?
    ///     System metric 0x87 - is this Windows XP Media Centre Edition?
    ///     System metric 0x89 - Returns the build number, but specifically and only if this is Windows Server 2003 R2.
    ///     System metric 0x2000 - is the system shutting down? This is entirely redundant!
    /// </rant>
    /// 
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