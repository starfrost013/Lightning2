using static NuCore.SDL2.SDL_ttf;
using NuCore.Utilities;
using System.Drawing;
using System.Numerics;

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
        /// <param name="cWindow">The window to render this button to.</param>
        public void Render(Window cWindow)
        {
            // This is a bit of a hack, but it works for now
            if (CurBackgroundColor == default(Color)) CurBackgroundColor = BackgroundColor;

            PrimitiveRenderer.DrawRectangle(cWindow, RenderPosition, Size, CurBackgroundColor, Filled, Bordercolor, BorderSize, SnapToScreen);

            Font curFont = FontManager.GetFont(Font);

            if (FontManager.GetFont(Font) == null)
            {
                // Use the default font.
                PrimitiveRenderer.DrawText(cWindow, Text, RenderPosition, ForegroundColor, true);
            }
            else
            {
                Vector2 textSize = FontManager.GetTextSize(curFont, Text);
                Vector2 textPos = (RenderPosition + Size);
                textPos.X = textPos.X - (Size.X / 2) - (textSize.X / 2);
                textPos.Y = textPos.Y - (Size.Y / 2) - (textSize.Y / 2);

                FontManager.DrawText(cWindow, Text, Font, textPos, ForegroundColor, default(Color), Style);
            }
        }
    }
}