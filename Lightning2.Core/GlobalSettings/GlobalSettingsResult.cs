using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core
{
    /// <summary>
    /// Result class for acquiring GlobalSettings.
    /// </summary>
    public class GlobalSettingsResult : Result
    {
        /// <summary>
        /// Assists in debugging.
        /// </summary>
        public Exception BaseException { get; set; }

        /// <summary>
        /// The global settings returned by the method that returns an instance of this result class.
        /// </summary>
        public GlobalSettings Settings { get; set; }
    }
}
