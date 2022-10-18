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
        public string Text { get; set; }

        /// <summary>
        /// The text style that this button will use.
        /// </summary>
        public TTF_FontStyle Style { get; set; }

        public Button() : base()
        {
            // Hook up event handlers
            OnRender += Render;
        }

        /// <summary>
        /// Renders this button.
        /// </summary>
        /// <param name="cRenderer">The window to render this button to.</param>
        public void Render(Renderer cRenderer)
        {
            // This is a bit of a hack, but it works for now
            if (CurBackgroundColor == default(Color)) CurBackgroundColor = BackgroundColor;

            PrimitiveRenderer.DrawRectangle(cRenderer, RenderPosition, Size, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen);

            Font curFont = FontManager.GetFont(Font);

            if (FontManager.GetFont(Font) == null)
            {
                // Use the default font.
                PrimitiveRenderer.DrawText(cRenderer, Text, RenderPosition, ForegroundColor, true);
            }
            else
            {
                Vector2 textSize = FontManager.GetTextSize(curFont, Text);
                Vector2 textPos = (RenderPosition + Size);
                textPos.X = textPos.X - (Size.X / 2) - (textSize.X / 2);
                textPos.Y = textPos.Y - (Size.Y / 2) - (textSize.Y / 2);

                TextManager.DrawText(cRenderer, Text, Font, textPos, ForegroundColor, default(Color), Style);
            }
        }
    }
}