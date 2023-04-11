﻿namespace LightningGL
{
    /// <summary>
    /// Renderable
    ///  
    /// Defines a renderable object.
    /// A renderable is an object in Lightning that does
    /// </summary>
    public abstract class Renderable
    {
        #region Event handler delegates

        [JsonIgnore]
        /// <summary>
        /// Event handler for <see cref="BindingKeyboardEvent"/> event.
        /// </summary>
        public BindingKeyboardEvent? OnKeyDown { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler fired when a key is released.
        /// </summary>
        public BindingKeyboardEvent? OnKeyUp { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler fired when the mouse is pressed.
        /// </summary>
        public BindingMouseEvent? OnMouseDown { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler fired when the mouse is released.
        /// </summary>
        public BindingMouseEvent? OnMouseUp { get; set; }

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
        /// Event handler for switching from a scene.
        /// </summary>
        public SceneEvent? OnSwitchFromScene { get; set; }

        /// <summary>
        /// Event handler for switching to a scene./
        /// </summary>
        [JsonIgnore]
        public SceneEvent? OnSwitchToScene { get; set; } 

        [JsonIgnore]
        /// <summary>
        /// Event handler for the mouse move event.
        /// </summary>
        public BindingMouseEvent? OnMouseMove { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the mouse wheel event.
        /// </summary>
        public BindingMouseEvent? OnMouseWheel { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for <see cref="RenderEvent"/> event.
        /// </summary>
        public GenericEvent OnRender { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the on-create event.
        /// </summary>
        public GenericEvent OnCreate { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the on-destroy event.
        /// </summary>
        public GenericEvent OnDestroy { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for the on-update event.
        /// </summary>
        public GenericEvent OnUpdate { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for when a new animation starts playing.
        /// </summary>
        public GenericEvent? OnAnimationStart { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for when a new animation stops playing.
        /// </summary>
        public GenericEvent? OnAnimationStop { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for <see cref="ShutdownEvent"/> event.
        /// </summary>
        public GenericEvent? OnShutdown { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for when a controller button starts being pressed.
        /// </summary>
        public BindingControllerEvent? OnControllerButtonDown { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Event handler for when a controller button stops being pressed.
        /// </summary>
        public BindingControllerEvent? OnControllerButtonUp { get; set; }

        #endregion

        /// <summary>
        /// Determines if the mouse is currently focused on this gadget or not.
        /// </summary>
        public bool Focused { get; internal set; }

        /// <summary>
        /// Determines if this renderable is allowed to receive events while unfocused.
        /// This only modifies the behaviour of events that require focus.
        /// </summary>
        public virtual bool CanReceiveEventsWhileUnfocused { get; set; }

        /// <summary>
        /// Determines if this Renderable will be drawn in world-relative space or camera-relative space.
        /// </summary>
        public virtual bool SnapToScreen { get; set; }

        /// <summary>
        /// The position of this texture.
        /// Must be valid to draw.
        /// </summary>
        public virtual Vector2 Position { get; set; }

        /// <summary>
        /// Determines if this renderable is cullable or not.
        /// </summary>
        public virtual bool NotCullable { get; set; }

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
        internal bool IsAnimating => AnimationTimer.IsRunning;

        /// <summary>
        /// Determines if this renderable is currently off-screen and therefore will not be rendered.
        /// </summary>
        internal bool IsOnScreen { get; set; }

        /// <summary>
        /// Determines if this Renderable is being rendered.
        /// This will always cull the renderable if it is set to <c>true</c>.
        /// </summary>
        public virtual bool IsNotRendering { get; set; }

        /// <summary>
        /// The Z-Index (priority) of this renderable.
        /// </summary>
        public virtual int ZIndex { get; set; }

        /// <summary>
        /// Backing field for <see cref="Name"/>.
        /// </summary>
        private string _name;

        /// <summary>
        /// The Name of this Renderable.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Logger.LogError("A Renderable must have a name!", 189, LoggerSeverity.FatalError);
                }
                else
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// The parent of this Renderable.
        /// </summary>
        public Renderable? Parent { get; internal set; } 

        /// <summary>
        /// The children of this Renderable.
        /// </summary>
        public List<Renderable> Children { get; internal set; }

        /// <summary>
        /// The components of this Renderable.
        /// </summary>
        public List<Component> Components { get; internal set; }    

        public Renderable(string name)
        {
            AnimationTimer = new Stopwatch();

            // Automatically render
            OnCreate += Create;
            OnRender += Draw;
            OnUpdate += Update;
            OnDestroy += Destroy;

            Name = name;
            _name = Name; // fix compile warnings
            Children = new List<Renderable>();
            Components = new List<Component>();
        }

        /// <summary>
        /// Sets the animation of this Renderable. 
        /// An error will be raised if the animation is not loaded.
        /// </summary>
        /// <param name="animation">The animation to set as the current animation.</param>
        public virtual void SetAnimation(Animation animation)
        {
            if (animation == null
                || !animation.Loaded)
            {
                Logger.LogError("You must load an animation before attaching it to a renderable! The animation will not be set.", 149, LoggerSeverity.Error);
                return;
            }

            CurrentAnimation = animation;
        }

        public virtual void StartCurrentAnimation()
        {
            if (CurrentAnimation == null
                || !CurrentAnimation.Loaded)
            {
                Logger.LogError("You must load an animation before playing it! The animation will not be set.", 151, LoggerSeverity.Error);
                return;
            }

            CurrentAnimation.StartAnimationFor(this);
        }

        public virtual void StopCurrentAnimation()
        {
            if (CurrentAnimation == null
                || !CurrentAnimation.Loaded)
            {
                return;
            }

            CurrentAnimation.StopAnimationFor(this);
        }

        /// <summary>
        /// Called when the renderable is created.
        /// </summary>
        public virtual void Create()
        {

        }

        /// <summary>
        /// Draws this Renderable.
        /// NOT RUN if the renderable is not rendered.
        /// </summary>
        public virtual void Draw()
        {
            // temp?
        }

        /// <summary>
        /// Draws components of this renderable.
        /// This is always called by the renderer.
        /// </summary>
        internal void DrawComponents()
        {
            foreach (var component in Components)
            {
                component.Draw();
            }
        }

        /// <summary>
        /// Draws components of this renderable.
        /// NOT RUN if the renderable is not rendered.
        /// </summary>
        internal void UpdateComponents()
        {
            foreach (var component in Components)
            {
                component.Update();
            }
        }


        /// <summary>
        /// Run each frame REGARDLESS of if the renderable is rendered or not.
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Called when the renderable is destroyed.
        /// </summary>
        public virtual void Destroy()
        {

        }

        public virtual Renderable? GetParent() => Parent;

        public virtual List<Renderable> GetChildren() => Children;

        public virtual Renderable? GetFirstChild()
        {
            if (Children.Count == 0)
            {
                Logger.LogError($"Tried to call Renderable::GetFirstChild on a renderable with no children!", 301, LoggerSeverity.Warning, null, true);
                return null;
            }

            return Children[0];
        }

        public virtual Renderable? GetLastChild()
        {
            if (Children.Count == 0)
            {
                Logger.LogError($"Tried to call Renderable::GetLastChild on a renderable with no children!", 192, LoggerSeverity.Warning, null, true);
                return null;
            }

            return Children[^1];
        }

        public virtual T AddComponent<T>() where T : Component, new()
        {
            T curT = new();

            Components.Add(curT);

            return curT;
        }
    }
}
