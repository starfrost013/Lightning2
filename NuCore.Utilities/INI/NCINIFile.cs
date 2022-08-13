using System;
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
        /// <exception cref="NCException">An error occurred during the INI parsing. Extended error information is present in the <see cref="NCException.Description"/> property.</exception>
        public static NCINIFile Parse(string path)
        {
            if (!File.Exists(path)) _ = new NCException($"INI parsing error: Cannot parse INI file at {path}: File not found!", 21, "NCINIFile::Parse could not find file", NCExceptionSeverity.Error);

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


                        // Check various 
                        switch (iniLineChar0)
                        {
                            case '[': // Section
                                if (iniLine.Contains(']'))
                                {
                                    NCINIFileSection iniSection = new NCINIFileSection();

                                    iniFile.CurSection = iniSection;

                                    int beginning = iniLine.IndexOf('[');
                                    int end = iniLine.IndexOf(']');

                                    if (beginning > end)
                                    {
                                        _ = new NCException("INI parsing error: Invalid section entry - ] before [!", 25, "NCINIFile::Parse", NCExceptionSeverity.Error);
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
                                    _ = new NCException("INI parsing error: Section name must terminate with [!", 24, "NCINIFile::Parse", NCExceptionSeverity.Error);
                                    return null;
                                }
                                continue;
                            case ';': // Comment
                            case '\n': // Newline (if anyone wishes to add padding)
                            case '\r': // Windows newline (just in case)
                                continue;
                            default: // Value
                                if (iniLine.Contains('='))
                                {
                                    if (iniFile.CurSection == null)
                                    {
                                        _ = new NCException("INI parsing error: Values must be within a section!", 26, "NCINIFile::Parse", NCExceptionSeverity.Error);
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
                                    _ = new NCException("INI parsing error: An INI item with no value was found!", 23, "NCINIFile::Parse", NCExceptionSeverity.Error);
                                    return null;
                                }
                                continue;
                        }
                    }

                }

                // close the stream
                iniStream.Close();
                // in case of success
                return iniFile;
            }
            catch (Exception ex)
            {
                _ = new NCException($"INI parsing error: Cannot parse INI file at {path}: \n\n{ex}", 22, "NCINIFile::Parse - unknown exception occurred", NCExceptionSeverity.Error);
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
                _ = new NCException($"Error writing to INi: {ex.Message}", 110, "An exception occurred in NCINIFile::Write", NCExceptionSeverity.Error);
                return false;
            }
        }

        /// <summary>
        /// Acquires the INI file section with the name <paramref name="Name"/>
        /// </summary>
        /// <param name="Name">The name of the INI file section you wish to obtain.</param>
        /// <returns>A <see cref="NCINIFileSection"/> instance representing the INI file with section <see cref="Name"/> if the section could be found, otherwise null.</returns>
        public NCINIFileSection GetSection(string Name)
        {
            foreach (NCINIFileSection iniSection in Sections)
            {
                if (iniSection.Name == Name)
                {
                    return iniSection;
                }
            }

            return null;
        }
    }
}
