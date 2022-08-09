using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// Camera
    /// 
    /// April 2, 2022 (modified June 12, 2022: Add renderable)
    /// 
    /// Defines a camera
    /// </summary>
    public class Camera : Renderable
    {
        private Vector2 _position { get; set; }

        public override Vector2 Position
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

        /// <summary>
        /// The type of this camera.
        /// See <see cref="CameraType"/>.
        /// </summary>
        public CameraType Type { get; internal set; }

        public Camera(CameraType type)
        {
            Type = type;
        }
    }
}
