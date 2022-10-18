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
using System.Diagnostics;
using System.Runtime.InteropServices;
#endregion

namespace LightningBase
{
    public static partial class SDL
    {
        #region SDL.h

        public enum SDL_InitFlags
        {
            SDL_INIT_TIMER = 0x01,
            SDL_INIT_AUDIO = 0x10,
            SDL_INIT_VIDEO = 0x20,
            SDL_INIT_JOYSTICK = 0x200,
            SDL_INIT_HAPTIC = 0x400,
            SDL_INIT_GAMECONTROLLER = 0x2000,
            SDL_INIT_EVENTS = 0x4000,
            SDL_INIT_SENSOR = 0x8000,
            SDL_INIT_NOPARACHUTE = 0x1000000,
            SDL_INIT_EVERYTHING = (
            SDL_INIT_TIMER | SDL_INIT_AUDIO | SDL_INIT_VIDEO |
            SDL_INIT_EVENTS | SDL_INIT_JOYSTICK | SDL_INIT_HAPTIC |
            SDL_INIT_GAMECONTROLLER | SDL_INIT_SENSOR
            )
        }

        public static int SDL_Init(SDL_InitFlags flags)
        {
            // print a sign-on message so that we know it's initialising
            Console.WriteLine(LightningBase.BASE_VERSION);

            // Prevent silent exit if debugger is attached in some cases
            if (Debugger.IsAttached) SDL_SetHint(SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");

            // verify a sufficient SDL version (as we dynamically link) - get the real version of SDL2.dll to do this
            SDL_GetVersion(out var realVersion);

            bool versionIsCompatible = SDL_VERSION_ATLEAST(SDL_EXPECTED_MAJOR_VERSION, SDL_EXPECTED_MINOR_VERSION, SDL_EXPECTED_PATCHLEVEL);

            // if SDL is too load
            if (!versionIsCompatible)
            {
                SDL_SetError($"Incorrect SDL version. Version {SDL_EXPECTED_MAJOR_VERSION}.{SDL_EXPECTED_MINOR_VERSION}.{SDL_EXPECTED_PATCHLEVEL} is required," +
                    $" got {realVersion.major}.{realVersion.minor}.{realVersion.patch}!");
                return -94001; // NEGATIVE is an error code
            }

            return INTERNAL_SDL_Init(flags);
        }

        [DllImport(nativeLibName, EntryPoint = "SDL_Init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int INTERNAL_SDL_Init(
        [MarshalAs(UnmanagedType.U4)]
        SDL_InitFlags flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_InitSubSystem(uint flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Quit();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_QuitSubSystem(uint flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WasInit(uint flags);

        /* Use this function with GDK/GDKX to call your C# Main() function!
        * Only available in SDL 2.24.0 or higher.
        */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GDKRunApp(
            SDL_main_func mainFunction,
            IntPtr reserved
        );

        #endregion
    }
}