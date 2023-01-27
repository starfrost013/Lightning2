namespace LightningGL
{
    /// <summary>
    /// Button
    /// 
    /// June 12, 2022 (modified June 15, 2022: add highlight color)
    /// 
    /// Defines a button class.
    /// </summary>
    public class Button : Gadget
    {
        /// <summary>
        /// The text that this button will hold.
        /// </summary>
        public string? Text { get; set; }

        private Rectangle? Rectangle { get; set; }

        private TextBlock? TextBlock { get; set; }

        //hack
        public override bool IsNotRendering
        {
            get => base.IsNotRendering;
            set
            {
                base.IsNotRendering = value;
                if (Rectangle != null) Rectangle.IsNotRendering = true;
                if (TextBlock != null) TextBlock.IsNotRendering = true;
            }
        }


        public Button(string name, string font) : base(name, font) { }

        public Button(string name, string font, string text) : base(name, font)
        {
            Text = text;
        }

        public Button(string name, string font, string text, Vector2 position, Vector2 size, Color foregroundColor, Color backgroundColor = default, 
            bool filled = true, Color borderColor = default, Vector2 borderSize = default, bool snapToScreen = false) : base(name, font)
        {
            Text = text;
            Position = position;
            Size = size;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            Filled = filled;
            BorderColor = borderColor;
            BorderSize = borderSize;
            SnapToScreen = snapToScreen;
        }

        public override void Create()
        {
            // This is a bit of a hack, but it works for now
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

            Rectangle = Lightning.Renderer.AddRenderable(new Rectangle("ButtonRectangle", Position, Size, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen), this);

            // hack for now
            Text ??= string.Empty;

            TextBlock = Lightning.Renderer.AddRenderable(new TextBlock("ButtonText", Text, Font, Position, ForegroundColor, default, Style), this);
            TextBlock.SnapToScreen = SnapToScreen;

            Debug.Assert(Rectangle != null);
        }

        /// <summary>
        /// Renders this button.
        /// </summary>
        public override void Draw()
        {
            Debug.Assert(Rectangle != null
                && TextBlock != null);
            if (string.IsNullOrWhiteSpace(Text))
            {
                NCLogging.LogError("Tried to draw a button with null, empty, or only whitespace text. Ignoring...", 283, NCLoggingSeverity.Warning, null, true);
                return;
            }

            Rectangle.Color = CurBackgroundColor;
            Rectangle.BorderColor = BorderColor;

            Font? curFont = (Font?)Lightning.Renderer.GetRenderableByName(Font);

            // this should NEVER return null because it's been made a fatal error if it is
            Debug.Assert(curFont != null);

            // calculate the initial text position
            Vector2 textSize = FontManager.GetTextSize(curFont, Text, ForegroundColor, Style, TextBlock.SmoothingType);
            Rectangle.Position = Position;
            TextBlock.Position = new(Position.X + ((Size.X / 2) - (textSize.X / 2)),
                Position.Y + ((Size.Y / 2) + (textSize.Y / 2)));
        }
    }
}