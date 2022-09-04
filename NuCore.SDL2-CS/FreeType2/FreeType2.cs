#region License
/* FreeType2# - C# Wrapper for FreeType 2
 * August 25, 2022
 * 
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
 * starfrost (mario64crashed@gmail.com)
 *
 */
#endregion

#region Using Statements
using System;
using System.Runtime.InteropServices;
using static NuCore.SDL2.Utf8Marshaling;
#endregion

namespace NuCore.SDL2
{
    /// <summary>
    /// FreeType2 bindings
    /// </summary>
    public static class FreeType2
    {
        public const int FREETYPE2_EXPECTED_MAJOR_VERSION = 2;
        public const int FREETYPE2_EXPECTED_MINOR_VERSION = 12;
        public const int FREETYPE2_EXPECTED_PATCHLEVEL = 1;

#if X64
        public const string NativeDLLPath = @"Libraries\FreeType2-x64.dll";
#else
        public const string NativeDLLPath = @"Libraries\FreeType2-ARM64.dll";
#endif

        #region Base Interface Core

        /// <summary>
        /// Defines a version of FreeType2
        /// </summary>
        public struct FT_Linked_Version
        {
            public int Major;
            public int Minor;
            public int Revision;

            public FT_Linked_Version(
                int major, int minor, int revision)
            {
                Major = major;
                Minor = minor;
                Revision = revision;
            }
        }

        [DllImport(NativeDLLPath, EntryPoint = "FT_Library_Version", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_FT_Library_Version(IntPtr library,
            out int major,
            out int minor,
            out int patch);

        public static FT_Linked_Version FT_Library_Version(IntPtr library)
        {
            // get the freetype version
            INTERNAL_FT_Library_Version(library, out var nMajor, out var nMinor, out var nPatch);
            return new FT_Linked_Version(nMajor, nMinor, nPatch);
        }

        private static bool FREETYPE2_VERSION_ATLEAST(IntPtr library,
            int x,
            int y,
            int z)
        {
            // verify correct version
            FT_Linked_Version version = FT_Library_Version(library);

            return (version.Major * 1000 + (version.Minor * 100) + version.Revision) >= (x * 1000) + (y * 100) + z;
        }

        /// <summary>
        /// Opens the FreeType Library.
        /// </summary>
        /// <returns>An IntPtr to an unmanaged FT_Library object.</returns>
        [DllImport(NativeDLLPath, EntryPoint = "FT_Open_Library", CallingConvention = CallingConvention.Cdecl)]
        public static extern int INTERNAL_FT_Open_Library(out IntPtr library);

        public static int FT_Open_Library(out IntPtr library)
        {
            int succeeded = INTERNAL_FT_Open_Library(out library);

            if (!FREETYPE2_VERSION_ATLEAST(library, FREETYPE2_EXPECTED_MAJOR_VERSION, FREETYPE2_EXPECTED_MINOR_VERSION, FREETYPE2_EXPECTED_PATCHLEVEL)) return -94004;

            return succeeded;
        }

        [DllImport(NativeDLLPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Close_Library(IntPtr library);

        [DllImport(NativeDLLPath, EntryPoint = "FT_Error_String", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern IntPtr INTERNAL_FT_Error_String(int errCode);

        public static string FT_Error_String(int errCode) => UTF8_ToManaged(INTERNAL_FT_Error_String(errCode));

        #endregion

        #region Base Interface Font loading

        [DllImport(NativeDLLPath, EntryPoint = "FT_New_Face", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int INTERNAL_FT_New_Face(IntPtr library,
            byte* filepathname,
            long index,
            out IntPtr face
            );

        public static unsafe int FT_New_Face(IntPtr library,
            string filepathname,
            long index,
            out IntPtr face)
        {
            int utf8TextSize = Utf8Size(filepathname);
            byte* utf8Buffer = stackalloc byte[utf8TextSize];

            return INTERNAL_FT_New_Face(library, utf8Buffer, index, out face);
        }

        [DllImport(NativeDLLPath, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int FT_Done_Face(IntPtr face);

        [DllImport(NativeDLLPath, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern uint FT_Get_Char_Index(IntPtr face,
            int charcode);

        [DllImport(NativeDLLPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern int FT_Set_Char_Size(IntPtr face,
            double char_width,
            double char_height,
            uint horz_dpi,
            uint vert_dpi);

        [DllImport(NativeDLLPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern int FT_Set_Pixel_Sizes(IntPtr face,
            uint pixel_width,
            uint pixel_height);

        [DllImport(NativeDLLPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern int FT_Load_Glyph(IntPtr face,
            uint index,
            [MarshalAs(UnmanagedType.I4)]
            FT2LoadFlags load_flags
            );

        [DllImport(NativeDLLPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern int FT_Render_Glyph(IntPtr slot,
            FT2RenderMode render_mode);

        #endregion
    }
}
