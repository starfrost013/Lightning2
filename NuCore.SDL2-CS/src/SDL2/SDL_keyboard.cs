#region License
/* Lightning SDL2 Wrapper
 * 
 * Version 3.0 (NuRender/Lightning) + SDL2_gfx
 * Copyright © 2022 starfrost
 * February 13, 2022
 * 
 * This software is based on the open-source SDL2# - C# Wrapper for SDL2 library.
 *
 * Copyright (c) 2013-2021 Ethan Lee.
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software in a
 * product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source distribution.
 *
 * Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
 *
 */
#endregion

#region Using Statements
using System;
using System.Runtime.InteropServices;
#endregion

namespace NuCore.SDL2
{
    public static partial class SDL
    {

        #region SDL_keyboard.h

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Keysym
        {
            public SDL_Scancode scancode;
            public SDL_Keycode sym;
            public SDL_Keymod mod; /* UInt16 */
            public UInt32 unicode; /* Deprecated */

            // April 13, 2021: Created
            // August 2, 2022: Modified to add punctuation replacements

            /// <summary>
            /// Converts the <see cref="sym"/> property of this <see cref="SDL_Keysym"/> to a string.
            /// </summary>
            /// <returns>A string containing the key that the user has just pressed.</returns>
            public override string ToString()
            {
                string processedString = sym.ToString();

                processedString = processedString.ToUpperInvariant();
                processedString = processedString.Replace("SDLK_", "");
                processedString = processedString.Replace("PERIOD", ".");
                processedString = processedString.Replace("COMMA", ",");
                processedString = processedString.Replace("SEMICOLON", ";");
                processedString = processedString.Replace("COLON", ":");
                processedString = processedString.Replace("KP_SPACE", " ");
                processedString = processedString.Replace("SPACE", " ");
                processedString = processedString.Replace("RETURN2", "\n");
                processedString = processedString.Replace("RETURN", "\n");
                processedString = processedString.Replace("MINUS", "-");
                return processedString; 
            }
        }

        /* Get the window which has kbd focus */
        /* Return type is an SDL_Window pointer */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetKeyboardFocus();

        /* Get a snapshot of the keyboard state. */
        /* Return value is a pointer to a UInt8 array */
        /* Numkeys returns the size of the array if non-null */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetKeyboardState(out int numkeys);

        /* Get the current key modifier state for the keyboard. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Keymod SDL_GetModState();

        /* Set the current key modifier state */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetModState(SDL_Keymod modstate);

        /* Get the key code corresponding to the given scancode
		 * with the current keyboard layout.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Keycode SDL_GetKeyFromScancode(SDL_Scancode scancode);

        /* Get the scancode for the given keycode */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Scancode SDL_GetScancodeFromKey(SDL_Keycode key);

        /* Wrapper for SDL_GetScancodeName */
        [DllImport(nativeLibName, EntryPoint = "SDL_GetScancodeName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetScancodeName(SDL_Scancode scancode);
        public static string SDL_GetScancodeName(SDL_Scancode scancode)
        {
            return UTF8_ToManaged(
                INTERNAL_SDL_GetScancodeName(scancode)
            );
        }

        /* Get a scancode from a human-readable name */
        [DllImport(nativeLibName, EntryPoint = "SDL_GetScancodeFromName", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe SDL_Scancode INTERNAL_SDL_GetScancodeFromName(
            byte* name
        );
        public static unsafe SDL_Scancode SDL_GetScancodeFromName(string name)
        {
            int utf8NameBufSize = Utf8Size(name);
            byte* utf8Name = stackalloc byte[utf8NameBufSize];
            return INTERNAL_SDL_GetScancodeFromName(
                Utf8Encode(name, utf8Name, utf8NameBufSize)
            );
        }

        /* Wrapper for SDL_GetKeyName */
        [DllImport(nativeLibName, EntryPoint = "SDL_GetKeyName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetKeyName(SDL_Keycode key);
        public static string SDL_GetKeyName(SDL_Keycode key)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetKeyName(key));
        }

        /* Get a key code from a human-readable name */
        [DllImport(nativeLibName, EntryPoint = "SDL_GetKeyFromName", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe SDL_Keycode INTERNAL_SDL_GetKeyFromName(
            byte* name
        );
        public static unsafe SDL_Keycode SDL_GetKeyFromName(string name)
        {
            int utf8NameBufSize = Utf8Size(name);
            byte* utf8Name = stackalloc byte[utf8NameBufSize];
            return INTERNAL_SDL_GetKeyFromName(
                Utf8Encode(name, utf8Name, utf8NameBufSize)
            );
        }

        /* Start accepting Unicode text input events, show keyboard */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_StartTextInput();

        /* Check if unicode input events are enabled */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsTextInputActive();

        /* Stop receiving any text input events, hide onscreen kbd */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_StopTextInput();

        /* Clear current composition - Requires >= 2.0.22 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ClearComposition();

        /* Toggle text input UI being shown - Requires >= 2.0.22 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsTextInputShown();

        /* Set the rectangle used for text input, hint for IME */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetTextInputRect(ref SDL_Rect rect);

        /* Does the platform support an on-screen keyboard? */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasScreenKeyboardSupport();

        /* Is the on-screen keyboard shown for a given window? */
        /* window is an SDL_Window pointer */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsScreenKeyboardShown(IntPtr window);

        /* Resets keyboard state.
         * Only available in 2.24.0 and later */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ResetKeyboard();
        #endregion

    }
}
