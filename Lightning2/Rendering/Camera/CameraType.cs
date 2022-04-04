using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// CameraType
    /// 
    /// April 3, 2022
    /// 
    /// Enumerates valid camera types.
    /// </summary>
    public enum CameraType
    {
        /// <summary>
        /// Follow camera - exactly focuses on the target position.
        /// </summary>
        Follow = 0,

        /// <summary>
        /// Chase camera - focuses behind the target position by <see cref="Camera.FocusDelta"/>, or (windowwidth-100) pixels behind the target position if not set.
        /// </summary>
        Chase = 1
    }
}
