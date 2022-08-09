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

        public static NCINIFile LocalSettingsFile { get; private set; }

        public static void Load()
        {
            LocalSettingsFile = NCINIFile.Parse(Path);
        }

        public static void Save()
        {
            LocalSettingsFile.Write(Path);
        }
    }
}
