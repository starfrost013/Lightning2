using System;
using System.Runtime.InteropServices;

namespace LightningBase
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Matrix
    {
        public nint xx, xy;
        public nint yx, yy;
    }
}
