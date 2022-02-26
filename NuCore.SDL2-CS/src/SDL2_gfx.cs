
using System;
using System.Collections.Generic;
using System.Text;

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

#if X64

#if DEBUG
        public const string NativeLibName = @"Content\NativeLibraries\SDL2_gfx-v1.0.5-x64-debug.dll";
#else
        public const string NativeLibName = @"Content\NativeLibraries\SDL2_gfx-v1.0.5-x64.dll";
#endif

#elif ARM32
#if DEBUG
        public const string NativeLibName = @"Content\NativeLibraries\SDL2_gfx-v1.0.5-ARM32-debug.dll";
#else
        public const string NativeLibName = @"Content\NativeLibraries\SDL2_gfx-v1.0.5-ARM32.dll";
#endif

#elif ARM64
#if DEBUG
        public const string NativeLibName = @"Content\NativeLibraries\SDL2_gfx-v1.0.5-ARM64-debug.dll";
#else
        public const string NativeLibName = @"Content\NativeLibraries\SDL2_gfx-v1.0.5-ARM64.dll";
#endif
#endif
        #endregion

        #region SDL2_gfxVersion.h 
        // requires 1.0.5 (Lightning/NuRender ONLY)

        public const int SDL_GFX_VERSION_MAJOR = 1;
        public const int SDL_GFX_VERSION_MINOR = 0;
        public const int SDL_GFX_VERSION_REVISION = 5;

        /// <summary>
        /// Returns the current version of SDL2_gfx. It is best to check this in your program.
        /// </summary>
        /// <returns>A <see cref="SDL.SDL_version"/> object containing the current version of SDL2_gfx.</returns>
        public static SDL.SDL_version SDLGFX_Version()
        {
            return new SDL.SDL_version()
            {
                major = SDL_GFX_VERSION_MAJOR,
                minor = SDL_GFX_VERSION_MINOR,
                patch = SDL_GFX_VERSION_REVISION
            };
        }

        #endregion

        #region SDL2_gfxPrimitives.h

        //references to render etc usually DON'T have out keyword.

        /// <summary>
        /// Draws an unfilled, non-anti-aliased polygon using renderer <paramref name="Renderer"/>, using the points <paramref name="VX"/> and <paramref name="VY"/>, in the colour
        ///<paramref name="R"/>,<paramref name="G"/>,<paramref name="B"/>,<paramref name="A"/>.
        /// </summary>
        /// <param name="Renderer">A pointer to the SDL2 renderer being used to render the polygon.</param>
        /// <param name="VX">An integer array containing the X coordinates of the points of this polygon.</param>
        /// <param name="VY">An integer array containing theY coordinates of the points of this polygon.</param>
        /// <param name="R">The red component of the colour of this polygon.</param>
        /// <param name="G">The green component of the colour of this polygon.</param>
        /// <param name="B">The blue component of the colour of this polygon.</param>
        /// <param name="A">The alpha component of the colour of this polygon.</param>
        public static void polygonRGBA(IntPtr Renderer, int[] VX, int[] VY, byte R, byte G, byte B, byte A)
        {
            SDL.SDL_SetRenderDrawColor(Renderer, R, G, B, A);

            if (VX.Length != VY.Length)
            {
                throw new InvalidOperationException("VX and VY parameters to SDL2_gfx.polygonRGBA in NuRender.Core.SDL2-CS must be same length!");
            }

            int N = VX.Length;

            for (int i = 0; i < N - 1; i++)
            {
                SDL.SDL_RenderDrawLine(Renderer, VX[i], VY[i], VX[i + 1], VY[i + 1]);
            }
        }

        /// <summary>
        /// Draws an unfilled, anti-aliased polygon using renderer <paramref name="Renderer"/>, using the points <paramref name="VX"/> and <paramref name="VY"/>, in the colour
        ///<paramref name="R"/>,<paramref name="G"/>,<paramref name="B"/>,<paramref name="A"/>.
        /// </summary>
        /// <param name="Renderer">A pointer to the SDL2 renderer being used to render the polygon.</param>
        /// <param name="VX">An integer array containing the X coordinates of the points of this polygon.</param>
        /// <param name="VY">An integer array containing theY coordinates of the points of this polygon.</param>
        /// <param name="R">The red component of the colour of this polygon.</param>
        /// <param name="G">The green component of the colour of this polygon.</param>
        /// <param name="B">The blue component of the colour of this polygon.</param>
        /// <param name="A">The alpha component of the colour of this polygon.</param>
        public static void aaPolygonRGBA(IntPtr Renderer, short[] VX, short[] VY, byte R, byte G, byte B, byte A)
        {
            SDL.SDL_SetRenderDrawColor(Renderer, R, G, B, A);

            if (VX.Length != VY.Length)
            {
                throw new InvalidOperationException("VX and VY parameters to SDL2_gfx.aaPolygonRGBA NuCore.SDL2-CS must be same length!");
            }

            int N = VX.Length;

            for (int i = 0; i < N - 1; i++)
            {
                aalineRGBA(Renderer, VX[i], VY[i], VX[i + 1], VY[i + 1], R, G, B, A);
            }
        }
        #endregion
    }
}
