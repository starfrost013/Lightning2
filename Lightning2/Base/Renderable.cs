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
        #region Event handlers

        [JsonIgnore]
        /// <summary>
        /// Event handler for <see cref="KeyPressedEvent"/> event.
        /// </summary>
        public KeyPressedEvent? OnKeyPressed { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the <see cref="KeyReleasedEvent"/> event.
        /// </summary>
        public KeyReleasedEvent? OnKeyReleased { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the <see cref="MousePressedEvent"/> event.
        /// </summary>
        public MouseEvent? OnMousePressed { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the <see cref="MouseReleasedEvent"/> event.
        /// </summary>
        public MouseEvent? OnMouseReleased { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the mouse enter event.
        /// </summary>
        public GenericEvent? OnMouseEnter { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the mouse leave event.
        /// </summary>
        public GenericEvent? OnMouseLeave { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the focus gained event.
        /// </summary>
        public GenericEvent? OnFocusGained { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the focus lost event.
        /// </summary>
        public GenericEvent? OnFocusLost { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the <see cref="MouseEvent"/> event.
        /// </summary>
        public MouseEvent? OnMouseMove { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for <see cref="RenderEvent"/> event.
        /// </summary>
        public RenderEvent OnRender { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for <see cref="ShutdownEvent"/> event.
        /// </summary>
        public ShutdownEvent? OnShutdown { get; set; }

        /// <summary>
        /// Determines if the mouse is currently focused on this gadget or not.
        /// </summary>
        public bool Focused { get; internal set; }

        #endregion

        /// <summary>
        /// The position of this texture.
        /// Must be valid to draw.
        /// </summary>
        public virtual Vector2 Position { get; set; }

        /// <summary>
        /// Backing field for <see cref="RenderPosition"/>
        /// </summary>
        private Vector2 _renderPosition;

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
                if (_renderPosition == default) return Position;
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

        [JsonIgnore]
        /// <summary>
        /// The current animation of this Renderable
        /// </summary>
        public Animation? CurrentAnimation { get; private set; }

        /// <summary>
        /// Animation timer.
        /// </summary>
        internal Stopwatch AnimationTimer { get; set; }

        /// <summary>
        /// Boolean determining if this animation is running automatically.
        /// </summary>
        internal bool AnimationRunning => AnimationTimer.IsRunning;

        /// <summary>
        /// Determines if this renderable is currently off-screen and therefore will not be rendered.
        /// </summary>
        internal bool IsOnScreen { get; set; }

        /// <summary>
        /// The Z-Index (priority) of this renderable.
        /// </summary>
        public int ZIndex { get; set; }

        public Renderable()
        {
            AnimationTimer = new Stopwatch();

            // Automatically render
            OnRender += Draw;
        }

        /// <summary>
        /// Sets the animation of this Renderable. 
        /// An error will be raised if the animation is not loaded.
        /// </summary>
        /// <param name="animation"></param>
        public virtual void SetAnimation(Animation animation)
        {
            if (animation == null
                || !animation.Loaded)
            {
                _ = new NCException("You must load an animation before attaching it to a renderable! The animation will not be set.", 149,
               "animation parameter to Renderable::SetAnimation's loaded property is FALSE", NCExceptionSeverity.Error);
                return;
            }

            CurrentAnimation = animation;
        }

        public virtual void StartCurrentAnimation()
        {
            if (CurrentAnimation == null
                || !CurrentAnimation.Loaded)
            {
                _ = new NCException("You must load an animation before playing it! The animation will not be set.", 151,
                "Renderable::StartCurrentAnimation called when CurrentAnimation::Loaded property is FALSE or it was never set.", NCExceptionSeverity.Error);
                return;
            }

            CurrentAnimation.StartAnimationFor(this);
        }

        public virtual void StopCurrentAnimation()
        {
            if (CurrentAnimation == null
                || !CurrentAnimation.Loaded)
            {
                _ = new NCException("You must load an animation before playing it! The animation will not be set.", 152,
                "Renderable::StopCurrentAnimation called when CurrentAnimation::Loaded property is FALSE or it was never set.", NCExceptionSeverity.Error);
                return;
            }

            CurrentAnimation.StopAnimationFor(this);
        } 


        internal virtual void Load(Renderer cRenderer)
        {

        }

        /// <summary>
        /// Called when the renderable is created.
        /// </summary>
        /// <param name="cRenderer">The renderer to draw the renderable to.</param>
        internal virtual void Create(Renderer cRenderer)
        {

        }

        /// <summary>
        /// Draws this Renderable.
        /// </summary>
        /// <param name="cRenderer">The renderer to draw the renderable to.</param>
        internal virtual void Draw(Renderer cRenderer)
        {
            // temp?
        }
    }
}
