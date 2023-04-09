namespace LightningBase
{
    /// <summary>
    /// SystemInfoCPU
    /// 
    /// Provides system information (CPU section)
    /// </summary>
    public class SystemInfoCPU
    {
        /// <summary>
        /// Number of hardware threads on this CPU.
        /// </summary>
        public int Threads { get; private set; }

        /// <summary>
        /// The name of this CPU.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Processor architecture of this CPU
        /// </summary>
        public Architecture SystemArchitecture { get; private set; }

        /// <summary>
        /// Processor architecture of the engine.
        /// This may differ on Windows 11 and Rosetta2 for example
        /// (or if the engine ever has a wasm version)
        /// </summary>
        public Architecture ProcessArchitecture { get; private set; }

        /// <summary>
        /// Processor capabilities. See <see cref="SystemInfoCPUCapabilities"/>.
        /// </summary>
        public SystemInfoCPUCapabilities Capabilities { get; private set; }

        private readonly StringBuilder StringBuilder = new StringBuilder();

        [RequiresPreviewFeatures]
        /// <summary>
        /// Acquires CPU information
        /// </summary>
        internal SystemInfoCPU()
        {
            Logger.Log("Acquiring CPU information...");
            Threads = SDL_GetCPUCount();
            Logger.Log($"Number of hardware threads = {Threads}");

            // get process architecture and system architecture
            SystemArchitecture = RuntimeInformation.OSArchitecture;
            ProcessArchitecture = RuntimeInformation.ProcessArchitecture;

            Logger.Log($"{ProcessArchitecture} engine, running on {SystemArchitecture} system");

            // processor specific stuff
#if X64
            Logger.Log($"Using x86 CPU, so using CPUID intrinsics to get CPU name...");

            // -2147483648 = 0x8000000
            // for some reason this API uses an int, it should use uint 
            ValueTuple<int, int, int, int> regs = X86Base.CpuId(-2147483648, 0);

            if ((uint)regs.Item1 < 0x80000004) // CPUID Processor Brand Identification supported
            {
                Name = "***NOT AVAILABLE - X86 CPU TOO OLD***";
            }
            else
            {
                // 0x80000002-0x80000004
                // Processor Brand Identification
                ValueTuple<int, int, int, int> cpuNamePart1 = X86Base.CpuId(-2147483646, 0);
                ValueTuple<int, int, int, int> cpuNamePart2 = X86Base.CpuId(-2147483645, 0);
                ValueTuple<int, int, int, int> cpuNamePart3 = X86Base.CpuId(-2147483644, 0);

                // copy to string
                // default tostring not sufficient for our needs

                // and this is a valuetuple so...

                StringBuilder.Append((char)(cpuNamePart1.Item1 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item1 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item1 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item1 >> 24) & 0xFF));
                StringBuilder.Append((char)(cpuNamePart1.Item2 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item2 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item2 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item2 >> 24) & 0xFF));
                StringBuilder.Append((char)(cpuNamePart1.Item3 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item3 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item3 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item3 >> 24) & 0xFF));
                StringBuilder.Append((char)(cpuNamePart1.Item4 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item4 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item4 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart1.Item4 >> 24) & 0xFF));

                StringBuilder.Append((char)(cpuNamePart2.Item1 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item1 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item1 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item1 >> 24) & 0xFF));
                StringBuilder.Append((char)(cpuNamePart2.Item2 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item2 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item2 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item2 >> 24) & 0xFF));
                StringBuilder.Append((char)(cpuNamePart2.Item3 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item3 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item3 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item3 >> 24) & 0xFF));
                StringBuilder.Append((char)(cpuNamePart2.Item4 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item4 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item4 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart2.Item4 >> 24) & 0xFF));

                StringBuilder.Append((char)(cpuNamePart3.Item1 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item1 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item1 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item1 >> 24) & 0xFF));
                StringBuilder.Append((char)(cpuNamePart3.Item2 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item2 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item2 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item2 >> 24) & 0xFF));
                StringBuilder.Append((char)(cpuNamePart3.Item3 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item3 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item3 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item3 >> 24) & 0xFF));
                StringBuilder.Append((char)(cpuNamePart3.Item4 & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item4 >> 8) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item4 >> 16) & 0xFF));
                StringBuilder.Append((char)((cpuNamePart3.Item4 >> 24) & 0xFF));

                Name = StringBuilder.ToString();
            }
#else
            Name = "***NOT AVAILABLE ON ARM CPUs (no CPU ID capability in .NET)***";
#endif

            // filter out /0 characters (as CPUID puts C terminated strings into registers)
            Name = Name.Replace("\0", "");
            Name = Name.Trim();

            Logger.Log($"CPU Name: {Name}");
            // Detect instruction sets

            // only sdl can check for these
            if (SDL_HasMMX() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.MMX;
            if (SDL_Has3DNow() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.ThreeDNow;
            if (SDL_HasRDTSC() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.RDTSC;
            if (SDL_HasAVX512F() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.AVX512;
#if X64
            if (Sse.IsSupported) Capabilities |= SystemInfoCPUCapabilities.SSE;
            if (Sse2.IsSupported) Capabilities |= SystemInfoCPUCapabilities.SSE2;
            if (Sse3.IsSupported) Capabilities |= SystemInfoCPUCapabilities.SSE3;
            if (Ssse3.IsSupported) Capabilities |= SystemInfoCPUCapabilities.SSSE3;
            if (Sse41.IsSupported) Capabilities |= SystemInfoCPUCapabilities.SSE41;
            if (Sse42.IsSupported) Capabilities |= SystemInfoCPUCapabilities.SSE42;
            if (Avx.IsSupported) Capabilities |= SystemInfoCPUCapabilities.AVX;
            if (Avx2.IsSupported) Capabilities |= SystemInfoCPUCapabilities.AVX2;
            if (Fma.IsSupported) Capabilities |= SystemInfoCPUCapabilities.FMA;
            if (AvxVnni.IsSupported) Capabilities |= SystemInfoCPUCapabilities.AVXVNNI;
            if (Bmi1.IsSupported) Capabilities |= SystemInfoCPUCapabilities.BMI1;
            if (Bmi2.IsSupported) Capabilities |= SystemInfoCPUCapabilities.BMI2;
            if (Lzcnt.IsSupported) Capabilities |= SystemInfoCPUCapabilities.LZCNT;
            if (Popcnt.IsSupported) Capabilities |= SystemInfoCPUCapabilities.POPCNT;
            if (Pclmulqdq.IsSupported) Capabilities |= SystemInfoCPUCapabilities.PCLMULQDQ;
            if (Aes.IsSupported) Capabilities |= SystemInfoCPUCapabilities.AES;
#endif
            if (SDL_HasNEON() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.NEON;
            if (SDL_HasARMSIMD() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.ARMSIMD;

            Logger.Log("CPU Capabilities: ");

            Logger.Log(Capabilities.ToString());
        }
    }
}
