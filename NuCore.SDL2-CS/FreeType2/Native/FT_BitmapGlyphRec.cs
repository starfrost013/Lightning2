using System;
using System.Runtime.InteropServices;

namespace LightningBase.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_BitmapGlyphRec
    {
        public FT_GlyphRec root;
        public int left;
        public int top;
        public FT_Bitmap bitmap;
    }
}
