﻿#region License
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
        #region SDL_filesystem.h

        /* Only available in 2.0.1 or higher. */
        [DllImport(nativeLibName, EntryPoint = "SDL_GetBasePath", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_GetBasePath();
        public static string SDL_GetBasePath()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetBasePath(), true);
        }

        /* Only available in 2.0.1 or higher. */
        [DllImport(nativeLibName, EntryPoint = "SDL_GetPrefPath", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_SDL_GetPrefPath(
            byte* org,
            byte* app
        );
        public static unsafe string SDL_GetPrefPath(string org, string app)
        {
            int utf8OrgBufSize = Utf8Size(org);
            byte* utf8Org = stackalloc byte[utf8OrgBufSize];

            int utf8AppBufSize = Utf8Size(app);
            byte* utf8App = stackalloc byte[utf8AppBufSize];

            return UTF8_ToManaged(
                INTERNAL_SDL_GetPrefPath(
                    Utf8Encode(org, utf8Org, utf8OrgBufSize),
                    Utf8Encode(app, utf8App, utf8AppBufSize)
                ),
                true
            );
        }

        #endregion
    }
}