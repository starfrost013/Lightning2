using static NuCore.SDL2.SDL;
using static NuCore.SDL2.SDL_ttf;
using NuCore.Utilities;
using System.Drawing;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// Button
    /// 
    /// June 12, 2022 (modified June 15, 2022: add highlight colour)
    /// 
    /// Defines a button class.
    /// </summary>
    public class Button : UIGadget
    {
        public string Text { get; set; }

        public string Font { get; set; }

        public Vector2 BorderSize { get; set; }

        public bool Filled { get; set; }

        public TTF_FontStyle Style { get; set; }

        public Button() : base()
        {
            // Hook up events
            OnRender += Render;
        }

        public void Render(Window cWindow)
        {
            // This is a bit of a hack, but it works for now
            if (CurBackgroundColour == default(Color)) CurBackgroundColour = BackgroundColour;

            Font curFont = TextManager.GetFont(Font);

            if (TextManager.GetFont(Font) == null) throw new NCException($"Button tried to load invalid font {Font}. Please load the font before using it!", 581, "TextManager::GetFont returned null on Font property of Button", NCExceptionSeverity.FatalError);

            Vector2 textSize = TextManager.GetTextSize(curFont, Text);
            Vector2 textPos = (Position + Size);
            textPos.X = textPos.X - (Size.X / 2) - (textSize.X / 2);
            textPos.Y = textPos.Y - (Size.Y / 2) - (textSize.Y / 2);

            PrimitiveRenderer.DrawRectangle(cWindow, Position, Size, CurBackgroundColour, Filled, SnapToScreen, BorderColour, BorderSize);
            TextManager.DrawTextTTF(cWindow, Text, Font, textPos, ForegroundColour, default(Color), Style, SnapToScreen);
        }


    }
}
