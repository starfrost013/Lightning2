using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// OnObjectCreated
    /// 
    /// August 19, 2021
    /// 
    /// Defines an event called at the moment an object is created. (runs within OnCreate())
    /// This event can only be called by calling DataModel.CreateInstance with an event handler as there is no realistic proposition for an event handler to be assigned before
    /// OnCreate() is called in the DDMS.
    /// </summary>
    /// <param name="Sender"></param>
    public delegate void ObjectCreated
    (
        object Sender
    );
}
