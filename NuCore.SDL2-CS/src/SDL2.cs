#region License
/* NuCore SDL2 Bindings
 * 
 * Version 3.0.4
 * Copyright © 2021-2022 starfrost
 * February 15, 2022
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
using System.Runtime.InteropServices;
using System.Text;
#endregion

namespace NuCore.SDL2
{
    public static partial class SDL
    {
        #region SDL2# Variables

        // This has been reverted, as it turns out that SDL2_image etc have a dependency on "SDL2.dll" therefore we can't rename it as it fails to load when renamed.
        // Currently using .csproj <Link> element to get around this currently.
        // We CAN get around it by trapping the failed to load assembly event, try to look at this
        private const string nativeLibName = @"Content\NativeLibraries\SDL2";

        /// <summary>
        /// The version of the NuCore SDL2 Bindings
        /// </summary>
        private static string SDL2CS_VERSION = $"NuCore SDL2 Bindings version {SDL2CS_VERSION_MAJOR}.{SDL2CS_VERSION_MINOR}.{SDL2CS_VERSION_REVISION} for Lightning2 ©2022 starfrost ©2013-2021 Ethan Lee"; // cannot be const

        private const int SDL2CS_VERSION_MAJOR = 3;
        private const int SDL2CS_VERSION_MINOR = 0;
        private const int SDL2CS_VERSION_REVISION = 8;
        #endregion

        #region UTF8 Marshaling

        /* Used for stack allocated string marshaling. */
        internal static int Utf8Size(string str)
        {
            if (str == null) return 0;

            return (str.Length * 4) + 1;
        }
        internal static unsafe byte* Utf8Encode(string str, byte* buffer, int bufferSize)
        {
            if (str == null)
            {
                return (byte*)0;
            }
            fixed (char* strPtr = str)
            {
                Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
            }
            return buffer;
        }

        /* Used for heap allocated string marshaling.
		 * Returned byte* must be free'd with FreeHGlobal.
		 */
        internal static unsafe byte* Utf8EncodeHeap(string str)
        {
            if (str == null) return (byte*)0;

            int bufferSize = Utf8Size(str);
            byte* buffer = (byte*)Marshal.AllocHGlobal(bufferSize);
            fixed (char* strPtr = str)
            {
                Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
            }
            return buffer;
        }

        /* This is public because SDL_DropEvent needs it! */
        public static unsafe string UTF8_ToManaged(IntPtr s, bool freePtr = false)
        {
            if (s == IntPtr.Zero) return null;

            /* We get to do strlen ourselves! */
            byte* ptr = (byte*)s;
            while (*ptr != 0) ptr++;

            /* Modern C# lets you just send the byte*, nice! */

            // this has been made the default, as .NET 6.0 is way beyond .NET Standard 2.0
            // but doesn't define its ifdef, resulting in bleh code getting executed
            string result = Encoding.UTF8.GetString(
                (byte*)s,
                (int)(ptr - (byte*)s)
            );

            /* Some SDL functions will malloc, we have to free! */
            if (freePtr) SDL_free(s);
            return result;
        }

        #endregion

    }
}