using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Result class for DataModel instantiation.
    /// </summary>
    public class InstantiationResult : Result
    {
        public object Instance { get; set; }
    }
}
