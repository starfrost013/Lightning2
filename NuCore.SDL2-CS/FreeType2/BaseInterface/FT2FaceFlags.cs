using System;

namespace NuCore.SDL2
{
    [Flags]
    public enum FT2FaceFlags
    {
        FT_FACE_FLAG_SCALABLE = 0x1,

        FT_FACE_FLAG_FIXED_SIZES = 0x2,

        FT_FACE_FLAG_FIXED_WIDTH = 0x4,

        FT_FACE_FLAG_SFNT = 0x8,

        FT_FACE_FLAG_HORIZONTAL = 0x10,

        FT_FACE_FLAG_VERTICAL = 0x20,   

        FT_FACE_FLAG_KERNING = 0x40,

        FT_FACE_FLAG_FAST_GLYPHS = 0x80,

        FT_FACE_FLAG_MULTIPLE_MASTERS = 0x100,

        FT_FACE_FLAG_GLYPH_NAMES = 0x200,

        FT_FACE_FLAG_EXTERNAL_STREAM = 0X400,

        FT_FACE_FLAG_HINTER = 0x800,

        FT_FACE_FLAG_CID_KEYED = 0x1000,

        FT_FACE_FLAG_TRICKY = 0x2000,

        FT_FACE_FLAG_COLOR = 0x4000,

        FT_FACE_FLAG_VARIATION = 0x8000,

        FT_FACE_FLAG_SVG = 0x10000,

        FT_FACE_FLAG_SBIX = 0x20000,
        
        FT_FACE_FLAG_SBIX_OVERLAY = 0x40000,
    }
}
