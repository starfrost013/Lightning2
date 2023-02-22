namespace LightningGL
{
    /// <summary>
    /// Lightning Localisation Manager
    /// 
    /// Handles string localisation
    /// </summary>
    public static class LocalisationManager
    {
        /// <summary>
        /// The dictionary holding localisation strings.
        /// </summary>
        public static Dictionary<string, string> Strings { get; private set; }

        /// <summary>
        /// Internal: Class holding metadata for the localisation manager
        /// </summary>
        internal static LocalisationMetadata Metadata { get; set; }

        static LocalisationManager()
        {
            Strings = new Dictionary<string, string>();
            Metadata = new LocalisationMetadata();
        }

        internal static void Load()
        {
            // globalsettings loader checks for valid file
            IniFile? localisationIni = IniFile.Parse(GlobalSettings.GeneralLanguage);

            if (localisationIni == null)
            {
                Logger.LogError($"Error in localisation INI {GlobalSettings.GeneralLanguage}!", 31, LoggerSeverity.FatalError);
                return;
            }

            IniSection? metadataSection = localisationIni.GetSection("Metadata");

            IniSection? stringsSection = localisationIni.GetSection("Strings");

            if (metadataSection == null)
            {
                Logger.LogError($"Error in localisation INI {GlobalSettings.GeneralLanguage}: No metadata section!", 32, LoggerSeverity.FatalError);
                return;
            }

            if (stringsSection == null)
            {
                Logger.LogError($"Error in localisation INI {GlobalSettings.GeneralLanguage}: No strings section!", 33, LoggerSeverity.FatalError);
                return;
            }

            Metadata.Name = metadataSection.GetValue("Name");
            Metadata.Description = metadataSection.GetValue("Description");
            Metadata.Version = metadataSection.GetValue("Version");

            foreach (var value in stringsSection.Values)
            {
                // Set up all the strings
                Strings.Add(value.Key, value.Value);
            }

            Logger.Log($"Loaded language: {Metadata.Description} (version {Metadata.Version})");
        }

        public static string? GetString(string Key)
        {
            foreach (var Value in Strings)
            {
                if (Value.Key == Key)
                {
                    return Value.Value;
                }
            }

            return null;
        }

        public static string ProcessString(string stringProcess)
        {
            if (string.IsNullOrWhiteSpace(stringProcess)) return "";

            string start = "#[";
            string end = "]";

            string[] stringSplitWithHash = stringProcess.Split(start);

            // no strings that need to be processed
            if (stringSplitWithHash.Length == 0) return stringProcess;

            // remove the indicators
            stringProcess = stringProcess.Replace(start, "");
            stringProcess = stringProcess.Replace(end, "");

            foreach (string stringSplit in stringSplitWithHash)
            {
                if (stringSplit.Contains(end, StringComparison.InvariantCultureIgnoreCase)) // so that we don't try to "localise" unrelated parts of the string
                {
                    // and the same for parts after it
                    string[] stringNoEnd = stringSplit.Split(end);

                    string localisationTextId = stringNoEnd[0].Replace(end, ""); // remove the end

                    if (localisationTextId.Length != 0)
                    {
                        string? localisationString = GetString(localisationTextId);

                        if (localisationString == null)
                        {
                            Logger.LogError($"Invalid localisation string - cannot find localised string {localisationTextId}! The string will not be displayed.", 
                                35, LoggerSeverity.Warning, null, true);
                            stringProcess = $"Unlocalised string {localisationTextId}!";
                        }
                        else
                        {
                            stringProcess = stringProcess.ReplaceExact(localisationTextId, localisationString);
                        }
                    }
                }
            }

            return stringProcess;
        }
    }
}