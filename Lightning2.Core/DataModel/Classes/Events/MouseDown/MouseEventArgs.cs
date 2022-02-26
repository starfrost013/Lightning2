using NuRender.SDL2;
using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// ClickEventArgs
    /// 
    /// June 29, 2021
    /// 
    /// Event args for the UI click event.
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        /// <summary>
        /// The mouse button that has been clicked - see <see cref="MouseButton"/>.
        /// </summary>
        public MouseButton Button { get; set; }
        
        /// <summary>
        /// Number of times the button has been clicked. 
        /// </summary>
        public int ClickCount { get; set; }

        /// <summary>
        /// The relative position of the button. 
        /// </summary>
        public Vector2 RelativePosition { get; set; }

    }
}
