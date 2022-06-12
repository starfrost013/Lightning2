﻿

namespace Lightning2
{
    /// <summary>
    /// UIElement
    /// 
    /// May 13, 2022 (modified June 12, 2022)
    /// </summary>
    public class UIElement
    {
        /// <summary>
        /// Event handler for <see cref="MousePressedEvent"/> event.
        /// </summary>
        public MousePressedEvent OnClick { get; set; }

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
        public MousePressedEvent OnMouseReleased { get; set; }

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
