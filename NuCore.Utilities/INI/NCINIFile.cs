﻿using System;
using System.Collections.Generic;
using System.IO;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCIniFile
    /// 
    /// NuCore INI parser
    /// Pretty simple - simply uses the first character to determine token type
    /// 
    /// Written February 2022
    /// Updated July 2, 2022 in order to handle comments on the same line as values, handle newlines and rename variables to camelCase
    /// Updated August 2, 2022 to fix a bug with INI comments on the same line as values in non-section lines, as well as to make searches case-insensitive.
    /// Updated August 9, 2022 to add serialisation to file.
    /// </summary>
    public class NCINIFile
    {
        /// <summary>
        /// The sections of this INI file. 
        /// </summary>
        public List<NCINIFileSection> Sections { get; set; }

        /// <summary>
        /// Parser private: used for parsing the current section.
        /// </summary>
        private NCINIFileSection CurSection { get; set; }

        public NCINIFile()
        {
            Sections = new List<NCINIFileSection>();
        }

        /// <summary>
        /// Parses an INI file at <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path of the INI file that is to be parsed.</param>
        /// <returns></returns>
        /// <exception cref="NCError">An error occurred during the INI parsing. Extended error information is present in the <see cref="NCError.Description"/> property.</exception>
        public static NCINIFile Parse(string path)
        {
            if (!File.Exists(path)) NCError.Throw($"INI parsing error: Cannot parse INI file at {path}: File not found!", 21, "NCINIFile::Parse could not find file", NCErrorSeverity.Error);

            NCINIFile iniFile = new NCINIFile();

            try
            {
                StreamReader iniStream = new StreamReader(new FileStream(path, FileMode.Open));

                while (!iniStream.EndOfStream)
                {
                    string iniLine = iniStream.ReadLine();

                    // trim leading and trailing spaces
                    // so first character detection works
                    iniLine = iniLine.Trim();

                    if (iniLine.Length > 0)
                    {
                        char iniLineChar0 = iniLine[0];

                        // Handle comments on the same line after the value
                        string[] iniLineComments = iniLine.Split(';');

                        // if there ARE comments...
                        if (iniLineChar0 != ';'
                            && iniLineComments.Length > 1)
                        {
                            // if the line starts with a ; we will have already ignored it earlier
                            // so simply cut off the comments
                            iniLine = iniLineComments[0];
                            iniLine = iniLine.Trim(); // trim again
                        }

                        // Check potential starts of a line to determine what type of INI content we are referencing
                        switch (iniLineChar0)
                        {
                            case '[': // Section
                                if (iniLine.Contains(']', StringComparison.InvariantCultureIgnoreCase))
                                {
                                    NCINIFileSection iniSection = new NCINIFileSection();

                                    iniFile.CurSection = iniSection;

                                    int beginning = iniLine.IndexOf('[');
                                    int end = iniLine.IndexOf(']');

                                    if (beginning > end)
                                    {
                                        NCError.Throw("INI parsing error: Invalid section entry - ] before [!", 25, "NCINIFile::Parse", NCErrorSeverity.Error);
                                        return null;
                                    }

                                    // we add and remove 1 so that the [ and ] markers don't become part of the file name
                                    // trim to remove leading spaces
                                    iniSection.Name = iniLine.Substring(beginning + 1, iniLine.Length - (iniLine.Length - end) - 1);
                                    iniSection.Name = iniSection.Name.Trim();

                                    iniFile.Sections.Add(iniSection);
                                }
                                else
                                {
                                    NCError.Throw("INI parsing error: Section name must terminate with ]!", 24, "NCINIFile::Parse", NCErrorSeverity.Error);
                                    return null;
                                }
                                continue;
                            case ';': // Comment
                            case '\n': // Newline (if anyone wishes to add padding)
                            case '\r': // Windows newline (just in case)
                                continue;
                            default: // Value
                                if (iniLine.Contains('=', StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (iniFile.CurSection == null)
                                    {
                                        NCError.Throw("INI parsing error: Values must be within a section!", 26, "NCINIFile::Parse", NCErrorSeverity.Error);
                                        return null;
                                    }

                                    string[] iniValue = iniLine.Split('=');

                                    // we just ignore anything after the first one. this also works for comments that are on the same line
                                    string iniValueKey = iniValue[0];
                                    string iniValueValue = iniValue[1];

                                    // strip characters that indicate strings

                                    iniValueValue = iniValueValue.Replace("\"", "");

                                    // trim to get rid of leading spaces etc
                                    iniValueKey = iniValueKey.Trim();
                                    iniValueValue = iniValueValue.Trim();


                                    // add it to the values
                                    iniFile.CurSection.Values.Add(iniValueKey, iniValueValue);
                                }
                                else
                                {
                                    NCError.Throw("INI parsing error: An INI item with no value was found!", 23, "NCINIFile::Parse", NCErrorSeverity.Error);
                                    return null;
                                }
                                continue;
                        }
                    }

                }

                // in case of success, close the stream and return
                // close the stream
                iniStream.Close();
                return iniFile;
            }
            catch (Exception ex)
            {
                NCError.Throw($"INI parsing error: Cannot parse INI file at {path}: \n\n{ex}", 22, "NCINIFile::Parse - unknown exception occurred", NCErrorSeverity.Error);
                return null;
            }
        }

        /// <summary>
        /// Writes the content of this <see cref="NCINIFile"/> to a physical .ini file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Write(string path)
        {
            List<string> iniLines = new List<string>();

            foreach (NCINIFileSection section in Sections)
            {
                iniLines.Add($"[{section.Name}]");

                foreach (KeyValuePair<string, string> item in section.Values)
                {
                    iniLines.Add($"{item.Key} = \"{item.Value}\"");
                }
            }

            string[] iniLineArray = iniLines.ToArray();

            // write to file

            try
            {
                File.WriteAllLines(path, iniLineArray);
                return true;
            }
            catch (Exception ex)
            {
                NCError.Throw($"Error writing to INI: {ex.Message}", 110, "An exception occurred in NCINIFile::Write", NCErrorSeverity.Error);
                return false;
            }
        }

        /// <summary>
        /// Acquires the INI file section with the name <paramref name="name"/>
        /// </summary>
        /// <param name="name">The name of the INI file section you wish to obtain.</param>
        /// <returns>A <see cref="NCINIFileSection"/> instance representing the INI file with section <see cref="Name"/> if the section could be found, otherwise null.</returns>
        public NCINIFileSection GetSection(string name)
        {
            foreach (NCINIFileSection iniSection in Sections)
            {
                if (iniSection.Name == name)
                {
                    return iniSection;
                }
            }

            return null;
        }
    }
}
