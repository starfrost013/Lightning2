using static NuCore.SDL2.SDL;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// MousePressedEvent
    /// 
    /// June 12, 2022 (modified June 13, 2022)
    /// 
    /// Defines a LightningGL event fired when the user clicks on a Gadget.
    /// </summary>
    /// <param name="buttonNo">The <see cref="SDL_MouseButton"/> that has been clicked. </param>
    /// <param name="position">The position of the last mouse click. </param> 
    public delegate void MousePressedEvent
    (
        SDL_MouseButton buttonNo,
        Vector2 position
    );
}
