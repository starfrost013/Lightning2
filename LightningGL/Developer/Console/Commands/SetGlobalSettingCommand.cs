
namespace LightningGL
{
    internal class SetGlobalSettingCommand : ConsoleCommand
    {
        public override string Name => "setgvar";

        private const string LOGGING_PREFIX = "Command: setgvar";

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

            IniSection? iniSection = GlobalSettings.IniFile.GetSection(sectionName);
            
            if (iniSection == null)
            {
                Logger.Log($"Invalid section name.\nValid values (case-insensitive): General, Graphics, Localisation, Requirements, Scene, Audio, Debug." +
                    $"\n\nUsage: {Description}", LOGGING_PREFIX);
                return false; 
            }

            iniSection.Values[key] = value;
            return true; 
        }

        public override string Description => "setgvar <section> <setting> <value>\nSets a global setting." +
            "\nParameters: Setting (string). The name of the global setting to use.\n" +
            "key (string): The name of the setting to set.\n" +
            "value (string): The value to set the setting referred to by the name parameter to." +
            "WARNING: ANY CHANGES YOU MAKE HERE WILL NOT TAKE EFFECT UNTIL YOU RESTART THE ENGINE!!!";
    }
}
