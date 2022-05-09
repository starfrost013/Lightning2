using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lightning2
{
    /// <summary>
    /// PerformanceProfiler
    /// 
    /// May 9, 2022
    /// 
    /// Defines a lightweight performance profiler for Lightning2.
    /// Dumps FPS information information to a CSV file.
    /// </summary>
    public static class PerformanceProfiler
    {
        private static string FileName { get; set; }

        private static StreamWriter FileStream { get; set; }

        private static List<double> FPSList { get; set; }

        private static bool Initialised { get; set; }

        static PerformanceProfiler()
        {
            FPSList = new List<double>();
        }

        public static void Init()
        {
            DateTime now  = DateTime.Now;
            FileName = $"Lightning2-Perf-{now.ToString("yyyyMMdd_HHmmss")}.csv";

            try
            {
                FileStream = new StreamWriter(new FileStream(FileName, FileMode.OpenOrCreate));
                FileStream.WriteLine("Frame,FrametimeMS,Fps");
                Initialised = true;
            }
            catch (Exception ex)
            {
                Initialised = false; 
                throw new NCException("An error occurred initialising performance profiler. Profiling will not be completed.", 70, "Exception occurred in PerformanceProfiler::Init", NCExceptionSeverity.Warning, ex, true);
            }

            return;
        }

        public static void Update(Window window)
        {
            if (!Initialised) return;
            FileStream.WriteLine($"{window.FrameNumber},{window.LastFrameTime},{window.CurFPS}");
            FPSList.Add(window.CurFPS);
        }

        public static void Shutdown()
        {
            if (!Initialised) return;

            FPSList.Sort();

            if (FPSList.Count > 0)
            {
                // Calculate some pretty basic fps values if we have measured at least 1 frame.
                // Common stuff - 1st, 5th, 50th, 95th, 99th percentiles
                double total = 0;

                for (int i = 0; i < FPSList.Count; i++) total += FPSList[i];

                double average = 0;

                if (total > 0) average = total / FPSList.Count;
                int onemaxIndex = (int)((FPSList.Count - 1) * 0.99);
                int fivemaxIndex = (int)((FPSList.Count - 1) * 0.95);
                int fiveminIndex = (int)((FPSList.Count - 1) * 0.05);
                int oneminIndex = (int)((FPSList.Count - 1) * 0.01);

                double onemax = FPSList[onemaxIndex];
                double fivemax = FPSList[fivemaxIndex];
                double fivemin = FPSList[fiveminIndex];
                double onemin = FPSList[oneminIndex];

                FileStream.WriteLine($"Average={average}");
                FileStream.WriteLine($"99th%ile={onemax.ToString("F1")}");
                FileStream.WriteLine($"95th%ile={fivemax.ToString("F1")}");
                FileStream.WriteLine($"5th%ile={fivemin.ToString("F1")}");
                FileStream.WriteLine($"1st%ile={onemin.ToString("F1")}");

            }

            FileStream.Close();
            Initialised = false;
        }
    }
}
