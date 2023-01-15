﻿namespace LightningGL
{
    /// <summary>
    /// ListBox
    /// 
    /// June 15, 2022
    /// 
    /// Defines a list box gadget. A list box is a list of options that can be selected by the user.
    /// This is a horrible hack and it needs to be rewritten.
    /// </summary>
    public class ListBox : Gadget
    {
        /// <summary>
        /// Determines if the list box is open or not.
        /// </summary>
        public bool Open { get; set; }

        /// <summary>
        /// Backing field for <see cref="SelectedIndex"/>
        /// </summary>
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
                    || value > Children.Count - 1) NCError.ShowErrorBox("Attempted to set an invalid SelectedIndex for this ListBox!", 83,
                        NCErrorSeverity.FatalError);

                _selectedindex = value;
            }
        }

        /// <summary>
        /// The current selected item. Simply returns Items[SelectedIndex].
        /// </summary>
        public ListBoxItem SelectedItem => (ListBoxItem)Children[SelectedIndex];

        /// <summary>
        /// Determines if item colors will not be alternated on every other item.
        /// </summary>
        public bool DontAlternateItemColors { get; set; }

        /// <summary>
        /// Determines how much the alternate item colors will be modified by.
        /// A negative value brightens the colors. Default value is 30.
        /// </summary>
        public int AlternateItemColorsAmount { get; set; }

        /// <summary>
        /// Private: Size used to determine the size of the box that is actually drawn when the item is not open
        /// </summary>
        private Vector2 BoxSize { get; set; }

        /// <summary>
        /// UI rectangle used for drawing this textbox.
        /// </summary>
        private Rectangle? Rectangle { get; set; }


        /// <summary>
        /// Static listbox constructor.
        /// </summary>
        public ListBox(string name, string font) : base(name, font)
        {
            // if you override the method it calls it twice (because it's defined for both classes)
            // so simply use a different method
            OnMousePressed += ListBoxMousePressed;
            OnMouseReleased += ListBoxMouseReleased;
            OnMouseMove += ListBoxMouseMove;

            AlternateItemColorsAmount = 30;
        }

        public override void Create()
        {
            // HACK: Don't make this a child of this so that it does not break everything
            // Make this hack go away in the future
            Rectangle = Lightning.Renderer.AddRenderable(new Rectangle("ListBoxRectangle", Position, BoxSize, CurBackgroundColor, Filled, BorderColor, BorderSize, SnapToScreen));
        }

        /// <summary>
        /// Adds an item to this ListBox.
        /// </summary>
        /// <param name="item">The <see cref="ListBoxItem"/> to add to this ListBox.</param>
        public void AddItem(ListBoxItem item)
        {
            Debug.Assert(Rectangle != null);

            if (Font == null)
            {
                NCError.ShowErrorBox($"Tried to add an item to a ListBox with an invalid font", 185,
                    NCErrorSeverity.FatalError);
                return;
            }

            Font? itemFont = (Font?)Lightning.Renderer.GetRenderableByName(Font);

            if (itemFont == null)
            {
                NCError.ShowErrorBox($"Tried to add an item to a ListBox with an invalid font", 187,
                   NCErrorSeverity.FatalError);
                return;
            }

            item.Font = Font;

            Vector2 itemFontSize = FontManager.GetTextSize(itemFont, item.Text, item.ForegroundColor, item.Style, FontSmoothingType.Default);

            // size it to the size of the text in the Y dimension
            BoxSize = new(Size.X, (itemFontSize.Y * 1.25f));
            item.Size = BoxSize;
            Rectangle.Size = BoxSize;
            
            // move the item so that it gets drawn in the right place
            item.Position = new(Position.X, Position.Y + ((itemFontSize.Y * 1.25f) * (Children.Count + 1)));
            if (item.BackgroundColor == default) item.BackgroundColor = BackgroundColor;
            if (item.ForegroundColor == default) item.ForegroundColor = ForegroundColor;
            if (item.BorderColor == default) item.BorderColor = BorderColor;

            // alternate the colors so they look a bit better (if hte user specified it)
            if (Children.Count % 2 == 0
                && !DontAlternateItemColors)
            {
                int altR = item.BackgroundColor.R - AlternateItemColorsAmount,
                    altG = item.BackgroundColor.G - AlternateItemColorsAmount,
                    altB = item.BackgroundColor.B - AlternateItemColorsAmount;

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
            Lightning.Renderer.AddRenderable(item, this);
        }

        /// <summary>
        /// Renders this ListBox.
        /// </summary>
        public override void Draw()
        {
            // set the default background color if it's not set. a hack...
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

            Debug.Assert(Rectangle != null);

            Rectangle.Color = CurBackgroundColor;

            Font? curFont = (Font?)Lightning.Renderer.GetRenderableByName(Font);

            if (curFont == null)
            {
                NCError.ShowErrorBox($"Tried to add an item to a ListBox with an invalid font", 186, NCErrorSeverity.FatalError);
                return;
            }

            // draw the currently selected item using the Font Managher
            // if the font is invalid throw an error

            if (Children.Count > 0) Lightning.Renderer.AddRenderable(new TextBlock("ListBoxItemText", SelectedItem.Text, Font, Position, ForegroundColor));

            // draw the items if they are open
            foreach (ListBoxItem item in Children) item.IsNotRendering = !Open; // this is never null (set in constructor) so we do not need to check if it is.
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
                for (int curItem = 0; curItem < Children.Count; curItem++)
                {
                    ListBoxItem item = (ListBoxItem)Children[curItem];

                    if (item != null)
                    {
                        if (AABB.Intersects(item, button.Position))
                        {
                            // select the messagebox 
                            SelectedIndex = curItem;

                            // allow the user to override positions
                            item.OnMousePressed?.Invoke(button);
                        }
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
                foreach (ListBoxItem item in Children)
                {
                    if (AABB.Intersects(item, button.Position))
                    {
                        // allow the user to override positions
                        item.OnMouseReleased?.Invoke(button);
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
            foreach (ListBoxItem item in Children)
            {
                // don't check for intersection as that's done here.
                item.OnMouseMove?.Invoke(button);
            }
        }
    }
}