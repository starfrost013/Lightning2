namespace LightningGL
{
    /// <summary>
    /// ClickEvent
    /// 
    /// May 15, 2022
    /// 
    /// Defines a LightningGL key release event.
    /// This event is thrown on key release.
    /// </summary>
    /// <param name="sender">The <see cref="UIElement"/> that sent this event.</param>
    /// <param name="key">The <see cref="Key"/> on the keyboard that has been released</param>
    /// <param name="Repeat">If true, the key is currently being repeated - this is NOT the same as held down!</param>
    public delegate void KeyReleaseEvent
    (
        Key key,
        bool Repeat
    );
}
