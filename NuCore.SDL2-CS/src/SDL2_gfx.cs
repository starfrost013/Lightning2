#region
/* Lightning SDL2 Wrapper
 * 
 * Version 3.0 (NuRender/Lightning) + SDL2_gfx
 * Copyright © 2021 starfrost
 * November 6, 2021
 * 
 * This software is based on the open-source SDL2# - C# Wrapper for SDL2 library.
 *
 * Copyright (c) 2013-2021 Ethan Lee.
 * Copyright © 2021 starfrost.
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
namespace NuCore.SDL2
{
    public static partial class SDL_gfx
    {
        #region SDL2# Defines

#if X64 // x86-64
        public const string NativeLibName = @"Content\Libraries\SDL2_gfx-v1.0.6-x64.dll";
#elif ARM64 // ARMv8/9
        public const string NativeLibName = @"Content\Libraries\SDL2_gfx-v1.0.6-ARM64.dll";
#endif
        #endregion

        #region SDL2_gfxVersion.h 

        // requires 1.0.6 (LightningGL version) 
        // todo: check on load
        public const int SDL_GFX_VERSION_MAJOR = 1;
        public const int SDL_GFX_VERSION_MINOR = 0;
        public const int SDL_GFX_VERSION_REVISION = 6;

        /// <summary>
        /// Returns the current version of SDL2_gfx. It is best to check this in your program.
        /// </summary>
        /// <returns>A <see cref="SDL.SDL_version"/> object containing the current version of SDL2_gfx.</returns>
        public static SDL.SDL_version GFX_Version()
        {
            return new SDL.SDL_version()
            {
                major = SDL_GFX_VERSION_MAJOR,
                minor = SDL_GFX_VERSION_MINOR,
                patch = SDL_GFX_VERSION_REVISION
            };
        }

        #endregion
    }
}
