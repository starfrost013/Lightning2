using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics; 
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// Camera
    /// 
    /// April 2, 2022
    /// 
    /// Defines a camera
    /// </summary>
    public class Camera
    {
        private Vector2 _position { get; set; }

        public Vector2 Position
        {
            get 
            {
                // default(Vector2) is (0,0), so if it has not been set it will add nothing by default, therefore you can do this in one line of code.
                return _position + FocusDelta;
            }
            set
            {
                _position = (value - FocusDelta); 
            }
        }

        /// <summary>
        /// If <see cref="CameraType"/> is set to <see cref="Chase"/>, the camera will be offset by this amount when object positions are being calculated relative to it.
        /// Otherwise it has no effect.
        /// </summary>
        public Vector2 FocusDelta { get; set; }

        public CameraType Type { get; internal set; }

        public Camera(CameraType new_type)
        {
            Type = new_type; 
        }
    }
}
