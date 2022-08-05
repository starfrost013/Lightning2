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

        public bool HideCursor { get; set; }

        public int Capacity { get; set; }

        public int CursorThickness { get; set; }
        
        public Color CursorColour { get; set; } 

        public int CursorBlinkFrequency { get; set; }

        public TextBox(int capacity) : base()
        {
            Capacity = capacity;
            OnKeyPressed += KeyPressed;
            OnRender += Render;
            CursorThickness = 2;
            CursorBlinkFrequency = 25;
            CursorColour = Color.White;
        }

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
                    return;
                default:
                    string keyStr = key.KeySym.ToString();

                    // check if lowercase
                    if (lowercase) keyStr = keyStr.ToLower();   
                    Text = $"{Text}{keyStr}";
                    return;
            }


        }

        public void Render(Window cWindow)
        {
            PrimitiveRenderer.DrawRectangle(cWindow, Position, Size, BackgroundColour, true, BorderColour, BorderSize);
            FontManager.DrawText(cWindow, Text, Font, Position, ForegroundColour);

            if (!HideCursor)
            {
                Vector2 fontSize = FontManager.GetLargestTextSize(Font, Text);
                Vector2 cursorPosition = new Vector2(Position.X + fontSize.X, Position.Y);

                // actually blink it
                if (cWindow.FrameNumber % CursorBlinkFrequency == 0) PrimitiveRenderer.DrawRectangle(cWindow, cursorPosition, new Vector2(CursorThickness, Size.Y), CursorColour);
            }
        }
    }
}
