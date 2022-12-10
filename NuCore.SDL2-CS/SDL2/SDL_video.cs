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
using static LightningBase.Utf8Marshaling;
#endregion

namespace LightningBase
{
    public static partial class SDL
    {
        #region SDL_video.h

        public enum SDL_GLattr
        {
            SDL_GL_RED_SIZE,
            SDL_GL_GREEN_SIZE,
            SDL_GL_BLUE_SIZE,
            SDL_GL_ALPHA_SIZE,
            SDL_GL_BUFFER_SIZE,
            SDL_GL_DOUBLEBUFFER,
            SDL_GL_DEPTH_SIZE,
            SDL_GL_STENCIL_SIZE,
            SDL_GL_ACCUM_RED_SIZE,
            SDL_GL_ACCUM_GREEN_SIZE,
            SDL_GL_ACCUM_BLUE_SIZE,
            SDL_GL_ACCUM_ALPHA_SIZE,
            SDL_GL_STEREO,
            SDL_GL_MULTISAMPLEBUFFERS,
            SDL_GL_MULTISAMPLESAMPLES,
            SDL_GL_ACCELERATED_VISUAL,
            SDL_GL_RETAINED_BACKING,
            SDL_GL_CONTEXT_MAJOR_VERSION,
            SDL_GL_CONTEXT_MINOR_VERSION,
            SDL_GL_CONTEXT_EGL,
            SDL_GL_CONTEXT_FLAGS,
            SDL_GL_CONTEXT_PROFILE_MASK,
            SDL_GL_SHARE_WITH_CURRENT_CONTEXT,
            SDL_GL_FRAMEBUFFER_SRGB_CAPABLE,
            SDL_GL_CONTEXT_RELEASE_BEHAVIOR,
            SDL_GL_CONTEXT_RESET_NOTIFICATION,  /* Requires >= 2.0.6 */
            SDL_GL_CONTEXT_NO_ERROR,        /* Requires >= 2.0.6 */
        }

        [Flags]
        public enum SDL_GLprofile
        {
            SDL_GL_CONTEXT_PROFILE_CORE = 0x0001,
            SDL_GL_CONTEXT_PROFILE_COMPATIBILITY = 0x0002,
            SDL_GL_CONTEXT_PROFILE_ES = 0x0004
        }

        [Flags]
        public enum SDL_GLcontext
        {
            SDL_GL_CONTEXT_DEBUG_FLAG = 0x0001,
            SDL_GL_CONTEXT_FORWARD_COMPATIBLE_FLAG = 0x0002,
            SDL_GL_CONTEXT_ROBUST_ACCESS_FLAG = 0x0004,
            SDL_GL_CONTEXT_RESET_ISOLATION_FLAG = 0x0008
        }

        public enum SDL_WindowEventID : byte
        {
            SDL_WINDOWEVENT_NONE,
            SDL_WINDOWEVENT_SHOWN,
            SDL_WINDOWEVENT_HIDDEN,
            SDL_WINDOWEVENT_EXPOSED,
            SDL_WINDOWEVENT_MOVED,
            SDL_WINDOWEVENT_RESIZED,
            SDL_WINDOWEVENT_SIZE_CHANGED,
            SDL_WINDOWEVENT_MINIMIZED,
            SDL_WINDOWEVENT_MAXIMIZED,
            SDL_WINDOWEVENT_RESTORED,
            SDL_WINDOWEVENT_ENTER,
            SDL_WINDOWEVENT_LEAVE,
            SDL_WINDOWEVENT_FOCUS_GAINED,
            SDL_WINDOWEVENT_FOCUS_LOST,
            SDL_WINDOWEVENT_CLOSE,
            /* Only available in 2.0.5 or higher. */
            SDL_WINDOWEVENT_TAKE_FOCUS,
            SDL_WINDOWEVENT_HIT_TEST,
            /* Only available in 2.0.18 or higher. */
            SDL_WINDOWEVENT_ICCPROF_CHANGED,
            SDL_WINDOWEVENT_DISPLAY_CHANGED
        }

        public enum SDL_DisplayEventID : byte
        {
            SDL_DISPLAYEVENT_NONE,
            SDL_DISPLAYEVENT_ORIENTATION,
            SDL_DISPLAYEVENT_CONNECTED, /* Requires >= 2.0.14 */
            SDL_DISPLAYEVENT_DISCONNECTED   /* Requires >= 2.0.14 */
        }

        public enum SDL_DisplayOrientation
        {
            SDL_ORIENTATION_UNKNOWN,
            SDL_ORIENTATION_LANDSCAPE,
            SDL_ORIENTATION_LANDSCAPE_FLIPPED,
            SDL_ORIENTATION_PORTRAIT,
            SDL_ORIENTATION_PORTRAIT_FLIPPED
        }

        /* Only available in 2.0.16 or higher. */
        public enum SDL_FlashOperation
        {
            SDL_FLASH_CANCEL,
            SDL_FLASH_BRIEFLY,
            SDL_FLASH_UNTIL_FOCUSED
        }

        [Flags]
        public enum SDL_WindowFlags : uint
        {
            SDL_WINDOW_FULLSCREEN = 0x00000001,
            SDL_WINDOW_OPENGL = 0x00000002,
            SDL_WINDOW_SHOWN = 0x00000004,
            SDL_WINDOW_HIDDEN = 0x00000008,
            SDL_WINDOW_BORDERLESS = 0x00000010,
            SDL_WINDOW_RESIZABLE = 0x00000020,
            SDL_WINDOW_MINIMIZED = 0x00000040,
            SDL_WINDOW_MAXIMIZED = 0x00000080,
            SDL_WINDOW_MOUSE_GRABBED = 0x00000100,
            SDL_WINDOW_INPUT_FOCUS = 0x00000200,
            SDL_WINDOW_MOUSE_FOCUS = 0x00000400,
            SDL_WINDOW_FULLSCREEN_DESKTOP =
                (SDL_WINDOW_FULLSCREEN | 0x00001000),
            SDL_WINDOW_FOREIGN = 0x00000800,
            SDL_WINDOW_ALLOW_HIGHDPI = 0x00002000,  /* Requires >= 2.0.1 */
            SDL_WINDOW_MOUSE_CAPTURE = 0x00004000,  /* Requires >= 2.0.4 */
            SDL_WINDOW_ALWAYS_ON_TOP = 0x00008000,  /* Requires >= 2.0.5 */
            SDL_WINDOW_SKIP_TASKBAR = 0x00010000,   /* Requires >= 2.0.5 */
            SDL_WINDOW_UTILITY = 0x00020000,    /* Requires >= 2.0.5 */
            SDL_WINDOW_TOOLTIP = 0x00040000,    /* Requires >= 2.0.5 */
            SDL_WINDOW_POPUP_MENU = 0x00080000, /* Requires >= 2.0.5 */
            SDL_WINDOW_KEYBOARD_GRABBED = 0x00100000,   /* Requires >= 2.0.16 */
            SDL_WINDOW_VULKAN = 0x10000000, /* Requires >= 2.0.6 */
            SDL_WINDOW_METAL = 0x2000000,   /* Requires >= 2.0.14 */

            SDL_WINDOW_INPUT_GRABBED =
                SDL_WINDOW_MOUSE_GRABBED,
        }

        /* Only available in 2.0.4 or higher. */
        public enum SDL_HitTestResult
        {
            SDL_HITTEST_NORMAL,     /* Region is normal. No special properties. */
            SDL_HITTEST_DRAGGABLE,      /* Region can drag entire window. */
            SDL_HITTEST_RESIZE_TOPLEFT,
            SDL_HITTEST_RESIZE_TOP,
            SDL_HITTEST_RESIZE_TOPRIGHT,
            SDL_HITTEST_RESIZE_RIGHT,
            SDL_HITTEST_RESIZE_BOTTOMRIGHT,
            SDL_HITTEST_RESIZE_BOTTOM,
            SDL_HITTEST_RESIZE_BOTTOMLEFT,
            SDL_HITTEST_RESIZE_LEFT
        }

        public const int SDL_WINDOWPOS_UNDEFINED_MASK = 0x1FFF0000;
        public const int SDL_WINDOWPOS_CENTERED_MASK = 0x2FFF0000;
        public const int SDL_WINDOWPOS_UNDEFINED = 0x1FFF0000;
        public const int SDL_WINDOWPOS_CENTERED = 0x2FFF0000;

        public static int SDL_WINDOWPOS_UNDEFINED_DISPLAY(int X)
        {
            return (SDL_WINDOWPOS_UNDEFINED_MASK | X);
        }

        public static bool SDL_WINDOWPOS_ISUNDEFINED(int X)
        {
            return (X & 0xFFFF0000) == SDL_WINDOWPOS_UNDEFINED_MASK;
        }

        public static int SDL_WINDOWPOS_CENTERED_DISPLAY(int X)
        {
            return (SDL_WINDOWPOS_CENTERED_MASK | X);
        }

        public static bool SDL_WINDOWPOS_ISCENTERED(int X)
        {
            return (X & 0xFFFF0000) == SDL_WINDOWPOS_CENTERED_MASK;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_DisplayMode
        {
            public uint format;
            public int w;
            public int h;
            public int refresh_rate;
            public nint driverdata; // void*
        }

        /* win refers to an SDL_Window*, area to a const SDL_Point*, data to a void*.
		 * Only available in 2.0.4 or higher.
		 */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate SDL_HitTestResult SDL_HitTest(nint win, nint area, nint data);

        /* nint refers to an SDL_Window* */
        [DllImport(nativeLibName, EntryPoint = "SDL_CreateWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_SDL_CreateWindow(
            byte* title,
            int x,
            int y,
            int w,
            int h,
            SDL_WindowFlags flags
        );
        public static unsafe nint SDL_CreateWindow(
            string title,
            int x,
            int y,
            int w,
            int h,
            SDL_WindowFlags flags
        )
        {
            int utf8TitleBufSize = Utf8Size(title);
            byte* utf8Title = stackalloc byte[utf8TitleBufSize];
            return INTERNAL_SDL_CreateWindow(
                Utf8Encode(title, utf8Title, utf8TitleBufSize),
                x, y, w, h,
                flags
            );
        }

        /* window refers to an SDL_Window*, renderer to an SDL_Renderer* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_CreateWindowAndRenderer(
            int width,
            int height,
            SDL_WindowFlags window_flags,
            out nint window,
            out nint renderer
        );

        /* data refers to some native window type, nint to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_CreateWindowFrom(nint data);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyWindow(nint window);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DisableScreenSaver();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_EnableScreenSaver();

        /* nint refers to an SDL_DisplayMode. Just use closest. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GetClosestDisplayMode(
            int displayIndex,
            ref SDL_DisplayMode mode,
            out SDL_DisplayMode closest
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetCurrentDisplayMode(
            int displayIndex,
            out SDL_DisplayMode mode
        );

        [DllImport(nativeLibName, EntryPoint = "SDL_GetCurrentVideoDriver", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_GetCurrentVideoDriver();
        public static string SDL_GetCurrentVideoDriver()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetCurrentVideoDriver());
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDesktopDisplayMode(
            int displayIndex,
            out SDL_DisplayMode mode
        );

        [DllImport(nativeLibName, EntryPoint = "SDL_GetDisplayName", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_GetDisplayName(int index);
        public static string SDL_GetDisplayName(int index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetDisplayName(index));
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDisplayBounds(
            int displayIndex,
            out SDL_Rect rect
        );

        /* Only available in 2.0.4 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDisplayDPI(
            int displayIndex,
            out float ddpi,
            out float hdpi,
            out float vdpi
        );

        /* Only available in 2.0.9 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_DisplayOrientation SDL_GetDisplayOrientation(
            int displayIndex
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDisplayMode(
            int displayIndex,
            int modeIndex,
            out SDL_DisplayMode mode
        );

        /* Only available in 2.0.5 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetDisplayUsableBounds(
            int displayIndex,
            out SDL_Rect rect
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumDisplayModes(
            int displayIndex
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumVideoDisplays();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumVideoDrivers();

        [DllImport(nativeLibName, EntryPoint = "SDL_GetVideoDriver", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_GetVideoDriver(
            int index
        );
        public static string SDL_GetVideoDriver(int index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetVideoDriver(index));
        }

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float SDL_GetWindowBrightness(
            nint window
        );

        /* window refers to an SDL_Window*
		 * Only available in 2.0.5 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowOpacity(
            nint window,
            float opacity
        );

        /* window refers to an SDL_Window*
		 * Only available in 2.0.5 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowOpacity(
            nint window,
            out float out_opacity
        );

        /* modal_window and parent_window refer to an SDL_Window*s
		 * Only available in 2.0.5 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowModalFor(
            nint modal_window,
            nint parent_window
        );

        /* window refers to an SDL_Window*
		 * Only available in 2.0.5 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowInputFocus(nint window);

        /* window refers to an SDL_Window*, nint to a void* */
        [DllImport(nativeLibName, EntryPoint = "SDL_GetWindowData", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_SDL_GetWindowData(
            nint window,
            byte* name
        );
        public static unsafe nint SDL_GetWindowData(
            nint window,
            string name
        )
        {
            int utf8NameBufSize = Utf8Size(name);
            byte* utf8Name = stackalloc byte[utf8NameBufSize];
            return INTERNAL_SDL_GetWindowData(
                window,
                Utf8Encode(name, utf8Name, utf8NameBufSize)
            );
        }

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowDisplayIndex(
            nint window
        );

        /* Only avaiable in 2.24.0 and higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetPointDisplayIndex(
            out SDL_Point point);

        /* Only avaiable in 2.24.0 and higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetRectDisplayIndex(
            out SDL_Rect point);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowDisplayMode(
            nint window,
            out SDL_DisplayMode mode
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetWindowFlags(nint window);

        /* nint refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GetWindowFromID(uint id);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowGammaRamp(
            nint window,
            [Out()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
                ushort[] red,
            [Out()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
                ushort[] green,
            [Out()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
                ushort[] blue
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GetWindowGrab(nint window);

        /* window refers to an SDL_Window*
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GetWindowKeyboardGrab(nint window);

        /* window refers to an SDL_Window*
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GetWindowMouseGrab(nint window);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetWindowID(nint window);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetWindowPixelFormat(
            nint window
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowMaximumSize(
            nint window,
            out int max_w,
            out int max_h
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowMinimumSize(
            nint window,
            out int min_w,
            out int min_h
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowPosition(
            nint window,
            out int x,
            out int y
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowSize(
            nint window,
            out int w,
            out int h
        );

        /* nint refers to an SDL_Surface*, window to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GetWindowSurface(nint window);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, EntryPoint = "SDL_GetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_GetWindowTitle(
            nint window
        );
        public static string SDL_GetWindowTitle(nint window)
        {
            return UTF8_ToManaged(
                INTERNAL_SDL_GetWindowTitle(window)
            );
        }

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_BindTexture(
            nint texture,
            out float texw,
            out float texh
        );

        /* nint and window refer to an SDL_GLContext and SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GL_CreateContext(nint window);

        /* context refers to an SDL_GLContext */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_DeleteContext(nint context);

        [DllImport(nativeLibName, EntryPoint = "SDL_GL_LoadLibrary", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_SDL_GL_LoadLibrary(byte* path);
        public static unsafe int SDL_GL_LoadLibrary(string path)
        {
            byte* utf8Path = Utf8EncodeHeap(path);
            int result = INTERNAL_SDL_GL_LoadLibrary(
                utf8Path
            );
            Marshal.FreeHGlobal((nint)utf8Path);
            return result;
        }

        /* nint refers to a function pointer, proc to a const char* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GL_GetProcAddress(nint proc);

        /* nint refers to a function pointer */
        public static unsafe nint SDL_GL_GetProcAddress(string proc)
        {
            int utf8ProcBufSize = Utf8Size(proc);
            byte* utf8Proc = stackalloc byte[utf8ProcBufSize];
            return SDL_GL_GetProcAddress(
                (nint)Utf8Encode(proc, utf8Proc, utf8ProcBufSize)
            );
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_UnloadLibrary();

        [DllImport(nativeLibName, EntryPoint = "SDL_GL_ExtensionSupported", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe SDL_bool INTERNAL_SDL_GL_ExtensionSupported(
            byte* extension
        );
        public static unsafe SDL_bool SDL_GL_ExtensionSupported(string extension)
        {
            int utf8ExtensionBufSize = Utf8Size(extension);
            byte* utf8Extension = stackalloc byte[utf8ExtensionBufSize];
            return INTERNAL_SDL_GL_ExtensionSupported(
                Utf8Encode(extension, utf8Extension, utf8ExtensionBufSize)
            );
        }

        /* Only available in SDL 2.0.2 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_ResetAttributes();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_GetAttribute(
            SDL_GLattr attr,
            out int value
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_GetSwapInterval();

        /* window and context refer to an SDL_Window* and SDL_GLContext */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_MakeCurrent(
            nint window,
            nint context
        );

        /* nint refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GL_GetCurrentWindow();

        /* nint refers to an SDL_Context */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GL_GetCurrentContext();

        /* window refers to an SDL_Window*.
		 * Only available in SDL 2.0.1 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_GetDrawableSize(
            nint window,
            out int w,
            out int h
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_SetAttribute(
            SDL_GLattr attr,
            int value
        );

        public static int SDL_GL_SetAttribute(
            SDL_GLattr attr,
            SDL_GLprofile profile
        )
        {
            return SDL_GL_SetAttribute(attr, (int)profile);
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_SetSwapInterval(int interval);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_SwapWindow(nint window);

        /* texture refers to an SDL_Texture* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GL_UnbindTexture(nint texture);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_HideWindow(nint window);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsScreenSaverEnabled();

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MaximizeWindow(nint window);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MinimizeWindow(nint window);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RaiseWindow(nint window);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RestoreWindow(nint window);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowBrightness(
            nint window,
            float brightness
        );

        /* nint and userdata are void*, window is an SDL_Window* */
        [DllImport(nativeLibName, EntryPoint = "SDL_SetWindowData", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint INTERNAL_SDL_SetWindowData(
            nint window,
            byte* name,
            nint userdata
        );
        public static unsafe nint SDL_SetWindowData(
            nint window,
            string name,
            nint userdata
        )
        {
            int utf8NameBufSize = Utf8Size(name);
            byte* utf8Name = stackalloc byte[utf8NameBufSize];
            return INTERNAL_SDL_SetWindowData(
                window,
                Utf8Encode(name, utf8Name, utf8NameBufSize),
                userdata
            );
        }

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowDisplayMode(
            nint window,
            ref SDL_DisplayMode mode
        );

        /* window refers to an SDL_Window* */
        /* NULL overload - use the window's dimensions and the desktop's format and refresh rate */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowDisplayMode(
            nint window,
            nint mode
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowFullscreen(
            nint window,
            uint flags
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowGammaRamp(
            nint window,
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
                ushort[] red,
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
                ushort[] green,
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
                ushort[] blue
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowGrab(
            nint window,
            SDL_bool grabbed
        );

        /* window refers to an SDL_Window*
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowKeyboardGrab(
            nint window,
            SDL_bool grabbed
        );

        /* window refers to an SDL_Window*
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowMouseGrab(
            nint window,
            SDL_bool grabbed
        );


        /* window refers to an SDL_Window*, icon to an SDL_Surface* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowIcon(
            nint window,
            nint icon
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowMaximumSize(
            nint window,
            int max_w,
            int max_h
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowMinimumSize(
            nint window,
            int min_w,
            int min_h
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowPosition(
            nint window,
            int x,
            int y
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowSize(
            nint window,
            int w,
            int h
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowBordered(
            nint window,
            SDL_bool bordered
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowBordersSize(
            nint window,
            out int top,
            out int left,
            out int bottom,
            out int right
        );

        /* window refers to an SDL_Window*
		 * Only available in 2.0.5 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowResizable(
            nint window,
            SDL_bool resizable
        );

        /* window refers to an SDL_Window*
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowAlwaysOnTop(
            nint window,
            SDL_bool on_top
        );

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, EntryPoint = "SDL_SetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void INTERNAL_SDL_SetWindowTitle(
            nint window,
            byte* title
        );
        public static unsafe void SDL_SetWindowTitle(
            nint window,
            string title
        )
        {
            int utf8TitleBufSize = Utf8Size(title);
            byte* utf8Title = stackalloc byte[utf8TitleBufSize];
            INTERNAL_SDL_SetWindowTitle(
                window,
                Utf8Encode(title, utf8Title, utf8TitleBufSize)
            );
        }

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ShowWindow(nint window);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateWindowSurface(nint window);

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateWindowSurfaceRects(
            nint window,
            [In] SDL_Rect[] rects,
            int numrects
        );

        [DllImport(nativeLibName, EntryPoint = "SDL_VideoInit", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_SDL_VideoInit(
            byte* driver_name
        );
        public static unsafe int SDL_VideoInit(string driver_name)
        {
            int utf8DriverNameBufSize = Utf8Size(driver_name);
            byte* utf8DriverName = stackalloc byte[utf8DriverNameBufSize];
            return INTERNAL_SDL_VideoInit(
                Utf8Encode(driver_name, utf8DriverName, utf8DriverNameBufSize)
            );
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_VideoQuit();

        /* window refers to an SDL_Window*, callback_data to a void*
		 * Only available in 2.0.4 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowHitTest(
            nint window,
            SDL_HitTest callback,
            nint callback_data
        );

        /* nint refers to an SDL_Window*
		 * Only available in 2.0.4 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GetGrabbedWindow();

        /* window refers to an SDL_Window*
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_FlashWindow(
            nint window,
            SDL_FlashOperation operation
        );

        /* nint refers to a void*
		* window refers to an SDL_Window*
		* mode refers to a size_t*
		* Only available in 2.0.18 or higher.
		*/
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GetWindowICCProfile(
            nint window,
            out nint mode
        );

        /* window refers to an SDL_Window*
 * Only available in 2.0.18 or higher.
 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowMouseRect(
            nint window,
            ref SDL_Rect rect
        );

        /* window refers to an SDL_Window*
		 * rect refers to an SDL_Rect*
		 * This overload allows for nint.Zero (null) to be passed for rect.
		 * Only available in 2.0.18 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetWindowMouseRect(
            nint window,
            nint rect
        );

        /* window refers to an SDL_Window*
		 * nint refers to an SDL_Rect*
		 * Only available in 2.0.18 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_GetWindowMouseRect(
            nint window
        );

        #endregion
    }
}
