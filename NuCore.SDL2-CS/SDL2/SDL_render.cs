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
        #region SDL_render.h

        [Flags]
        public enum SDL_RendererFlags : uint
        {
            SDL_RENDERER_SOFTWARE = 0x00000001,
            SDL_RENDERER_ACCELERATED = 0x00000002,
            SDL_RENDERER_PRESENTVSYNC = 0x00000004,
            SDL_RENDERER_TARGETTEXTURE = 0x00000008
        }

        [Flags]
        public enum SDL_RendererFlip
        {
            SDL_FLIP_NONE = 0x00000000,
            SDL_FLIP_HORIZONTAL = 0x00000001,
            SDL_FLIP_VERTICAL = 0x00000002
        }

        public enum SDL_TextureAccess
        {
            SDL_TEXTUREACCESS_STATIC,
            SDL_TEXTUREACCESS_STREAMING,
            SDL_TEXTUREACCESS_TARGET
        }

        [Flags]
        public enum SDL_TextureModulate
        {
            SDL_TEXTUREMODULATE_NONE = 0x00000000,
            SDL_TEXTUREMODULATE_HORIZONTAL = 0x00000001,
            SDL_TEXTUREMODULATE_VERTICAL = 0x00000002
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct SDL_RendererInfo
        {
            public nint name; // const char*
            public uint flags;
            public uint num_texture_formats;
            public fixed uint texture_formats[16];
            public int max_texture_width;
            public int max_texture_height;
        }

        /* Only available in 2.0.11 or higher. */
        public enum SDL_ScaleMode
        {
            SDL_ScaleModeNearest,
            SDL_ScaleModeLinear,
            SDL_ScaleModeBest
        }

        /* Only available in 2.0.18 or higher. */
        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Vertex
        {
            public SDL_FPoint position;
            public SDL_Color color;
            public SDL_FPoint tex_coord;
        }

        /* nint refers to an SDL_Renderer*, window to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_CreateRenderer(
            nint window,
            int index,
            SDL_RendererFlags flags
        );

        /* nint refers to an SDL_Renderer*, surface to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_CreateSoftwareRenderer(nint surface);

        /* nint refers to an SDL_Texture*, renderer to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_CreateTexture(
            nint renderer,
            uint format,
            [MarshalAs(UnmanagedType.I4)]
            SDL_TextureAccess access,
            int w,
            int h
        );

        /* nint refers to an SDL_Texture*
		 * renderer refers to an SDL_Renderer*
		 * surface refers to an SDL_Surface*
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_CreateTextureFromSurface(
            nint renderer,
            nint surface
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyRenderer(nint renderer);

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyTexture(nint texture);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumRenderDrivers();

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRenderDrawBlendMode(
            nint renderer,
            out SDL_BlendMode blendMode
        );

        /* texture refers to an SDL_Texture*
		 * Only available in 2.0.11 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureScaleMode(
            nint texture,
            SDL_ScaleMode scaleMode
        );

        /* texture refers to an SDL_Texture*
		 * Only available in 2.0.11 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetTextureScaleMode(
            nint texture,
            out SDL_ScaleMode scaleMode
        );

        /* texture refers to an SDL_Texture*
		* userdata refers to a void*
		* Only available in 2.0.18 or higher.
		*/
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureUserData(
            nint texture,
            nint userdata
        );

        /* nint refers to a void*, texture refers to an SDL_Texture*
		 * Only available in 2.0.18 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GetTextureUserData(nint texture);

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRenderDrawColor(
            nint renderer,
            out byte r,
            out byte g,
            out byte b,
            out byte a
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRenderDriverInfo(
            int index,
            out SDL_RendererInfo info
        );

        /* nint refers to an SDL_Renderer*, window to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GetRenderer(nint window);

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRendererInfo(
            nint renderer,
            out SDL_RendererInfo info
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRendererOutputSize(
            nint renderer,
            out int w,
            out int h
        );

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetTextureAlphaMod(
            nint texture,
            out byte alpha
        );

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetTextureBlendMode(
            nint texture,
            out SDL_BlendMode blendMode
        );

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetTextureColorMod(
            nint texture,
            out byte r,
            out byte g,
            out byte b
        );

        /* texture refers to an SDL_Texture*, pixels to a void* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockTexture(
            nint texture,
            ref SDL_Rect rect,
            out nint pixels,
            out int pitch
        );

        /* texture refers to an SDL_Texture*, pixels to a void*.
		 * Internally, this function contains logic to use default values when
		 * the rectangle is passed as NULL.
		 * This overload allows for nint.Zero to be passed for rect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockTexture(
            nint texture,
            nint rect,
            out nint pixels,
            out int pitch
        );

        /* texture refers to an SDL_Texture*, surface to an SDL_Surface*
		 * Only available in 2.0.11 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockTextureToSurface(
            nint texture,
            ref SDL_Rect rect,
            out nint surface
        );

        /* texture refers to an SDL_Texture*, surface to an SDL_Surface*
		 * Internally, this function contains logic to use default values when
		 * the rectangle is passed as NULL.
		 * This overload allows for nint.Zero to be passed for rect.
		 * Only available in 2.0.11 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_LockTextureToSurface(
            nint texture,
            nint rect,
            out nint surface
        );

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_QueryTexture(
            nint texture,
            out uint format,
            out int access,
            out int w,
            out int h
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderClear(nint renderer);

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            ref SDL_Rect dstrect
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for srcrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(
            nint renderer,
            nint texture,
            nint srcrect,
            ref SDL_Rect dstrect
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for dstrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            nint dstrect
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both SDL_Rects.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(
            nint renderer,
            nint texture,
            nint srcrect,
            nint dstrect
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            ref SDL_Rect dstrect,
            double angle,
            ref SDL_Point center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for srcrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            nint srcrect,
            ref SDL_Rect dstrect,
            double angle,
            ref SDL_Point center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for dstrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            nint dstrect,
            double angle,
            ref SDL_Point center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for center.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            ref SDL_Rect dstrect,
            double angle,
            nint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both
		 * srcrect and dstrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            nint srcrect,
            nint dstrect,
            double angle,
            ref SDL_Point center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both
		 * srcrect and center.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            nint srcrect,
            ref SDL_Rect dstrect,
            double angle,
            nint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both
		 * dstrect and center.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            nint dstrect,
            double angle,
            nint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for all
		 * three parameters.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            nint srcrect,
            nint dstrect,
            double angle,
            nint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawLine(
            nint renderer,
            int x1,
            int y1,
            int x2,
            int y2
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawLines(
            nint renderer,
            [In] SDL_Point[] points,
            int count
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawPoint(
            nint renderer,
            int x,
            int y
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawPoints(
            nint renderer,
            [In] SDL_Point[] points,
            int count
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRect(
            nint renderer,
            ref SDL_Rect rect
        );

        /* renderer refers to an SDL_Renderer*, rect to an SDL_Rect*.
		 * This overload allows for nint.Zero (null) to be passed for rect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRect(
            nint renderer,
            nint rect
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRects(
            nint renderer,
            [In] SDL_Rect[] rects,
            int count
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRect(
            nint renderer,
            ref SDL_Rect rect
        );

        /* renderer refers to an SDL_Renderer*, rect to an SDL_Rect*.
		 * This overload allows for nint.Zero (null) to be passed for rect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRect(
            nint renderer,
            nint rect
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRects(
            nint renderer,
            [In] SDL_Rect[] rects,
            int count
        );

        #region Floating Point Render Functions

        /* This region only available in SDL 2.0.10 or higher. */

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyF(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            ref SDL_FRect dstrect
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for srcrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyF(
            nint renderer,
            nint texture,
            nint srcrect,
            ref SDL_FRect dstrect
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for dstrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyF(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            nint dstrect
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both SDL_Rects.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyF(
            nint renderer,
            nint texture,
            nint srcrect,
            nint dstrect
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            ref SDL_FRect dstrect,
            double angle,
            ref SDL_FPoint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for srcrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyEx(
            nint renderer,
            nint texture,
            nint srcrect,
            ref SDL_FRect dstrect,
            double angle,
            ref SDL_FPoint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for dstrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            nint dstrect,
            double angle,
            ref SDL_FPoint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for center.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            ref SDL_FRect dstrect,
            double angle,
            nint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both
		 * srcrect and dstrect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(
            nint renderer,
            nint texture,
            nint srcrect,
            nint dstrect,
            double angle,
            ref SDL_FPoint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both
		 * srcrect and center.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(
            nint renderer,
            nint texture,
            nint srcrect,
            ref SDL_FRect dstrect,
            double angle,
            nint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for both
		 * dstrect and center.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(
            nint renderer,
            nint texture,
            ref SDL_Rect srcrect,
            nint dstrect,
            double angle,
            nint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source, destination, and/or center are passed as NULL.
		 * This overload allows for nint.Zero (null) to be passed for all
		 * three parameters.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopyExF(
            nint renderer,
            nint texture,
            nint srcrect,
            nint dstrect,
            double angle,
            nint center,
            SDL_RendererFlip flip
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawPointF(
            nint renderer,
            float x,
            float y
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawPointsF(
            nint renderer,
            [In] SDL_FPoint[] points,
            int count
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawLineF(
            nint renderer,
            float x1,
            float y1,
            float x2,
            float y2
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawLinesF(
            nint renderer,
            [In] SDL_FPoint[] points,
            int count
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRectF(
            nint renderer,
            ref SDL_FRect rect
        );

        /* renderer refers to an SDL_Renderer*, rect to an SDL_Rect*.
		 * This overload allows for nint.Zero (null) to be passed for rect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRectF(
            nint renderer,
            nint rect
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderDrawRectsF(
            nint renderer,
            [In] SDL_FRect[] rects,
            int count
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRectF(
            nint renderer,
            ref SDL_FRect rect
        );

        /* renderer refers to an SDL_Renderer*, rect to an SDL_Rect*.
		 * This overload allows for nint.Zero (null) to be passed for rect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRectF(
            nint renderer,
            nint rect
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFillRectsF(
            nint renderer,
            [In] SDL_FRect[] rects,
            int count
        );

        #endregion

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderGetClipRect(
            nint renderer,
            out SDL_Rect rect
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderGetLogicalSize(
            nint renderer,
            out int w,
            out int h
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderGetScale(
            nint renderer,
            out float scaleX,
            out float scaleY
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderGetViewport(
            nint renderer,
            out SDL_Rect rect
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderPresent(nint renderer);

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderReadPixels(
            nint renderer,
            ref SDL_Rect rect,
            uint format,
            nint pixels,
            int pitch
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetClipRect(
            nint renderer,
            ref SDL_Rect rect
        );

        /* renderer refers to an SDL_Renderer*
		 * This overload allows for nint.Zero (null) to be passed for rect.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetClipRect(
            nint renderer,
            nint rect
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetLogicalSize(
            nint renderer,
            int w,
            int h
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetScale(
            nint renderer,
            float scaleX,
            float scaleY
        );

        /* renderer refers to an SDL_Renderer*
		 * Only available in 2.0.5 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetIntegerScale(
            nint renderer,
            SDL_bool enable
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetViewport(
            nint renderer,
            ref SDL_Rect rect
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetRenderDrawBlendMode(
            nint renderer,
            SDL_BlendMode blendMode
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetRenderDrawColor(
            nint renderer,
            byte r,
            byte g,
            byte b,
            byte a
        );

        /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetRenderTarget(
            nint renderer,
            nint texture
        );

        /* renderer refers to an SDL_Renderer*
	 * texture refers to an SDL_Texture*
	 * Only available in 2.0.18 or higher.
	 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderGeometry(
            nint renderer,
            nint texture,
            [In] SDL_Vertex[] vertices,
            int num_vertices,
            [In] int[] indices,
            int num_indices
        );

        /* renderer refers to an SDL_Renderer*
		 * texture refers to an SDL_Texture*
		 * indices refers to a void*
		 * Only available in 2.0.18 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderGeometryRaw(
            nint renderer,
            nint texture,
            [In] float[] xy,
            int xy_stride,
            [In] int[] color,
            int color_stride,
            [In] float[] uv,
            int uv_stride,
            int num_vertices,
            nint indices,
            int num_indices,
            int size_indices
        );

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureAlphaMod(
            nint texture,
            byte alpha
        );


        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureBlendMode(
            nint texture,
            SDL_BlendMode blendMode
        );

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetTextureColorMod(
            nint texture,
            byte r,
            byte g,
            byte b
        );

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockTexture(nint texture);

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateTexture(
            nint texture,
            ref SDL_Rect rect,
            nint pixels,
            int pitch
        );

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateTexture(
            nint texture,
            nint rect,
            nint pixels,
            int pitch
        );

        /* texture refers to an SDL_Texture*
		 * Only available in 2.0.1 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateYUVTexture(
            nint texture,
            ref SDL_Rect rect,
            nint yPlane,
            int yPitch,
            nint uPlane,
            int uPitch,
            nint vPlane,
            int vPitch
        );

        /* texture refers to an SDL_Texture*.
		 * yPlane and uvPlane refer to const Uint*.
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateNVTexture(
            nint texture,
            ref SDL_Rect rect,
            nint yPlane,
            int yPitch,
            nint uvPlane,
            int uvPitch
        );

        /* renderer refers to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_RenderTargetSupported(
            nint renderer
        );

        /* nint refers to an SDL_Texture*, renderer to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GetRenderTarget(nint renderer);

        /* renderer refers to an SDL_Renderer*
		 * Only available in 2.0.8 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_RenderGetMetalLayer(
            nint renderer
        );

        /* renderer refers to an SDL_Renderer*
		 * Only available in 2.0.8 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_RenderGetMetalCommandEncoder(
            nint renderer
        );

        /* renderer refers to an SDL_Renderer*
		 * Only available in 2.0.4 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_RenderIsClipEnabled(nint renderer);

        /* renderer refers to an SDL_Renderer*
		 * Only available in 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderFlush(nint renderer);

        /* renderer refers to an SDL_Renderer*
		* Only available in 2.0.18 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderWindowToLogical(
            nint renderer,
            int windowX,
            int windowY,
            out float logicalX,
            out float logicalY
        );

        /* renderer refers to an SDL_Renderer*
		 * Only available in 2.0.18 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderLogicalToWindow(
            nint renderer,
            float logicalX,
            float logicalY,
            out int windowX,
            out int windowY
        );

        /* renderer refers to an SDL_Renderer*
		* Only available in 2.0.18 or higher.
		*/
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderSetVSync(nint renderer, int vsync);

        #endregion
    }
}
