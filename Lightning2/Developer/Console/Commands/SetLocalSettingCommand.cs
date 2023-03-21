
namespace LightningGL
{
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
            Logger.Log("SetLocalSettingCommand NOT YET IMPLEMENTED");
            return true; 
        }

        public override string Description => "setlvar.\nSets a local setting.\nParameters: Setting (string). The name of the global setting to use";
    }
}
