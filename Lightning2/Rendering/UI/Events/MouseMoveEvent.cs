using static NuCore.SDL2.SDL;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// MouseMoveEvent
    /// 
    /// June 15, 2022
    /// 
    /// Defines a LightningGL event fired when the mouse is moved.
    /// </summary>
    /// <param name="position">The position of the last mouse move.</param> 
    public delegate void MouseMoveEvent
    (
        Vector2 position,
        Vector2 velocity, 
        SDL_MouseButton mouseButton
    );
}