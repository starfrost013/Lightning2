namespace LightningGL
{
    /// <summary>
    /// RenderEvent
    /// 
    /// Defines a LightningGL event called each time the window is rendered.
    /// </summary>
    /// <param name="cRenderer">The window that is currently being rendered.</param>
    public delegate void RenderEvent
    (
        Renderer cRenderer
    );
}
