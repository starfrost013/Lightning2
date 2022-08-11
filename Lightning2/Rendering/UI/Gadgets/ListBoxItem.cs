using System.Drawing;

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

        public ListBoxItem(string text) : base()
        {
            Text = text;
            OnRender += Render;
        }

        /// <summary>
        /// Renders the ListBoxItem
        /// </summary>
        /// <param name="cWindow">The <see cref="Window"/> to render the ListBoxItem to.</param>
        public void Render(Window cWindow)
        {
            // set the default background colour
            if (CurBackgroundColour == default(Color)) CurBackgroundColour = BackgroundColour;

            PrimitiveRenderer.DrawRectangle(cWindow, Position, Size, CurBackgroundColour, Filled, BorderColour, BorderSize, SnapToScreen);

            Font curFont = FontManager.GetFont(Font);

            if (curFont == null)
            {
                PrimitiveRenderer.DrawText(cWindow, Text, Position, ForegroundColour, true);
            }
            else
            {
                FontManager.DrawText(cWindow, Text, Font, Position, ForegroundColour);
            }
        }
    }
}
