

namespace LightningGL
{
    /// <summary>
    /// UIElement
    /// 
    /// May 13, 2022 (modified June 12, 2022)
    /// </summary>
    public class UIGadget : Renderable
    {
        /// <summary>
        /// Event handler for <see cref="KeyPressedEvent"/> event.
        /// </summary>
        public KeyPressedEvent OnKeyPressed { get; set; }

        /// <summary>
        /// Event handler for the <see cref="KeyReleaseEvent"/> event.
        /// </summary>
        public KeyReleaseEvent OnKeyReleased { get; set; }

        /// <summary>
        /// Event handler for the <see cref="MousePressedEvent"/> event.
        /// </summary>
        public MousePressedEvent OnMousePressed { get; set; }

        /// <summary>
        /// Event handler for the <see cref="MouseReleasedEvent"/> event.
        /// </summary>
        public MouseReleasedEvent OnMouseReleased { get; set; }

        /// <summary>
        /// Event handler for the <see cref="GenericEvent"/> event.
        /// </summary>
        public GenericEvent OnMouseEnter { get; set; }

        /// <summary>
        /// Event handler for the <see cref="GenericEvent"/> event.
        /// </summary>
        public GenericEvent OnMouseLeave { get; set; }

        /// <summary>
        /// Event handler for the <see cref="MouseMoveEvent"/> event.
        /// </summary>
        public MouseMoveEvent OnMouseMove { get; set; }

        /// <summary>
        /// Event handler for <see cref="RenderEvent"/> event.
        /// </summary>
        public RenderEvent OnRender { get; set; }

        /// <summary>
        /// Event handler for <see cref="ShutdownEvent"/> event.
        /// </summary>
        public ShutdownEvent OnShutdown { get; set; }
    }
}
