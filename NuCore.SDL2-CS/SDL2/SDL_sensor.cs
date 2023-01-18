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
        #region SDL_sensor.h

        /* This region is only available in 2.0.9 or higher. */

        public enum SDL_SensorType
        {
            SDL_SENSOR_INVALID = -1,
            SDL_SENSOR_UNKNOWN,
            SDL_SENSOR_ACCEL,
            SDL_SENSOR_GYRO
        }

        public const float SDL_STANDARD_GRAVITY = 9.80665f;

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_NumSensors();

        [DllImport(nativeLibName, EntryPoint = "SDL_SensorGetDeviceName", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_SensorGetDeviceName(int device_index);
        public static string SDL_SensorGetDeviceName(int device_index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_SensorGetDeviceName(device_index));
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_SensorType SDL_SensorGetDeviceType(int device_index);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SensorGetDeviceNonPortableType(int device_index);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_SensorGetDeviceInstanceID(int device_index);

        /* nint refers to an SDL_Sensor* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_SensorOpen(int device_index);

        /* nint refers to an SDL_Sensor* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_SensorFromInstanceID(
            Int32 instance_id
        );

        /* sensor refers to an SDL_Sensor* */
        [DllImport(nativeLibName, EntryPoint = "SDL_SensorGetName", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_SensorGetName(nint sensor);
        public static string SDL_SensorGetName(nint sensor)
        {
            return UTF8_ToManaged(INTERNAL_SDL_SensorGetName(sensor));
        }

        /* sensor refers to an SDL_Sensor* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_SensorType SDL_SensorGetType(nint sensor);

        /* sensor refers to an SDL_Sensor* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SensorGetNonPortableType(nint sensor);

        /* sensor refers to an SDL_Sensor* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_SensorGetInstanceID(nint sensor);

        /* sensor refers to an SDL_Sensor* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SensorGetData(
            nint sensor,
            float[] data,
            int num_values
        );

        /* sensor refers to an SDL_Sensor* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SensorGetDataWithTimestamp(
            nint sensor,
            long timestamp, // datetime.ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0)?
            float[] data,
            int num_values
        );

        /* sensor refers to an SDL_Sensor* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SensorClose(nint sensor);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SensorUpdate();

        /* Only available in 2.0.14 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_LockSensors();

        /* Only available in 2.0.14 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockSensors();

        #endregion
    }
}
