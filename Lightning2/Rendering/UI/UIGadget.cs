using static NuCore.SDL2.SDL;
using System.Drawing;
using System.Numerics; 

namespace LightningGL
{
    /// <summary>
    /// UIElement
    /// 
    /// May 13, 2022 (modified June 12, 2022)
    /// </summary>
    public class UIGadget : Renderable
    {
        #region Events

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

        public Color BackgroundColour { get; set; }

        public Color ForegroundColour { get; set; }

        public Color HighlightColour { get; set; }

        public Color PressedColour { get; set; }

        public Color BorderColour { get; set; }

        public bool Pressed { get; set; }


        /// <summary>
        /// Private: current colour used for swapping between pressed/held colour
        /// </summary>
        protected Color CurBackgroundColour { get; set; }

        public UIGadget()
        {
            OnMousePressed += MousePressed;
            OnMouseReleased += MouseReleased;
            OnMouseMove += MouseMove;
        }

        public void MousePressed(SDL_MouseButton button, Vector2 position)
        {
            CurBackgroundColour = PressedColour;
            Pressed = true;
        }

        public void MouseReleased(SDL_MouseButton button, Vector2 position)
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

        public void MouseMove(Vector2 position, Vector2 velocity, SDL_MouseButton button)
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
    }
}
