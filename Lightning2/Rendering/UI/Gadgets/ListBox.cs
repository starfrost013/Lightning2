namespace LightningGL
{
    /// <summary>
    /// ListBox
    /// 
    /// June 15, 2022
    /// 
    /// Defines a list box gadget. A list box is a list of sel
    /// </summary>
    public class ListBox : Gadget
    {
        /// <summary>
        /// Determines if the list box is open or not.
        /// </summary>
        public bool Open { get; set; }

        /// <summary>
        /// List of items in the list box.
        /// </summary>
        public List<ListBoxItem> Items { get; private set; }

        private int _selectedindex;

        /// <summary>
        /// The current selected index
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _selectedindex;
            }
            set
            {
                if (value < 0
                    || value > Items.Count - 1) _ = new NCException("Attempted to set an invalid SelectedIndex for this ListBox!", 83, "ListBox::SelectedIndex > ListBox::Items::Count - 1!", NCExceptionSeverity.FatalError);

                _selectedindex = value;
            }
        }

        /// <summary>
        /// The current selected item. Simply returns Items[SelectedIndex].
        /// </summary>
        public ListBoxItem SelectedItem => Items[SelectedIndex];

        /// <summary>
        /// Determines if item colors will not be alternated on every other item.
        /// </summary>
        public bool DontAlternateItemcolors { get; set; }

        /// <summary>
        /// Determines how much the alternate item colors will be modified by.
        /// A negative value brightens the colors. Default value is 30.
        /// </summary>
        public int AlternateItemcolorsAmount { get; set; }

        /// <summary>
        /// Private: Size used to determine the size of the box that is actually drawn when the item is not open
        /// </summary>
        private Vector2 BoxSize { get; set; }

        /// <summary>
        /// Static listbox constructor.
        /// </summary>
        public ListBox() : base()
        {
            Items = new List<ListBoxItem>();
            OnRender += Render;

            // if you override the method it calls it twice (because it's defined for both classes)
            // so simply use a different method
            OnMousePressed += ListBoxMousePressed;
            OnMouseReleased += ListBoxMouseReleased;
            OnMouseMove += ListBoxMouseMove;

            AlternateItemcolorsAmount = 30;
        }

        /// <summary>
        /// Adds an item to this ListBox.
        /// </summary>
        /// <param name="item">The <see cref="ListBoxItem"/> to add to this ListBox.</param>
        public void AddItem(ListBoxItem item)
        {
            item.Font = Font;
            Font itemFont = FontManager.GetFont(Font);

            Vector2 itemFontSize = FontManager.GetTextSize(itemFont, item.Text);

            // size it to the size of the text in the Y dimension
            BoxSize = new Vector2(Size.X, (itemFontSize.Y * 1.25f));
            item.Size = BoxSize;

            // move the item so that it gets drawn in the right place
            item.Position = new Vector2(Position.X, Position.Y + ((itemFontSize.Y * 1.25f) * (Items.Count + 1)));
            if (item.BackgroundColor == default(Color)) item.BackgroundColor = BackgroundColor;
            if (item.ForegroundColor == default(Color)) item.ForegroundColor = ForegroundColor;
            if (item.Bordercolor == default(Color)) item.Bordercolor = Bordercolor;

            // alternate the colors so they look a bit better
            if (Items.Count % 2 == 0
                && !DontAlternateItemcolors)
            {
                int altR = item.BackgroundColor.R - AlternateItemcolorsAmount,
                    altG = item.BackgroundColor.G - AlternateItemcolorsAmount,
                    altB = item.BackgroundColor.B - AlternateItemcolorsAmount;

                // make sure we are in valid color ranges (not required for alpha)
                if (altR < 0) altR = 0;
                if (altG < 0) altG = 0;
                if (altB < 0) altB = 0;

                if (altR > 255) altR = 255;
                if (altG > 255) altG = 255;
                if (altB > 255) altB = 255;

                item.BackgroundColor = Color.FromArgb(item.BackgroundColor.A,
                        altR,
                        altG,
                        altB);
            }

            item.HoverColor = HoverColor;
            item.PressedColor = PressedColor;

            item.Filled = true; // set for now

            // resize the listbox 
            Size = new Vector2(Size.X, Size.Y + (BoxSize.Y));
            Items.Add(item);
        }

        /// <summary>
        /// Renders this ListBox.
        /// </summary>
        /// <param name="cRenderer">The window to render this listbox to.</param>
        public void Render(Renderer cRenderer)
        {
            // set the default background color if it's not set. a hack...
            if (CurBackgroundColor == default(Color)) CurBackgroundColor = BackgroundColor;

            PrimitiveRenderer.DrawRectangle(cRenderer, Position, BoxSize, CurBackgroundColor, Filled, Bordercolor, BorderSize, SnapToScreen);

            Font curFont = FontManager.GetFont(Font);

            // draw the currently selected item:
            // if the font is invalid use the default font
            // otherwise, use the Font Manager
            if (Items.Count > 0)
            {
                if (curFont == null)
                {
                    PrimitiveRenderer.DrawText(cRenderer, Items[SelectedIndex].Text, Position, ForegroundColor, true);
                }
                else
                {
                    FontManager.DrawText(cRenderer, Items[SelectedIndex].Text, Font, Position, ForegroundColor);
                }
            }

            // draw the items if they are open
            if (Open)
            {
                foreach (ListBoxItem item in Items)
                {
                    item.OnRender(cRenderer); // this is never null (set in constructor) so we do not need to check if it is.
                }
            }

        }

        /// <summary>
        /// Default mouse press event handler for listbox.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> that triggered the event.</param>
        public void ListBoxMousePressed(MouseButton button)
        {
            if (Open)
            {
                // call the default handler
                base.MousePressed(button);

                // check if the item is intersecting and if so pass the event on
                // and don't close it
                for (int curItem = 0; curItem < Items.Count; curItem++)
                {
                    ListBoxItem item = Items[curItem];

                    if (AABB.Intersects(item, button.Position))
                    {
                        // select the messagebox 
                        SelectedIndex = curItem;

                        // allow the user to override positions
                        item.OnMousePressed(button);
                    }

                }

                // change the open state of the listbox
                if (AABB.Intersects(this, button.Position)) Open = !Open;
            }
            else
            {
                if (BoxIntersects(button.Position))
                {
                    base.MousePressed(button);
                    Open = !Open;
                }
            }
        }

        private bool BoxIntersects(Vector2 position)
        {
            // write our own handler here.
            // this is VGUI tier code but its the only way it works, temporarily set the size to the box size and then do another collision test.
            Vector2 tSize = Size;

            Size = BoxSize;

            bool intersects = (AABB.Intersects(this, position));

            Size = tSize;

            return intersects;
        }

        /// <summary>
        /// Default mouse release event handler for listbox.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> that triggered the event.</param>
        public void ListBoxMouseReleased(MouseButton button)
        {
            base.MouseReleased(button);

            if (Open)
            {
                // check if the item is intersecting and if so pass the event on
                // and don't close it
                foreach (ListBoxItem item in Items)
                {
                    if (AABB.Intersects(item, button.Position))
                    {
                        // allow the user to override positions
                        item.OnMouseReleased(button);
                    }
                }
            }
            else
            {
                // terrible handler
                if (BoxIntersects(button.Position))
                {
                    // we are hovering over the button so switch to the background color
                    CurBackgroundColor = HoverColor;
                }
                else
                {
                    CurBackgroundColor = BackgroundColor;
                }
            }

        }

        /// <summary>
        /// Default mouse move event handler for listbox.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> that triggered the event.</param>
        public virtual void ListBoxMouseMove(MouseButton button)
        {
            if (Open)
            {
                base.MouseMove(button);
            }
            else
            {
                if (BoxIntersects(button.Position))
                {
                    CurBackgroundColor = HoverColor;
                }
                else
                {
                    CurBackgroundColor = BackgroundColor;
                }
            }

            // pass the event on
            // and don't close it
            foreach (ListBoxItem item in Items)
            {
                // don't check for intersection as that's done here.
                item.OnMouseMove(button);
            }
        }
    }
}