using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Workspace
    /// 
    /// The run-time root of the DataModel
    /// This class literally only exists so that we can institute an incredibly ugly hack so that we can enforce
    /// class cohesion (children drive from class X, parent derives from class Y), and to allow functions and actions to be performed
    /// on all extant member sof the DataModel.
    /// 
    /// April 5, 2021
    /// </summary>
    public class Workspace : Instance
    {
        internal override string ClassName => "Workspace";
        internal override InstanceTags Attributes => InstanceTags.Archivable | InstanceTags.Instantiable | InstanceTags.ParentLocked | InstanceTags.ShownInIDE | InstanceTags.ParentCanBeNull | InstanceTags.ShownInProperties;
        
        
    }
}
