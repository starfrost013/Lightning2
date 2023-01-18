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
using static LightningBase.Utf8Marshaling;
#endregion

namespace LightningBase
{
    public static partial class SDL
    {
        #region SDL_system.h

        /* Windows */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate nint SDL_WindowsMessageHook(
            nint userdata,
            nint hWnd,
            uint message,
            ulong wParam,
            long lParam
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowsMessageHook(
            SDL_WindowsMessageHook callback,
            nint userdata
        );

        /* renderer refers to an SDL_Renderer*
		 * nint refers to an IDirect3DDevice9*
		 * Only available in 2.0.1 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_RenderGetD3D9Device(nint renderer);

        /* renderer refers to an SDL_Renderer*
		 * nint refers to an ID3D11Device*
		 * Only available in 2.0.16 or higher.
		 */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_RenderGetD3D11Device(nint renderer);

        /* iOS */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_iPhoneAnimationCallback(nint p);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_iPhoneSetAnimationCallback(
            nint window, /* SDL_Window* */
            int interval,
            SDL_iPhoneAnimationCallback callback,
            nint callbackParam
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_iPhoneSetEventPump(SDL_bool enabled);

        /* Android */

        public const int SDL_ANDROID_EXTERNAL_STORAGE_READ = 0x01;
        public const int SDL_ANDROID_EXTERNAL_STORAGE_WRITE = 0x02;

        /* nint refers to a JNIEnv* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_AndroidGetJNIEnv();

        /* nint refers to a jobject */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_AndroidGetActivity();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsAndroidTV();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsChromebook();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_bool SDL_IsDeXMode();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_AndroidBackButton();

        [DllImport(nativeLibName, EntryPoint = "SDL_AndroidGetInternalStoragePath", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_AndroidGetInternalStoragePath();

        public static string SDL_AndroidGetInternalStoragePath()
        {
            return UTF8_ToManaged(
                INTERNAL_SDL_AndroidGetInternalStoragePath()
            );
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_AndroidGetExternalStorageState();

        [DllImport(nativeLibName, EntryPoint = "SDL_AndroidGetExternalStoragePath", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_AndroidGetExternalStoragePath();

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
            Marshal.FreeHGlobal((nint)permissionPtr);
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
            Marshal.FreeHGlobal((nint)messagePtr);
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
