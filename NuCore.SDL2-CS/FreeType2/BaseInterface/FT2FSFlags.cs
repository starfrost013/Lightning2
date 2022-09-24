using System;

namespace LightningBase
{
    [Flags]
    public enum FT2FSFlags
    {
        FT_FSTYPE_INSTALLABLE_EMBEDDING = 0,

        FT_FSTYPE_RESTRICTED_LICENSE_EMBEDDING = 0x2,

        FT_FSTYPE_PREVIEW_AND_PRINT_EMBEDDING = 0x4,

        FT_FSTYPE_EDITABLE_EMBEDDING = 0x8,

        FT_FSTYPE_NO_SUBSETTING = 0x100,

        FT_FSTYPE_BITMAP_EMBEDDING_ONLY = 0x200
    }
}
