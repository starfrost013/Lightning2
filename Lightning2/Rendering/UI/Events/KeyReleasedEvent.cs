namespace LightningGL
{
    /// <summary>
    /// KeyReleasedEvent
    /// 
    /// May 15, 2022 (modified July 10, 2022: Remove unnecessary "repeat" parameter, as it's already stored in the key field)
    /// 
    /// Defines a LightningGL key release event.
    /// This event is thrown on key release.
    /// </summary>
    /// <param name="key">The <see cref="Key"/> on the keyboard that has been released</param>
    /// <param name="Repeat">If true, the key is currently being repeated - this is NOT the same as held down!</param>
    public delegate void KeyReleaseEvent
    (
        Key key
    );
}
