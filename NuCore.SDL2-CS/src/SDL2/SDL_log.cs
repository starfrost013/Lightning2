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
        #region SDL_log.h

        public enum SDL_LogCategory
        {
            SDL_LOG_CATEGORY_APPLICATION,
            SDL_LOG_CATEGORY_ERROR,
            SDL_LOG_CATEGORY_ASSERT,
            SDL_LOG_CATEGORY_SYSTEM,
            SDL_LOG_CATEGORY_AUDIO,
            SDL_LOG_CATEGORY_VIDEO,
            SDL_LOG_CATEGORY_RENDER,
            SDL_LOG_CATEGORY_INPUT,
            SDL_LOG_CATEGORY_TEST,

            /* Reserved for future SDL library use */
            SDL_LOG_CATEGORY_RESERVED1,
            SDL_LOG_CATEGORY_RESERVED2,
            SDL_LOG_CATEGORY_RESERVED3,
            SDL_LOG_CATEGORY_RESERVED4,
            SDL_LOG_CATEGORY_RESERVED5,
            SDL_LOG_CATEGORY_RESERVED6,
            SDL_LOG_CATEGORY_RESERVED7,
            SDL_LOG_CATEGORY_RESERVED8,
            SDL_LOG_CATEGORY_RESERVED9,
            SDL_LOG_CATEGORY_RESERVED10,

            /* Beyond this point is reserved for application use, e.g.
			enum {
				MYAPP_CATEGORY_AWESOME1 = SDL_LOG_CATEGORY_CUSTOM,
				MYAPP_CATEGORY_AWESOME2,
				MYAPP_CATEGORY_AWESOME3,
				...
			};
			*/
            SDL_LOG_CATEGORY_CUSTOM
        }

        public enum SDL_LogPriority
        {
            SDL_LOG_PRIORITY_VERBOSE = 1,
            SDL_LOG_PRIORITY_DEBUG,
            SDL_LOG_PRIORITY_INFO,
            SDL_LOG_PRIORITY_WARN,
            SDL_LOG_PRIORITY_ERROR,
            SDL_LOG_PRIORITY_CRITICAL,
            SDL_NUM_LOG_PRIORITIES
        }

        /* userdata refers to a void*, message to a const char* */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_LogOutputFunction(
            IntPtr userdata,
            [MarshalAs(UnmanagedType.I4)] // treat as nt32
			SDL_LogCategory category,
            SDL_LogPriority priority,
            IntPtr message
        );

        /* Use string.Format for arglists */
        [DllImport(nativeLibName, EntryPoint = "SDL_Log", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_Log(byte* fmtAndArglist);
        public static unsafe void SDL_Log(string fmtAndArglist)
        {
            int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
            byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
            INTERNAL_SDL_Log(
                Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
            );
        }

        /* Use string.Format for arglists */
        [DllImport(nativeLibName, EntryPoint = "SDL_LogVerbose", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_LogVerbose
        (
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            byte* fmtAndArglist
        );
        public static unsafe void SDL_LogVerbose
        (
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            string fmtAndArglist
        )
        {
            int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
            byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
            INTERNAL_SDL_LogVerbose(
                category,
                Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
            );
        }

        /* Use string.Format for arglists */
        [DllImport(nativeLibName, EntryPoint = "SDL_LogDebug", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_LogDebug
        (
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            byte* fmtAndArglist
        );
        public static unsafe void SDL_LogDebug(
            int category,
            string fmtAndArglist
        )
        {
            int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
            byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
            INTERNAL_SDL_LogDebug(
                (SDL_LogCategory)category,
                Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
            );
        }

        /* Use string.Format for arglists */
        [DllImport(nativeLibName, EntryPoint = "SDL_LogInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_LogInfo
        (
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            byte* fmtAndArglist
        );
        public static unsafe void SDL_LogInfo(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            string fmtAndArglist
        )
        {
            int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
            byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
            INTERNAL_SDL_LogInfo(
                category,
                Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
            );
        }

        /* Use string.Format for arglists */
        [DllImport(nativeLibName, EntryPoint = "SDL_LogWarn", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_LogWarn(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            byte* fmtAndArglist
        );
        public static unsafe void SDL_LogWarn(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            string fmtAndArglist
        )
        {
            int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
            byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
            INTERNAL_SDL_LogWarn(
                category,
                Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
            );
        }

        /* Use string.Format for arglists */
        [DllImport(nativeLibName, EntryPoint = "SDL_LogError", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_LogError(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            byte* fmtAndArglist
        );
        public static unsafe void SDL_LogError(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            string fmtAndArglist
        )
        {
            int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
            byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
            INTERNAL_SDL_LogError(
                category,
                Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
            );
        }

        /* Use string.Format for arglists */
        [DllImport(nativeLibName, EntryPoint = "SDL_LogCritical", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_LogCritical(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            byte* fmtAndArglist
        );
        public static unsafe void SDL_LogCritical(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            string fmtAndArglist
        )
        {
            int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
            byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
            INTERNAL_SDL_LogCritical(
                category,
                Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
            );
        }

        /* Use string.Format for arglists */
        [DllImport(nativeLibName, EntryPoint = "SDL_LogMessage", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_LogMessage(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            SDL_LogPriority priority,
            byte* fmtAndArglist
        );
        public static unsafe void SDL_LogMessage(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            SDL_LogPriority priority,
            string fmtAndArglist
        )
        {
            int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
            byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
            INTERNAL_SDL_LogMessage(
                category,
                priority,
                Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
            );
        }

        /* Use string.Format for arglists */
        [DllImport(nativeLibName, EntryPoint = "SDL_LogMessageV", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_LogMessageV(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            SDL_LogPriority priority,
            byte* fmtAndArglist
        );
        public static unsafe void SDL_LogMessageV(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            SDL_LogPriority priority,
            string fmtAndArglist
        )
        {
            int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
            byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
            INTERNAL_SDL_LogMessageV(
                category,
                priority,
                Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
            );
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_LogPriority SDL_LogGetPriority(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LogSetPriority(
            [MarshalAs(UnmanagedType.I4)] // treat as int32
			SDL_LogCategory category,
            SDL_LogPriority priority
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LogSetAllPriority(
            SDL_LogPriority priority
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LogResetPriorities();

        /* userdata refers to a void* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_LogGetOutputFunction(
            out IntPtr callback,
            out IntPtr userdata
        );
        public static void SDL_LogGetOutputFunction(
            out SDL_LogOutputFunction callback,
            out IntPtr userdata
        )
        {
            IntPtr result = IntPtr.Zero;
            SDL_LogGetOutputFunction(
                out result,
                out userdata
            );
            if (result != IntPtr.Zero)
            {
                callback = (SDL_LogOutputFunction)Marshal.GetDelegateForFunctionPointer(
                    result,
                    typeof(SDL_LogOutputFunction)
                );
            }
            else
            {
                callback = null;
            }
        }

        /* userdata refers to a void* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LogSetOutputFunction(
            SDL_LogOutputFunction callback,
            IntPtr userdata
        );

        #endregion
    }
}
