﻿using static NuCore.SDL2.SDL;

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

        /// <summary>
        /// Determines if this key is being repeated or not. This begins a short time after it is first held down, NOT when it is held down!
        /// </summary>
        public bool Repeated { get; set; }
        
        /// <summary>
        /// Key cast operator from SDL_KeyboardEvent
        /// </summary>
        /// <param name="nKeySym">The <see cref="SDL_KeyboardEvent"/> to convert to <see cref="Key"/></param>

        public static explicit operator Key(SDL_KeyboardEvent nKeySym)
        {
            return new Key
            {
                KeySym = nKeySym.keysym,
                Modifiers = SDL_GetModState(),
                Repeated = (nKeySym.repeat > 0)
            };
        }

        /// <summary>
        /// Converts a key to its string representation.
        /// </summary>
        /// <returns>A string representation of the key pressed.</returns>
        public override string ToString() => KeySym.ToString();

        /// <summary>
        /// Determines if either SHIFT key is pressed. Do not use <see cref="SDL_Keymod.KMOD_SHIFT"/> for this as it checks BOTH shift keys!
        /// </summary>
        /// <returns></returns>
        public bool EitherShiftPressed() => (Modifiers.HasFlag(SDL_Keymod.KMOD_LSHIFT) || Modifiers.HasFlag(SDL_Keymod.KMOD_RSHIFT));

        /// <summary>
        /// Determines if either ALT key is pressed. Do not use <see cref="SDL_Keymod.KMOD_ALT"/> for this as it checks BOTH ALT keys!
        /// </summary>
        /// <returns></returns>
        public bool EitherAltPressed() => (Modifiers.HasFlag(SDL_Keymod.KMOD_LALT) || Modifiers.HasFlag(SDL_Keymod.KMOD_RALT));

        /// <summary>
        /// Determines if either CTRL key is pressed. Do not use <see cref="SDL_Keymod.KMOD_CTRL"/> for this as it checks BOTH CTRL keys!
        /// </summary>
        /// <returns></returns>
        public bool EitherCtrlPressed() => (Modifiers.HasFlag(SDL_Keymod.KMOD_LCTRL) || Modifiers.HasFlag(SDL_Keymod.KMOD_RCTRL));
    }
}
