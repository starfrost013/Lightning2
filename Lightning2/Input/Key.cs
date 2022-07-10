using static NuCore.SDL2.SDL;

namespace LightningGL
{
    public class Key
    {
        public SDL_Keysym KeySym { get; set; }

        /// <summary>
        /// The keyboard modifier state at the time that this button was pressed.
        /// </summary>
        public SDL_Keymod KeyMod { get; set; }

        public bool Repeated { get; set; }

        public static explicit operator Key(SDL_KeyboardEvent nKeySym)
        {
            return new Key
            {
                KeySym = nKeySym.keysym,
                KeyMod = SDL_GetModState(),
                Repeated = (nKeySym.repeat > 0)
            };
        }

        public override string ToString() => KeySym.ToString();

    }
}
