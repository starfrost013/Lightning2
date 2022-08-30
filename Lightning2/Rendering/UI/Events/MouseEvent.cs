namespace LightningGL
{
    /// <summary>
    /// MouseEvent
    /// 
    /// June 15, 2022 (modified August 30, 2022: Merge all MouseButton classes)
    /// 
    /// Defines a LightningGL event fired when the mouse is moved.
    /// </summary>
    /// <param name="button">The mouse that moved.</param> 
    public delegate void MouseEvent
    (
        MouseButton button
    );
}