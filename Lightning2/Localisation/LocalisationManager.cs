namespace LightningGL
{
    /// <summary>
    /// Lightning Localisation Manager
    /// 
    /// March 7, 2022 (modified June 15, 2022: correct variable naming convention)
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
            NCINIFile localisationIni = NCINIFile.Parse(GlobalSettings.Language);

            if (localisationIni == null) _ = new NCException($"Error in localisation INI {GlobalSettings.Language}!", 31, "LocalisationManager::Load call to NCINIFile::Parse failed", NCExceptionSeverity.FatalError);

            NCINIFileSection metadataSection = localisationIni.GetSection("Metadata");

            NCINIFileSection stringsSection = localisationIni.GetSection("Strings");

            if (metadataSection == null) _ = new NCException($"Error in localisation INI {GlobalSettings.Language}: No metadata section!", 32, "LocalisationManager::Load failed to obtain the Metadata section of a localisation file.", NCExceptionSeverity.FatalError);
            if (stringsSection == null) _ = new NCException($"Error in localisation INI {GlobalSettings.Language}: No strings section!", 33, "LocalisationManager.Load failed to obtain the Strings section of a localisation file.", NCExceptionSeverity.FatalError);

            Metadata.Name = metadataSection.GetValue("Name");
            Metadata.Description = metadataSection.GetValue("Description");
            Metadata.Version = metadataSection.GetValue("Version");

            foreach (var value in stringsSection.Values)
            {
                // Set up all the strings
                Strings.Add(value.Key, value.Value);
            }

            NCLogging.Log($"Loaded language: {Metadata.Description} (version {Metadata.Version})");
        }

        public static string GetString(string Key)
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
                if (stringSplit.Contains(end)) // so that we don't try to "localise" unrelated parts of the string
                {
                    // and the same for parts after it
                    string[] stringNoEnd = stringSplit.Split(end);

                    string localisationTextId = stringNoEnd[0].Replace(end, ""); // remove the end

                    if (localisationTextId.Length != 0)
                    {
                        string localisationString = GetString(localisationTextId);

                        if (localisationString == null) _ = new NCException($"Invalid localisation string - cannot find localised string {localisationTextId}!", 35, "LocalisationManager.ProcessString", NCExceptionSeverity.FatalError);

                        stringProcess = stringProcess.ReplaceExact(localisationTextId, localisationString);
                    }
                }
            }

            return stringProcess;
        }
    }
}