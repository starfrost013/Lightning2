
namespace LightningGL
{
    internal class SetSceneCommand : ConsoleCommand
    {
        public override string Name => "setscene";

        private const string LOGGING_PREFIX = "Command: setscene";

        public override bool Execute(params string[] parameters)
        {
            if (parameters.Length < 1)
            {
                Logger.Log($"Not enough parameters")
            }

            string sceneName = parameters[0];

            SetCurrentScene(sceneName);
            return true; 
        }

        public override string Description => "setscene.\nChanges scene.\nParameters: Setting (string). The name of the scene to switch to";
    }
}
