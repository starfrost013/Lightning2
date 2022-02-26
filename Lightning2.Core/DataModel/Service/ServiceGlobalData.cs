using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Lightning.Core.API
{
    /// <summary>
    /// Global data required across all Services.
    /// </summary>
    public class ServiceGlobalData
    {
        public Stopwatch ServiceUpdateTimer { get; set; }

#if DEBUG
        /// <summary>
        /// TEMPORARY
        /// 
        /// A frame counter
        /// </summary>
        internal long StopwatchMsAtLastFPSCheck { get; set; }

        internal int FrameCount { get; set; }
        /// <summary>
        /// TEMPORARY
        /// 
        /// FPS counter for performance test build (574 / Pre-TP2)
        /// </summary>
        internal double FPS { get; set; }
#endif

    }
}
