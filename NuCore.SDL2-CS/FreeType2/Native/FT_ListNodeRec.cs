using System;
using System.Runtime.InteropServices;

namespace LightningBase
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_ListNodeRec
    {
        public IntPtr prev;
        public IntPtr next;
        public IntPtr data;
    }
}
