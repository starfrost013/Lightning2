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
        public delegate long SDLRWopsSizeCallback(nint context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long SDLRWopsSeekCallback(
            nint context,
            long offset,
            int whence
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate nint SDLRWopsReadCallback(
            nint context,
            nint ptr,
            nint size,
            nint maxnum
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate nint SDLRWopsWriteCallback(
            nint context,
            nint ptr,
            nint size,
            nint num
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SDLRWopsCloseCallback(
            nint context
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_RWops
        {
            public nint size;
            public nint seek;
            public nint read;
            public nint write;
            public nint close;

            [MarshalAs(UnmanagedType.U4)]
            public SDL_RWOPS_TYPE type;

            /* NOTE: This isn't the full structure since
			 * the native SDL_RWops contains a hidden union full of
			 * internal information and platform-specific stuff depending
			 * on what conditions the native library was built with
			 */
        }


        /* nint refers to an SDL_RWops* */
        [DllImport(nativeLibName, EntryPoint = "SDL_RWFromFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_SDL_RWFromFile(
            byte* file,
            byte* mode
        );
        public static unsafe nint SDL_RWFromFile(
            string file,
            string mode
        )
        {
            byte* utf8File = Utf8EncodeHeap(file);
            byte* utf8Mode = Utf8EncodeHeap(mode);
            nint rwOps = INTERNAL_SDL_RWFromFile(
                utf8File,
                utf8Mode
            );
            Marshal.FreeHGlobal((nint)utf8Mode);
            Marshal.FreeHGlobal((nint)utf8File);
            return rwOps;
        }

        /* nint refers to an SDL_RWops* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_AllocRW();

        /* area refers to an SDL_RWops* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeRW(nint area);

        [Obsolete("Will be removed in SDL 3.0 - see https://github.com/libsdl-org/SDL/blob/main/docs/README-migration.md")]
        /* fp refers to a void* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_RWFromFP(nint fp, SDL_bool autoclose);

        /* mem refers to a void*, nint to an SDL_RWops* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_RWFromMem(nint mem, int size);

        /* mem refers to a const void*, nint to an SDL_RWops* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_RWFromConstMem(nint mem, int size);

        /* context refers to an SDL_RWops*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWsize(nint context);

        /* context refers to an SDL_RWops*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWseek(
            nint context,
            long offset,
            int whence
        );

        /* context refers to an SDL_RWops*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWtell(nint context);

        /* context refers to an SDL_RWops*, ptr refers to a void*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWread(
            nint context,
            nint ptr,
            nint size,
            nint maxnum
        );

        /* context refers to an SDL_RWops*, ptr refers to a const void*.
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWwrite(
            nint context,
            nint ptr,
            nint size,
            nint maxnum
        );

        /* Read endian functions */

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_ReadU8(nint src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt16 SDL_ReadLE16(nint src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt16 SDL_ReadBE16(nint src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_ReadLE32(nint src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_ReadBE32(nint src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_ReadLE64(nint src);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_ReadBE64(nint src);

        /* Write endian functions */

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteU8(nint dst, byte value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE16(nint dst, UInt16 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE16(nint dst, UInt16 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE32(nint dst, UInt32 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE32(nint dst, UInt32 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE64(nint dst, UInt64 value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE64(nint dst, UInt64 value);

        /* context refers to an SDL_RWops*
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWclose(nint context);

        /* datasize refers to a size_t*
		 * nint refers to a void*
		 * Only available in SDL 2.0.10 or higher.
		 */
        [DllImport(nativeLibName, EntryPoint = "SDL_LoadFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_SDL_LoadFile(byte* file, out nint datasize);
        public static unsafe nint SDL_LoadFile(string file, out nint datasize)
        {
            byte* utf8File = Utf8EncodeHeap(file);
            nint result = INTERNAL_SDL_LoadFile(utf8File, out datasize);
            Marshal.FreeHGlobal((nint)utf8File);
            return result;
        }

        #endregion
    }
}
