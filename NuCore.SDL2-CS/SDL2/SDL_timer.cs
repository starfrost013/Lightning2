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
#endregion

namespace NuCore.SDL2
{
    public static partial class SDL
    {
        #region SDL_timer.h

        /* System timers rely on different OS mechanisms depending on
		 * which operating system SDL2 is compiled against.
		 */

        /* Compare tick values, return true if A has passed B. Introduced in SDL 2.0.1,
		 * but does not require it (it was a macro).
		 */
        public static bool SDL_TICKS_PASSED(UInt32 A, UInt32 B)
        {
            return ((Int32)(B - A) <= 0);
        }

        /* Delays the thread's processing based on the milliseconds parameter */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Delay(UInt32 ms);

        /* Returns the milliseconds that have passed since SDL was initialized */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_GetTicks();

        /* Returns the milliseconds that have passed since SDL was initialized
		 * Only available in 2.0.18 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_GetTicks64();

        /* Get the current value of the high resolution counter */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_GetPerformanceCounter();

        /* Get the count per second of the high resolution counter */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_GetPerformanceFrequency();

        /* param refers to a void* */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate UInt32 SDL_TimerCallback(UInt32 interval, IntPtr param);

        /* int refers to an SDL_TimerID, param to a void* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AddTimer(
            UInt32 interval,
            SDL_TimerCallback callback,
            IntPtr param
        );

        /* id refers to an SDL_TimerID */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_RemoveTimer(int id);

        #endregion
    }
}
