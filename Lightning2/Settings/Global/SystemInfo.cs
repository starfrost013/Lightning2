using static NuCore.SDL2.SDL;
using NuCore.Utilities;
using System;
using System.Runtime.InteropServices;

namespace LightningGL
{
    /// <summary>
    /// SystemInfo
    /// 
    /// August 7, 2022
    /// 
    /// Defines system information.
    /// </summary>
    public static class SystemInfo
    {
        /// <summary>
        /// Screen resolution X; loaded by the settings loader and not set by the developer.
        /// </summary>
        public static uint ScreenResolutionX { get; private set; }

        /// <summary>
        /// Screen resolution X; loaded by the settings loader and not set by the developer.
        /// </summary>
        public static uint ScreenResolutionY { get; private set; }

        /// <summary>
        /// The total amount of system RAM in MiB.
        /// </summary>
        public static int SystemRam { get; private set; }

        /// <summary>
        /// CPU information.
        /// </summary>
        public static SystemInfoCPU Cpu { get; private set; }

        /// <summary>
        /// Operating system version information
        /// </summary>
        public static SystemInfoOperatingSystem OperatingSystem { get; private set; }

        public static void Load()
        {
            // cannot put in static constructor as this depends on SDL being initialised.
            // Initialise CPU info
            Cpu = new SystemInfoCPU();

            // get the resolution of the first monitor as most people have one monitor. 
            // this is pre-window initialisation so we can't query the monitor the window is on because there's no window yet, there is no other way SDL provides this

            SDL_DisplayMode displayMode = new SDL_DisplayMode();

            SDL_GetCurrentDisplayMode(0, out displayMode);

            // store the screen resolution
            ScreenResolutionX = Convert.ToUInt32(displayMode.w);
            ScreenResolutionY = Convert.ToUInt32(displayMode.h);

            NCLogging.Log($"Screen resolution of the primary monitor = {ScreenResolutionX}x{ScreenResolutionY}");

            SystemRam = SDL_GetSystemRAM();

            NCLogging.Log($"Total system RAM (MiB) = {SystemRam}");

            // detect various windows versions
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // detect each version of windows
                if (OperatingSystem.IsWindowsVersionAtLeast(6, 1, 7600, 0)) OperatingSystem = SystemInfoOperatingSystem.Win7;
                if (OperatingSystem.IsWindowsVersionAtLeast(6, 2, 9200, 0)) OperatingSystem = SystemInfoOperatingSystem.Win8;
                if (OperatingSystem.IsWindowsVersionAtLeast(6, 3, 9600, 0)) OperatingSystem = SystemInfoOperatingSystem.Win81;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 10240, 0)) OperatingSystem = SystemInfoOperatingSystem.Win10TH1;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 10586, 0)) OperatingSystem = SystemInfoOperatingSystem.Win10TH2;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 14393, 0)) OperatingSystem = SystemInfoOperatingSystem.Win10RS1;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 15063, 0)) OperatingSystem = SystemInfoOperatingSystem.Win10RS2;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 16299, 0)) OperatingSystem = SystemInfoOperatingSystem.Win10RS3;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17134, 0)) OperatingSystem = SystemInfoOperatingSystem.Win10RS4;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763, 0)) OperatingSystem = SystemInfoOperatingSystem.Win10RS5;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 18362, 0)) OperatingSystem = SystemInfoOperatingSystem.Win1019H1;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 18363, 0)) OperatingSystem = SystemInfoOperatingSystem.Win1019H2;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 19041, 0)) OperatingSystem = SystemInfoOperatingSystem.Win1020H1;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 19042, 0)) OperatingSystem = SystemInfoOperatingSystem.Win1020H2;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 19043, 0)) OperatingSystem = SystemInfoOperatingSystem.Win1021H1;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 19044, 0)) OperatingSystem = SystemInfoOperatingSystem.Win1021H2;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 19045, 0)) OperatingSystem = SystemInfoOperatingSystem.Win1022H2;
                // special case - was never released (but probably at least 1 person using it) so we use first compiled build (19480), 19480-19645 are valid
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 19480, 0)) OperatingSystem = SystemInfoOperatingSystem.Manganese;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 20348, 0)) OperatingSystem = SystemInfoOperatingSystem.Iron;
                // earliest publicly available version
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 21996, 0)) OperatingSystem = SystemInfoOperatingSystem.Win11;
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22621, 0)) OperatingSystem = SystemInfoOperatingSystem.Nickel;
                // moving target so use earliest known build
                if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 25054, 0)) OperatingSystem = SystemInfoOperatingSystem.Copper;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (OperatingSystem.IsMacOSVersionAtLeast(10, 13, 0)) OperatingSystem = SystemInfoOperatingSystem.MacOS1013;
                if (OperatingSystem.IsMacOSVersionAtLeast(10, 14, 0)) OperatingSystem = SystemInfoOperatingSystem.MacOS1014;
                if (OperatingSystem.IsMacOSVersionAtLeast(10, 15, 0)) OperatingSystem = SystemInfoOperatingSystem.MacOS1015;
                if (OperatingSystem.IsMacOSVersionAtLeast(11, 0, 0)) OperatingSystem = SystemInfoOperatingSystem.MacOS11;
                if (OperatingSystem.IsMacOSVersionAtLeast(12, 0, 0)) OperatingSystem = SystemInfoOperatingSystem.MacOS12;
                if (OperatingSystem.IsMacOSVersionAtLeast(13, 0, 0)) OperatingSystem = SystemInfoOperatingSystem.MacOS13;
            }
            else
            {
                // detect all linuxes
                OperatingSystem = SystemInfoOperatingSystem.Linux;
            }

            NCLogging.Log($"Operating system = {OperatingSystem}");
        }
    }
}
