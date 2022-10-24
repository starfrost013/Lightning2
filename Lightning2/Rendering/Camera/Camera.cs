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

        /// <summary>
        /// <para>If this value is above (0,0), camera shake will be implemented.
        /// The value of this property is the maximum displacement the camera will shake to.</para>
        /// Correction is applied to prevent the camera from drifting (this behaviour can be disabled with the <see cref="AllowCameraMoveOnShake"/> property).
        /// </summary>
        public Vector2 ShakeAmount { get; set; } 

        /// <summary>
        /// Turns off movement correction on camera shake. Has no effect if the <see cref="ShakeAmount"/> property is set to <code>0,0</code>.
        /// </summary>
        public bool AllowCameraMoveOnShake { get; set; }

        /// <summary>
        /// The velocity of this camera.
        /// </summary>
        public Vector2 Velocity { get; set; }

        public void Update()
        {
            // automatically takes care of position
            Position += Velocity;

            // check if camera shake not 0
            if (ShakeAmount != default)
            {
                // randomly generate values between -(CameraShakeAmount) and CameraShakeAmount
                float velChangeX = Random.Shared.NextSingle() * (ShakeAmount.X - -ShakeAmount.X) + -ShakeAmount.X,
                      velChangeY = Random.Shared.NextSingle() * (ShakeAmount.Y - -ShakeAmount.Y) + -ShakeAmount.Y;

                // make sure it doesn't actually move the camera around
                Vector2 newPosition = Position + new Vector2(velChangeX, velChangeY);

                float diffX = newPosition.X - Position.X,
                      diffY = newPosition.Y - Position.Y;

                if ((Math.Abs(diffX) > ShakeAmount.X
                    || Math.Abs(diffY) > ShakeAmount.Y)
                    && !AllowCameraMoveOnShake)
                {
                    float correctionX = diffX - ShakeAmount.X;
                    float correctionY = diffY - ShakeAmount.Y;
                    
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