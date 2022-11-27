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

        /// <summary>
        /// The text style that this button will use.
        /// </summary>
        public TTF_FontStyle Style { get; set; }
        
        public Rectangle? Rectangle { get; set; }

        public Button(string name, string font) : base(name, font)
        {
            // Hook up event handlers
            OnRender += Render;
        }

        internal override void Create()
        {
            Rectangle = PrimitiveManager.AddRectangle(RenderPosition, Size, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen, this);

            Debug.Assert(Rectangle != null);
        }

        /// <summary>
        /// Renders this button.
        /// </summary>
        /// <param name="Lightning.Renderer">The window to render this button to.</param>
        public void Render()
        {
            if (Text == null) return;

            // This is a bit of a hack, but it works for now
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

#pragma warning disable CS8602 // create is always called before this method and this can never be null
            Rectangle.Color = CurBackgroundColor;
            Rectangle.BorderColor = BorderColor;
#pragma warning restore CS8602

            Font? curFont = FontManager.GetFont(Font);

            // this should NEVER return null because it's been made a fatal error if it is
            Debug.Assert(Font != null);

            Vector2 textSize = FontManager.GetTextSize(curFont, Text);
            Vector2 textPos = (RenderPosition + Size);
            textPos.X = textPos.X - (Size.X / 2) - (textSize.X / 2);
            textPos.Y = textPos.Y - (Size.Y / 2) - (textSize.Y / 2);

            TextManager.DrawText(Text, Font, textPos, ForegroundColor, default, Style);

        }
    }
}