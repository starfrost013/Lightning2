namespace LightningGL
{
    /// <summary>
    /// KeyPressedEvent
    /// 
    /// May 15, 2022 (modified July 10, 2022: remove unnecessary "repeat" parameter, as it's already stored in the key class)
    /// 
    /// Defines a LightningGL keypress event.
    /// </summary>
    /// <param name="key">The <see cref="Key"/> on the keyboard that has been pressed.</param>
    /// <param name="Repeat">If true, the key is currently being repeated - this is noNOT the same as held down!</param>
    public delegate void KeyPressedEvent
    (
        Key key
    );
}
