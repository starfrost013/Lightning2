using static NuCore.SDL2.SDL;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NuCore.SDL2
{
    /// <summary>
    /// Utf8Marshaling
    /// 
    /// Provies UTF8 marshaling services
    /// </summary>
    internal static class Utf8Marshaling
    {
        /* Used for stack allocated string marshaling. */
        internal static int Utf8Size(string str)
        {
            if (str == null) return 0;

            return (str.Length * 4) + 1;
        }
        internal static unsafe byte* Utf8Encode(string str, byte* buffer, int bufferSize)
        {
            if (str == null)
            {
                return (byte*)0;
            }
            fixed (char* strPtr = str)
            {
                Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
            }
            return buffer;
        }

        /* Used for heap allocated string marshaling.
		 * Returned byte* must be free'd with FreeHGlobal.
		 */
        internal static unsafe byte* Utf8EncodeHeap(string str)
        {
            if (str == null) return (byte*)0;

            int bufferSize = Utf8Size(str);
            byte* buffer = (byte*)Marshal.AllocHGlobal(bufferSize);
            fixed (char* strPtr = str)
            {
                Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
            }
            return buffer;
        }

        /* This is public because SDL_DropEvent needs it! */
        public static unsafe string UTF8_ToManaged(IntPtr s, bool freePtr = false)
        {
            if (s == IntPtr.Zero) return null;

            /* We get to do strlen ourselves! */
            byte* ptr = (byte*)s;
            while (*ptr != 0) ptr++;

            /* Modern C# lets you just send the byte*, nice! */

            // this has been made the default, as .NET 6.0 is way beyond .NET Standard 2.0
            // but doesn't define its ifdef, resulting in bleh code getting executed
            string result = Encoding.UTF8.GetString(
                (byte*)s,
                (int)(ptr - (byte*)s)
            );

            /* Some SDL functions will malloc, we have to free! */
            if (freePtr) SDL_free(s);
            return result;
        }
    }
}
