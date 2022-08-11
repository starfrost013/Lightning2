using static NuCore.SDL2.SDL;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// CheckBox
    /// 
    /// August 10, 2022
    /// 
    /// Defines a checkbox gadget
    /// </summary>
    public class CheckBox : Gadget
    {
        /// <summary>
        /// Determines if this check box is checked.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// The thickness of the line of this CheckBox.
        /// </summary>
        public short Thickness { get; set; }

        /// <summary>
        /// Constructor for the CheckBox class.
        /// </summary>
        public CheckBox() : base()
        {
            OnRender += Render;
            OnMousePressed += CheckBoxMousePressed;
            if (Thickness == 0) Thickness = 2;
        }

        public void Render(Window cWindow)
        {
            PrimitiveRenderer.DrawRectangle(cWindow, Position, Size, CurBackgroundColour, Filled, BorderColour, BorderSize, SnapToScreen);
            
            if (Checked)
            {
                Vector2 line1Start = new Vector2(Position.X, Position.Y + (Size.Y / 2));
                Vector2 line1End = new Vector2(Position.X + (Size.X / 3), Position.Y + Size.Y);
                Vector2 line2Start = line1End;
                Vector2 line2End = new Vector2(Position.X + Size.X, Position.Y);

                PrimitiveRenderer.DrawLine(cWindow, line1Start, line1End, Thickness, ForegroundColour, true, SnapToScreen);
                PrimitiveRenderer.DrawLine(cWindow, line2Start, line2End, Thickness, ForegroundColour, true, SnapToScreen);
            }
        }

        public void CheckBoxMousePressed(SDL_MouseButton button, Vector2 position)
        {
            base.MousePressed(button, position);
            Checked = !Checked;
        }
    }
}
