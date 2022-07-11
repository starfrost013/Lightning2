using static NuCore.SDL2.SDL;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing; 
using System.Numerics;

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
        public bool Open { get; set; }

        public List<ListBoxItem> Items { get; private set; }

        private int _selectedindex;

        public int SelectedIndex
        {
            get
            {
                return _selectedindex; 
            }
            set
            {
                if (value > Items.Count - 1) _ = new NCException("Attempted to set an invalid SelectedIndex for this ListBox!", 83, "ListBox::SelectedIndex > ListBox::Items::Count - 1!", NCExceptionSeverity.FatalError);
                
                _selectedindex = value;
            }
        }

        public ListBoxItem SelectedItem => Items[SelectedIndex];

        /// <summary>
        /// Determines if item colours will be alternated on every other item.
        /// </summary>
        public bool AlternateItemColours { get; set; }

        /// <summary>
        /// Private: Size used to determine the size of the box that is actually drawn when the item is not open
        /// </summary>
        private Vector2 BoxSize { get; set; }

        public ListBox() : base()
        {
            Items = new List<ListBoxItem>();
            OnRender += Render;

            // if you override the method it calls it twice (because it's defined for both classes)
            // so simply use a different method
            OnMousePressed += ListBoxMousePressed;
            OnMouseReleased += ListBoxMouseReleased;
            OnMouseMove += ListBoxMouseMove;
        }

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
            if (item.BackgroundColour == default(Color)) item.BackgroundColour = BackgroundColour;
            if (item.ForegroundColour == default(Color)) item.ForegroundColour = ForegroundColour;
            if (item.BorderColour == default(Color)) item.BorderColour = BorderColour;

            // alternate the colours so they look a bit better
            if (Items.Count % 2 == 0)
            {
                int altR = item.BackgroundColour.R - 30,
                    altG = item.BackgroundColour.G - 30,
                    altB = item.BackgroundColour.B - 30;

                // make sure we are in valid colour ranges (not required for alpha)
                if (altR < 0) altR = 0;
                if (altG < 0) altG = 0;
                if (altB < 0) altB = 0;

                item.BackgroundColour = Color.FromArgb(item.BackgroundColour.A,
                        altR,
                        altG,
                        altB);
            }


            item.Filled = true; // set for now

            // resize the listbox 
            Size = new Vector2(Size.X, Size.Y + (BoxSize.Y));
            Items.Add(item);
        }

        public void Render(Window cWindow)
        {
            // set the default background colour
            if (CurBackgroundColour == default(Color)) CurBackgroundColour = BackgroundColour;

            PrimitiveRenderer.DrawRectangle(cWindow, Position, BoxSize, CurBackgroundColour, Filled, BorderColour, BorderSize);

            Font curFont = FontManager.GetFont(Font);

            // draw text:
            // if the font is invalid use the default font
            // otherwise, use the Font Manager
            if (Items.Count > 0) 
            {
                if (curFont == null)
                {
                    PrimitiveRenderer.DrawText(cWindow, Items[SelectedIndex].Text, Position, ForegroundColour, true);
                }
                else
                {
                    FontManager.DrawText(cWindow, Items[SelectedIndex].Text, Font, Position, ForegroundColour);
                }
            }

            if (Open)
            {
                foreach (ListBoxItem item in Items)
                {
                    item.OnRender(cWindow); // this is never null (set in constructor) so we do not need to check if it is.
                }
            }

        }

        public void ListBoxMousePressed(SDL_MouseButton button, Vector2 position)
        {
            // call the default handler
            base.MousePressed(button, position);

            // check if the item is intersecting and if so pass the event on
            // and don't close it
            for (int curItem = 0; curItem < Items.Count; curItem++)
            {
                ListBoxItem item = Items[curItem];

                if (AABB.Intersects(item, position)
                    && Open)
                {
                    // select the messagebox 
                    SelectedIndex = curItem;

                    // allow the user to override positions
                    item.OnMousePressed(button, position);
                }

            }

            if (AABB.Intersects(this, position))
            {
                Open = !Open;
                return;
            }
        }


        public void ListBoxMouseReleased(SDL_MouseButton button, Vector2 position)
        {
            base.MouseReleased(button, position);

            // check if the item is intersecting and if so pass the event on
            // and don't close it
            foreach (ListBoxItem item in Items)
            {
                if (AABB.Intersects(item, position)
                    && Open)
                {
                    // allow the user to override positions
                    item.OnMouseReleased(button, position);
                }
            }
        }

        public virtual void ListBoxMouseMove(SDL_MouseButton button, Vector2 position, Vector2 velocity)
        {
            base.MouseMove(button, position, velocity);

            // check if the item is intersecting and if so pass the event on
            // and don't close it
            foreach (ListBoxItem item in Items)
            {
                if (AABB.Intersects(item, position)
                    && Open)
                {
                    // allow the user to override positions
                    item.OnMouseMove(button, position, velocity);
                }
            }
        }
    }
}