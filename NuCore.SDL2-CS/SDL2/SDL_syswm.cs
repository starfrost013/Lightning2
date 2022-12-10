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
namespace LightningBase
{
    public static partial class SDL
    {
        #region SDL_syswm.h

        public enum SDL_SYSWM_TYPE
        {
            SDL_SYSWM_UNKNOWN,
            SDL_SYSWM_WINDOWS,
            SDL_SYSWM_X11,
            SDL_SYSWM_DIRECTFB,
            SDL_SYSWM_COCOA,
            SDL_SYSWM_UIKIT,
            SDL_SYSWM_WAYLAND,
            SDL_SYSWM_MIR,
            SDL_SYSWM_WINRT,
            SDL_SYSWM_ANDROID,
            SDL_SYSWM_VIVANTE,
            SDL_SYSWM_OS2,
            SDL_SYSWM_HAIKU,
            SDL_SYSWM_KMSDRM /* requires >= 2.0.16 */
        }

        // FIXME: I wish these weren't public...
        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_windows_wminfo
        {
            public nint window; // Refers to an HWND
            public nint hdc; // Refers to an HDC
            public nint hinstance; // Refers to an HINSTANCE
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_winrt_wminfo
        {
            public nint window; // Refers to an IInspectable*
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_x11_wminfo
        {
            public nint display; // Refers to a Display*
            public nint window; // Refers to a Window (XID, use ToInt64!)
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_directfb_wminfo
        {
            public nint dfb; // Refers to an IDirectFB*
            public nint window; // Refers to an IDirectFBWindow*
            public nint surface; // Refers to an IDirectFBSurface*
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_cocoa_wminfo
        {
            public nint window; // Refers to an NSWindow*
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_uikit_wminfo
        {
            public nint window; // Refers to a UIWindow*
            public uint framebuffer;
            public uint colorbuffer;
            public uint resolveFramebuffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_wayland_wminfo
        {
            public nint display; // Refers to a wl_display*
            public nint surface; // Refers to a wl_surface*
            public nint shell_surface; // Refers to a wl_shell_surface*
            public nint egl_window; // Refers to an egl_window*, requires >= 2.0.16
            public nint xdg_surface; // Refers to an xdg_surface*, requires >= 2.0.16
            public nint xdg_toplevel; // Refers to an xdg_toplevel*, requires >= 2.0.18
            public nint xdg_popup; // Refers to an xdg_popup*, requires >= 2.0.22
            public nint xdg_positioner; // Refers to an xdg_positioner*, requires >= 2.0.22
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_mir_wminfo
        {
            public nint connection; // Refers to a MirConnection*
            public nint surface; // Refers to a MirSurface*
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_android_wminfo
        {
            public nint window; // Refers to an ANativeWindow
            public nint surface; // Refers to an EGLSurface
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_vivante_wminfo
        {
            public nint display; // Refers to an EGLNativeDisplayType
            public nint window; // Refers to an EGLNativeWindowType
        }

        /* Only available in 2.0.14 or higher. */
        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_os2_wminfo
        {
            public nint hwnd; // Refers to an HWND
            public nint hwndFrame; // Refers to an HWND
        }

        /* Only available in 2.0.16 or higher. */
        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNAL_kmsdrm_wminfo
        {
            int dev_index;
            int drm_fd;
            nint gbm_dev; // Refers to a gbm_device*
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INTERNAL_SysWMDriverUnion
        {
            [FieldOffset(0)]
            public INTERNAL_windows_wminfo win;
            [FieldOffset(0)]
            public INTERNAL_winrt_wminfo winrt;
            [FieldOffset(0)]
            public INTERNAL_x11_wminfo x11;
            [FieldOffset(0)]
            public INTERNAL_directfb_wminfo dfb;
            [FieldOffset(0)]
            public INTERNAL_cocoa_wminfo cocoa;
            [FieldOffset(0)]
            public INTERNAL_uikit_wminfo uikit;
            [FieldOffset(0)]
            public INTERNAL_wayland_wminfo wl;
            [FieldOffset(0)]
            public INTERNAL_mir_wminfo mir;
            [FieldOffset(0)]
            public INTERNAL_android_wminfo android;
            [FieldOffset(0)]
            public INTERNAL_os2_wminfo os2;
            [FieldOffset(0)]
            public INTERNAL_vivante_wminfo vivante;
            [FieldOffset(0)]
            public INTERNAL_kmsdrm_wminfo ksmdrm;
            // private int dummy;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_SysWMinfo
        {
            public SDL_version version;
            public SDL_SYSWM_TYPE subsystem;
            public INTERNAL_SysWMDriverUnion info;
        }

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_GetWindowWMInfo(
            nint window,
            ref SDL_SysWMinfo info
        );

        #endregion
    }
}
