using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// ClickEvent
    /// 
    /// June 29, 2021
    /// 
    /// Defines a click event for UI.
    /// </summary>
    public delegate void MouseDownEvent
    (
        object Sender,
        MouseEventArgs EventArgs
    );
}
