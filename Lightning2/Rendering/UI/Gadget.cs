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

        #endregion

        /// <summary>
        /// The colour used for the background of this gadget.
        /// </summary>
        public Color BackgroundColour { get; set; }

        /// <summary>
        /// The colour used for the foreground of this gadget.
        /// </summary>
        public Color ForegroundColour { get; set; }

        /// <summary>
        /// The colour used when the mouse is pressing down on this
        /// </summary>
        public Color HighlightColour { get; set; }

        /// <summary>
        /// The colour used when the mouse is pressing down on this gadget.
        /// </summary>
        public Color PressedColour { get; set; }

        /// <summary>
        /// The colour used for the border of this gadget.
        /// </summary>
        public Color BorderColour { get; set; }

        /// <summary>
        /// Border size of this gadget.
        /// </summary>
        public Vector2 BorderSize { get; set; }

        /// <summary>
        /// Determines whether this gadget will be filled or not.
        /// </summary>
        public bool Filled { get; set; }

        /// <summary>
        /// Private: current colour used for swapping between pressed/held colour
        /// </summary>
        protected Color CurBackgroundColour { get; set; }

        /// <summary>
        /// Determines if the mouse is currently clicking on this gadget or not.
        /// </summary>
        public bool Pressed { get; private set; }

        /// <summary>
        /// Determines if the mouse is currently focused on this gadget or not.
        /// </summary>
        public bool Focused { get; internal set; }

        /// <summary>
        /// The font this button will use.
        /// If this value is set to null or a font that is not loaded is specified, the default SDL2_gfx font will be used.
        /// </summary>
        public string Font { get; set; }

        public Gadget()
        {
            OnMousePressed += MousePressed;
            OnMouseReleased += MouseReleased;
            OnMouseMove += MouseMove;
        }

        #region Default event handlers

        public virtual void MousePressed(SDL_MouseButton button, Vector2 position)
        {
            CurBackgroundColour = PressedColour;
            Pressed = true;
        }

        public virtual void MouseReleased(SDL_MouseButton button, Vector2 position)
        {
            if (AABB.Intersects(this, position))
            {
                // we are hovering over the button so switch to the background colour
                CurBackgroundColour = HighlightColour;
            }
            else
            {
                CurBackgroundColour = BackgroundColour;
            }
            Pressed = false;
        }

        public virtual void MouseMove(Vector2 position, Vector2 velocity, SDL_MouseButton button)
        {
            if (AABB.Intersects(this, position))
            {
                CurBackgroundColour = HighlightColour;
            }
            else
            {
                CurBackgroundColour = BackgroundColour;
            }
        }
        #endregion
    }
}
