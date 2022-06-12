namespace Lightning2
{
    /// <summary>
    /// ClickEvent
    /// 
    /// May 15, 2022
    /// 
    /// Defines a Lightning2 keypress event.
    /// </summary>
    /// <param name="sender">The <see cref="UIElement"/> that sent this event.</param>
    /// <param name="key">The <see cref="Key"/> on the keyboard that has been pressed.</param>
    /// <param name="Repeat">If true, the key is currently being repeated - this is noNOT the same as held down!</param>
    public delegate void KeyPressedEvent
    (
        Key key,
        bool Repeat
    );
}
