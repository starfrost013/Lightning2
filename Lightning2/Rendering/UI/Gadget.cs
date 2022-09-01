using static NuCore.SDL2.SDL;
using System.Drawing;
using System.Numerics; 

namespace LightningGL
{
    /// <summary>
    /// Gadget
    /// 
    /// May 13, 2022 (modified June 12, 2022)
    /// </summary>
    public class Gadget : Renderable
    {
        #region Event handlers

        /// <summary>
        /// Event handler for <see cref="KeyPressedEvent"/> event.
        /// </summary>
        public KeyPressedEvent OnKeyPressed { get; set; }

        /// <summary>
        /// Event handler for the <see cref="KeyReleasedEvent"/> event.
        /// </summary>
        public KeyReleasedEvent OnKeyReleased { get; set; }

        /// <summary>
        /// Event handler for the <see cref="MousePressedEvent"/> event.
        /// </summary>
        public MouseEvent OnMousePressed { get; set; }

        /// <summary>
        /// Event handler for the <see cref="MouseReleasedEvent"/> event.
        /// </summary>
        public MouseEvent OnMouseReleased { get; set; }

        /// <summary>
        /// Event handler for the mouse enter event.
        /// </summary>
        public GenericEvent OnMouseEnter { get; set; }

        /// <summary>
        /// Event handler for the mouse leave event.
        /// </summary>
        public GenericEvent OnMouseLeave { get; set; }

        /// <summary>
        /// Event handler for the focus gained event.
        /// </summary>
        public GenericEvent OnFocusGained { get; set; }

        /// <summary>
        /// Event handler for the focus lost event.
        /// </summary>
        public GenericEvent OnFocusLost { get; set; }

        /// <summary>
        /// Event handler for the <see cref="MouseEvent"/> event.
        /// </summary>
        public MouseEvent OnMouseMove { get; set; }

        /// <summary>
        /// Event handler for <see cref="RenderEvent"/> event.
        /// </summary>
        public RenderEvent OnRender { get; set; }

        /// <summary>
        /// Event handler for <see cref="ShutdownEvent"/> event.
        /// </summary>
        public ShutdownEvent OnShutdown { get; set; }

        #endregion

        /// <summary>
        /// The colour used for the background of this gadget.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// The colour used for the foreground of this gadget.
        /// </summary>
        public Color ForegroundColor { get; set; }

        /// <summary>
        /// The colour used when the mouse is hovering over this Gadget.
        /// </summary>
        public Color HoverColor { get; set; }

        /// <summary>
        /// The colour used when the mouse is pressing down on this gadget.
        /// </summary>
        public Color PressedColor { get; set; }

        /// <summary>
        /// The colour used for the border of this gadget.
        /// </summary>
        public Color Bordercolor { get; set; }

        /// <summary>
        /// Border size of this gadget.
        /// </summary>
        public Vector2 BorderSize { get; set; }

        /// <summary>
        /// Determines whether this gadget will be filled or not.
        /// </summary>
        public bool Filled { get; set; }

        /// <summary>
        /// Private: current color used for swapping between pressed/held color
        /// </summary>
        protected Color CurBackgroundColor { get; set; }

        /// <summary>
        /// Determines if the mouse is currently clicking on this gadget or not.
        /// </summary>
        public bool Pressed { get; private set; }

        /// <summary>
        /// Determines if the mouse is currently focused on this gadget or not.
        /// </summary>
        public bool Focused { get; internal set; }

        /// <summary>
        /// The FriendlyName property of the font this button will use.
        /// If this value is set to null or a font that is not loaded is specified, the default SDL2_gfx font will be used.
        /// </summary>
        public string Font { get; set; }

        /// <summary>
        /// Gadget constructor.
        /// </summary>
        public Gadget()
        {
            OnMousePressed += MousePressed;
            OnMouseReleased += MouseReleased;
            OnMouseMove += MouseMove;
        }

        #region Default event handlers

        /// <summary>
        /// Default gadget event handler for mouse pressed events.
        /// </summary>
        /// <param name="button">The mouse button that has been pressed.</param>
        /// <param name="position">The position of that mouse button.</param>
        public virtual void MousePressed(MouseButton button)
        {
            CurBackgroundColor = PressedColor;
            Pressed = true;
        }

        /// <summary>
        /// Default gadget event handler for mouse released events.
        /// </summary>
        /// <param name="button">The mouse button that has been pressed.</param>
        /// <param name="position">The position of that mouse button.</param>
        public virtual void MouseReleased(MouseButton button)
        {
            // this changes from pressed to hover color
            if (AABB.Intersects(this, button.Position))
            {
                // we are hovering over the button so switch to the background color
                CurBackgroundColor = HoverColor;
            }
            else
            {
                CurBackgroundColor = BackgroundColor;
            }
            
            Pressed = false;
        }

        /// <summary>
        /// Default gadget event handler for mouse move events.
        /// </summary>
        /// <param name="button">The mouse button that has been pressed.</param>
        /// <param name="position">The position of that mouse button.</param>
        /// <param name="velocity">The movement of the button.</param>
        public virtual void MouseMove(MouseButton button)
        {
            if (AABB.Intersects(this, button.Position))
            {
                CurBackgroundColor = HoverColor;
            }
            else
            {
                CurBackgroundColor = BackgroundColor;
            }
        }
        #endregion
    }
}
