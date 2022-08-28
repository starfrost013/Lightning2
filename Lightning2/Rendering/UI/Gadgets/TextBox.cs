using static NuCore.SDL2.SDL;
using NuCore.Utilities;
using System.Drawing;
using System.Numerics;

namespace LightningGL
{
    public class TextBox : Gadget
    {
        public string Text { get; set; }    

        private int _curcursorposition { get; set; }

        private int CurCursorPosition
        {
            get
            {
                return _curcursorposition;
            }
            set
            {
                if (_curcursorposition > Text.Length) _ = new NCException("Error: Attempted to move a textbox cursor beyond the text length!", 106, "TextBox::CurCursorPosition > TextBox::Text::Length!", NCExceptionSeverity.FatalError);
            }
        }

        /// <summary>
        /// A boolean determining if the cursor is hidden or not.
        /// </summary>
        public bool HideCursor { get; set; }

        /// <summary>
        /// The capacity of the textbox.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// The thickness of the cursor in pixels.
        /// </summary>
        public short CursorThickness { get; set; }
        
        /// <summary>
        /// The colour of the cursor.
        /// </summary>
        public Color CursorColour { get; set; } 

        /// <summary>
        /// The number of frames between cursor blinks.
        /// </summary>
        public int CursorBlinkFrequency { get; set; }

        /// <summary>
        /// The length of time the cursor will blink.
        /// </summary>
        public int CursorBlinkLength { get; set; }

        public TextBox(int capacity) : base()
        {
            Capacity = capacity;
            OnKeyPressed += KeyPressed;
            OnRender += Render;
            if (CursorThickness == 0) CursorThickness = 2;
            if (CursorBlinkFrequency == 0) CursorBlinkFrequency = 100;
            if (CursorColour == default) CursorColour = Color.White;
            if (CursorBlinkLength == 0) CursorBlinkLength = 50;
        }

        /// <summary>
        /// Keypress handler for TextBoxes.
        /// </summary>
        /// <param name="key">The key that has been pressed.</param>
        public void KeyPressed(Key key)
        {
            // reject if text longer than capacity
            if (Text != null
                && Text.Length > Capacity) return;

            SDL_Scancode keySym = key.KeySym.scancode;
            SDL_Keymod keyMod = key.KeySym.mod;

            // SDL_EnableUNICODE is not supported and also doesn't translate key release events
            // so let's roll our own...

            bool lowercase = (!keyMod.HasFlag(SDL_Keymod.KMOD_CAPS));

            // invert case on shift
            if (keyMod.HasFlag(SDL_Keymod.KMOD_LSHIFT) || keyMod.HasFlag(SDL_Keymod.KMOD_RSHIFT)) lowercase = !lowercase;

            // switch various selected keys
            switch (keySym)
            {
                // remove a character
                case SDL_Scancode.SDL_SCANCODE_BACKSPACE:
                case SDL_Scancode.SDL_SCANCODE_KP_BACKSPACE:
                    if (Text.Length > 0) Text = Text.Remove(Text.Length - 1, 1);
                    return;
                // do nothing
                case SDL_Scancode.SDL_SCANCODE_LEFT:
                case SDL_Scancode.SDL_SCANCODE_RIGHT:
                case SDL_Scancode.SDL_SCANCODE_UP:
                case SDL_Scancode.SDL_SCANCODE_DOWN:
                case SDL_Scancode.SDL_SCANCODE_LALT:
                case SDL_Scancode.SDL_SCANCODE_RALT:
                // already handled these three
                case SDL_Scancode.SDL_SCANCODE_LSHIFT:
                case SDL_Scancode.SDL_SCANCODE_RSHIFT:
                case SDL_Scancode.SDL_SCANCODE_CAPSLOCK:
                // some more:
                case SDL_Scancode.SDL_SCANCODE_LGUI:
                case SDL_Scancode.SDL_SCANCODE_RGUI:
                case SDL_Scancode.SDL_SCANCODE_LCTRL:
                case SDL_Scancode.SDL_SCANCODE_RCTRL:
                case SDL_Scancode.SDL_SCANCODE_RETURN:
                case SDL_Scancode.SDL_SCANCODE_RETURN2:
                case SDL_Scancode.SDL_SCANCODE_KP_ENTER:
                    return;
                default:
                    string keyStr = key.KeySym.ToString();

                    // check if lowercase
                    if (lowercase) keyStr = keyStr.ToLower();   
                    Text = $"{Text}{keyStr}";
                    return;
            }
        }

        /// <summary>
        /// Renders this textbox to a window.
        /// </summary>
        /// <param name="cWindow">The window to render this textbox to.</param>
        public void Render(Window cWindow)
        {
            PrimitiveRenderer.DrawRectangle(cWindow, Position, Size, CurBackgroundColour, true, BorderColour, BorderSize, SnapToScreen);
            FontManager.DrawText(cWindow, Text, Font, Position, ForegroundColour);

            if (!HideCursor)
            {
                Vector2 fontSize = FontManager.GetLargestTextSize(Font, Text);
                Vector2 cursorPosition = new Vector2(Position.X + fontSize.X, Position.Y);

                // actually blink it
                if (cWindow.FrameNumber % CursorBlinkFrequency <= (CursorBlinkLength - 1)) PrimitiveRenderer.DrawLine(cWindow, cursorPosition, 
                    new Vector2(cursorPosition.X, cursorPosition.Y + Size.Y), CursorThickness, CursorColour);
            }
        }
    }
}
