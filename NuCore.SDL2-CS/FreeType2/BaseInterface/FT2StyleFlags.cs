using System;

namespace LightningBase
{
    [Flags]
    public enum FT2StyleFlags
    {
        FT_STYLE_FLAG_ITALIC = 0x1,

        FT_STYLE_FLAG_BOLD = 0x2,

        #region Unofficial ones

        FT_STYLE_FLAG_UNDERLINE = 0x4,

        FT_STYLE_FLAG_STRIKETHROUGH = 0x8,

        #endregion
    }
}
