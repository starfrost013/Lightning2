namespace LightningGL
{
    /// <summary>
    /// CheckBox
    /// 
    /// August 10, 2022
    /// 
    /// Defines a checkbox gadget
    /// </summary>
    public class CheckBox : Gadget
    {
        /// <summary>
        /// Determines if this check box is checked.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// UI rectangle used for drawing this textbox.
        /// </summary>
        private Rectangle? Rectangle { get; set; }

        /// <summary>
        /// UI line #1 used for drawing this textbox.
        /// </summary>
        private Line? CheckBoxLine1 { get; set; }

        /// <summary>
        /// UI line #2 used for drawing this textbox.
        /// </summary>
        private Line? CheckBoxLine2 { get; set; }

        /// <summary>
        /// Constructor for the CheckBox class.
        /// </summary>
        public CheckBox(string name, string font) : base(name, font)
        {
            OnRender += Render;
            OnMousePressed += CheckBoxMousePressed;
        }

        internal override void Create()
        {
            // issue: won't update if you modify it again later
            // this is why we might need referents for renderble
            Rectangle = PrimitiveManager.AddRectangle(Position, Size, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen, this);

            CheckBoxLine1 = PrimitiveManager.AddLine(default, default, ForegroundColor, true, SnapToScreen);
            CheckBoxLine2 = PrimitiveManager.AddLine(default, default, ForegroundColor, true, SnapToScreen);

            Debug.Assert(Rectangle != null);
            Debug.Assert(CheckBoxLine1 != null);
            Debug.Assert(CheckBoxLine2 != null);
        }

        /// <summary>
        /// Renders this CheckBox.
        /// </summary>
        /// <param name="Lightning.Renderer">The window to render this checkbox to.</param>
        public void Render()
        {
#pragma warning disable CS8602 // not applicable because this cannot be null (as a method that cannot return null is called) and it asserts if it is
            Rectangle.Color = CurBackgroundColor;
#pragma warning restore CS8602 

            if (Checked)
            {
                Vector2 line1Start = new(Position.X, Position.Y + (Size.Y / 2));
                Vector2 line1End = new(Position.X + (Size.X / 3), Position.Y + Size.Y);
                Vector2 line2Start = line1End;
                Vector2 line2End = new(Position.X + Size.X, Position.Y);

#pragma warning disable CS8602 // we assert if they are, and they can never be null anyway
                CheckBoxLine1.Start = line1Start;
                CheckBoxLine1.End = line1End;
                CheckBoxLine2.Start = line2Start;
                CheckBoxLine2.End = line2End;
#pragma warning restore CS8602

            }
        }

        /// <summary>
        /// The default mouse pressed event handler for CheckBoxes.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> that has been pressed.</param>
        public void CheckBoxMousePressed(MouseButton button)
        {
            base.MousePressed(button);
            Checked = !Checked;
        }
    }
}
