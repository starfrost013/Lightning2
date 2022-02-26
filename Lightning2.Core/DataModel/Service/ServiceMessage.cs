using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// ServiceMessage
    /// 
    /// July 20, 2021
    /// 
    /// Defines a service message. Service messages are one way and can be sent between classes.
    /// </summary>
    public class ServiceMessage
    {
        public string Name { get; set; }

        /// <summary>
        /// Data to send to the service.
        /// </summary>
        public List<object> Data { get; set; }
        
        public ServiceMessage()
        {
            Data = new List<object>();
        }
    }
}
