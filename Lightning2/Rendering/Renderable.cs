namespace LightningGL
{
    /// <summary>
    /// Renderable
    /// 
    /// June 12, 2022
    /// 
    /// Defines a renderable object. Simply introduced to reduce code reuse.
    /// </summary>
    public abstract class Renderable
    {
        /// <summary>
        /// The position of this texture.
        /// Must be valid to draw.
        /// </summary>
        public virtual Vector2 Position { get; set; }

        /// <summary>
        /// Backing field for <see cref="RenderPosition"/>
        /// </summary>
        private Vector2 _renderPosition { get; set; }

        /// <summary>
        /// Internal value holding the position the item is actually rendered at.
        /// 
        /// HACK WARNING!
        /// </summary>
        internal Vector2 RenderPosition
        {
            get
            {
                // this MAY cause issues if the renderPosition is eve ractually (0,0)
                // will be fixed in GL version
                if (_renderPosition == default(Vector2)) return Position;
                return _renderPosition;

            }
            set
            {
                _renderPosition = value;
            }
        }

        /// <summary>
        /// The size of this texture. 
        /// Does not have to be equal to image size.
        /// </summary>
        public virtual Vector2 Size { get; set; }

        /// <summary>
        /// Determines if this Renderable will be drawn in world-relative space or camera-relative space.
        /// </summary>
        public bool SnapToScreen { get; set; }

        /// <summary>
        /// Determines if this Renderable has actually been loaded or not.
        /// </summary>
        internal bool Loaded { get; set; }

        /// <summary>
        /// List of animations in this Renderable
        /// </summary>
        public List<Animation> Animations { get; private set; }

        /// <summary>
        /// Animation timer.
        /// </summary>
        internal Stopwatch AnimationTimer { get; set; }

        internal bool AnimationRunning => AnimationTimer.IsRunning;

        /// <summary>
        /// Constructor for the Renderable class.
        /// </summary>
        public Renderable()
        {
            Animations = new List<Animation>();
        }

        /// <summary>
        /// Draws this Renderable.
        /// </summary>
        /// <param name="cWindow">The window to draw the renderable to.</param>
        public virtual void Draw(Window cWindow)
        {

        }
    }
}
