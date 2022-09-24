using System;
namespace LightningBase
{
    [Flags]
    public enum FT2SubglyphFlags
    {
        FT_SUBGLYPH_FLAG_ARGS_ARE_WORDS = 0x1,

        FT_SUBGLYPH_FLAG_ARGS_ARE_XY_VALUES = 0x2,

        FT_SUBGLYPH_FLAG_ROUND_XY_TO_GRID = 0x4,

        FT_SUBGLYPH_FLAG_SCALE = 0x8,

        FT_SUBGLYPH_FLAG_XY_SCALE = 0x40,

        FT_SUBGLYPH_FLAG_2X2 = 0x80,

        FT_SUBGLYPH_FLAG_USE_MY_METRICS = 0x200
    }
}
