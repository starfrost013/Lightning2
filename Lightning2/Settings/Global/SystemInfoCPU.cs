using System.Runtime.InteropServices;

namespace LightningGL
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
            NCLogging.Log("Acquiring CPU information...");
            Threads = SDL_GetCPUCount();
            NCLogging.Log($"Number of hardware threads = {Threads}");

            // get process architecture and system architecture
            SystemArchitecture = RuntimeInformation.OSArchitecture;
            ProcessArchitecture = RuntimeInformation.ProcessArchitecture;

            NCLogging.Log($"{ProcessArchitecture} engine, running on {SystemArchitecture} system");

            // Detect instruction sets
            NCLogging.Log("CPU Capabilities: ");
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

            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.MMX)) NCLogging.Log("MMX");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.ThreeDNow)) NCLogging.Log("3DNow!");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.RDTSC)) NCLogging.Log("RDTSC");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.AltiVec)) NCLogging.Log("AltiVec");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE)) NCLogging.Log("SSE");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE2)) NCLogging.Log("SSE2");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE3)) NCLogging.Log("SSE3");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE41)) NCLogging.Log("SSE4+SSE4.1");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.SSE42)) NCLogging.Log("SSE4.2");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.AVX)) NCLogging.Log("AVX");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.AVX2)) NCLogging.Log("AVX2");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.AVX512)) NCLogging.Log("AVX512F");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.NEON)) NCLogging.Log("NEON");
            if (Capabilities.HasFlag(SystemInfoCPUCapabilities.ARMSIMD)) NCLogging.Log("ARM SIMD");
        }
    }
}
