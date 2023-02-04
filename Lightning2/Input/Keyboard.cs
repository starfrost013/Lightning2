namespace LightningGL
{
    /// <summary>
    /// Keyboard
    /// 
    /// September 25, 2022
    /// 
    /// Static class providing APIs for the current keyboard state. See <see cref="NewKey"/> for information on the key press event api.
    /// </summary>
    public static class Keyboard
    {
        public static SDL_Keymod Modifiers => SDL_GetModState();

        public static bool IsEitherCtrlPressed() => SDL_GetModState().HasFlag(SDL_Keymod.KMOD_LCTRL) | SDL_GetModState().HasFlag(SDL_Keymod.KMOD_RCTRL);

        public static bool IsEitherShiftPressed() => SDL_GetModState().HasFlag(SDL_Keymod.KMOD_LSHIFT) | SDL_GetModState().HasFlag(SDL_Keymod.KMOD_RSHIFT);

        public static bool IsEitherAltPressed() => SDL_GetModState().HasFlag(SDL_Keymod.KMOD_LALT) | SDL_GetModState().HasFlag(SDL_Keymod.KMOD_RALT);

        public static bool IsEitherGuiPressed() => SDL_GetModState().HasFlag(SDL_Keymod.KMOD_LGUI) | SDL_GetModState().HasFlag(SDL_Keymod.KMOD_RGUI);
    }
}
