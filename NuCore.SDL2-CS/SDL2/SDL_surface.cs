#region License
/* Lightning SDL2 Wrapper
 * 
 * 
 * This software is based on the open-source SDL2# - C# Wrapper for SDL2 library.
 *
 * Copyright © 2013-2022 Ethan Lee.
 * Copyright © 2021-2023 starfrost
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
using static LightningBase.SdlMarshaling;
#endregion

namespace LightningBase
{
    public static partial class SDL
    {
        #region SDL_surface.h

        public const uint SDL_SWSURFACE = 0x00000000;
        public const uint SDL_PREALLOC = 0x00000001;
        public const uint SDL_RLEACCEL = 0x00000002;
        public const uint SDL_DONTFREE = 0x00000004;

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Surface
        {
            public uint flags;
            public nint format; // SDL_PixelFormat*
            public int w;
            public int h;
            public int pitch;
            public nint pixels; // void*
            public nint userdata; // void*
            public int locked;
            public nint list_blitmap; // void*
            public SDL_Rect clip_rect;
            public nint map; // SDL_BlitMap*
            public int refcount;
        }

        /* surface refers to an SDL_Surface* */
        public static bool SDL_MUSTLOCK(nint surface)
        {
            SDL_Surface sur;
            sur = PtrToStructure<SDL_Surface>(
                    surface
                );
            return (sur.flags & SDL_RLEACCEL) != 0;
        }

        /* src and dst refer to an SDL_Surface* */
        [DllImport(nativeLibName, EntryPoint = "SDL_UpperBlit", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_BlitSurface(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* src and dst refer to an SDL_Surface*
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for srcrect.
		 */
        [DllImport(nativeLibName, EntryPoint = "SDL_UpperBlit", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_BlitSurface(
            nint src,
            nint srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* src and dst refer to an SDL_Surface*
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for dstrect.
		 */
        [DllImport(nativeLibName, EntryPoint = "SDL_UpperBlit", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_BlitSurface(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            nint dstrect
        );

        /* src and dst refer to an SDL_Surface*
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both SDL_Rects.
		 */
        [DllImport(nativeLibName, EntryPoint = "SDL_UpperBlit", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_BlitSurface(
            nint src,
            nint srcrect,
            nint dst,
            nint dstrect
        );

        /* src and dst refer to an SDL_Surface* */
        [DllImport(nativeLibName, EntryPoint = "SDL_UpperBlitScaled", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_BlitScaled(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* src and dst refer to an SDL_Surface*
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for srcrect.
		 */
        [DllImport(nativeLibName, EntryPoint = "SDL_UpperBlitScaled", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_BlitScaled(
            nint src,
            nint srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* src and dst refer to an SDL_Surface*
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for dstrect.
		 */
        [DllImport(nativeLibName, EntryPoint = "SDL_UpperBlitScaled", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_BlitScaled(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            nint dstrect
        );

        /* src and dst refer to an SDL_Surface*
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both SDL_Rects.
		 */
        [DllImport(nativeLibName, EntryPoint = "SDL_UpperBlitScaled", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_BlitScaled(
            nint src,
            nint srcrect,
            nint dst,
            nint dstrect
        );

        /* src and dst are void* pointers */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_ConvertPixels(
            int width,
            int height,
            uint src_format,
            nint src,
            int src_pitch,
            uint dst_format,
            nint dst,
            int dst_pitch
        );

        /* nint refers to an SDL_Surface*
		 * src refers to an SDL_Surface*
		 * fmt refers to an SDL_PixelFormat*
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_ConvertSurface(
            nint src,
            nint fmt,
            uint flags
        );

        /* nint refers to an SDL_Surface*, src to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_ConvertSurfaceFormat(
            nint src,
            uint pixel_format,
            uint flags
        );

        /* nint refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_CreateRGBSurface(
            uint flags,
            int width,
            int height,
            int depth,
            uint Rmask,
            uint Gmask,
            uint Bmask,
            uint Amask
        );

        /* nint refers to an SDL_Surface*, pixels to a void* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_CreateRGBSurfaceFrom(
            nint pixels,
            int width,
            int height,
            int depth,
            int pitch,
            uint Rmask,
            uint Gmask,
            uint Bmask,
            uint Amask
        );

        /* nint refers to an SDL_Surface*
		 * Only available in 2.0.5 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_CreateRGBSurfaceWithFormat(
            uint flags,
            int width,
            int height,
            int depth,
            uint format
        );

        /* nint refers to an SDL_Surface*, pixels to a void*
		 * Only available in 2.0.5 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_CreateRGBSurfaceWithFormatFrom(
            nint pixels,
            int width,
            int height,
            int depth,
            int pitch,
            uint format
        );

        /* dst refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_FillRect(
            nint dst,
            ref SDL_Rect rect,
            uint color
        );

        /* dst refers to an SDL_Surface*.
		 * This overload allows passing NULL to rect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_FillRect(
            nint dst,
            nint rect,
            uint color
        );

        /* dst refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_FillRects(
            nint dst,
            [In] SDL_Rect[] rects,
            int count,
            uint color
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeSurface(nint surface);

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetClipRect(
            nint surface,
            out SDL_Rect rect
        );

        /* surface refers to an SDL_Surface*.
		 * Only available in 2.0.9 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasColorKey(nint surface);

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetColorKey(
            nint surface,
            out uint key
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSurfaceAlphaMod(
            nint surface,
            out byte alpha
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSurfaceBlendMode(
            nint surface,
            out SDL_BlendMode blendMode
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetSurfaceColorMod(
            nint surface,
            out byte r,
            out byte g,
            out byte b
        );

        /* src and dst are void* pointers
		* Only available in 2.0.18 or higher.
		*/
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_PremultiplyAlpha(
            int width,
            int height,
            uint src_format,
            nint src,
            int src_pitch,
            uint dst_format,
            nint dst,
            int dst_pitch
        );

        /* These are for SDL_LoadBMP, which is a macro in the SDL headers. */
        /* nint refers to an SDL_Surface* */
        /* THIS IS AN RWops FUNCTION! */
        [DllImport(nativeLibName, EntryPoint = "SDL_LoadBMP_RW", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_LoadBMP_RW(
            nint src,
            int freesrc
        );
        public static nint SDL_LoadBMP(string file)
        {
            nint rwops = SDL_RWFromFile(file, "rb");
            return INTERNAL_SDL_LoadBMP_RW(rwops, 1);
        }

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockSurface(nint surface);

        /* src and dst refer to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LowerBlit(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* src and dst refer to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LowerBlitScaled(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* These are for SDL_SaveBMP, which is a macro in the SDL headers. */
        /* nint refers to an SDL_Surface* */
        /* THIS IS AN RWops FUNCTION! */
        [DllImport(nativeLibName, EntryPoint = "SDL_SaveBMP_RW", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_SDL_SaveBMP_RW(
            nint surface,
            nint src,
            int freesrc
        );
        public static int SDL_SaveBMP(nint surface, string file)
        {
            nint rwops = SDL_RWFromFile(file, "wb");
            return INTERNAL_SDL_SaveBMP_RW(surface, rwops, 1);
        }

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_SetClipRect(
            nint surface,
            ref SDL_Rect rect
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetColorKey(
            nint surface,
            int flag,
            uint key
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfaceAlphaMod(
            nint surface,
            byte alpha
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfaceBlendMode(
            nint surface,
            SDL_BlendMode blendMode
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfaceColorMod(
            nint surface,
            byte r,
            byte g,
            byte b
        );

        /* surface refers to an SDL_Surface*, palette to an SDL_Palette* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfacePalette(
            nint surface,
            nint palette
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetSurfaceRLE(
            nint surface,
            int flag
        );

        /* surface refers to an SDL_Surface*.
		 * Only available in 2.0.14 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_HasSurfaceRLE(
            nint surface
        );

        /* src and dst refer to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SoftStretch(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* src and dst refer to an SDL_Surface*
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SoftStretchLinear(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockSurface(nint surface);

        /* src and dst refer to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpperBlit(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* src and dst refer to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpperBlitScaled(
            nint src,
            ref SDL_Rect srcrect,
            nint dst,
            ref SDL_Rect dstrect
        );

        /* surface and nint refer to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_DuplicateSurface(nint surface);

        #endregion
    }
}
