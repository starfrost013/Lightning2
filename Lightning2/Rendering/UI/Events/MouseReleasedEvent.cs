using static NuCore.SDL2.SDL;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// MouseReleasedEvent
    /// 
    /// June 12, 2022 (modified June 13, 2022)
    /// 
    /// Defines a LightningGL event triggered when a Gadget is no longer being clicked.
    /// </summary>
    /// <param name="buttonNo">The <see cref="SDL_MouseButton"/> that has been clicked. </param>
    /// <param name="position">The position of the last mouse click. </param> 
    public delegate void MouseReleasedEvent
    (
        SDL_MouseButton buttonNo,
        Vector2 position
    );
}
