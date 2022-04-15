using NuCore.SDL2;

namespace Lightning2
{
    public class Key
    {
        public SDL.SDL_Keysym KeySym { get; set; }

        /// <summary>
        /// Any modifier state allowed
        /// </summary>
        public SDL.SDL_Keymod KeyMod => SDL.SDL_GetModState();

        public bool Repeated { get; set; }

        public static explicit operator Key(SDL.SDL_KeyboardEvent NKeySym)
        {
            return new Key
            {
                KeySym = NKeySym.keysym,
                Repeated = (NKeySym.repeat > 0)
            };
        }

        public override string ToString() => KeySym.ToString();

    }
}
