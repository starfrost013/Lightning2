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
        /// The thickness of the line of this CheckBox.
        /// </summary>
        public short Thickness { get; set; }

        /// <summary>
        /// Constructor for the CheckBox class.
        /// </summary>
        public CheckBox() : base()
        {
            OnRender += Render;
            OnMousePressed += CheckBoxMousePressed;
            if (Thickness == 0) Thickness = 2;
        }

        /// <summary>
        /// Renders this CheckBox.
        /// </summary>
        /// <param name="cRenderer">The window to render this checkbox to.</param>
        public void Render(Renderer cRenderer)
        {
            PrimitiveRenderer.DrawRectangle(cRenderer, Position, Size, CurBackgroundColor, Filled, Bordercolor, BorderSize, SnapToScreen);

            if (Checked)
            {
                Vector2 line1Start = new Vector2(Position.X, Position.Y + (Size.Y / 2));
                Vector2 line1End = new Vector2(Position.X + (Size.X / 3), Position.Y + Size.Y);
                Vector2 line2Start = line1End;
                Vector2 line2End = new Vector2(Position.X + Size.X, Position.Y);

                PrimitiveRenderer.DrawLine(cRenderer, line1Start, line1End, Thickness, ForegroundColor, true, SnapToScreen);
                PrimitiveRenderer.DrawLine(cRenderer, line2Start, line2End, Thickness, ForegroundColor, true, SnapToScreen);
            }
        }

        /// <summary>
        /// The default mouse pressed event handler for CheckBoxes.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="position"></param>
        public void CheckBoxMousePressed(MouseButton button)
        {
            base.MousePressed(button);
            Checked = !Checked;
        }
    }
}
