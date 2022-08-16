﻿namespace LightningGL
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
        /// Chase camera - focuses behind the target position by <see cref="Camera.FocusDelta"/>, or -(Settings.Size.X / 2) pixels behind the target position if not set.
        /// </summary>
        Chase = 1
    }
}
