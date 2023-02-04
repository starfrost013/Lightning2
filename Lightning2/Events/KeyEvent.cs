namespace LightningGL
{
    /// <summary>
    /// KeyPressedEvent
    /// 
    /// May 15, 2022 (modified July 10, 2022: remove unnecessary "repeat" parameter, as it's already stored in the key class)
    /// 
    /// Defines a LightningGL keypress event.
    /// </summary>
    /// <param name="key">The <see cref="Keyboard"/> on the keyboard that has been pressed.</param>
    public delegate void KeyEvent
    (
        Keyboard key
    );
}
