using NuCore.Utilities;
using System.Collections.Generic;

namespace LightningGL
{
    /// <summary>
    /// LightningGL Localisation Manager
    /// 
    /// March 7, 2022 (modified June 15, 2022: correct variable naming convention)
    /// 
    /// Handles string localisation
    /// </summary>
    public static class LocalisationManager
    {
        public static Dictionary<string, string> Strings { get; set; }

        public static LocalisationMetadata Metadata { get; set; }

        static LocalisationManager()
        {
            Strings = new Dictionary<string, string>();
            Metadata = new LocalisationMetadata();
        }

        public static void Load()
        {
            // globalsettings loader checks for valid file
            NCINIFile localisationIni = NCINIFile.Parse(GlobalSettings.LocalisationFile);

            if (localisationIni == null) throw new NCException($"Error in localisation INI {GlobalSettings.LocalisationFile}!", 31, "LocalisationManager.Load", NCExceptionSeverity.FatalError);

            NCINIFileSection metaSection = localisationIni.GetSection("Metadata");

            NCINIFileSection stringsSection = localisationIni.GetSection("Strings");

            if (metaSection == null) throw new NCException($"Error in localisation INI {GlobalSettings.LocalisationFile}: No metadata section!", 32, "LocalisationManager.Load", NCExceptionSeverity.FatalError);
            if (stringsSection == null) throw new NCException($"Error in localisation INI {GlobalSettings.LocalisationFile}: No strings section!", 33, "LocalisationManager.Load", NCExceptionSeverity.FatalError);

            Metadata.Name = metaSection.GetValue("Name");
            Metadata.Description = metaSection.GetValue("Description");
            Metadata.Version = metaSection.GetValue("Version");

            foreach (var value in stringsSection.Values)
            {
                // Set up all the strings
                Strings.Add(value.Key, value.Value);
            }

#if DEBUG
            NCLogging.Log($"Loaded language: {Metadata.Description} (Version {Metadata.Version})");
#endif
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

                        if (localisationString == null) throw new NCException($"Invalid localisation string - cannot find localised string {localisationTextId}!", 35, "LocalisationManager.ProcessString", NCExceptionSeverity.FatalError);

                        stringProcess = stringProcess.Replace(localisationTextId, localisationString);
                    }
                }
            }

            return stringProcess;
        }
    }
}