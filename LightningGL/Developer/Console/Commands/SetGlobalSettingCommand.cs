
namespace LightningGL
{
    internal class SetGlobalSettingCommand : ConsoleCommand
    {
        public override string Name => "setgvar";
        public override bool Execute(params string[] parameters)
        {
            Logger.Log("SetGlobalSettingCommand NOT YET IMPLEMENTED");
            return true; 
        }

        public override string Description => "Sets a global setting.\nParameters: Setting (string). The name of the global setting to use";
    }
}
