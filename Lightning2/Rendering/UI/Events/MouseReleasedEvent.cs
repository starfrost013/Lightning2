using static NuCore.SDL2.SDL;

namespace LightningGL
{
    /// <summary>
    /// MouseReleasedEvent
    /// 
    /// June 12, 2022
    /// 
    /// Defines a LightningGL click event.
    /// </summary>
    /// <param name="sender">The <see cref="UIGadget"/> that sent this event.</param>
    /// <param name="buttonNo">The <see cref="SDL_MouseButton"/> that has been clicked. </param>
    public delegate void MouseReleasedEvent
    (
        SDL_MouseButton buttonNo
    );
}
