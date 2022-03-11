using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCIniFileSection
    /// 
    /// March 7, 2022
    /// 
    /// Defines an MC INI file section.
    /// </summary>
    public class NCINIFileSection
    {
        public Dictionary<string, string> Values { get; set; }

        public string Name { get; set; }

        public NCINIFileSection()
        {
            Values = new Dictionary<string, string>();
        }

        public string GetValue(string Key)
        {
            foreach (var kvp in Values)
            {
                if (kvp.Key == Key)
                {
                    return kvp.Value;
                }
            }

            return null;
        }
    }
}
