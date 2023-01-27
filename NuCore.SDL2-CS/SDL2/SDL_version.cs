#region License
/* Lightning SDL2 Wrapper
 * 
 * Version 3.1.0
 * Copyright © 2022 starfrost
 * August 31, 2022
 * 
 * This software is based on the open-source SDL2# - C# Wrapper for SDL2 library.
 *
 * Copyright © 2013-2021 Ethan Lee.
 * Copyright © 2022 starfrost
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
using static LightningBase.Utf8Marshaling;
#endregion

namespace LightningBase
{
    public static partial class SDL
    {
        #region SDL_version.h, SDL_revision.h

        /* Similar to the headers, this is the version we're expecting to be
		 * running with. You will likely want to check this somewhere in your
		 * program!
		 */
        public const int SDL_EXPECTED_MAJOR_VERSION = 2;
        public const int SDL_EXPECTED_MINOR_VERSION = 26;
        public const int SDL_EXPECTED_PATCHLEVEL = 2;

        public static readonly int SDL_EXPECTED_COMPILEDVERSION = SDL_VERSIONNUM(
            SDL_EXPECTED_MAJOR_VERSION,
            SDL_EXPECTED_MINOR_VERSION,
            SDL_EXPECTED_PATCHLEVEL
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_version
        {
            public byte major;
            public byte minor;
            public byte patch;
        }

        public static void SDL_VERSION(out SDL_version x)
        {
            x.major = SDL_EXPECTED_MAJOR_VERSION;
            x.minor = SDL_EXPECTED_MINOR_VERSION;
            x.patch = SDL_EXPECTED_PATCHLEVEL;
        }

        public static int SDL_VERSIONNUM(int X, int Y, int Z) => (X * 10000) + (Y * 100) + Z; // changed to 10000 because of 2.24+ high version numbers

        public static bool SDL_VERSION_ATLEAST(int X, int Y, int Z) => SDL_EXPECTED_COMPILEDVERSION >= SDL_VERSIONNUM(X, Y, Z);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetVersion(out SDL_version ver);

        [DllImport(nativeLibName, EntryPoint = "SDL_GetRevision", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_GetRevision();
        public static string SDL_GetRevision()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetRevision());
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRevisionNumber();

        #endregion

    }
}
