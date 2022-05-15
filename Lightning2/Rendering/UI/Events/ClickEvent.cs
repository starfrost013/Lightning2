using static NuCore.SDL2.SDL;

namespace Lightning2
{
    /// <summary>
    /// ClickEvent
    /// 
    /// May 15, 2022
    /// 
    /// Defines a Lightning2 click event.
    /// </summary>
    /// <param name="sender">The <see cref="UIElement"/> that sent this event.</param>
    /// <param name="buttonNo">The <see cref="SDL_MouseButton"/> that has been clicked. </param>
    /// <param name="Repeat">If true, the key is currently being repeated - this is noNOT the same as held down!</param>
    public delegate void ClickEvent
    (
        UIElement sender,
        SDL_MouseButton buttonNo,
        bool Repeat
    );
}
