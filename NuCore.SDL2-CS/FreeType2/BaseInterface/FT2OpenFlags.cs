using System;

namespace NuCore.SDL2
{
    [Flags]
    public enum FT2OpenFlags
    {
        FT_OPEN_MEMORY = 0x1,

        FT_OPEN_STREAM = 0x2,

        FT_OPEN_PATHNAME = 0x4,

        FT_OPEN_DRIVER = 0x8,

        FT_OPEN_PARAMS = 0x10,
    }
}
