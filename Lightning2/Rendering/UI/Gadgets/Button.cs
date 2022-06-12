﻿using static NuCore.SDL2.SDL_ttf;
using NuCore.Utilities;
using System.Drawing;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// Button
    /// 
    /// June 12, 2022
    /// 
    /// Defines a button class.
    /// </summary>
    public class Button : UIGadget
    {
        public string Text { get; set; }

        public string Font { get; set; }

        public Vector2 BorderSize { get; set; }

        public Color BackgroundColour { get; set; }

        public Color ForegroundColour { get; set; }

        public Color BorderColour { get; set; }

        public bool Filled { get; set; }

        public TTF_FontStyle Style { get; set; }

        /// <summary>
        /// Private: current colour used for swapping between pressed/held colour
        /// </summary>
        private Color CurColour { get; set; }

        public Button()
        {
            // Hook up events
            OnRender += Render; 
        }

        public void Render(Window cWindow)
        {
            Font curFont = TextManager.GetFont(Font);

            if (TextManager.GetFont(Font) == null) throw new NCException($"Button tried to load invalid font {Font}. Please load the font before using it!", 581, "TextManager::GetFont returned null on Font property of Button", NCExceptionSeverity.FatalError);

            Vector2 textSize = TextManager.GetTextSize(curFont, Text);
            Vector2 textPos = (Position + Size);
            textPos.X = textPos.X - (Size.X / 2) - (textSize.X / 2);
            textPos.Y = textPos.Y - (Size.Y / 2) - (textSize.Y / 2);

            PrimitiveRenderer.DrawRectangle(cWindow, Position, Size, BackgroundColour, Filled, SnapToScreen, BorderColour, BorderSize);
            TextManager.DrawTextTTF(cWindow, Text, Font, textPos, ForegroundColour, default(Color), Style, SnapToScreen);
        }

    }
}
