using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// MouseLeaveEvent
    /// 
    /// July 19, 2021
    /// 
    /// Defines an event to be triggered on the mouse leaving the Lightning window.
    /// </summary>
    /// <param name="Sender">The sender of this object - always <see cref="RenderService"/></param>
    /// <param name="EventArguments">Event arguments - there are none.</param>
    public delegate void MouseLeaveEvent
    (
        object Sender,
        EventArgs EventArguments
    );
}
