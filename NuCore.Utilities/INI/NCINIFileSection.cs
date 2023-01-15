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
        /// <summary>
        /// The values of this section.
        /// </summary>
        public Dictionary<string, string> Values { get; set; }

        /// <summary>
        /// The name of this section.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor for <see cref="NCINIFileSection"/> with a parameter for the section name.
        /// </summary>
        public NCINIFileSection(string name)
        {
            Name = name;
            Values = new();
        }

        /// <summary>
        /// Gets the value with the key <see cref="key"/>
        /// </summary>
        /// <param name="key">The INI key contained within the section.</param>
        /// <returns>A string containing the value corresponding to the key <paramref name="key"/>.</returns>
        public string? GetValue(string key)
        {
            key = key.ToLowerInvariant();

            foreach (var kvp in Values)
            {
                string caseInsensitiveKey = kvp.Key.ToLowerInvariant();

                if (caseInsensitiveKey == key)
                {
                    return kvp.Value;
                }
            }

            return null;
        }
    }
}
