#if PROFILING

namespace LightningGL
{
    /// <summary>
    /// Enhanced profiling 
    /// </summary>
    internal static class ProfilingTimers
    {
        internal static Stopwatch Clear { get; set; }

        internal static Stopwatch EventHandling { get; set; }

        internal static Stopwatch RunScene { get; set; }

        internal static Stopwatch Order { get; set; }

        internal static Stopwatch Cull { get; set; }

        internal static Stopwatch UpdateRenderables { get; set; }

        internal static Stopwatch UpdateLightmap { get; set; }

        internal static Stopwatch Purge { get; set; }

        internal static Stopwatch UpdateCamera { get; set; }

        internal static Stopwatch Present { get; set; }

        static ProfilingTimers()
        {
            Clear = new Stopwatch();
            EventHandling = new Stopwatch();
            RunScene = new Stopwatch();  
            Order = new Stopwatch();
            Cull = new Stopwatch();
            UpdateRenderables = new Stopwatch();
            UpdateLightmap = new Stopwatch();
            Purge = new Stopwatch();
            UpdateCamera = new Stopwatch();
            Present = new Stopwatch();
        }

        internal static void EndOfFrame()
        {
            // Logging is turned off on profiling builds to increase perf so use console.writeline
            // 1000 to get milliseconds, not seconds
            Console.WriteLine($"#{Lightning.Renderer.FrameNumber} " +
                $"Clear={(double)Clear.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"Event={(double)EventHandling.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"RunScene={(double)RunScene.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"Order={(double)Order.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"Cull={(double)Cull.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"UR={(double)UpdateRenderables.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"UL={(double)UpdateLightmap.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"Purge={(double)Purge.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"Camera={(double)UpdateCamera.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"Present={(double)Present.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms " +
                $"T={(double)Lightning.Renderer.FrameTimer.ElapsedTicks * (1000d / (double)Stopwatch.Frequency):F2}ms"
                );

            // catch any errant timers
            Clear.Reset();
            EventHandling.Reset();
            RunScene.Reset();
            Order.Reset();
            Cull.Reset();
            UpdateRenderables.Reset();
            UpdateLightmap.Reset();
            Purge.Reset();
            UpdateCamera.Reset();
            Present.Reset();
        }
    }
}
#endif