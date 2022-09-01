using System;

namespace NuCore.SDL2
{
    [Flags]
    public enum FT2LoadTargetFlags
    {
        FT_LOAD_TARGET_NORMAL = 0,

        FT_LOAD_TARGET_LIGHT = 0x10000,

        FT_LOAD_TARGET_MONO = 0x20000,

        FT_LOAD_TARGET_LCD = 0x30000,

        FT_LOAD_TARGET_LCD_V = 0x40000,
    }
}
