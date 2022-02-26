using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// KeyEventArgs
    /// 
    /// July 21, 2021 (modified August 20, 2021: Inherit from EventArgs)
    /// 
    /// Defines event arguments for the <see cref="KeyDownEvent"/>.
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        /// <summary>
        /// Defines the pressed key - see <see cref="Control"/>.
        /// </summary>
        public Control Key { get; set; }

        /// <summary>
        /// Determines if the key has been repeated.
        /// </summary>
        public bool Repeat { get; set; }

        /// <summary>
        /// If <see cref="Repeat"/> is true, the number of times the key stored in <see cref="Key"/> has been pressed. Otherwise 0.
        /// 
        /// Sorry it's a byte, blame SDL
        /// </summary>
        public byte RepeatCount { get; set; }
    }
}
