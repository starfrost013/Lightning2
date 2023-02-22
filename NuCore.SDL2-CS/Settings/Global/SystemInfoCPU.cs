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

            // Detect instruction sets
            Logger.Log("CPU Capabilities: ");
            if (SDL_HasMMX() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.MMX;
            if (SDL_Has3DNow() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.ThreeDNow;
            if (SDL_HasRDTSC() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.RDTSC;
            if (SDL_HasAltiVec() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.AltiVec;
            if (SDL_HasSSE() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.SSE;
            if (SDL_HasSSE2() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.SSE2;
            if (SDL_HasSSE3() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.SSE3;
            if (SDL_HasSSE41() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.SSE41;
            if (SDL_HasSSE42() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.SSE42;
            if (SDL_HasAVX() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.AVX;
            if (SDL_HasAVX2() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.AVX2;
            if (SDL_HasAVX512F() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.AVX512;
            if (SDL_HasNEON() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.NEON;
            if (SDL_HasARMSIMD() == SDL_bool.SDL_TRUE) Capabilities |= SystemInfoCPUCapabilities.ARMSIMD;

            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.MMX)) Logger.Log("MMX");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.ThreeDNow)) Logger.Log("3DNow!");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.RDTSC)) Logger.Log("RDTSC");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.AltiVec)) Logger.Log("AltiVec");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE)) Logger.Log("SSE");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE2)) Logger.Log("SSE2");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE3)) Logger.Log("SSE3");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE41)) Logger.Log("SSE4+SSE4.1");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE42)) Logger.Log("SSE4.2");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.AVX)) Logger.Log("AVX");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.AVX2)) Logger.Log("AVX2");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.AVX512)) Logger.Log("AVX512F");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.NEON)) Logger.Log("NEON");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.ARMSIMD)) Logger.Log("ARM SIMD");
        }
    }
}
