using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Non-DataModel
    /// 
    /// ServiceImportance enum (internal: only for SCM)
    /// 
    /// 2021-04-03
    /// 
    /// Determines the importance of a service.
    /// </summary>
    public enum ServiceImportance
    {
        /// <summary>
        /// This service can crash without being immediately reinitialised. An example of this is the SettingsService, as it only requires to be called when settings are changed.
        /// </summary>
        Low = 0,

        /// <summary>
        /// This service requires the engine to quit, restart, or exit the current game. An example of this is the NetworkReplicator for network-based Lightning games; if it fails, the current game must be exited. 
        /// </summary>
        High = 1

    }
}
