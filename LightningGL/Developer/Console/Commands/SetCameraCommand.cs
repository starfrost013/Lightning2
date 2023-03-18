
namespace LightningGL
{
    internal class SetCameraCommand : ConsoleCommand
    {
        public override string Name => "setcamera";
        public override bool Execute(params string[] parameters)
        {
            if (parameters.Length < 1)
            {
                //todo: error handling
                return false;
            }

            CameraType? camType = (CameraType?)Enum.Parse(typeof(CameraType), parameters[0]);
            
            switch (camType)
            {
                default:
                    // todo: logging
                    return false;
                case CameraType.Follow:
                case CameraType.Chase:
                case CameraType.Floor:
                    Logger.Log("The command was accepted [placeholder]");
                    break;

            }

            return true; 
        }

        public override string Description => "Sets a global setting.\nParameters: Setting (string). The name of the global setting to use";
    }
}
