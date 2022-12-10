using System;
using System.Runtime.InteropServices;
using FT_Pos = System.nint;

namespace LightningBase
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_BBox
    {
        public FT_Pos xMin, yMin;
        public FT_Pos xMax, yMax;
    }
}
