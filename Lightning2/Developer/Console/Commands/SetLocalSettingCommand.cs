
namespace LightningGL
{
    internal class SetLocalSettingCommand : ConsoleCommand
    {
        public override string Name => "setlvar";
        public override bool Execute(params string[] parameters)
        {
            Logger.Log("SetLocalSettingCommand NOT YET IMPLEMENTED");
            return true; 
        }

        public override string Description => "Sets a local setting.\nParameters: Setting (string). The name of the global setting to use";
    }
}
