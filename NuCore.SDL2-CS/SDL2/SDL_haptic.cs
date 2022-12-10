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
        #region SDL_haptic.h

        /* SDL_HapticEffect type */
        public const ushort SDL_HAPTIC_CONSTANT = (1 << 0);
        public const ushort SDL_HAPTIC_SINE = (1 << 1);
        public const ushort SDL_HAPTIC_LEFTRIGHT = (1 << 2);
        public const ushort SDL_HAPTIC_TRIANGLE = (1 << 3);
        public const ushort SDL_HAPTIC_SAWTOOTHUP = (1 << 4);
        public const ushort SDL_HAPTIC_SAWTOOTHDOWN = (1 << 5);
        public const ushort SDL_HAPTIC_SPRING = (1 << 7);
        public const ushort SDL_HAPTIC_DAMPER = (1 << 8);
        public const ushort SDL_HAPTIC_INERTIA = (1 << 9);
        public const ushort SDL_HAPTIC_FRICTION = (1 << 10);
        public const ushort SDL_HAPTIC_CUSTOM = (1 << 11);
        public const ushort SDL_HAPTIC_GAIN = (1 << 12);
        public const ushort SDL_HAPTIC_AUTOCENTER = (1 << 13);
        public const ushort SDL_HAPTIC_STATUS = (1 << 14);
        public const ushort SDL_HAPTIC_PAUSE = (1 << 15);

        /* SDL_HapticDirection type */
        public const byte SDL_HAPTIC_POLAR = 0;
        public const byte SDL_HAPTIC_CARTESIAN = 1;
        public const byte SDL_HAPTIC_SPHERICAL = 2;
        public const byte SDL_HAPTIC_STEERING_AXIS = 3; /* Requires >= 2.0.14 */

        /* SDL_HapticRunEffect */
        public const uint SDL_HAPTIC_INFINITY = 4294967295U;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct SDL_HapticDirection
        {
            public byte type;
            public fixed int dir[3];
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_HapticConstant
        {
            // Header
            public ushort type;
            public SDL_HapticDirection direction;
            // Replay
            public uint length;
            public ushort delay;
            // Trigger
            public ushort button;
            public ushort interval;
            // Constant
            public short level;
            // Envelope
            public ushort attack_length;
            public ushort attack_level;
            public ushort fade_length;
            public ushort fade_level;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_HapticPeriodic
        {
            // Header
            public ushort type;
            public SDL_HapticDirection direction;
            // Replay
            public uint length;
            public ushort delay;
            // Trigger
            public ushort button;
            public ushort interval;
            // Periodic
            public ushort period;
            public short magnitude;
            public short offset;
            public ushort phase;
            // Envelope
            public ushort attack_length;
            public ushort attack_level;
            public ushort fade_length;
            public ushort fade_level;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct SDL_HapticCondition
        {
            // Header
            public ushort type;
            public SDL_HapticDirection direction;
            // Replay
            public uint length;
            public ushort delay;
            // Trigger
            public ushort button;
            public ushort interval;
            // Condition
            public fixed ushort right_sat[3];
            public fixed ushort left_sat[3];
            public fixed short right_coeff[3];
            public fixed short left_coeff[3];
            public fixed ushort deadband[3];
            public fixed short center[3];
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_HapticRamp
        {
            // Header
            public ushort type;
            public SDL_HapticDirection direction;
            // Replay
            public uint length;
            public ushort delay;
            // Trigger
            public ushort button;
            public ushort interval;
            // Ramp
            public short start;
            public short end;
            // Envelope
            public ushort attack_length;
            public ushort attack_level;
            public ushort fade_length;
            public ushort fade_level;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_HapticLeftRight
        {
            // Header
            public ushort type;
            // Replay
            public uint length;
            // Rumble
            public ushort large_magnitude;
            public ushort small_magnitude;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_HapticCustom
        {
            // Header
            public ushort type;
            public SDL_HapticDirection direction;
            // Replay
            public uint length;
            public ushort delay;
            // Trigger
            public ushort button;
            public ushort interval;
            // Custom
            public byte channels;
            public ushort period;
            public ushort samples;
            public nint data; // Uint16*
                                // Envelope
            public ushort attack_length;
            public ushort attack_level;
            public ushort fade_length;
            public ushort fade_level;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SDL_HapticEffect
        {
            [FieldOffset(0)]
            public ushort type;
            [FieldOffset(0)]
            public SDL_HapticConstant constant;
            [FieldOffset(0)]
            public SDL_HapticPeriodic periodic;
            [FieldOffset(0)]
            public SDL_HapticCondition condition;
            [FieldOffset(0)]
            public SDL_HapticRamp ramp;
            [FieldOffset(0)]
            public SDL_HapticLeftRight leftright;
            [FieldOffset(0)]
            public SDL_HapticCustom custom;
        }

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_HapticClose(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_HapticDestroyEffect(
            nint haptic,
            int effect
        );

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticEffectSupported(
            nint haptic,
            ref SDL_HapticEffect effect
        );

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticGetEffectStatus(
            nint haptic,
            int effect
        );

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticIndex(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, EntryPoint = "SDL_HapticName", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint INTERNAL_SDL_HapticName(int device_index);
        public static string SDL_HapticName(int device_index)
        {
            return UTF8_ToManaged(INTERNAL_SDL_HapticName(device_index));
        }

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticNewEffect(
            nint haptic,
            ref SDL_HapticEffect effect
        );

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticNumAxes(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticNumEffects(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticNumEffectsPlaying(nint haptic);

        /* nint refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_HapticOpen(int device_index);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticOpened(int device_index);

        /* nint refers to an SDL_Haptic*, joystick to an SDL_Joystick* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_HapticOpenFromJoystick(
            nint joystick
        );

        /* nint refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint SDL_HapticOpenFromMouse();

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticPause(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_HapticQuery(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRumbleInit(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRumblePlay(
            nint haptic,
            float strength,
            uint length
        );

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRumbleStop(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRumbleSupported(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticRunEffect(
            nint haptic,
            int effect,
            uint iterations
        );

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticSetAutocenter(
            nint haptic,
            int autocenter
        );

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticSetGain(
            nint haptic,
            int gain
        );

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticStopAll(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticStopEffect(
            nint haptic,
            int effect
        );

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticUnpause(nint haptic);

        /* haptic refers to an SDL_Haptic* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_HapticUpdateEffect(
            nint haptic,
            int effect,
            ref SDL_HapticEffect data
        );

        /* joystick refers to an SDL_Joystick* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickIsHaptic(nint joystick);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_MouseIsHaptic();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_NumHaptics();

        #endregion
    }
}
