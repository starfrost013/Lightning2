namespace LightningBase
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_ListNodeRec
    {
        public nint prev;
        public nint next;
        public nint data;
    }
}
