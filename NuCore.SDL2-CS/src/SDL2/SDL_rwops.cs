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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
#endregion

namespace NuCore.SDL2
{
    public static partial class SDL
    {
        #region SDL_rwops.h

        public const int RW_SEEK_SET = 0;
        public const int RW_SEEK_CUR = 1;
        public const int RW_SEEK_END = 2;

        public const UInt32 SDL_RWOPS_UNKNOWN = 0; /* Unknown stream type */
        public const UInt32 SDL_RWOPS_WINFILE = 1; /* Win32 file */
        public const UInt32 SDL_RWOPS_STDFILE = 2; /* Stdio file */
        public const UInt32 SDL_RWOPS_JNIFILE = 3; /* Android asset */
        public const UInt32 SDL_RWOPS_MEMORY = 4; /* Memory stream */
        public const UInt32 SDL_RWOPS_MEMORY_RO = 5; /* Read-Only memory stream */

        public enum SDL_RWOPS_TYPE
        {
            SDL_RWOPS_UNKNOWN = 0, /* Unknown stream type */
            SDL_RWOPS_WINFILE = 1, /* Win32 file */
            SDL_RWOPS_STDFILE = 2, /* Stdio file */
            SDL_RWOPS_JNIFILE = 3, /* Android asset */
            SDL_RWOPS_MEMORY = 4, /* Memory stream */
            SDL_RWOPS_MEMORY_RO = 5 /* Read-Only memory stream */
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long SDLRWopsSizeCallback(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long SDLRWopsSeekCallback(
            IntPtr context,
            long offset,
            int whence
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDLRWopsReadCallback(
            IntPtr context,
            IntPtr ptr,
            IntPtr size,
            IntPtr maxnum
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDLRWopsWriteCallback(
            IntPtr context,
            IntPtr ptr,
            IntPtr size,
            IntPtr num
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SDLRWopsCloseCallback(
            IntPtr context
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_RWops
        {
            public IntPtr size;
            public IntPtr seek;
            public IntPtr read;
            public IntPtr write;
            public IntPtr close;

            [MarshalAs(UnmanagedType.U4)]
            public SDL_RWOPS_TYPE type;

            /* NOTE: This isn't the full structure since
			 * the native SDL_RWops contains a hidden union full of
			 * internal information and platform-specific stuff depending
			 * on what conditions the native library was built with
			 */
        }


        /* IntPtr refers to an SDL_RWops* */
        [DllImport(nativeLibName, EntryPoint = "SDL_RWFromFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe IntPtr INTERNAL_SDL_RWFromFile(
            byte* file,
            byte* mode
        );
        public static unsafe IntPtr SDL_RWFromFile(
            string file,
            string mode
        )
        {
            byte* utf8File = Utf8EncodeHeap(file);
            byte* utf8Mode = Utf8EncodeHeap(mode);
            IntPtr rwOps = INTERNAL_SDL_RWFromFile(
                utf8File,
                utf8Mode
            );
            Marshal.FreeHGlobal((IntPtr)utf8Mode);
            Marshal.FreeHGlobal((IntPtr)utf8File);
            return rwOps;
        }

        /* IntPtr refers to an SDL_RWops* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AllocRW();

        /* area refers to an SDL_RWops* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeRW(IntPtr area);

        /* fp refers to a void* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromFP(IntPtr fp, SDL_bool autoclose);

        /* mem refers to a void*, IntPtr to an SDL_RWops* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromMem(IntPtr mem, int size);

        /* mem refers to a const void*, IntPtr to an SDL_RWops* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromConstMem(IntPtr mem, int size);

        /* context refers to an SDL_RWops*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWsize(IntPtr context);

        /* context refers to an SDL_RWops*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWseek(
            IntPtr context,
            long offset,
            int whence
        );

        /* context refers to an SDL_RWops*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWtell(IntPtr context);

        /* context refers to an SDL_RWops*, ptr refers to a void*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWread(
            IntPtr context,
            IntPtr ptr,
            IntPtr size,
            IntPtr maxnum
        );

        /* context refers to an SDL_RWops*, ptr refers to a const void*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWwrite(
            IntPtr context,
            IntPtr ptr,
            IntPtr size,
            IntPtr maxnum
        );

        /* Read endian functions */

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_ReadU8(IntPtr src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt16 SDL_ReadLE16(IntPtr src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt16 SDL_ReadBE16(IntPtr src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_ReadLE32(IntPtr src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_ReadBE32(IntPtr src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_ReadLE64(IntPtr src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_ReadBE64(IntPtr src);

        /* Write endian functions */

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteU8(IntPtr dst, byte value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE16(IntPtr dst, UInt16 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE16(IntPtr dst, UInt16 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE32(IntPtr dst, UInt32 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE32(IntPtr dst, UInt32 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE64(IntPtr dst, UInt64 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE64(IntPtr dst, UInt64 value);

        /* context refers to an SDL_RWops*
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWclose(IntPtr context);

        /* datasize refers to a size_t*
		 * IntPtr refers to a void*
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, EntryPoint = "SDL_LoadFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe IntPtr INTERNAL_SDL_LoadFile(byte* file, out IntPtr datasize);
        public static unsafe IntPtr SDL_LoadFile(string file, out IntPtr datasize)
        {
            byte* utf8File = Utf8EncodeHeap(file);
            IntPtr result = INTERNAL_SDL_LoadFile(utf8File, out datasize);
            Marshal.FreeHGlobal((IntPtr)utf8File);
            return result;
        }

        #endregion

    }
}
