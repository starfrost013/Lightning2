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

        public ListBoxItem(string name, string text, string font) : base(name, font)
        {
            Text = text;
            OnRender += Render;
        }

        /// <summary>
        /// Renders the ListBoxItem
        /// </summary>
        /// <param name="Lightning.Renderer">The <see cref="Renderer"/> to render the ListBoxItem to.</param>
        public void Render()
        {
            // set the default background color
            if (CurBackgroundColor == default(Color)) CurBackgroundColor = BackgroundColor;

            PrimitiveManager.AddRectangle(Position, Size, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen);

            Font? curFont = FontManager.GetFont(Font);

            if (curFont == null)
            {
                PrimitiveManager.AddText(Text, Position, ForegroundColor, true);
            }
            else
            {
                TextManager.DrawText(Text, Font, Position, ForegroundColor);
            }
        }
    }
}
