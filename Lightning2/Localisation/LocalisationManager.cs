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
    public class LocalisationManager
    {
        public Dictionary<string, string> Strings { get; set; }

        private NCINIFile IniFile { get; set; }

        /// <summary>
        /// The language INI currently in use. An INI file. 
        /// </summary>
        public string LanguageFile { get; private set; }

        public LocalisationManager()
        {
            Strings = new Dictionary<string, string>();
        }

        
    }
}
