

namespace Lightning2
{
    /// <summary>
    /// UIElement
    /// 
    /// May 13, 2022 (modified May 14, 2022)
    /// </summary>
    public class UIElement
    {
        /// <summary>
        /// Event handler for <see cref="ClickEvent"/>.
        /// </summary>
        public ClickEvent OnClick { get; set; }

        /// <summary>
        /// Event handler for <see cref="KeyPressedEvent"/>.
        /// </summary>
        public KeyPressedEvent OnKeyPressed { get; set; }

        /// <summary>
        /// Event handler for <see cref="RenderEvent"/>.
        /// </summary>
        public RenderEvent OnRender { get; set; }

        /// <summary>
        /// Event handler for <see cref="ShutdownEvent"/>.
        /// </summary>
        public ShutdownEvent OnShutdown { get; set; }
    }
}
