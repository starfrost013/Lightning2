using System;
using System.Runtime.InteropServices;

namespace LightningBase
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphLoaderRec
    {
        public nint memory;
        public uint max_points;
        public uint max_contours;
        public uint max_subglyphs;
        public bool use_extra;

        public FT_GlyphLoadRec @base;
        public FT_GlyphLoadRec current;

        public nint other;
    }
}
