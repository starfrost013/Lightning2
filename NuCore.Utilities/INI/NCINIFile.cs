using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCIniFile
    /// 
    /// NuCore INI parser
    /// Pretty simple - simply uses the first character to determine token type
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

        // A rather naf INI parser.
        // Cba to tokenise

        /// <summary>
        /// Parses an INI file at <paramref name="Path"/>.
        /// </summary>
        /// <param name="Path">The path of the INI file that is to be parsed.</param>
        /// <returns></returns>
        /// <exception cref="NCException">An error occurred during the INI parsing. Extended error information is present in the <see cref="NCException.Description"/> property.</exception>
        public static NCINIFile Parse(string Path)
        {
            bool error_occurred = false; // for release builds where you continue past exceptions

            if (!File.Exists(Path)) throw new NCException($"INI parsing error: Cannot parse INI file at {Path}: File not found!", 21, "NCINIFile.Parse()", NCExceptionSeverity.Error);
            
            NCINIFile ini_file = new NCINIFile();   

            try
            {
                StreamReader ini_stream = new StreamReader(new FileStream(Path, FileMode.Open));

                while (!ini_stream.EndOfStream)
                {
                    string ini_line = ini_stream.ReadLine();
                    
                    ini_line = ini_line.Trim(); // trim leading spaces

                    if (ini_line.Length == 0) continue; // go to next line, empty line

                    char ini_line_char0 = ini_line[0];

                    switch (ini_line_char0)
                    {
                        case '[': // Section
                            if (ini_line.Contains(']'))
                            {
                                NCINIFileSection ini_section = new NCINIFileSection();

                                ini_file.CurSection = ini_section;
                                
                                int beginning = ini_line.IndexOf('[');
                                int end = ini_line.IndexOf(']');
                                
                                if (beginning > end)
                                {
                                    error_occurred = true;
                                    throw new NCException("INI parsing error: Invalid section entry - ] before [!", 25, "NCINIFile.Parse()", NCExceptionSeverity.Error);
                                }

                                // we add and remove 1 so that the [ and ] markers don't become part of the file name
                                // trim to remove leading spaces
                                ini_section.Name = ini_line.Substring(beginning + 1, ini_line.Length - (ini_line.Length - end) - 1);
                                ini_section.Name = ini_section.Name.Trim();

                                ini_file.Sections.Add(ini_section);

                            }
                            else
                            {
                                error_occurred = true;
                                throw new NCException("INI parsing error: Section name must terminate with [!", 24, "NCINIFile.Parse()", NCExceptionSeverity.Error);
                            }
                            continue;
                        case ';': // Comment
                            continue;
                        default: // Value
                            if (ini_line.Contains('='))
                            {
                                if (ini_file.CurSection == null)
                                {
                                    error_occurred = true;
                                    throw new NCException("INI parsing error: Values must be within a section!", 26, "NCINIFile.Parse()", NCExceptionSeverity.Error); 
                                }

                                string[] ini_value = ini_line.Split('=');

                                // we just ignore anything after the first one. this also works for comments i guess. as long as they are after
                                string ini_value_name = ini_value[0];
                                string ini_value_value = ini_value[1];

                                // strip characters that indicate strings

                                ini_value_value = ini_value_value.Replace("\"", "");

                                // trim to get rid of leading spaces etc
                                ini_value_name = ini_value_name.Trim();
                                ini_value_value = ini_value_value.Trim(); 


                                // add it to the values
                                ini_file.CurSection.Values.Add(ini_value_name, ini_value_value);
                            }
                            else
                            {
                                error_occurred = true;
                                throw new NCException("INI parsing error: INI item with no value!", 23, "NCINIFile.Parse()", NCExceptionSeverity.Error);
                            }
                            continue;
                    }
                }

                if (!error_occurred) return ini_file;
                
                // return null if we failed to parse
                return null;
            }
            catch (Exception ex)
            {
                throw new NCException($"INI parsing error: Cannot parse INI file at {Path}: \n\n{ex}", 22, "NCINIFile.Parse()", NCExceptionSeverity.Error);
            }
            
        }

        /// <summary>
        /// Acquires the INI file section with the name <paramref name="Name"/>
        /// </summary>
        /// <param name="Name">The name of the INI file section you wish to obtain.</param>
        /// <returns>A <see cref="NCINIFileSection"/> instance representing the INI file with section <see cref="Name"/> if the section could be found, otherwise null.</returns>
        public NCINIFileSection GetSection(string Name)
        {
            foreach (NCINIFileSection ini_section in Sections)
            {
                if (ini_section.Name == Name)
                {
                    return ini_section;
                }
            }

            return null; 
        }
    }
}
