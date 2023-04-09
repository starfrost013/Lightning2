namespace LightningBase
{
    [Flags]
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
        /// AMD (before Ryzen 1000 series): This system supports the 3DNow! instruction set.
        /// </summary>
        ThreeDNow = 0x2,

        /// <summary>
        /// x86/64: This system supports the RDTSC instruction set.
        /// </summary>
        RDTSC = 0x4,

        /// <summary>
        /// x86/64: This system supports the SSE instruction set.
        /// </summary>
        SSE = 0x8,

        /// <summary>
        /// x86/64: This system supports the SSE2 instruction set.
        /// </summary>
        SSE2 = 0x10,

        /// <summary>
        /// x86/64: This system supports the SSE3 instruction set.
        /// </summary>
        SSE3 = 0x20,

        /// <summary>
        /// x86/64: This system supports the SSSE3 instruction set.
        /// </summary>
        SSSE3 = 0x40,

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
        /// x86/64: This system supports the FMA instruction set.
        /// </summary>
        FMA = 0x400,

        /// <summary>
        /// x86/64: This system supports the AVX2 instruction set.
        /// </summary>
        AVX2 = 0x800,

        /// <summary>
        /// x86/64: This system supports the AVX512 instruction set.
        /// </summary>
        AVX512 = 0x1000,

        /// <summary>
        /// x86/64: This system supports the AVX512 VNNI instruction set.
        /// </summary>
        AVXVNNI = 0x2000,

        /// <summary>
        /// x86/64: This system supports the BMI1 instruction set.
        /// </summary>
        BMI1 = 0x4000,

        /// <summary>
        /// x86/64: This system supports the BMI2 instruction set.
        /// </summary>
        BMI2 = 0x8000,

        /// <summary>
        /// x86/64: This system supports the POPCNT instruction.
        /// </summary>
        POPCNT = 0x10000,

        /// <summary>
        /// x86/64: This system supports the PCLMULQDQ instruction.
        /// </summary>
        PCLMULQDQ = 0x20000,

        /// <summary>
        /// x86/64: This system supports the PCLMULQDQ instruction.
        /// </summary>
        LZCNT = 0x40000,

        /// <summary>
        /// x86/64: This system supports the Intel AES instruction set.
        /// </summary>
        AES = 0x80000,

        /// <summary>
        /// ARM32: This system supports NEON.
        /// </summary>
        NEON = 0x100000,

        /// <summary>
        /// ARM32/64: This system supports ARM SIMD instructions.
        /// </summary>
        ARMSIMD = 0x200000,
    }
}
