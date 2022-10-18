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
        /// <summary>
        /// Backing field for <see cref="Position"/>.
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// The position of this <see cref="Camera"/>.
        /// </summary>
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

        public bool AllowCameraMoveOnShake { get; set; }

        public Vector2 Velocity { get; set; }

        public void Update()
        {
            // automatically takes care of position
            Position += Velocity;

            // check if camera shake not 0
            if (CameraShakeAmount != default)
            {
                // randomly generate values between -(CameraShakeAmount) and CameraShakeAmount
                float velChangeX = Random.Shared.NextSingle() * (CameraShakeAmount.X - -CameraShakeAmount.X) + -CameraShakeAmount.X,
                      velChangeY = Random.Shared.NextSingle() * (CameraShakeAmount.Y - -CameraShakeAmount.Y) + -CameraShakeAmount.Y;

                // make sure it doesn't actually move the camera around
                Vector2 newPosition = Position + new Vector2(velChangeX, velChangeY);

                float diffX = newPosition.X - Position.X,
                      diffY = newPosition.Y - Position.Y;

                if ((Math.Abs(diffX) > CameraShakeAmount.X
                    || Math.Abs(diffY) > CameraShakeAmount.Y)
                    && !AllowCameraMoveOnShake)
                {
                    float correctionX = diffX - CameraShakeAmount.X;
                    float correctionY = diffY - CameraShakeAmount.Y;
                    
                    // correct so that the camera doesn't drift off
                    newPosition.X += correctionX;
                    newPosition.Y += correctionY;
                }

                Position = newPosition;
            }
        }

        public Camera(CameraType type)
        {
            Type = type;
        }
    }
}