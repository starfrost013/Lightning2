using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// ObjectDestroyed
    /// 
    /// August 20, 2021
    /// 
    /// Event called on an object immediately before its destruction.
    /// </summary>
    public delegate void ObjectDestroyed
    (
        object Sender
    );
}
