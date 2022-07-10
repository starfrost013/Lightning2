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
            // set some default properties so the listbox renders properly
            item.Size = Size;
            // move the item so that it gets drawn in the right place
            item.Position = new Vector2(Position.X, Position.Y + (Size.Y * (Items.Count)));
            if (item.BackgroundColour == default(Color)) item.BackgroundColour = BackgroundColour;
            if (item.ForegroundColour == default(Color)) item.ForegroundColour = ForegroundColour;
            if (item.BorderColour == default(Color)) item.BorderColour = BorderColour;
            item.Filled = true; // set for now
            item.Font = Font;
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
                    item.OnRender(cWindow); // this is never null (set in constructor) so we do not need to check that it is.
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