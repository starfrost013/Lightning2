using FT_Pos = System.IntPtr;

namespace LightningBase
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Bitmap_Size
    {
        public short height;
        public short width;

        public FT_Pos size;

        public FT_Pos x_ppem;
        public FT_Pos y_ppem;
    }
}
