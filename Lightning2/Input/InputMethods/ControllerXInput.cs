

namespace LightningGL
{
    /// <summary>
    /// 
    /// </summary>
    public class ControllerXinput : InputMethod
    {
        private nint _handle;

        public ControllerFeatures Features { get; private set; }    

        public string? Name { get; private set; }

        public ushort VendorID { get; private set; }

        public ushort ProductID { get; private set; }

        internal override bool DetectPresence()
        {
            NCLogging.Log("Detecting game controller...", "Device Detection");
            _handle = SDL_GameControllerOpen(0); // hardcode first controller for now

            if (_handle == IntPtr.Zero)
            {
                NCLogging.Log("Game controller not connected!", "Device Detection");
                return false;
            }

            // feature detection
            NCLogging.Log("Detecting features...", "Device Detection");
            // buttons
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.AButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.BButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.XButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.YButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_BACK) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.BackButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_GUIDE) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.GuideButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_START) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.StartButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSTICK) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.LeftStickButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSTICK) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.RightStickButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSHOULDER) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.LeftShoulderButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSHOULDER) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.RightShoulderButton;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_UP) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.DpadUp;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_DOWN) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.DpadDown;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_LEFT) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.DpadLeft;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_RIGHT) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.DpadRight;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_MISC1) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.Misc1;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_PADDLE1) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.Paddle1;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_PADDLE2) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.Paddle2;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_PADDLE3) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.Paddle3;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_PADDLE4) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.Paddle4;
            if (SDL_GameControllerHasButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_TOUCHPAD) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.Touchpad;

            // axes
            if (SDL_GameControllerHasAxis(_handle, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTX) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.AxisLeftX;
            if (SDL_GameControllerHasAxis(_handle, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTY) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.AxisLeftY;
            if (SDL_GameControllerHasAxis(_handle, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTX) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.AxisRightX;
            if (SDL_GameControllerHasAxis(_handle, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTY) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.AxisRightY;
            if (SDL_GameControllerHasAxis(_handle, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.AxisTriggerLeft;
            if (SDL_GameControllerHasAxis(_handle, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.AxisTriggerRight;

            // misc features
            if (SDL_GameControllerHasLED(_handle) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.LED;
            if (SDL_GameControllerHasRumble(_handle) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.Rumble;
            if (SDL_GameControllerHasRumbleTriggers(_handle) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.RumbleTriggers;
            if (SDL_GameControllerHasSensor(_handle, SDL_SensorType.SDL_SENSOR_ACCEL) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.Accelerometer;
            if (SDL_GameControllerHasSensor(_handle, SDL_SensorType.SDL_SENSOR_GYRO) == SDL_bool.SDL_TRUE) Features |= ControllerFeatures.Gyroscope;

            Name = SDL_GameControllerName(_handle);
            NCLogging.Log($"Name = {Name}");

            VendorID = SDL_GameControllerGetVendor(_handle);


            return true;
        }
    }
}
