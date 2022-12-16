#region License
/* SDL2# - C# Wrapper for SDL2
 * Version 3.1.0
 * Copyright © 2022 starfrost
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
using static LightningBase.SDL;
using static LightningBase.Utf8Marshaling;
#endregion

namespace LightningBase
{
    public static class SDL_image
    {
        #region SDL2# Variables

        /* Used by DllImport to load the native library. */
        private const string nativeLibName = @"SDL2_image";

        #endregion

        #region SDL_image.h

        /* Similar to the headers, this is the version we're expecting to be
		 * running with. You will likely want to check this somewhere in your
		 * program!
		 */
        public const int SDL_IMAGE_EXPECTED_MAJOR_VERSION = 2;
        public const int SDL_IMAGE_EXPECTED_MINOR_VERSION = 6;
        public const int SDL_IMAGE_EXPECTED_PATCHLEVEL = 2;

        [Flags]
        public enum IMG_InitFlags
        {
            IMG_INIT_JPG = 0x1, // JPEG
            IMG_INIT_PNG = 0x2, // PNG (Portable Network Graphics)
            IMG_INIT_TIF = 0x4, // TIF (Tag Image File Format)
            IMG_INIT_WEBP = 0x8, // WebPicture
            IMG_INIT_JXL = 0x10, // JPEG-XL 
            IMG_INIT_AVIF = 0x20, // Advanced Video and Image Format
            IMG_INIT_EVERYTHING =
            (IMG_INIT_JPG | IMG_INIT_PNG | IMG_INIT_TIF | IMG_INIT_WEBP | IMG_INIT_JXL | IMG_INIT_AVIF)
        }

        public static void SDL_IMAGE_VERSION(out SDL_version X)
        {
            X.major = SDL_IMAGE_EXPECTED_MAJOR_VERSION;
            X.minor = SDL_IMAGE_EXPECTED_MINOR_VERSION;
            X.patch = SDL_IMAGE_EXPECTED_PATCHLEVEL;
        }

        [DllImport(nativeLibName, EntryPoint = "IMG_Linked_Version", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_IMG_Linked_Version();
        public static SDL_version IMG_Linked_Version()
        {
            SDL_version result;
            nint result_ptr = INTERNAL_IMG_Linked_Version();
            result = (SDL_version)Marshal.PtrToStructure(
                result_ptr,
                typeof(SDL_version)
            );
            return result;
        }

        public static readonly int SDL_IMAGE_EXPECTED_COMPILEDVERSION = SDL_VERSIONNUM(
            SDL_IMAGE_EXPECTED_MAJOR_VERSION,
            SDL_IMAGE_EXPECTED_MINOR_VERSION,
            SDL_IMAGE_EXPECTED_PATCHLEVEL
            );

        public static bool IMG_VERSION_ATLEAST(int X, int Y, int Z) => SDL_IMAGE_EXPECTED_COMPILEDVERSION >= SDL_VERSIONNUM(X, Y, Z);

        [DllImport(nativeLibName, EntryPoint = "IMG_Init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int INTERNAL_IMG_Init(IMG_InitFlags flags);

        public static int IMG_Init(IMG_InitFlags flags)
        {
            // check for version information
            // verify a sufficient SDL version (as we dynamically link) - get the real version of SDL2.dll to do this
            SDL_version realVersion = IMG_Linked_Version();

            bool versionIsCompatible = IMG_VERSION_ATLEAST(SDL_IMAGE_EXPECTED_MAJOR_VERSION, SDL_IMAGE_EXPECTED_MINOR_VERSION, SDL_IMAGE_EXPECTED_PATCHLEVEL);

            // if SDL is too load
            if (!versionIsCompatible)
            {
                IMG_SetError($"Incorrect SDL_image version. Version {SDL_IMAGE_EXPECTED_MAJOR_VERSION}.{SDL_IMAGE_EXPECTED_MINOR_VERSION}.{SDL_IMAGE_EXPECTED_PATCHLEVEL} is required," +
                    $" got {realVersion.major}.{realVersion.minor}.{realVersion.patch}!");
                return -94002; // NEGATIVE is an error code
            }

            return INTERNAL_IMG_Init(flags);
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void IMG_Quit();

        /* nint refers to an SDL_Surface* */
        [DllImport(nativeLibName, EntryPoint = "IMG_Load", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_IMG_Load(
            byte* file
        );
        public static unsafe nint IMG_Load(string file)
        {
            byte* utf8File = Utf8EncodeHeap(file);
            nint handle = INTERNAL_IMG_Load(
                utf8File
            );
            Marshal.FreeHGlobal((nint)utf8File);
            return handle;
        }

        /* src refers to an SDL_RWops*, nint return typeto an SDL_Surface* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint IMG_Load_RW(
            nint src,
            int freesrc
        );

        /* src refers to an SDL_RWops*, nint return type to an SDL_Surface* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint IMG_LoadSizedSVG_RW(
            nint src,
            int width,
            int height
        );

        /* src refers to an SDL_RWops*, nint to an SDL_Surface* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, EntryPoint = "IMG_LoadTyped_RW", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_IMG_LoadTyped_RW(
            nint src,
            int freesrc,
            byte* type
        );

        public static unsafe nint IMG_LoadTyped_RW(
            nint src,
            int freesrc,
            string type
        )
        {
            int utf8TypeBufSize = Utf8Size(type);
            byte* utf8Type = stackalloc byte[utf8TypeBufSize];
            return INTERNAL_IMG_LoadTyped_RW(
                src,
                freesrc,
                Utf8Encode(type, utf8Type, utf8TypeBufSize)
            );
        }

        /* nint refers to an SDL_Texture*, renderer to an SDL_Renderer* */
        [DllImport(nativeLibName, EntryPoint = "IMG_LoadTexture", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_IMG_LoadTexture(
            nint renderer,
            byte* file
        );
        public static unsafe nint IMG_LoadTexture(
            nint renderer,
            string file
        )
        {
            byte* utf8File = Utf8EncodeHeap(file);
            nint handle = INTERNAL_IMG_LoadTexture(
                renderer,
                utf8File
            );
            Marshal.FreeHGlobal((nint)utf8File);
            return handle;
        }

        /* renderer refers to an SDL_Renderer*.
		 * src refers to an SDL_RWops*.
		 * nint to an SDL_Texture*.
		 */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint IMG_LoadTexture_RW(
            nint renderer,
            nint src,
            int freesrc
        );

        /* renderer refers to an SDL_Renderer*.
		 * src refers to an SDL_RWops*.
		 * nint to an SDL_Texture*.
		 */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, EntryPoint = "IMG_LoadTextureTyped_RW", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_IMG_LoadTextureTyped_RW(
            nint renderer,
            nint src,
            int freesrc,
            byte* type
        );
        public static unsafe nint IMG_LoadTextureTyped_RW(
            nint renderer,
            nint src,
            int freesrc,
            string type
        )
        {
            byte* utf8Type = Utf8EncodeHeap(type);
            nint handle = INTERNAL_IMG_LoadTextureTyped_RW(
                renderer,
                src,
                freesrc,
                utf8Type
            );
            Marshal.FreeHGlobal((nint)utf8Type);
            return handle;
        }

        /* nint refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint IMG_ReadXPMFromArray(
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
                string[] xpm
        );

        /* nint refers to an SDL_Surface*
         * Only available in 2.6.0 and higher */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint IMG_ReadXPMFromArrayToRGB888(
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] // null terminated 
                string[] xpm
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, EntryPoint = "IMG_SavePNG", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_IMG_SavePNG(
            nint surface,
            byte* file
        );

        public static unsafe int IMG_SavePNG(nint surface, string file)
        {
            byte* utf8File = Utf8EncodeHeap(file);
            int result = INTERNAL_IMG_SavePNG(
                surface,
                utf8File
            );
            Marshal.FreeHGlobal((nint)utf8File);
            return result;
        }

        /* surface refers to an SDL_Surface*, dst to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int IMG_SavePNG_RW(
            nint surface,
            nint dst,
            int freedst
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, EntryPoint = "IMG_SaveJPG", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_IMG_SaveJPG(
            nint surface,
            byte* file,
            int quality
        );
        public static unsafe int IMG_SaveJPG(nint surface, string file, int quality)
        {
            byte* utf8File = Utf8EncodeHeap(file);
            int result = INTERNAL_IMG_SaveJPG(
                surface,
                utf8File,
                quality
            );
            Marshal.FreeHGlobal((nint)utf8File);
            return result;
        }

        /* surface refers to an SDL_Surface*, dst to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int IMG_SaveJPG_RW(
            nint surface,
            nint dst,
            int freedst,
            int quality
        );

        public static string IMG_GetError() => SDL_GetError();

        public static void IMG_SetError(string fmtAndArglist) => SDL_SetError(fmtAndArglist);

        #region Animated Image Support

        /* This region is only available in 2.0.6 or higher. */

        public struct IMG_Animation
        {
            public int w;
            public int h;
            public nint frames; /* SDL_Surface** */
            public nint delays; /* int* */
        }

        /* nint refers to an IMG_Animation* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint IMG_LoadAnimation(
            [In()] [MarshalAs(UnmanagedType.LPStr)]
                string file
        );

        /* nint refers to an IMG_Animation*, src to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint IMG_LoadAnimation_RW(
            nint src,
            int freesrc
        );

        /* nint refers to an IMG_Animation*, src to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint IMG_LoadAnimationTyped_RW(
            nint src,
            int freesrc,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
                string type
        );

        /* anim refers to an IMG_Animation* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void IMG_FreeAnimation(nint anim);

        /* nint refers to an IMG_Animation*, src to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint IMG_LoadGIFAnimation_RW(nint src);

        #endregion

        #endregion
    }
}