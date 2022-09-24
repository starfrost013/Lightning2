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
        #region SDL_messagebox.h

        /// <summary>
        /// Flags that indicate which icons will be used for an SDL messagebox.
        /// </summary>
        [Flags]
        public enum SDL_MessageBoxFlags : uint
        {
            /// <summary>
            /// If set, an error icon will be used.
            /// </summary>
            SDL_MESSAGEBOX_ERROR = 0x00000010,

            /// <summary>
            /// If set, a warning icon will be used.
            /// </summary>
            SDL_MESSAGEBOX_WARNING = 0x00000020,

            /// <summary>
            /// If set, an information icon will be used.
            /// </summary>
            SDL_MESSAGEBOX_INFORMATION = 0x00000040
        }

        /// <summary>
        /// Flags that control the behaviour of an SDL messagebox.
        /// </summary>
        [Flags]
        public enum SDL_MessageBoxButtonFlags : uint
        {
            /// <summary>
            /// If set, indicates that this button will be the default when the return key is pressed.
            /// </summary>
            SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT = 0x00000001,

            /// <summary>
            /// If set, indicates that this button will be the default when the Escape key is pressed.
            /// </summary>
            SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT = 0x00000002
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INTERNAL_SDL_MessageBoxButtonData
        {
            public SDL_MessageBoxButtonFlags flags;
            public int buttonid;
            public IntPtr text; /* The UTF-8 button text */
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_MessageBoxButtonData
        {
            public SDL_MessageBoxButtonFlags flags;
            public int buttonid;
            public string text; /* The UTF-8 button text */
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_MessageBoxColor
        {
            public byte r, g, b;
        }

        public enum SDL_MessageBoxColorType
        {
            SDL_MESSAGEBOX_COLOR_BACKGROUND,
            SDL_MESSAGEBOX_COLOR_TEXT,
            SDL_MESSAGEBOX_COLOR_BUTTON_BORDER,
            SDL_MESSAGEBOX_COLOR_BUTTON_BACKGROUND,
            SDL_MESSAGEBOX_COLOR_BUTTON_SELECTED,
            SDL_MESSAGEBOX_COLOR_MAX
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_MessageBoxColorScheme
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = (int)SDL_MessageBoxColorType.SDL_MESSAGEBOX_COLOR_MAX)]
            public SDL_MessageBoxColor[] colors;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INTERNAL_SDL_MessageBoxData
        {
            public SDL_MessageBoxFlags flags;
            public IntPtr window;               /* Parent window, can be NULL */
            public IntPtr title;                /* UTF-8 title */
            public IntPtr message;              /* UTF-8 message text */
            public int numbuttons;
            public IntPtr buttons;
            public IntPtr colorScheme;          /* Can be NULL to use system settings */
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_MessageBoxData
        {
            public SDL_MessageBoxFlags flags;
            public IntPtr window;               /* Parent window, can be NULL */
            public string title;                /* UTF-8 title */
            public string message;              /* UTF-8 message text */
            public int numbuttons;
            public SDL_MessageBoxButtonData[] buttons;
            public SDL_MessageBoxColorScheme? colorScheme;  /* Can be NULL to use system settings */
        }

        [DllImport(nativeLibName, EntryPoint = "SDL_ShowMessageBox", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_SDL_ShowMessageBox([In()] ref INTERNAL_SDL_MessageBoxData messageboxdata, out int buttonid);

        /* Ripped from Jameson's LpUtf8StrMarshaler */
        private static IntPtr INTERNAL_AllocUTF8(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return IntPtr.Zero;
            }
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str + '\0');
            IntPtr mem = SDL.SDL_malloc((IntPtr)bytes.Length);
            Marshal.Copy(bytes, 0, mem, bytes.Length);
            return mem;
        }

        public static unsafe int SDL_ShowMessageBox([In()] ref SDL_MessageBoxData messageboxdata, out int buttonid)
        {
            var data = new INTERNAL_SDL_MessageBoxData()
            {
                flags = messageboxdata.flags,
                window = messageboxdata.window,
                title = INTERNAL_AllocUTF8(messageboxdata.title),
                message = INTERNAL_AllocUTF8(messageboxdata.message),
                numbuttons = messageboxdata.numbuttons,
            };

            var buttons = new INTERNAL_SDL_MessageBoxButtonData[messageboxdata.numbuttons];
            for (int i = 0; i < messageboxdata.numbuttons; i++)
            {
                buttons[i] = new INTERNAL_SDL_MessageBoxButtonData()
                {
                    flags = messageboxdata.buttons[i].flags,
                    buttonid = messageboxdata.buttons[i].buttonid,
                    text = INTERNAL_AllocUTF8(messageboxdata.buttons[i].text),
                };
            }

            if (messageboxdata.colorScheme != null)
            {
                data.colorScheme = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SDL_MessageBoxColorScheme)));
                Marshal.StructureToPtr(messageboxdata.colorScheme.Value, data.colorScheme, false);
            }

            int result;
            fixed (INTERNAL_SDL_MessageBoxButtonData* buttonsPtr = &buttons[0])
            {
                data.buttons = (IntPtr)buttonsPtr;
                result = INTERNAL_SDL_ShowMessageBox(ref data, out buttonid);
            }

            Marshal.FreeHGlobal(data.colorScheme);
            for (int i = 0; i < messageboxdata.numbuttons; i++)
            {
                SDL_free(buttons[i].text);
            }
            SDL_free(data.message);
            SDL_free(data.title);

            return result;
        }

        /* window refers to an SDL_Window* */
        [DllImport(nativeLibName, EntryPoint = "SDL_ShowSimpleMessageBox", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_SDL_ShowSimpleMessageBox(
            SDL_MessageBoxFlags flags,
            byte* title,
            byte* message,
            IntPtr window
        );
        public static unsafe int SDL_ShowSimpleMessageBox(
            SDL_MessageBoxFlags flags,
            string title,
            string message,
            IntPtr window
        )
        {
            int utf8TitleBufSize = Utf8Size(title);
            byte* utf8Title = stackalloc byte[utf8TitleBufSize];

            int utf8MessageBufSize = Utf8Size(message);
            byte* utf8Message = stackalloc byte[utf8MessageBufSize];

            return INTERNAL_SDL_ShowSimpleMessageBox(
                flags,
                Utf8Encode(title, utf8Title, utf8TitleBufSize),
                Utf8Encode(message, utf8Message, utf8MessageBufSize),
                window
            );
        }

        #endregion
    }
}
