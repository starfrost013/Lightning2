﻿namespace LightningGL
{
    /// <summary>
    /// PerformanceProfiler
    /// 
    /// May 9, 2022
    /// 
    /// Defines a lightweight performance profiler for LightningGL.
    /// Dumps FPS information information to a CSV file.
    /// </summary>
    public static class PerformanceProfiler
    {
        /// <summary>
        /// The filename to use for outputting performance information.
        /// </summary>
        private static string? FileName { get; set; }

        /// <summary>
        /// The stream used for outputting performance information.
        /// </summary>
        private static StreamWriter? FileStream { get; set; }

        /// <summary>
        /// List of FPS values for the performance profiler.
        /// </summary>
        private static List<double> FPSList { get; set; }

        /// <summary>
        /// Indicates if the performance profiler has been initialised.
        /// </summary>
        private static bool Initialised { get; set; }

        /// <summary>
        /// The current 99.9th percentile FPS value.
        /// </summary>
        public static double Current999thPercentile { get; private set;  }
        /// <summary>
        /// The current 99th percentile FPS value.
        /// </summary>
        public static double Current99thPercentile { get; private set; }

        /// <summary>
        /// The current 95th percentile FPS value.
        /// </summary>
        public static double Current95thPercentile { get; private set; }

        /// <summary>
        /// The current average FPS value.
        /// </summary>
        public static double CurrentAverage { get; private set; }

        /// <summary>
        /// The current average 5th percentile value.
        /// </summary>
        public static double Current5thPercentile { get; private set; }

        /// <summary>
        /// The current average 1st percentile value.
        /// </summary>
        public static double Current1stPercentile { get; private set; }

        /// <summary>
        /// The current average 0.1th percentile value.
        /// </summary>
        public static double Current01thPercentile { get; private set; }

        /// <summary>
        /// Constructor for the performance profiler.
        /// </summary>
        static PerformanceProfiler()
        {
            FPSList = new List<double>();
        }

        /// <summary>
        /// Initialises the performance profiler.
        /// </summary>
        /// <exception cref="NCError">An error occurred initialising the performance profiler.</exception>
        internal static void Start()
        {
            DateTime now = DateTime.Now;
            FileName = $"Lightning-Perf-{now.ToString("yyyyMMdd_HHmmss")}.csv";

            try
            {
                FileStream = new StreamWriter(new FileStream(FileName, FileMode.OpenOrCreate));
                FileStream.WriteLine("Frame,FrametimeMs,Fps");
                Initialised = true;
            }
            catch (Exception ex)
            {
                Initialised = false;
                NCError.ShowErrorBox("An error occurred initialising performance profiler. Profiling will not be completed.", 70, "Exception occurred in PerformanceProfiler::Init", NCErrorSeverity.Warning, ex, true);
            }

            return;
        }

        /// <summary>
        /// Writes a new FPS and frametime to the performance profiler CSV.
        /// </summary>
        /// <param name="window">The window to measure the frametime and FPS of.</param>
        public static void Update(SdlRenderer window)
        {
            if (!Initialised
                || FileStream == null) return;

            FileStream.WriteLine($"{window.FrameNumber},{window.DeltaTime},{window.CurFPS}");

            FPSList.Add(window.CurFPS);

            // Calculate some pretty basic fps values if we have measured at least 1 frame.
            // Common stuff - 1st, 5th, 50th, 95th, 99th percentiles
            if (FPSList.Count > 0)
            {
                double total = 0;

                for (int i = 0; i < FPSList.Count; i++) total += FPSList[i];

                double average = 0;

                if (total > 0) average = total / FPSList.Count;
                int percentile999Index = (int)((FPSList.Count - 1) * 0.999);
                int percentile99Index = (int)((FPSList.Count - 1) * 0.99);
                int percentile95Index = (int)((FPSList.Count - 1) * 0.95);
                int percentile5Index = (int)((FPSList.Count - 1) * 0.05);
                int percentile1Index = (int)((FPSList.Count - 1) * 0.01);
                int percentile01Index = (int)((FPSList.Count - 1) * 0.001);

                Current999thPercentile = FPSList[percentile999Index];
                Current99thPercentile = FPSList[percentile99Index];  
                Current95thPercentile = FPSList[percentile95Index];
                CurrentAverage = average;
                Current5thPercentile = FPSList[percentile5Index];
                Current1stPercentile = FPSList[percentile1Index];
                Current01thPercentile = FPSList[percentile01Index];

                double percentile99 = FPSList[percentile99Index];
                double percentile95 = FPSList[percentile95Index];
                double percentile5 = FPSList[percentile5Index];
                double percentile1 = FPSList[percentile1Index];
                double percentile01 = FPSList[percentile01Index];

            }


        }

        /// <summary>
        /// Writes percentile information and shuts down the performance profiler. Run at shutdown.
        /// </summary>
        public static void Shutdown()
        {
            if (!Initialised
                || FileStream == null) return;

            FPSList.Sort();

            if (FPSList.Count > 0)
            {
                // Write some notable values.
                FileStream.WriteLine("Notable values:\n");
                FileStream.WriteLine($"Average={CurrentAverage}");
                FileStream.WriteLine($"99.9th%ile={Current999thPercentile:F1}");
                FileStream.WriteLine($"99th%ile={Current99thPercentile:F1}");
                FileStream.WriteLine($"95th%ile={Current95thPercentile:F1}");
                FileStream.WriteLine($"5th%ile={Current5thPercentile:F1}");
                FileStream.WriteLine($"1st%ile={Current1stPercentile:F1}");
                FileStream.WriteLine($"0.1st%ile={Current01thPercentile:F1}");
            }

            FileStream.Close();
            Initialised = false;
        }
    }
}