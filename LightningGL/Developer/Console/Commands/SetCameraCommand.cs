
namespace LightningGL
{
    /// <summary>
    /// SetCameraCommand
    /// 
    /// Command to configure the current camera.
    /// </summary>
    internal class SetCameraCommand : ConsoleCommand
    {
        public override string Name => "setcamera";

        private const string LOGGING_PREFIX = "Command: setcamera";

        public override bool Execute(params string[] parameters)
        {
            if (parameters.Length < 1)
            {
                Logger.Log($"Not enough parameters for command!\n\nUsage: {Description}", LOGGING_PREFIX);
                return false;
            }

            CameraType? camTypeValue = (CameraType?)Enum.Parse(typeof(CameraType), parameters[0], true);
            
            if (camTypeValue == null)
            {
                Logger.Log($"Invalid camera type ({parameters[0]})\n\nUsage: {Description}");
                return false;
            }

            float posXValue = default, posYValue = default,
                focusXValue = default, focusYValue = default,
                shakeXValue = default, shakeYValue = default;

            bool allowMoveValue = default;

            if (parameters.Length >= 3)
            {
                if (!float.TryParse(parameters[1], out posXValue)
                    || !float.TryParse(parameters[2], out posYValue))
                {
                    Logger.Log($"Invalid position value! ({parameters[1]},{parameters[2]})\n\nUsage: {Description}");
                    return false; 
                }
            }

            if (parameters.Length >= 5)
            {
                if (!float.TryParse(parameters[3], out focusXValue)
                    || !float.TryParse(parameters[4], out focusYValue))
                {
                    Logger.Log($"Invalid focus value ({parameters[3]},{parameters[4]})\n\nUsage: {Description}");
                    return false;
                }
            }

            if (parameters.Length >= 7)
            {
                if (!float.TryParse(parameters[5], out shakeXValue)
                    || !float.TryParse(parameters[6], out shakeYValue))
                {
                    Logger.Log($"Invalid shake value ({parameters[5]},{parameters[6]})\n\nUsage: {Description}");
                    return false;
                }
            }

            if (parameters.Length >= 8)
            {
                if (!bool.TryParse(parameters[7], out allowMoveValue))
                {
                    Logger.Log($"Invalid allow movement value ({parameters[7]})\n\nUsage: {Description}");
                    return false;
                }
            }

            // we already checked it cannot be null
            // default value for those that failed to parse is 0
            Camera camera = new((CameraType)camTypeValue)
            {
                Position = new(posXValue, posYValue),
                FocusDelta = new(focusXValue, focusYValue),
                ShakeAmount = new(shakeXValue, shakeYValue),
                AllowCameraMoveOnShake = allowMoveValue,
            };

            return true; 
        }

        public override string Description => "setcamera <CameraType> <x pos> <y pos> <x vel> <y vel> <x focus> <y focus> <x shake> <y shake> <allow move>" +
            "\nSets the camera type and parameters.\nParameters: CameraType (Follow, Ignore, Chase): the type of the camera to set.\n" +
            "<x pos, y pos>: Position of the camera in world space.\n" +
            "<x focus, y focus>: Position, relative to the screen - where 0,0 is the top left corner of the screen - the camera will focus on.\n" +
            "<x shake, y shake>: Amount the camera is allowed to shake by.\n" +
            "<allow move>: Determines if the camera is allowed to move while it shakes.";
    }
}
