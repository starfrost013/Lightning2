﻿namespace LightningGL
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
        public FontStyle Style { get; set; }
        
        private Rectangle? Rectangle { get; set; }

        private TextBlock? TextBlock { get; set; }

        public Button(string name, string font) : base(name, font) { }

        public override void Create()
        {
            Rectangle = Lightning.Renderer.AddRenderable(new Rectangle("ButtonRectangle", RenderPosition, Size, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen), this);

            // hack for now
            Text ??= string.Empty;

            TextBlock = Lightning.Renderer.AddRenderable(new TextBlock("ButtonText", Text, Font, Position, ForegroundColor, default, Style), this);

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
                NCError.ShowErrorBox("Tried to draw a button with null, empty, or only whitespace text. Ignoring...", 283, "string.IsNullOrWhiteSpace(Text) returned TRUE" +
                    "in Button::Draw");
                return;
            }

            // This is a bit of a hack, but it works for now
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

            Rectangle.Color = CurBackgroundColor;
            Rectangle.BorderColor = BorderColor;

            Font? curFont = (Font?)Lightning.Renderer.GetRenderableByName(Font);

            // this should NEVER return null because it's been made a fatal error if it is
            Debug.Assert(curFont != null);

            // calculate the initial text position
            Vector2 textSize = FontManager.GetTextSize(curFont, Text, ForegroundColor);
            Vector2 textPos = (RenderPosition + Size);
            textPos.X = textPos.X - (Size.X / 2) - (textSize.X / 2);
            textPos.Y = textPos.Y - (Size.Y / 2) - (textSize.Y / 2);

            TextBlock.Position = textPos;
        }
    }
}