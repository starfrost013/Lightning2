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

        /// <summary>
        /// UI rectangle used for drawing this textbox.
        /// </summary>
        private Rectangle? Rectangle { get; set; }

        public ListBoxItem(string name, string text, string font) : base(name, font)
        {
            Text = text;
            OnRender += Render;
        }

        internal override void Create()
        {
            Rectangle = PrimitiveManager.AddRectangle(Position, Size, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen);
        }

        /// <summary>
        /// Renders the ListBoxItem
        /// </summary>
        /// <param name="Lightning.Renderer">The <see cref="Renderer"/> to render the ListBoxItem to.</param>
        public void Render()
        {
            // set the default background color
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

#pragma warning disable CS8602
            Rectangle.Color = CurBackgroundColor;
#pragma warning restore CS8602

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
