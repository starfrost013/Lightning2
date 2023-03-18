
namespace LightningGL
{
    internal class SetSceneCommand : ConsoleCommand
    {
        public override string Name => "setscene";
        public override bool Execute(params string[] parameters)
        {
            string sceneName = parameters[0];

            Lightning.SetCurrentScene(sceneName);
            return true; 
        }

        public override string Description => "Changes scene\nParameters: Setting (string). The name of the scene to switch to";
    }
}
