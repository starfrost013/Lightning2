using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Lightning Core
    /// 
    /// Instance Tagging Services
    /// 
    /// Holds attributes that are instance-wide and control how that instance behaves and its interactions with other components of Lightning.
    /// </summary>
    public enum InstanceTags
    {
        /// <summary>
        /// This instance is instantiable from ESX2.
        /// </summary>
        Instantiable = 1,

        /// <summary>
        /// This Instance is shown in the IDE.
        /// </summary>
        ShownInIDE = 2,

        /// <summary>
        /// This Instance is serialisable.
        /// </summary>
        Serialisable = 4,

        /// <summary>
        /// This instance is saveable.
        /// </summary>
        Archivable = 8,

        /// <summary>
        /// This instance is destroyable.
        /// </summary>
        Destroyable = 16,

        /// <summary>
        /// Not sure if we should use this?
        /// 
        /// Is shown in explorer
        /// </summary>
        ShownInProperties = 32,

        /// <summary>
        /// The Parent of this Instance is locked
        /// </summary>
        ParentLocked = 64,

        /// <summary>
        /// The Parent of this Instance can be null (can be on the bare DataModel) or outside of the Workspace.
        /// </summary>
        ParentCanBeNull = 128,

        /// <summary>
        /// Uses a custom, non-standard render path.
        /// 
        /// Render(); will not be called on this object if this is set.
        /// </summary>
        UsesCustomRenderPath = 256
    }
}
