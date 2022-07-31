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
using System.Runtime.InteropServices;
#endregion

namespace NuCore.SDL2
{
    public static partial class SDL
    {
        #region SDL_touch.h

        public const uint SDL_TOUCH_MOUSEID = uint.MaxValue;

        public struct SDL_Finger
        {
            public long id; // SDL_FingerID
            public float x;
            public float y;
            public float pressure;
        }

        /* Only available in 2.0.10 or higher. */
        public enum SDL_TouchDeviceType
        {
            SDL_TOUCH_DEVICE_INVALID = -1,
            SDL_TOUCH_DEVICE_DIRECT,            /* touch screen with window-relative coordinates */
            SDL_TOUCH_DEVICE_INDIRECT_ABSOLUTE, /* trackpad with absolute device coordinates */
            SDL_TOUCH_DEVICE_INDIRECT_RELATIVE  /* trackpad with screen cursor-relative coordinates */
        }

        /**
		 *  \brief Get the number of registered touch devices.
 		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumTouchDevices();

        /**
		 *  \brief Get the touch ID with the given index, or 0 if the index is invalid.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_GetTouchDevice(int index);

        /**
		 *  \brief Get the number of active fingers for a given touch device.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumTouchFingers(long touchID);

        /**
		 *  \brief Get the finger object of the given touch, with the given index.
		 *  Returns pointer to SDL_Finger.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTouchFinger(long touchID, int index);

        /* Only available in 2.0.10 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_TouchDeviceType SDL_GetTouchDeviceType(Int64 touchID);

        /* Only available in 2.0.22 or higher. */
        [DllImport(nativeLibName, EntryPoint = "SDL_GetTouchName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetTouchName(int index);

        /* Only available in 2.0.22 or higher. */
        public static string SDL_GetTouchName(int index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetTouchName(index));
        }
        #endregion
    }
}
