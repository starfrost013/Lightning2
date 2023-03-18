namespace LightningGL
{
    public class Key
    {
        /// <summary>
        /// The <see cref="SDL_Keysym"/> of the keyboard that has been pressed.
        /// </summary>
        public SDL_Keysym KeySym { get; internal set; }

        /// <summary>
        /// The keyboard modifier state at the time that this button was pressed.
        /// </summary>
        public SDL_Keymod Modifiers { get; internal set; }

        /// <summary>
        /// Determines if this key is being repeated or not. This begins a short time after it is first held down, NOT when it is held down!
        /// </summary>
        public bool Repeated { get; internal set; }

        /// <summary>
        /// Key cast operator from SDL_KeyboardEvent
        /// </summary>
        /// <param name="nKeySym">The <see cref="SDL_KeyboardEvent"/> to convert to <see cref="InputMethodKeyboardMouse"/></param>

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
        /// Determines if either SHIFT key is pressed as of when the <see cref="Key"/> object was created.
        /// </summary>
        /// <returns>True if either SHIFT key was held when the <see cref="Key"/> object was created.</returns>
        public bool EitherShiftPressed() => Modifiers.HasFlag(SDL_Keymod.KMOD_LSHIFT) || Modifiers.HasFlag(SDL_Keymod.KMOD_RSHIFT);

        /// <summary>
        /// Determines if either ALT key is pressed as of when the <see cref="Key"/> object was created. 
        /// </summary>
        /// <returns>True if either ALT key was held when the <see cref="Key"/> object was created.</returns>
        public bool EitherAltPressed() => Modifiers.HasFlag(SDL_Keymod.KMOD_LALT) || Modifiers.HasFlag(SDL_Keymod.KMOD_RALT);

        /// <summary>
        /// Determines if either CTRL keywas pressed as of when the <see cref="Key"/> object was created.
        /// </summary>
        /// <returns>True if either CTRL key was held when the <see cref="Key"/> object was created.</returns>
        public bool EitherCtrlPressed() => Modifiers.HasFlag(SDL_Keymod.KMOD_LCTRL) || Modifiers.HasFlag(SDL_Keymod.KMOD_RCTRL);

        /// <summary>
        /// Determines if either GUI (e.g. Apple, Windows) key was pressed as of when the <see cref="Key"/> object was created.
        /// </summary>
        /// <returns>True if either GUI (e.g. Apple, Windows) key was pressed as of when the <see cref="Key"/> object was created.</returns>
        public bool EitherGuiPressed() => Modifiers.HasFlag(SDL_Keymod.KMOD_LGUI) || Modifiers.HasFlag(SDL_Keymod.KMOD_RGUI);

        /// <summary>
        /// Returns the capslock state as of when the <see cref="Key"/> object was created.
        /// </summary>
        /// <returns>A boolean value holding the capslock state as of when the <see cref="Key"/> object was created.</returns>
        public bool GetCapsLockState() => Modifiers.HasFlag(SDL_Keymod.KMOD_CAPS);

        /// <summary>
        /// Returns the numlock state as of when the <see cref="Key"/> object was created.
        /// </summary>
        /// <returns>A boolean value holding the numlock state as of when the <see cref="Key"/> object was created.</returns>
        public bool GetNumLockState() => Modifiers.HasFlag(SDL_Keymod.KMOD_NUM);

        /// <summary>
        /// Returns the scroll-lock state as of when the <see cref="Key"/> object was created.
        /// </summary>
        /// <returns>A boolean value holding the scroll-lock state as of when the <see cref="Key"/> object was created.</returns>
        public bool GetScrollLockState() => Modifiers.HasFlag(SDL_Keymod.KMOD_SCROLL);

        /// <summary>
        /// Determines if either SHIFT key is pressed.
        /// </summary>
        /// <returns>A boolean determining if either SHIFT key is pressed.</returns>
        public static bool EitherShiftCurrentlyPressed()
        {
            // sdl nonvideo is allowed for multiplatform as we use sdl for non-video things
            SDL_Keymod curState = SDL_GetModState();

            return curState.HasFlag(SDL_Keymod.KMOD_LSHIFT) || curState.HasFlag(SDL_Keymod.KMOD_RSHIFT);
        }

        /// <summary>
        /// Determines if either ALT key is pressed.
        /// </summary>
        /// <returns>A boolean determining if either ALT key is pressed.</returns>
        public static bool EitherAltCurrentlyPressed()
        {
            // sdl nonvideo is allowed for multiplatform as we use sdl for non-video things
            SDL_Keymod curState = SDL_GetModState();

            return curState.HasFlag(SDL_Keymod.KMOD_LALT) || curState.HasFlag(SDL_Keymod.KMOD_RALT);
        }

        /// <summary>
        /// Determines if either CTRL key is pressed.
        /// </summary>
        /// <returns>A boolean determining if either CTRL key is pressed.</returns>
        public static bool EitherCtrlCurrentlyPressed()
        {
            // sdl nonvideo is allowed for multiplatform as we use sdl for non-video things
            SDL_Keymod curState = SDL_GetModState();

            return curState.HasFlag(SDL_Keymod.KMOD_LCTRL) || curState.HasFlag(SDL_Keymod.KMOD_RCTRL);
        }

        /// <summary>
        /// Determines if either GUI key is pressed.
        /// </summary>
        /// <returns>A boolean determining if either GUI key is pressed.</returns>
        public static bool EitherGuiCurrentlyPressed()
        {
            // sdl nonvideo is allowed for multiplatform as we use sdl for non-video things
            SDL_Keymod curState = SDL_GetModState();

            return curState.HasFlag(SDL_Keymod.KMOD_LGUI) || curState.HasFlag(SDL_Keymod.KMOD_RGUI);
        }

        /// <summary>
        /// Determines the current state of the CAPS LOCK key.
        /// </summary>
        /// <returns>A boolean holding the current state of the CAPS LOCK key.</returns>
        public static bool GetCapsLockCurrentState()
        {
            // sdl nonvideo is allowed for multiplatform as we use sdl for non-video things
            SDL_Keymod curState = SDL_GetModState();

            return curState.HasFlag(SDL_Keymod.KMOD_CAPS);
        }

        /// <summary>
        /// Determines the current state of the NUM LOCK key.
        /// </summary>
        /// <returns>A boolean holding the current state of the NUM LOCK key.</returns>
        public static bool GetNumLockCurrentState()
        {
            // sdl nonvideo is allowed for multiplatform as we use sdl for non-video things
            SDL_Keymod curState = SDL_GetModState();

            return curState.HasFlag(SDL_Keymod.KMOD_NUM);
        }

        /// <summary>
        /// Determines the current state of the SCROLL LOCK key.
        /// </summary>
        /// <returns>A boolean holding the current state of the SCROLL LOCK key.</returns>
        public static bool GetScrollLockCurrentState()
        {
            // sdl nonvideo is allowed for multiplatform as we use sdl for non-video things
            SDL_Keymod curState = SDL_GetModState();

            return curState.HasFlag(SDL_Keymod.KMOD_SCROLL);
        }
    }
}
