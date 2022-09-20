namespace LightningGL
{
    /// <summary>
    /// Gadget
    /// 
    /// May 13, 2022 (modified June 12, 2022)
    /// </summary>
    public class Gadget : Renderable
    {
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
