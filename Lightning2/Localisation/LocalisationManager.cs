using NuCore.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// Lightning2 Localisation Manager
    /// 
    /// March 7, 2022
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
            NCINIFile loc_ini = NCINIFile.Parse(GlobalSettings.LocalisationFile);

            if (loc_ini == null) throw new NCException($"Error in localisation INI {GlobalSettings.LocalisationFile}!", 31, "LocalisationManager.Load", NCExceptionSeverity.FatalError);

            NCINIFileSection meta_section = loc_ini.GetSection("Metadata");

            NCINIFileSection strings_section = loc_ini.GetSection("Strings");

            if (meta_section == null) throw new NCException($"Error in localisation INI {GlobalSettings.LocalisationFile}: No metadata section!", 32, "LocalisationManager.Load", NCExceptionSeverity.FatalError);
            if (strings_section == null) throw new NCException($"Error in localisation INI {GlobalSettings.LocalisationFile}: No strings section!", 33, "LocalisationManager.Load", NCExceptionSeverity.FatalError);

            Metadata.Name = meta_section.GetValue("Name");
            Metadata.Description = meta_section.GetValue("Description");
            Metadata.Version = meta_section.GetValue("Version");


            foreach (var Values in strings_section.Values)
            {
                // Set up all the strings
                Strings.Add(Values.Key, Values.Value);  
            }
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

        public static string ProcessString(string StringProcess)
        {
            string start = "#{";
            string end = "}";

            string[] string_split_with_hash = StringProcess.Split(start);

            if (string_split_with_hash.Length == 0) return StringProcess; // no strings that need to be processed

            // remove the indicators
            StringProcess = StringProcess.Replace(start, "");
            StringProcess = StringProcess.Replace(end, "");

            foreach (string string_split in string_split_with_hash)
            {
                if (string_split.Contains(end)) // so that we don't try to "localise" unrelated parts of the string
                {
                    // and the same for parts after it
                    string[] string_no_end = string_split.Split(end);

                    string string_to_be_replaced = string_no_end[0].Replace(end, ""); // remove the end

                    if (string_to_be_replaced.Length != 0)
                    {
                        string loc_string = GetString(string_to_be_replaced);

                        if (loc_string == null) throw new NCException($"Invalid localisation string - cannot find localised string {string_to_be_replaced}!", 35, "LocalisationManager.ProcessString", NCExceptionSeverity.FatalError);

                        StringProcess = StringProcess.Replace(string_to_be_replaced, loc_string);
                    }
                }


            }

            return StringProcess; 
        }
    }
}