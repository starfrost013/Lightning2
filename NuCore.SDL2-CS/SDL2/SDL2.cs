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
#endregion

namespace LightningBase
{
    public static partial class SDL
    {
        #region SDL2# Variables

        /// <summary>
        /// Name of the SDL2 library.
        /// </summary>
        private const string nativeLibName = @"Libraries\SDL2";

        /// <summary>
        /// The version of the NuCore SDL2 Bindings
        /// </summary>
        private static string SDL2CS_VERSION = $"Using NuCore SDL2 Bindings version {SDL2CS_VERSION_MAJOR}.{SDL2CS_VERSION_MINOR}.{SDL2CS_VERSION_REVISION} " +
            $"(Expected SDL version {SDL_EXPECTED_MAJOR_VERSION}.{SDL_EXPECTED_MINOR_VERSION}.{SDL_EXPECTED_PATCHLEVEL}) for Lightning 1.1"; // cannot be const

        private const int SDL2CS_VERSION_MAJOR = 3;
        private const int SDL2CS_VERSION_MINOR = 1;
        private const int SDL2CS_VERSION_REVISION = 0;

        #endregion
    }
}