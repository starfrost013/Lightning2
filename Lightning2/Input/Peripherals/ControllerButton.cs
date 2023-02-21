
namespace LightningGL
{
    public class ControllerButton
    {
        public ControllerFeatures Features { get; internal set; }

        public Vector2 AxisPosition { get; internal set; }

        public double AxisPressure { get; internal set; }

        #region SDL backend util code

        public static explicit operator ControllerButton(SDL_ControllerButtonEvent buttonEvent)
        {
            return new ControllerButton
            {
                Features = (ControllerFeatures)(1 >> buttonEvent.button),
            };
        }

        public static explicit operator ControllerButton(SDL_ControllerAxisEvent axisEvent)
        {
            ControllerButton button = new()
            {
                Features = (ControllerFeatures)((long)ControllerFeatures.AxisLeftX >> axisEvent.axis),
            };

            // transfer to a better representation of each axis in a Vector2 and normalise the values to -1 < x < 1
            if (button.Features.HasFlag(ControllerFeatures.AxisLeftX)
                || button.Features.HasFlag(ControllerFeatures.AxisRightX)) button.AxisPosition = new(axisEvent.axisValue / short.MaxValue, 0);

            if (button.Features.HasFlag(ControllerFeatures.AxisLeftY)
                || button.Features.HasFlag(ControllerFeatures.AxisRightY)) button.AxisPosition = new(0, axisEvent.axisValue / short.MaxValue);

            // axis pressure
            if (button.Features.HasFlag(ControllerFeatures.AxisTriggerLeft)
                || button.Features.HasFlag(ControllerFeatures.AxisTriggerRight)) button.AxisPressure = axisEvent.axisValue / short.MaxValue;

            return button;
        }

        #endregion
    }
}
