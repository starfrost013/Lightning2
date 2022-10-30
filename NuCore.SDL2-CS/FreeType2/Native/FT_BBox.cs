using System;
using System.Runtime.InteropServices;
using FT_Pos = System.IntPtr;

namespace LightningBase.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_BBox
    {
        public FT_Pos xMin, yMin;
        public FT_Pos xMax, yMax;
    }
}
