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
        /// If the value of <see cref="CameraType"/> is not set to <see cref="CameraType.Follow"/>, the camera will be offset by this amount when object positions are being 
        /// calculated relative to it.
        /// Otherwise it has no effect.
        /// </summary>
        public Vector2 FocusDelta { get; set; }

        /// <summary>
        /// Backing field for <see cref="Type"/>
        /// </summary>
        private CameraType _type;

        /// <summary>
        /// The type of this camera.
        /// See <see cref="CameraType"/>.
        /// </summary>
        public CameraType Type
        {
            get
            {
                if (FocusDelta == default)
                {
                    switch (_type)
                    {
                        default:
                            break;
                        case CameraType.Chase:
                            FocusDelta = new Vector2(-(GlobalSettings.ResolutionX / 2), 0);
                            break;
                        case CameraType.Floor:
                            FocusDelta = new Vector2(0, -(GlobalSettings.ResolutionY / 2));
                            break;
                    }
                    
                }

                return _type;
            }
            internal set
            {
                _type = value;
            }
        }

        public Vector2 CameraShakeAmount { get; set; } 

        public Camera(CameraType type)
        {
            Type = type;
        }
    }
}
