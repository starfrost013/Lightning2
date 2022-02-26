using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    public class InstanceInfoResult : Result 
    {
        public InstanceInfo InstanceInformation { get; set; }

        public InstanceInfoResult()
        {
            InstanceInformation = new InstanceInfo();
        }

    }
}
