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

        /// <summary>
        /// Determines if item colours will be alternated on every other item.
        /// </summary>
        public bool AlternateItemColours { get; set; }

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

        public ListBox() : base()
        {
            Items = new List<ListBoxItem>();
            OnRender += Render;

            // if you override the method it calls it twice (because it's defined for both classes)
            // so simply use a different method
            OnMousePressed += ListBoxMousePressed;
            OnMouseReleased += MouseReleased;
        }

        public void AddItem(ListBoxItem item)
        {
            item.Font = Font;
            Font itemFont = FontManager.GetFont(Font);

            Vector2 itemFontSize = FontManager.GetTextSize(itemFont, item.Text);

            // size it to the size of the text in the Y dimension
            item.Size = new Vector2(Size.X, itemFontSize.Y);

            // move the item so that it gets drawn in the right place
            item.Position = new Vector2(Position.X, Position.Y + ((itemFontSize.Y * 1.25f) * Items.Count));
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

            Items.Add(item);
        }

        public void Render(Window cWindow)
        {
            PrimitiveRenderer.DrawRectangle(cWindow, Position, Size, CurBackgroundColour, Filled, BorderColour, BorderSize);

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
                    FontManager.DrawText(cWindow, Items[SelectedIndex].Text, Font, Position, ForegroundColour, BackgroundColour);
                }
            }

            if (Open)
            {
                foreach (ListBoxItem item in Items)
                {
                    if (item != SelectedItem)
                    {
                        item.OnRender(cWindow); // this is never null (set in constructor) so we do not need to check that it is.
                    }
                }
            }

        }

        public void ListBoxMousePressed(SDL_MouseButton button, Vector2 position)
        {
            // call the default handler
            base.MousePressed(button, position);

            // call teh
            foreach (ListBoxItem item in Items)
            {
                // allow the user to override positions
                item.OnMousePressed(button, position);
            }

            if (AABB.Intersects(this, position))
            {
                Open = !Open;
                return;
            }
        }
    }
}