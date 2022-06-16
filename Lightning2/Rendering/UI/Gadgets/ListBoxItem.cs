using System.Numerics; 

namespace LightningGL
{
    /// <summary>
    /// ListBoxItem
    /// 
    /// June 15, 2022
    /// 
    /// Defines a listbox item.
    /// </summary>
    public class ListBoxItem : Gadget
    {
        public string Text { get; set; }

        public ListBoxItem(string text)
        {
            Text = text;
            OnRender += Render;
        }

        public void Render(Window cWindow)
        {
            PrimitiveRenderer.DrawRectangle(cWindow, Position, Size, BackgroundColour, Filled, BorderColour, BorderSize);

            Font curFont = FontManager.GetFont(Font);

            if (curFont == null)
            {
                PrimitiveRenderer.DrawText(cWindow, Text, Position, ForegroundColour, true);
            }
            else
            {
                FontManager.DrawText(cWindow, Text, Font, Position, ForegroundColour, BackgroundColour);
            }
        }
    }
}
