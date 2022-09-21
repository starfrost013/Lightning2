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

        /// <summary>
        /// Event handler for <see cref="KeyPressedEvent"/> event.
        /// </summary>
        public KeyPressedEvent OnKeyPressed { get; set; }

        /// <summary>
        /// Event handler for the <see cref="KeyReleasedEvent"/> event.
        /// </summary>
        public KeyReleasedEvent OnKeyReleased { get; set; }

        /// <summary>
        /// Event handler for the <see cref="MousePressedEvent"/> event.
        /// </summary>
        public MouseEvent OnMousePressed { get; set; }

        /// <summary>
        /// Event handler for the <see cref="MouseReleasedEvent"/> event.
        /// </summary>
        public MouseEvent OnMouseReleased { get; set; }

        /// <summary>
        /// Event handler for the mouse enter event.
        /// </summary>
        public GenericEvent OnMouseEnter { get; set; }

        /// <summary>
        /// Event handler for the mouse leave event.
        /// </summary>
        public GenericEvent OnMouseLeave { get; set; }

        /// <summary>
        /// Event handler for the focus gained event.
        /// </summary>
        public GenericEvent OnFocusGained { get; set; }

        /// <summary>
        /// Event handler for the focus lost event.
        /// </summary>
        public GenericEvent OnFocusLost { get; set; }

        /// <summary>
        /// Event handler for the <see cref="MouseEvent"/> event.
        /// </summary>
        public MouseEvent OnMouseMove { get; set; }

        /// <summary>
        /// Event handler for <see cref="RenderEvent"/> event.
        /// </summary>
        public RenderEvent OnRender { get; set; }

        /// <summary>
        /// Event handler for <see cref="ShutdownEvent"/> event.
        /// </summary>
        public ShutdownEvent OnShutdown { get; set; }

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
        /// The current animation of this Renderable
        /// </summary>
        public Animation CurrentAnimation { get; private set; }

        /// <summary>
        /// Animation timer.
        /// </summary>
        internal Stopwatch AnimationTimer { get; set; }

        /// <summary>
        /// Boolean determining if this animation is running automatically.
        /// </summary>
        internal bool AnimationRunning => AnimationTimer.IsRunning;

        public Renderable()
        {
            AnimationTimer = new Stopwatch();
        }

        /// <summary>
        /// Sets the animation of this Renderable. 
        /// An error will be raised if the animation is not loaded.
        /// </summary>
        /// <param name="animation"></param>
        public virtual void SetAnimation(Animation animation)
        {
            if (!animation.Loaded)
            {
                _ = new NCException("You must load an animation before attaching it to a renderable! The animation will not be set.", 150,
               "animation parameter to Renderable::SetAnimation's loaded property is FALSE", NCExceptionSeverity.Error);
                return;
            }

            CurrentAnimation = animation;
        }

        public virtual void StartCurrentAnimation() => CurrentAnimation.StartAnimationFor(this);

        public virtual void StopCurrentAnimation() => CurrentAnimation.StopAnimationFor(this);

        public virtual void UpdateCurrentAnimation() => CurrentAnimation.UpdateAnimationFor(this);
        /// <summary>
        /// The Z-Index (priority) of this renderable.
        /// </summary>
        public int ZIndex { get; set; }

        internal virtual void Load(Renderer cRenderer)
        {

        }

        /// <summary>
        /// Draws this Renderable.
        /// </summary>
        /// <param name="cRenderer">The window to draw the renderable to.</param>
        internal virtual void Draw(Renderer cRenderer)
        {
            // temp?
            
        }
    }
}
