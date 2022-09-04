using System;

namespace NuCore.SDL2
{
    [Flags]
    public enum FT2LoadFlags
    {
        FT_LOAD_DEFAULT = 0x1,

        FT_LOAD_NO_SCALE = 0x2,

        FT_LOAD_NO_HINTING = 0x4,

        FT_LOAD_RENDER = 0x8,

        FT_LOAD_NO_BITMAP = 0x10,

        FT_LOAD_VERTICAL_LAYOUT = 0x20,

        FT_LOAD_FORCE_AUTOHINT = 0x40,

        FT_LOAD_CROP_BITMAP = 0x80,

        FT_LOAD_PEDANTIC = 0x100,

        FT_LOAD_IGNORE_GLOBAL_ADVANCED_WIDTH = 0x200,

        FT_LOAD_NO_RECURSE = 0x400,

        FT_LOAD_IGNORE_TRANSFORM = 0x800,

        FT_LOAD_MONOCHROME = 0x1000,

        FT_LOAD_LINEAR_DESIGN = 0x2000,

        FT_LOAD_SBITS_ONLY = 0x4000,

        FT_LOAD_NO_AUTOHINT = 0x8000,

        FT_LOAD_COLOR = 0x80000,

        FT_LOAD_COMPUTE_METRICS = 0x100000,

        FT_LOAD_BITMAP_METRICS_ONLY = 0x200000
    }
}
