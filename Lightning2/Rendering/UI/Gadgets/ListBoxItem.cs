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
        /// <param name="cRenderer">The <see cref="Renderer"/> to render the ListBoxItem to.</param>
        public void Render(Renderer cRenderer)
        {
            // set the default background color
            if (CurBackgroundColor == default(Color)) CurBackgroundColor = BackgroundColor;

            PrimitiveRenderer.DrawRectangle(cRenderer, Position, Size, CurBackgroundColor, Filled, Bordercolor, BorderSize, SnapToScreen);

            Font curFont = FontManager.GetFont(Font);

            if (curFont == null)
            {
                PrimitiveRenderer.DrawText(cRenderer, Text, Position, ForegroundColor, true);
            }
            else
            {
                TextManager.DrawText(cRenderer, Text, Font, Position, ForegroundColor);
            }
        }
    }
}
