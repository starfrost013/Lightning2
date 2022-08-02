using static NuCore.SDL2.SDL;
using NuCore.Utilities;

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

        public int Capacity { get; set; }

        public TextBox(int capacity) : base()
        {
            Capacity = capacity;
            OnKeyPressed += KeyPressed;
            OnRender += Render;
        }

        public void KeyPressed(Key key)
        {
            SDL_Scancode keySym = key.KeySym.scancode;
            SDL_Keymod keyMod = key.KeySym.mod;

            // SDL_EnableUNICODE is not supported and also doesn't translate key release events
            // so let's roll our own...

            bool lowercase = (keyMod.HasFlag(SDL_Keymod.KMOD_LSHIFT) || keyMod.HasFlag(SDL_Keymod.KMOD_RSHIFT) || !keyMod.HasFlag(SDL_Keymod.KMOD_CAPS));
            
            // switch various selected keys
            switch (keySym)
            {
                // remove a character
                case SDL_Scancode.SDL_SCANCODE_BACKSPACE:
                case SDL_Scancode.SDL_SCANCODE_KP_BACKSPACE:
                    if (Text.Length > 0) Text = Text.Remove(Text.Length - 1, 1);
                    return;
                // append a space
                case SDL_Scancode.SDL_SCANCODE_SPACE:
                case SDL_Scancode.SDL_SCANCODE_KP_SPACE:
                    Text = $"{Text} ";
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
            //PrimitiveRenderer.DrawRectangle(this, )
            FontManager.DrawText(cWindow, Text, Font, Position, ForegroundColour, BackgroundColour);
        }
    }
}
