using NuCore.Utilities;
using System.IO;

namespace LightningBase
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
        public static string Path => GlobalSettings.GeneralLocalSettingsPath;

        /// <summary>
        /// The local settings file - see <see cref="NCINIFile"/>.
        /// </summary>
        public static NCINIFile LocalSettingsFile { get; private set; }

        /// <summary>
        /// Determines if the local settings were changed.
        /// </summary>
        public static bool WasChanged { get; private set; }

        /// <summary>
        /// Loads the Local Settings.
        /// </summary>
        public static void Load()
        {
            if (!File.Exists(Path)
                && Path.IsValidPath())
            {
                NCLogging.Log($"LocalSettingsPath set but Local Settings INI file does not exist. Creating it...");
                File.Create(Path).Close(); // close it to prevent potential conflicts
            }

            LocalSettingsFile = NCINIFile.Parse(Path);
        }

        /// <summary>
        /// Saves the local settings.
        /// </summary>
        public static void Save()
        {
            // we already create it if it does not exist
            if (LocalSettingsFile == null)
            {
                NCError.ShowErrorBox($"Tried to save LocalSettings without creating it - set the LocalSettingsPath GlobalSettings first!", 170, NCErrorSeverity.Warning);
                return;
            }

            LocalSettingsFile.Write(Path);
            WasChanged = false; // don't save it automatically again
        }

        /// <summary>
        /// Adds a section to the local settings file.
        /// </summary>
        /// <param name="sectionName">The name of the section to add.</param>
        public static void AddSection(string sectionName)
        {
            // we already create it if it does not exist
            if (LocalSettingsFile == null)
            {
                NCError.ShowErrorBox($"Tried to edit LocalSettings without creating it - set the LocalSettingsPath GlobalSettings first!", 171, NCErrorSeverity.Warning);
                return;
            }

            LocalSettingsFile.Sections.Add(new NCINIFileSection(sectionName));
            WasChanged = true;
        }

        public static void DeleteSection(string sectionName)
        {
            // we already create it if it does not exist
            if (LocalSettingsFile == null)
            {
                NCError.ShowErrorBox($"Tried to edit LocalSettings without creating it - set the LocalSettingsPath GlobalSettings first!", 172, NCErrorSeverity.Warning);
                return;
            }

            LocalSettingsFile.Sections.Remove(LocalSettingsFile.GetSection(sectionName));
            WasChanged = true;
        }

        /// <summary>
        /// Adds a value to the local settings file.
        /// </summary>
        /// <param name="sectionName">The name of the setting to add to hte local settings file.</param>
        /// <param name="key">The key of the value to add.</param>
        /// <param name="value">The value of the value.</param>
        public static void AddValue(string sectionName, string key, string value)
        {
            // we already create it if it does not exist
            if (LocalSettingsFile == null)
            {
                NCError.ShowErrorBox($"Tried to edit LocalSettings without creating it - set the LocalSettingsPath GlobalSettings first!", 173, NCErrorSeverity.Warning);
                return;
            }

            NCINIFileSection section = LocalSettingsFile.GetSection(sectionName);

            section.Values.Add(key, value);
            WasChanged = true;
        }

        public static void SetValue(string sectionName, string key, string value)
        {
            // we already create it if it does not exist
            if (LocalSettingsFile == null)
            {
                NCError.ShowErrorBox($"Tried to edit LocalSettings without creating it - set the LocalSettingsPath GlobalSettings first!", 174, NCErrorSeverity.Warning);
                return;
            }

            NCINIFileSection section = LocalSettingsFile.GetSection(sectionName);

            section.Values[key] = value;
            WasChanged = true;
        }

        public static void DeleteKey(string sectionName, string key)
        {
            // we already create it if it does not exist
            if (LocalSettingsFile == null)
            {
                NCError.ShowErrorBox($"Tried to edit LocalSettings without creating it - set the LocalSettingsPath GlobalSettings first!", 175, NCErrorSeverity.Warning);
                return;
            }

            NCINIFileSection section = LocalSettingsFile.GetSection(sectionName);
            section.Values.Remove(key);
            WasChanged = true;
        }
    }
}
