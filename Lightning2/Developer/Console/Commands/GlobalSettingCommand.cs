
namespace LightningGL
{
    internal class GlobalSettingCommand : ConsoleCommand
    {
        public override string Name => "setgvar";
        public override void Execute(params object[] parameters)
        {
            
        }

        public override string Description => "Sets a global setting.\nParameters: Setting (string). The name of the global setting to use";
    }
}
