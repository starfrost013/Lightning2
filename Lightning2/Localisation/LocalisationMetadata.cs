namespace LightningGL
{
    /// <summary>
    /// LocalisationMetadata
    /// 
    /// March 11, 2022
    /// 
    /// Holds metadata about the current locale
    /// </summary>
    public class LocalisationMetadata
    {
        /// <summary>
        /// The name of this language.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A brief description of the language.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The version of the localisation.
        /// </summary>
        public string Version { get; set; }
    }
}
