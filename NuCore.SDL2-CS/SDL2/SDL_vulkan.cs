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
using System;
using System.Runtime.InteropServices;
using static NuCore.SDL2.Utf8Marshaling;
#endregion

namespace NuCore.SDL2
{
    public static partial class SDL
    {
        #region SDL_vulkan.h

        /* Only available in 2.0.6 or higher. */
        [DllImport(nativeLibName, EntryPoint = "SDL_Vulkan_LoadLibrary", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_SDL_Vulkan_LoadLibrary(
            byte* path
        );
        public static unsafe int SDL_Vulkan_LoadLibrary(string path)
        {
            byte* utf8Path = Utf8EncodeHeap(path);
            int result = INTERNAL_SDL_Vulkan_LoadLibrary(
                utf8Path
            );
            Marshal.FreeHGlobal((IntPtr)utf8Path);
            return result;
        }

        /* Only available in 2.0.6 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_Vulkan_GetVkGetInstanceProcAddr();

        /* Only available in 2.0.6 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Vulkan_UnloadLibrary();

        /* window refers to an SDL_Window*, pNames to a const char**.
		 * Only available in 2.0.6 or higher.
		 * This overload allows for IntPtr.Zero (null) to be passed for pNames.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_Vulkan_GetInstanceExtensions(
            IntPtr window,
            out uint pCount,
            IntPtr pNames
        );

        /* window refers to an SDL_Window*, pNames to a const char**.
		 * Only available in 2.0.6 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_Vulkan_GetInstanceExtensions(
            IntPtr window,
            out uint pCount,
            IntPtr[] pNames
        );

        /* window refers to an SDL_Window.
		 * instance refers to a VkInstance.
		 * surface refers to a VkSurfaceKHR.
		 * Only available in 2.0.6 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_Vulkan_CreateSurface(
            IntPtr window,
            IntPtr instance,
            out ulong surface
        );

        /* window refers to an SDL_Window*.
		 * Only available in 2.0.6 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Vulkan_GetDrawableSize(
            IntPtr window,
            out int w,
            out int h
        );

        #endregion
    }
}
