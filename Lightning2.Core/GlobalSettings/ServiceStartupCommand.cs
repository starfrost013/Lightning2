using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Lightning.Core
{
    /// <summary>
    /// Non-DataModel (Instance Core)
    /// 
    /// ServiceStartupCommand
    /// 
    /// 2021-04-03
    /// 
    /// Holds information for instructing the SCM to start a service at system boot time. 
    /// </summary>
    public class ServiceStartupCommand
    {

        /// <summary>
        /// The start order of this service.
        /// </summary>
        public int StartOrder { get; set; }

        /// <summary>
        /// The service name to be started.
        /// </summary>
        public string ServiceName { get; set; }
    }
}
