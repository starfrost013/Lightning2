namespace LightningGL
{
    /// <summary>
    /// SystemInfoCPUCapabilities
    /// </summary>
    public enum SystemInfoCPUCapabilities
    {
        /// <summary>
        /// x86/64: This system supports the MMX instruction set.
        /// </summary>
        MMX = 0x1,

        /// <summary>
        /// AMD (before Bulldozer series): This system supports the 3DNow! instruction set.
        /// </summary>
        ThreeDNow = 0x2,

        /// <summary>
        /// x86/64: This system supports the RDTSC instruction set.
        /// </summary>
        RDTSC = 0x4,

        /// <summary>
        /// This system supports the AltiVec instruction set.
        /// 
        /// This is only used in PPC so should always be 0. This is for the sake of completeness,
        /// as SDL still implements it.
        /// </summary>
        AltiVec = 0x8,

        /// <summary>
        /// x86/64: This system supports the SSE instruction set.
        /// </summary>
        SSE = 0x10,

        /// <summary>
        /// x86/64: This system supports the SSE2 instruction set.
        /// </summary>
        SSE2 = 0x20,

        /// <summary>
        /// x86/64: This system supports the SSE3 instruction set.
        /// </summary>
        SSE3 = 0x40,

        /// <summary>
        /// x86/64: This system supports the SSE4 & SSE4.1 instruction set.
        /// </summary>
        SSE41 = 0x80,

        /// <summary>
        /// x86/64: This system supports the SSE4.2 instruction set.
        /// </summary>
        SSE42 = 0x100,

        /// <summary>
        /// x86/64: This system supports the AVX instruction set.
        /// </summary>
        AVX = 0x200,

        /// <summary>
        /// x86/64: This system supports the AVX2 instruction set.
        /// </summary>
        AVX2 = 0x400,

        /// <summary>
        /// x86/64 (Intel only): This system supports the AVX512 instruction set.
        /// </summary>
        AVX512 = 0x800,

        /// <summary>
        /// ARM32: This system supports NEON.
        /// </summary>
        NEON = 0x1000,

        /// <summary>
        /// ARM32/64: This system supports ARM SIMD instructions.
        /// </summary>
        ARMSIMD = 0x2000
    }
}
