using NuCore.Utilities;
using System;

namespace LightningGL
{
    /// <summary>
    /// LocalSettings
    /// 
    /// August 9, 2022
    /// 
    /// APIs for game-specific (local) settings
    /// </summary>
    public static class LocalSettings
    {
        /// <summary>
        /// Path to the game settings file.
        /// </summary>
        public static string Path => GlobalSettings.LocalSettingsPath;

        /// <summary>
        /// The local settings file - see <see cref="NCINIFile"/>.
        /// </summary>
        public static NCINIFile LocalSettingsFile { get; private set; }

        /// <summary>
        /// Loads the Local Settings.
        /// </summary>
        internal static void Load()
        {
            LocalSettingsFile = NCINIFile.Parse(Path);
        }

        /// <summary>
        /// Saves the LocaL sETTINGS.
        /// </summary>
        public static void Save()
        {
            LocalSettingsFile.Write(Path);
        }
    }
}
