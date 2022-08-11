using static NuCore.SDL2.SDL;

namespace LightningGL
{
    /// <summary>
    /// MousePressedEvent
    /// 
    /// June 12, 2022 (modified August 11, 2022)
    /// 
    /// Defines a LightningGL event fired when the user clicks on a Gadget.
    /// </summary>
    /// <param name="button">The mouse that moved.</param>
    public delegate void MousePressedEvent
    (
        MouseButton button
    );
}
