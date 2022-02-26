using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// GetMultiInstanceResult
    /// 
    /// May 5, 2021 (modified December 29, 2021: Add this comment block)
    /// 
    /// Defines a result class for multiple DataModel instances.
    /// </summary>
    public class GetMultiInstanceResult : Result
    {
        /// <summary>
        /// A list of instances that will be returned by a method using this class.
        /// </summary>
        public List<Instance> Instances { get; set; }

        /// <summary>
        /// Constructor of the <see cref="GetMultiInstanceResult"/> class.
        /// </summary>
        public GetMultiInstanceResult()
        {
            Instances = new List<Instance>(); 
        }
    }
}
