using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Lightning [DataModel API]
    /// 
    /// Service Root Class
    /// 
    /// Provides the root class for a service in the Lightning game engine.
    /// 
    /// A service is an instance that is running at all times and can be called on
    /// by any component of the DataModel current state at any time.
    /// 
    /// It can also be called from scripts using ESX2 GetService() method.
    /// </summary>
    public abstract class Service : Instance
    {
        internal override InstanceTags Attributes => InstanceTags.Instantiable | InstanceTags.Destroyable | InstanceTags.Archivable | InstanceTags.Serialisable | InstanceTags.ParentCanBeNull;
        public bool RunningNow { get; set; }
        internal abstract ServiceImportance Importance { get; }
        public abstract Result OnStart();

        internal int FailureCount { get; set; }

        /// <summary>
        /// Ran each frame. 
        /// </summary>
        public abstract void Poll();
        public abstract Result OnShutdown();
        public abstract void OnDataSent(ServiceMessage Data);
    }
}
