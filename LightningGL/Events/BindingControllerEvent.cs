namespace LightningGL
{
    /// <summary>
    /// BindingControllerEvent
    /// 
    /// Defines a LightningGL input binding controller event.
    /// </summary>
    /// <param name="binding">The <see cref="InputBinding"/> on the keyboard that has been pressed.</param>
    public delegate void BindingControllerEvent
    (
        InputBinding? binding,
        ControllerButton button
    );
}
