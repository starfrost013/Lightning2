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
        #region SDL_system.h

        /* Windows */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDL_WindowsMessageHook(
            IntPtr userdata,
            IntPtr hWnd,
            uint message,
            ulong wParam,
            long lParam
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowsMessageHook(
            SDL_WindowsMessageHook callback,
            IntPtr userdata
        );

        /* renderer refers to an SDL_Renderer*
		 * IntPtr refers to an IDirect3DDevice9*
		 * Only available in 2.0.1 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RenderGetD3D9Device(IntPtr renderer);

        /* renderer refers to an SDL_Renderer*
		 * IntPtr refers to an ID3D11Device*
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RenderGetD3D11Device(IntPtr renderer);

        /* iOS */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_iPhoneAnimationCallback(IntPtr p);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_iPhoneSetAnimationCallback(
            IntPtr window, /* SDL_Window* */
            int interval,
            SDL_iPhoneAnimationCallback callback,
            IntPtr callbackParam
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_iPhoneSetEventPump(SDL_bool enabled);

        /* Android */

        public const int SDL_ANDROID_EXTERNAL_STORAGE_READ = 0x01;
        public const int SDL_ANDROID_EXTERNAL_STORAGE_WRITE = 0x02;

        /* IntPtr refers to a JNIEnv* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AndroidGetJNIEnv();

        /* IntPtr refers to a jobject */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AndroidGetActivity();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsAndroidTV();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsChromebook();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsDeXMode();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_AndroidBackButton();

        [DllImport(nativeLibName, EntryPoint = "SDL_AndroidGetInternalStoragePath", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_AndroidGetInternalStoragePath();

        public static string SDL_AndroidGetInternalStoragePath()
        {
            return UTF8_ToManaged(
                INTERNAL_SDL_AndroidGetInternalStoragePath()
            );
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AndroidGetExternalStorageState();

        [DllImport(nativeLibName, EntryPoint = "SDL_AndroidGetExternalStoragePath", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_AndroidGetExternalStoragePath();

        public static string SDL_AndroidGetExternalStoragePath()
        {
            return UTF8_ToManaged(
                INTERNAL_SDL_AndroidGetExternalStoragePath()
            );
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetAndroidSDKVersion();

        /* Only available in 2.0.14 or higher. */
        [DllImport(nativeLibName, EntryPoint = "SDL_AndroidRequestPermission", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern SDL_bool INTERNAL_SDL_AndroidRequestPermission(
            byte* permission
        );
        public static unsafe SDL_bool SDL_AndroidRequestPermission(
            string permission
        )
        {
            byte* permissionPtr = Utf8EncodeHeap(permission);
            SDL_bool result = INTERNAL_SDL_AndroidRequestPermission(
                permissionPtr
            );
            Marshal.FreeHGlobal((IntPtr)permissionPtr);
            return result;
        }

        /* Only available in 2.0.16 or higher. */
        [DllImport(nativeLibName, EntryPoint = "SDL_AndroidShowToast", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int INTERNAL_SDL_AndroidShowToast(
            byte* message,
            int duration,
            int gravity,
            int xOffset,
            int yOffset
        );
        public static unsafe int SDL_AndroidShowToast(
            string message,
            int duration,
            int gravity,
            int xOffset,
            int yOffset
        )
        {
            byte* messagePtr = Utf8EncodeHeap(message);
            int result = INTERNAL_SDL_AndroidShowToast(
                messagePtr,
                duration,
                gravity,
                xOffset,
                yOffset
            );
            Marshal.FreeHGlobal((IntPtr)messagePtr);
            return result;
        }

        /* WinRT */

        public enum SDL_WinRT_DeviceFamily
        {
            SDL_WINRT_DEVICEFAMILY_UNKNOWN,
            SDL_WINRT_DEVICEFAMILY_DESKTOP,
            SDL_WINRT_DEVICEFAMILY_MOBILE,
            SDL_WINRT_DEVICEFAMILY_XBOX
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_WinRT_DeviceFamily SDL_WinRTGetDeviceFamily();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsTablet();

        #endregion

    }
}
