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

        /// <summary>
        /// UI text used for drawing this <see cref="ListBoxItem"/>
        /// </summary>
        private TextBlock? TextBlock { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool IsNotRendering 
        {
            get => base.IsNotRendering; 
            set
            {
                base.IsNotRendering = value;
                if (Rectangle != null) Rectangle.IsNotRendering = value;
                if (TextBlock != null) TextBlock.IsNotRendering = value;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool SnapToScreen 
        { 
            get => base.SnapToScreen; 
            set
            {
                base.SnapToScreen = value;
                if (Rectangle != null) Rectangle.SnapToScreen = value;
                if (TextBlock != null) TextBlock.SnapToScreen = value;
            }
        }

        public ListBoxItem(string name, string font) : base(name, font)
        {
            Text = string.Empty; 
        }

        public ListBoxItem(string name, string font, string text) : base(name, font)
        {
            Text = text;
        }

        public override void Create()
        {
            // set the default background color
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

            Rectangle = Lightning.Tree.AddRenderable(new Rectangle("ListBoxItemRectangle", Position, Size, CurBackgroundColor, Filled, 
                BorderColor, BorderSize, SnapToScreen), this);

            TextBlock = Lightning.Tree.AddRenderable(new TextBlock("ListBoxItemText", Text, Font, Position, ForegroundColor), this);

            Debug.Assert(Rectangle != null);
        }

        /// <summary>
        /// Renders the ListBoxItem.
        /// </summary>
        public override void Draw()
        {
            Debug.Assert(Rectangle != null
                && TextBlock != null);

            Rectangle.Color = CurBackgroundColor;

            // stupid hack to ensure it changes
            TextBlock.Text = Text;
            
        }
    }
}
