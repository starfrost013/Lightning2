using System;
using System.Runtime.InteropServices;
using FT_Pos = System.IntPtr;

namespace LightningBase
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Outline_Funcs
    {
        public nint moveTo;
        public nint lineTo;
        public nint conicTo;
        public nint cubicTo;
        public int shift;
        public FT_Pos delta;
    }
}