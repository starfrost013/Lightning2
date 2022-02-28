#if WINDOWS
using NuCore.NativeInterop.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NuCore.NativeInterop.Win32
{
    [Obsolete("The legacy Win32-only, WPF-compatible messagebox API is deprecated.\nUse the platform-independent NuCore.Utilities.MessageBox API, which uses SDL messageboxes, instead")]
    /// <summary>
    /// A Win32 messagebox API. Uses P/Invoke.
    /// 
    /// Compatible with WPF API.
    /// </summary>
    public static class MessageBox
    {
        public static MessageBoxResult Show(string Text) => DoShow(Text, "", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxOptions.None); // null for caption?
        public static MessageBoxResult Show(IntPtr WindowHWND, string Text) => DoShow(Text, "", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxOptions.None, WindowHWND); // null for caption?
        public static MessageBoxResult Show(string Text, string Caption) => DoShow(Text, Caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxOptions.None); // null for caption?
        public static MessageBoxResult Show(IntPtr WindowHWND, string Text, string Caption) => DoShow(Text, Caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxOptions.None, WindowHWND); // null for caption?
        public static MessageBoxResult Show(string Text, string Caption, MessageBoxButton Button) => DoShow(Text, Caption, Button, MessageBoxImage.None, MessageBoxOptions.None); // null for caption?
        public static MessageBoxResult Show(IntPtr WindowHWND, string Text, string Caption, MessageBoxButton Button) => DoShow(Text, Caption, Button, MessageBoxImage.None, MessageBoxOptions.None, WindowHWND); // null for caption?
        public static MessageBoxResult Show(string Text, string Caption, MessageBoxButton Button, MessageBoxImage Image) => DoShow(Text, Caption, Button, Image, MessageBoxOptions.None); // null for caption?
        public static MessageBoxResult Show(IntPtr WindowHWND, string Text, string Caption, MessageBoxButton Button, MessageBoxImage Image) => DoShow(Text, Caption, Button, Image, MessageBoxOptions.None, WindowHWND); // null for caption?
        public static MessageBoxResult Show(string Text, string Caption, MessageBoxButton Button, MessageBoxImage Image, MessageBoxOptions Options) => DoShow(Text, Caption, Button, Image, Options); // null for caption?
        public static MessageBoxResult Show(IntPtr WindowHWND, string Text, string Caption, MessageBoxButton Button, MessageBoxImage Image, MessageBoxOptions Options) => DoShow(Text, Caption, Button, Image, Options, WindowHWND); // null for caption?

        /// <summary>
        /// Private: performs the action of showing the message box.
        /// </summary>
        /// <param name="Text">The text to be displayed in the message box.</param>
        /// <param name="Caption">The caption to be displayed within the message box.</param>
        /// <param name="ButtonSet">The buttons to use in the message box - see <see cref="MessageBoxButton"/> enum.</param>
        /// <param name="Image">The icon to use for the messagebox - see <see cref="MessageBoxImage"/> enum.</param>
        /// <param name="Options">The options for the messagebox - see <see cref="MessageBoxOptions"/> enum.</param>
        /// <param name="HWND">The Win32 window HWND to display on top of. OPTIONAL.</param>
        /// <returns>A <see cref="MessageBoxResult"/> object containing the option selected by the user.</returns>
        private static MessageBoxResult DoShow(string Text, string Caption, MessageBoxButton ButtonSet, MessageBoxImage Image, MessageBoxOptions Options, IntPtr? HWND = null)
        {
            uint ButtonType = (uint)ButtonSet;
            uint ImageType = (uint)Image;
            uint MBOptions = (uint)Options;

            uint FinalOptions = ButtonType + ImageType + MBOptions;

            // IntPtr? to get around bullshit

            if (HWND == null)
            {
                return (MessageBoxResult)NativeMethodsWin32.MessageBoxA(IntPtr.Zero, Text, Caption, (MessageBoxType)FinalOptions);
            }
            else
            {
                return (MessageBoxResult)NativeMethodsWin32.MessageBoxA((IntPtr)HWND, Text, Caption, (MessageBoxType)FinalOptions);
            }


        }
    }
}
#endif