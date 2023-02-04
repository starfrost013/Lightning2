
namespace LightningGL
{
    [Flags]
    /// <summary>
    /// ControllerFeatures 
    /// 
    /// Enumerates supported controller features.
    /// </summary>
    public enum ControllerFeatures : long
    {
        Invalid = 0,

        AButton = 0x1,

        BButton = 0x2,

        XButton = 0x4,

        YButton = 0x8,

        BackButton = 0x10,

        GuideButton = 0x20,

        StartButton = 0x40,

        LeftStickButton = 0x80,

        RightStickButton = 0x100,

        LeftShoulderButton = 0x200,

        RightShoulderButton = 0x400,

        DpadUp = 0x800,

        DpadDown = 0x1000,

        DpadLeft = 0x2000,

        DpadRight = 0x4000, 

        Misc1 = 0x8000,

        Paddle1 = 0x10000,

        Paddle2 = 0x20000,

        Paddle3 = 0x40000,

        Paddle4 = 0x80000,

        Touchpad = 0x100000,

        AxisLeftX = 0x200000,

        AxisLeftY = 0x400000,

        AxisRightX = 0x800000,

        AxisRightY = 0x1000000,
        
        AxisTriggerLeft = 0x2000000,

        AxisTriggerRight = 0x4000000,

        LED = 0x8000000,

        Rumble = 0x10000000,

        RumbleTriggers = 0x20000000,

        Sensor = 0x40000000,
    }
}
