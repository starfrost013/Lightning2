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
using static NuCore.SDL2.SDL;
using static NuCore.SDL2.Utf8Marshaling;
#endregion

namespace NuCore.SDL2
{
    public static class SDL_image
    {
        #region SDL2# Variables

        /* Used by DllImport to load the native library. */
#if X64 // x86-64
        private const string nativeLibName = @"Libraries\SDL2_image-x64";
#elif ARM64 // AArch64 v8.0+
		private const string nativeLibName = @"Libraries\SDL2_image-ARM64";
#endif

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

        public static void SDL_IMAGE_VERSION(out SDL.SDL_version X)
        {
            X.major = SDL_IMAGE_EXPECTED_MAJOR_VERSION;
            X.minor = SDL_IMAGE_EXPECTED_MINOR_VERSION;
            X.patch = SDL_IMAGE_EXPECTED_PATCHLEVEL;
        }

        [DllImport(nativeLibName, EntryPoint = "IMG_Linked_Version", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_IMG_Linked_Version();
        public static SDL_version IMG_Linked_Version()
        {
            SDL_version result;
            IntPtr result_ptr = INTERNAL_IMG_Linked_Version();
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

        /* IntPtr refers to an SDL_Surface* */
        [DllImport(nativeLibName, EntryPoint = "IMG_Load", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe IntPtr INTERNAL_IMG_Load(
            byte* file
        );
        public static unsafe IntPtr IMG_Load(string file)
        {
            byte* utf8File = Utf8EncodeHeap(file);
            IntPtr handle = INTERNAL_IMG_Load(
                utf8File
            );
            Marshal.FreeHGlobal((IntPtr)utf8File);
            return handle;
        }

        /* src refers to an SDL_RWops*, IntPtr return typeto an SDL_Surface* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_Load_RW(
            IntPtr src,
            int freesrc
        );

        /* src refers to an SDL_RWops*, IntPtr return type to an SDL_Surface* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadSizedSVG_RW(
            IntPtr src,
            int width,
            int height
        );

        /* src refers to an SDL_RWops*, IntPtr to an SDL_Surface* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, EntryPoint = "IMG_LoadTyped_RW", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe IntPtr INTERNAL_IMG_LoadTyped_RW(
            IntPtr src,
            int freesrc,
            byte* type
        );

        public static unsafe IntPtr IMG_LoadTyped_RW(
            IntPtr src,
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

        /* IntPtr refers to an SDL_Texture*, renderer to an SDL_Renderer* */
        [DllImport(nativeLibName, EntryPoint = "IMG_LoadTexture", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe IntPtr INTERNAL_IMG_LoadTexture(
            IntPtr renderer,
            byte* file
        );
        public static unsafe IntPtr IMG_LoadTexture(
            IntPtr renderer,
            string file
        )
        {
            byte* utf8File = Utf8EncodeHeap(file);
            IntPtr handle = INTERNAL_IMG_LoadTexture(
                renderer,
                utf8File
            );
            Marshal.FreeHGlobal((IntPtr)utf8File);
            return handle;
        }

        /* renderer refers to an SDL_Renderer*.
		 * src refers to an SDL_RWops*.
		 * IntPtr to an SDL_Texture*.
		 */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadTexture_RW(
            IntPtr renderer,
            IntPtr src,
            int freesrc
        );

        /* renderer refers to an SDL_Renderer*.
		 * src refers to an SDL_RWops*.
		 * IntPtr to an SDL_Texture*.
		 */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, EntryPoint = "IMG_LoadTextureTyped_RW", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe IntPtr INTERNAL_IMG_LoadTextureTyped_RW(
            IntPtr renderer,
            IntPtr src,
            int freesrc,
            byte* type
        );
        public static unsafe IntPtr IMG_LoadTextureTyped_RW(
            IntPtr renderer,
            IntPtr src,
            int freesrc,
            string type
        )
        {
            byte* utf8Type = Utf8EncodeHeap(type);
            IntPtr handle = INTERNAL_IMG_LoadTextureTyped_RW(
                renderer,
                src,
                freesrc,
                utf8Type
            );
            Marshal.FreeHGlobal((IntPtr)utf8Type);
            return handle;
        }

        /* IntPtr refers to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_ReadXPMFromArray(
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
                string[] xpm
        );

        /* IntPtr refers to an SDL_Surface*
         * Only available in 2.6.0 and higher */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_ReadXPMFromArrayToRGB888(
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] // null terminated 
                string[] xpm
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, EntryPoint = "IMG_SavePNG", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_IMG_SavePNG(
            IntPtr surface,
            byte* file
        );

        public static unsafe int IMG_SavePNG(IntPtr surface, string file)
        {
            byte* utf8File = Utf8EncodeHeap(file);
            int result = INTERNAL_IMG_SavePNG(
                surface,
                utf8File
            );
            Marshal.FreeHGlobal((IntPtr)utf8File);
            return result;
        }

        /* surface refers to an SDL_Surface*, dst to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int IMG_SavePNG_RW(
            IntPtr surface,
            IntPtr dst,
            int freedst
        );

        /* surface refers to an SDL_Surface* */
        [DllImport(nativeLibName, EntryPoint = "IMG_SaveJPG", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_IMG_SaveJPG(
            IntPtr surface,
            byte* file,
            int quality
        );
        public static unsafe int IMG_SaveJPG(IntPtr surface, string file, int quality)
        {
            byte* utf8File = Utf8EncodeHeap(file);
            int result = INTERNAL_IMG_SaveJPG(
                surface,
                utf8File,
                quality
            );
            Marshal.FreeHGlobal((IntPtr)utf8File);
            return result;
        }

        /* surface refers to an SDL_Surface*, dst to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int IMG_SaveJPG_RW(
            IntPtr surface,
            IntPtr dst,
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
            public IntPtr frames; /* SDL_Surface** */
            public IntPtr delays; /* int* */
        }

        /* IntPtr refers to an IMG_Animation* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadAnimation(
            [In()] [MarshalAs(UnmanagedType.LPStr)]
                string file
        );

        /* IntPtr refers to an IMG_Animation*, src to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadAnimation_RW(
            IntPtr src,
            int freesrc
        );

        /* IntPtr refers to an IMG_Animation*, src to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadAnimationTyped_RW(
            IntPtr src,
            int freesrc,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
                string type
        );

        /* anim refers to an IMG_Animation* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void IMG_FreeAnimation(IntPtr anim);

        /* IntPtr refers to an IMG_Animation*, src to an SDL_RWops* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadGIFAnimation_RW(IntPtr src);

        #endregion

        #endregion
    }
}