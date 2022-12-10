using System;
using System.Runtime.InteropServices;

namespace LightningBase
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphRec
    {
        public FT_LibraryRec* library;
        public nint clazz;
        public FT_Glyph_Format format;
        public FT_Vector advance;
    }
}
