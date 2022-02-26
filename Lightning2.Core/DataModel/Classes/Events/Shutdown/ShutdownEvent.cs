using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// ShutdownEvent
    /// 
    /// July 19, 2021
    /// 
    /// Defines a delegate for an event triggered immediately before Lightning shuts down.
    /// </summary>
    /// <param name="Sender">The object that is sending the event - always <see cref="RenderService"/>.</param>
    /// <param name="EventArgs">Event arguments - see <see cref="ShutdownEventArgs"/></param>
    public delegate void ShutdownEvent
    (
        object Sender,
        ShutdownEventArgs EventArgs
    );
}
