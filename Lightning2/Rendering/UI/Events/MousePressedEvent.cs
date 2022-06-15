using static NuCore.SDL2.SDL;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// MousePressedEvent
    /// 
    /// June 12, 2022 (modified June 13, 2022)
    /// 
    /// Defines a LightningGL click event.
    /// </summary>
    /// <param name="sender">The <see cref="UIGadget"/> that sent this event.</param>
    /// <param name="buttonNo">The <see cref="SDL_MouseButton"/> that has been clicked. </param>
    public delegate void MousePressedEvent
    (
        SDL_MouseButton buttonNo,
        Vector2 position
    );
}
