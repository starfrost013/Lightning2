using System;
using System.Runtime.InteropServices;

namespace LightningBase
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_ListRec
    {
        public IntPtr head;
        public IntPtr tail;
    }
}
