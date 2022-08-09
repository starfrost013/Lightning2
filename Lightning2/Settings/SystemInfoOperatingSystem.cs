using System;


namespace LightningGL
{
    /// <summary>
    /// SystemInfoOperatingSystem
    /// 
    /// Defines system information (operating system). Enumerates operating systems compatible with Lightning
    /// </summary>
    public enum SystemInfoOperatingSystem
    {
        /// <summary>
        /// Microsoft Windows 7
        /// </summary>
        Win7 = 0,

        /// <summary>
        /// Microsoft Windows 8
        /// </summary>
        Win8 = 1,

        /// <summary>
        /// Microsoft Windows 8.1
        /// </summary>
        Win81 = 2,

        /// <summary>
        /// Microsoft Windows 10, Threshold 1 (1507 release)
        /// </summary>
        Win10TH1 = 3,

        /// <summary>
        /// Microsoft Windows 10, Threshold 2 (1511 release)
        /// </summary>
        Win10TH2 = 4,  

        /// <summary>
        /// Microsoft Windows 10, Redstone 1 (1607 release)
        /// </summary>
        Win10RS1 = 5,

        /// <summary>
        /// Microsoft Windows 10, Redstone 2 (1703 release)
        /// </summary>
        Win10RS2 = 6,

        /// <summary>
        /// Microsoft Windows 10, Redstone 3 (1709 release)
        /// </summary>
        Win10RS3 = 7,

        /// <summary>
        /// Microsoft Windows 10, Redstone 4 (1803 release)
        /// </summary>
        Win10RS4 = 8,

        /// <summary>
        /// Microsoft Windows 10, Redstone 5 (1809 release)
        /// </summary>
        Win10RS5 = 9,

        /// <summary>
        /// Microsoft Windows 10, 19H1 (1903 release)
        /// </summary>
        Win1019H1 = 10,

        /// <summary>
        /// Microsoft Windows 10, 19H2 (1909 release)
        /// </summary>
        Win1019H2 = 11,    

        /// <summary>
        /// Microsoft Windows 10, Vibranium (2003 release)
        /// </summary>
        Win1020H1 = 12,

        /// <summary>
        /// Microsoft Windows 10, 20H2
        /// </summary>
        Win1020H2 = 13,

        /// <summary>
        /// Microsoft Windows 10, 21H1
        /// </summary>
        Win1021H1 = 14,

        /// <summary>
        /// Microsoft Windows 10, 21H2
        /// </summary>
        Win1021H2 = 15,

        /// <summary>
        /// Microsoft Windows 10, 22H2
        /// </summary>
        Win1022H2 = 16,

        /// <summary>
        /// Manganese (Windows Dev Channel 2019)
        /// </summary>
        Manganese = 20,

        /// <summary>
        /// Windows Server 2022 (Iron)
        /// </summary>
        Iron = 21,

        /// <summary>
        /// Windows 11 (Cobalt)
        /// </summary>
        Win11 = 22,

        /// <summary>
        /// Windows 11, version 22H2 (Nickel)
        /// </summary>
        Nickel = 23,

        /// <summary>
        /// Copper (Windows Dev Channel 2022-23)
        /// </summary>
        Copper = 24,

        /// <summary>
        /// OSX 10.13 (High Sierra)
        /// </summary>
        MacOS1013 = 50,

        /// <summary>
        /// OSX 10.14 (Mojave)
        /// </summary>
        MacOS1014 = 51,

        /// <summary>
        /// OSX 10.15 (Catalina)
        /// </summary>
        MacOS1015 = 52,

        /// <summary>
        /// OSX 11.0 (Big Sur)
        /// </summary>
        MacOS11 = 53,

        /// <summary>
        /// OSX 12.0 (Monterey)
        /// </summary>
        MacOS12 = 54,

        /// <summary>
        /// OSX 13.0 (Ventura)
        /// </summary>
        MacOS13 = 55,

        /// <summary>
        /// Don't bother doing Linux kernel detection until we can figure out how osversion behaves
        /// </summary>
        Linux = 100,
    }
}
