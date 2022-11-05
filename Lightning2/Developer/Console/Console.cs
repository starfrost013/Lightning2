namespace LightningGL
{
    /// <summary>
    /// Console 
    /// 
    /// Defines the main class for the inengine debugging console.
    /// </summary>
    public class Console
    {
        /// <summary>
        /// Exposed GlobalSettings.
        /// </summary>
        internal List<string> ExposedGlobalSettings { get; set; }

        internal void AddGlobalSettingToConsole(string globalSetting) => ExposedGlobalSettings.Add(globalSetting);
    }
}
