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
        public Color BorderColor { get; set; }

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
        public Gadget(string name, string font) : base(name)
        {
            OnMouseDown += MousePressed;
            OnMouseUp += MouseReleased;
            OnMouseMove += MouseMove;
            Font = font; 
        }

        #region Default event handlers

        /// <summary>
        /// Default gadget event handler for mouse pressed events.
        /// </summary>
        /// <param name="binding">The input binding relating to the button that has been pressed.</param>
        /// <param name="button">The mouse button that has been pressed.</param>
        public virtual void MousePressed(InputBinding? binding, MouseButton button)
        {
            if (binding == null) return;

            switch (binding.Name)
            {
                case "MOUSE1":
                    CurBackgroundColor = PressedColor;
                    Pressed = true;
                    break;
            }
        }

        /// <summary>
        /// Default gadget event handler for mouse released events.
        /// </summary>
        /// <param name="binding">The input binding relating to the button that has been pressed.</param>
        /// <param name="button">The mouse button that has been pressed.</param>
        public virtual void MouseReleased(InputBinding? binding, MouseButton button)
        {
            if (binding == null) return;

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
        /// <param name="binding">The input binding relating to the button that has been pressed.</param>
        /// <param name="button">The mouse button that has been pressed.</param>
        public virtual void MouseMove(InputBinding? binding, MouseButton button)
        {
            if (binding == null) return;

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
