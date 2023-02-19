namespace LightningGL
{
    /// <summary>
    /// BindingControllerEvent
    /// 
    /// Defines a LightningGL input binding controller event.
    /// </summary>
    /// <param name="key">The <see cref="ControllerKeyboardMouse"/> on the keyboard that has been pressed.</param>
    public delegate void BindingControllerEvent
    (
        InputBinding binding
    );
}
