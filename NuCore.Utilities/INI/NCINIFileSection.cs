using System.Collections.Generic;

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
            Key = Key.ToLowerInvariant();

            foreach (var kvp in Values)
            {
                string caseInsensitiveKey = kvp.Key.ToLowerInvariant();

                if (caseInsensitiveKey == Key)
                {
                    return kvp.Value;
                }
            }

            return null;
        }
    }
}
