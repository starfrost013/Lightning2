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
        /// <summary>
        /// The text of this <see cref="ListBoxItem"/>
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// UI rectangle used for drawing this <see cref="ListBoxItem"/>.
        /// </summary>
        private Rectangle? Rectangle { get; set; }

        public ListBoxItem(string name, string text, string font) : base(name, font)
        {
            Text = text;
            OnRender += Render;
        }

        public override void Create()
        {
            Rectangle = PrimitiveManager.AddRectangle(Position, Size, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen, this);

            Debug.Assert(Rectangle != null);
        }

        /// <summary>
        /// Renders the ListBoxItem
        /// </summary>
        public void Render()
        {
            // set the default background color
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

#pragma warning disable CS8602
            Rectangle.Color = CurBackgroundColor;
#pragma warning restore CS8602

            TextManager.DrawText(Text, Font, Position, ForegroundColor);
        }
    }
}
