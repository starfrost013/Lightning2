using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// GetInstanceResult
    /// 
    /// March 21, 2021 (modified December 29, 2021: Improve comments)
    /// 
    /// Defines a reuslt class for acquiring a single instance.
    /// </summary>
    public class GetInstanceResult : Result 
    {
        /// <summary>
        /// The instance; polymorphically will be the type we want
        /// </summary>
        public object Instance { get; set; }

    }
}
