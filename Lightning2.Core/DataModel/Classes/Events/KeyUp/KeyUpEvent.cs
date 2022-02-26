using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{   
    
    /// <summary>
    /// Event fired when a key is released.
    /// </summary>
    /// <param name="Sender">The DataModel object that sent the event.</param>
    /// <param name="EventArgs">The arguments for this event - see <see cref="KeyUpEvent"/></param>
    public delegate void KeyUpEvent
    (
        object Sender,
        KeyEventArgs EventArgs
    );
}
