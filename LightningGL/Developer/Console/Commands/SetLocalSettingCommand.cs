
namespace LightningGL
{
    /// <summary>
    /// Command for setting local settings.
    /// </summary>
    internal class SetLocalSettingCommand : ConsoleCommand
    {
        public override string Name => "setlvar";

        private const string LOGGING_PREFIX = "Command: setlvar";

        public override bool Execute(params string[] parameters)
        {
            if (parameters.Length != 3)
            {
                Logger.Log($"Invalid number of parameters.\n\nUsage: {Description}", LOGGING_PREFIX);
                return false; 
            }

            string sectionName = parameters[0];
            string key = parameters[1];   
            string value = parameters[2];

            LocalSettings.SetValue(sectionName, key, value);
            return true; 
        }

        public override string Description => "setlvar <section name> <key> <value>\nSets a local setting.\n" +
            "Parameters: sectionName (string): The name of the section within LocalSettings.ini to set.\n" +
            "key (string): The name of the setting to set.\n" +
            "value (string): The value to set the setting referred to by the name parameter to.";
    }
}
