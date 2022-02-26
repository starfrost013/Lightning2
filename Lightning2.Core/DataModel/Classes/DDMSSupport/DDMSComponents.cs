using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Dynamic Datamodel Serialiser (DDMS) 
    /// 
    /// 2021-03-15
    /// 
    /// Valid components for a DDMS-compliant file.
    /// </summary>
    public enum DDMSComponents
    {
        /// <summary>
        /// Component 0 - Metadata
        /// 
        /// Holds metadata about the DDMS file.
        /// </summary>
        Metadata = 0,

        /// <summary>
        /// Component 1 - Game Settings
        /// 
        /// Holds information about the game settings.
        /// </summary>
        Settings = 1,
        
        /// <summary>
        /// Component 2 - Instance Tree
        /// 
        /// Holds the instance tree of the game. 
        /// </summary>
        Workspace = 2,
    }
}
