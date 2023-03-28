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
        private bool _checked;

        /// <summary>
        /// Determines if this check box is checked.
        /// </summary>
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                if (CheckBoxLine1 != null) CheckBoxLine1.IsNotRendering = !value;
                if (CheckBoxLine2 != null) CheckBoxLine2.IsNotRendering = !value;
            }
        }

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
        /// <inheritdoc/>
        /// </summary>
        public override bool SnapToScreen 
        { 
            get => base.SnapToScreen; 
            set
            {
                base.SnapToScreen = value;
                if (Rectangle != null) Rectangle.SnapToScreen = value;
                if (CheckBoxLine1 != null) CheckBoxLine1.SnapToScreen = value;
                if (CheckBoxLine2 != null) CheckBoxLine2.SnapToScreen = value;  
            }
        
        }
        public override bool IsNotRendering 
        { 
            get => base.IsNotRendering; 
            set
            {
                if (Rectangle != null) Rectangle.IsNotRendering = value;
                if (CheckBoxLine1 != null) CheckBoxLine1.IsNotRendering = value;
                if (CheckBoxLine2 != null) CheckBoxLine2.IsNotRendering = value;
            }
        }

        /// <summary>
        /// Constructor for the CheckBox class.
        /// </summary>
        public CheckBox(string name, string font) : base(name, font)
        {
            OnMouseDown += CheckBoxMousePressed;
        }

        public CheckBox(string name, string font, bool isChecked) : base(name, font)
        {
            Checked = isChecked;
        }

        public CheckBox(string name, string font, Vector2 position, Vector2 size, Color foregroundColor, bool filled = false, Color borderColor = default, 
            Vector2 borderSize = default, bool snapToScreen = false, bool isChecked = false) : base(name, font)
        {
            Position = position;
            Size = size;
            ForegroundColor = foregroundColor;
            Filled = filled;
            BorderColor = borderColor;
            BorderSize = borderSize;
            SnapToScreen = snapToScreen;
            Checked = isChecked;
        }

        public override void Create()
        {
            // issue: won't update if you modify it again later
            Rectangle = Lightning.Renderer.AddRenderable(new Rectangle("CheckBoxRectangle", Position, Size, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen), this);

            CheckBoxLine1 = Lightning.Renderer.AddRenderable(new Line("CheckBoxLine1", default, default, ForegroundColor, SnapToScreen), this);
            CheckBoxLine2 = Lightning.Renderer.AddRenderable(new Line("CheckBoxLine2", default, default, ForegroundColor, SnapToScreen), this);
        }

        /// <summary>
        /// Renders this CheckBox.
        /// </summary>
        public override void Draw()
        {
            Debug.Assert(Rectangle != null
                && CheckBoxLine1 != null
                && CheckBoxLine2 != null);

            Rectangle.Color = CurBackgroundColor;

            if (Checked)
            {
                // inherited from earlier code
                Vector2 line1Start = new(Position.X, Position.Y + (Size.Y / 2));
                Vector2 line1End = new(Position.X + (Size.X / 3), Position.Y + Size.Y);
                Vector2 line2Start = line1End;
                Vector2 line2End = new(Position.X + Size.X, Position.Y);

                CheckBoxLine1.Position = line1Start;
                CheckBoxLine1.Size = line1End - line1Start;
                CheckBoxLine2.Position = line2Start;
                CheckBoxLine2.Size = line2End - line2Start;
            }
        }

        /// <summary>
        /// The default mouse pressed event handler for CheckBoxes.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> that has been pressed.</param>
        public void CheckBoxMousePressed(InputBinding? binding, MouseButton button)
        {
            if (binding == null) return;

            //base.MousePressed(binding, button);
            Checked = !Checked;
        }
    }
}
