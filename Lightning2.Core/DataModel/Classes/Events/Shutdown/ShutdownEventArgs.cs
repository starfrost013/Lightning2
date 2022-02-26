using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// ShutdownEventArgs
    /// 
    /// July 19, 2021
    /// 
    /// Defines event arguments for shutdown,
    /// </summary>
    public class ShutdownEventArgs : EventArgs
    {
        public bool Expected { get; set; }
    }
}
