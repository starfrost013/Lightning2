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

        private TextBlock? TextBlock { get; set; }  

        public ListBoxItem(string name, string text, string font) : base(name, font)
        {
            Text = text;
        }

        public override void Create()
        {
            Rectangle = Lightning.Renderer.AddRenderable(new Rectangle("ListBoxItemRectangle", Position, Size, CurBackgroundColor, Filled, 
                BorderColor, BorderSize, SnapToScreen), this);

            TextBlock = Lightning.Renderer.AddRenderable(new TextBlock("ListBoxItemText", Text, Font, Position, ForegroundColor), this);

            Debug.Assert(Rectangle != null);
        }

        /// <summary>
        /// Renders the ListBoxItem.
        /// </summary>
        public override void Draw()
        {
            Debug.Assert(Rectangle != null
                && TextBlock != null);

            // set the default background color
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

            Rectangle.Color = CurBackgroundColor;

            // stupid hack to ensure it changes
            TextBlock.Text = Text; 
        }
    }
}
