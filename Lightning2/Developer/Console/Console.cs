namespace LightningGL
{
    /// <summary>
    /// Console 
    /// 
    /// Defines the main class for the inengine debugging console.
    /// The console has the capability to set any arbitrary registered global settings,
    /// as well as execute commands that inherit from the <see cref="ConsoleCommand"/> class.
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
