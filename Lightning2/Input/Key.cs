using static NuCore.SDL2.SDL;

namespace LightningGL
{
    public class Key
    {
        /// <summary>
        /// The <see cref="SDL_Keysym"/> of the keyboard that has been pressed.
        /// </summary>
        public SDL_Keysym KeySym { get; set; }

        /// <summary>
        /// The keyboard modifier state at the time that this button was pressed.
        /// </summary>
        public SDL_Keymod Modifiers { get; set; }

        public bool Repeated { get; set; }

        public static explicit operator Key(SDL_KeyboardEvent nKeySym)
        {
            return new Key
            {
                KeySym = nKeySym.keysym,
                Modifiers = SDL_GetModState(),
                Repeated = (nKeySym.repeat > 0)
            };
        }

        public override string ToString() => KeySym.ToString();

    }
}
