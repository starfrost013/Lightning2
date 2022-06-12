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
    public class Button : UIElement
    {
        public string Text { get; set; }

        public string Font { get; set; }

        public Vector2 Size { get; set; }

        public Vector2 BorderSize { get; set; }

        public Vector2 Position { get; set; }

        public Color Color { get; set; }

        public Color BorderColor { get; set; }

        public bool Filled { get; set; }

        public bool SnapToScreen { get; set; }

        public void Render(Window cWindow)
        {
            if (TextManager.GetFont(Font) == null) throw new NCException($"Button tried to load invalid font {Font}. Please load the font before using it!", 581, "TextManager::GetFont returned null on Font property of Button", NCExceptionSeverity.FatalError);
            PrimitiveRenderer.DrawRectangle(cWindow, Position, Size, Color, Filled, SnapToScreen, BorderColor, BorderSize);
            PrimitiveRenderer.DrawText(cWindow, Text, Position, Color, SnapToScreen, true);
        }

    }
}
