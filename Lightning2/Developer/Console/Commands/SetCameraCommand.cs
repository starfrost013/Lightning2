
namespace LightningGL
{
    internal class SetCameraCommand : ConsoleCommand
    {
        public override string Name => "setcamera";
        public override void Execute(params object[] parameters)
        {
            if (parameters.Length < 1)
            {
                //todo: error handling
                return;
            }

            CameraType? camType = (CameraType?)Enum.Parse(typeof(CameraType), (string)parameters[0]);
            
            switch (camType)
            {
                default:
                    // todo: logging
                    return;
                case CameraType.Follow:
                case CameraType.Chase:
                case CameraType.Floor:
                    NCLogging.Log("The command was accepted [placeholder]");
                    break;

            }
        }

        public override string Description => "Sets a global setting.\nParameters: Setting (string). The name of the global setting to use";
    }
}
