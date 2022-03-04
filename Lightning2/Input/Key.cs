using NuCore.SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    public class Key
    {
        public SDL.SDL_Keysym KeySym { get; set; }

        /// <summary>
        /// Any modifier state allowed
        /// </summary>
        public SDL.SDL_Keymod KeyMod => SDL.SDL_GetModState();

        public static explicit operator Key(SDL.SDL_Keysym NKeySym)
        {
            return new Key { KeySym = NKeySym };
        }

    }
}
