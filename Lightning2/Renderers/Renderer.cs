namespace LightningGL
{
    /// <summary>
    /// Renderer
    /// 
    /// Base class for all renderers.
    /// </summary>
    public abstract class Renderer
    {
        /// <summary>
        /// Composed renderable list
        /// </summary>
        internal List<Renderable> Renderables = new();

        /// <summary>
        /// The settings of this window - see <see cref="RendererSettings"/>.
        /// </summary>
        public virtual RendererSettings Settings { get; protected set; }

        /// <summary>
        /// Private: The time the current frame took. Used to measure FPS.
        /// </summary>
        protected long ThisTime { get; set; }

        /// <summary>
        /// Delta-time used for ensuring objects move at the same speed regardless of framerate.
        /// </summary>
        public double DeltaTime { get; protected set; }

        /// <summary>
        /// Private: Frame-timer used for measuring frametime.
        /// </summary>
        protected Stopwatch FrameTimer { get; init; }

        /// <summary>
        /// Internal: The current rate of frames rendered per second.
        /// </summary>
        public double CurFPS { get; protected set; }

        /// <summary>
        /// Internal: The number of frames since the engine started rendering. 
        /// </summary>
        internal int FrameNumber { get; set; }

        /// <summary>
        /// Determines if an event is waiting. 
        /// </summary>
        public bool EventWaiting { get; protected set; }

        /// <summary>
        /// Number of renderables actually rendered this frame.
        /// </summary>
        internal int RenderedLastFrame { get; set; }

        public Renderer()
        {
            Settings = new();
            FrameTimer = new();   
        }

        internal virtual void Start(RendererSettings renderer)
        { 
        
        }

        internal virtual bool Run()
        {
            return false;
        }


        internal virtual void Render()
        {

        }

        /// <summary>
        /// Sets the window's current <see cref="Camera"/> to <paramref name="nCamera"/>.
        /// </summary>
        /// <param name="nCamera">The <see cref="Camera"/> instance to set the window's current camerat o</param>
        public void SetCurrentCamera(Camera nCamera) => Settings.Camera = nCamera;

        public virtual void SetFullscreen(bool fullscreen)
        {

        }

        public virtual void Clear(Color clearColor)
        {

        }

        /// <summary>
        /// Internal - used as a part of LightningGL.Shutdown
        /// </summary>
        internal virtual void Shutdown()
        {
            NCLogging.Log("Renderer destruction requested. Calling shutdown events...");
            NotifyShutdown();

            NCLogging.Log("Destroying all renderables...");
            foreach (Renderable renderable in Renderables)
            {
                renderable.Destroy();
            }

            SDL_DestroyRenderer(Settings.RendererHandle);
            SDL_DestroyWindow(Settings.WindowHandle);
        }

        private void NotifyShutdown()
        {
            foreach (Renderable uiElement in Renderables)
            {
                if (uiElement.OnShutdown != null)
                {
                    // stop animating this renderable - prevents "collection modified" issues
                    uiElement.StopCurrentAnimation();
                    uiElement.OnShutdown();
                }
            }
        }

        /// <summary>
        /// Adds a renderable..
        /// </summary>
        public virtual void AddRenderable(Renderable renderable)
        {
            NCLogging.Log($"Adding renderable of type {renderable.GetType().Name} ({renderable.Name})");
            Renderables.Add(renderable);

            // guaranteed never null
            renderable.OnCreate();
        }

        /// <summary>
        /// Removes a renderable.
        /// </summary>
        public virtual void RemoveRenderable(Renderable renderable)
        {
            NCLogging.Log($"Removing renderable of type {renderable.GetType().Name} ({renderable.Name})");
            renderable.OnDestroy();
            Renderables.Remove(renderable);
        }

        public virtual Renderable? GetRenderableByName(string name)
        {
            foreach (Renderable renderable in Renderables)
            {
                if (renderable.Name == name)
                {
                    return renderable;
                }
            }

            return null;
        }

        public virtual void RemoveRenderableByName(string name)
        {
            Renderable? renderable = GetRenderableByName(name);

            if (renderable == null)
            {
                NCError.Throw($"Tried to remove nonexistent renderable name {name}", 190,
                    "Renderer::RemoveRenderableByName's name property did not correspond to a valid Renderable", NCErrorSeverity.FatalError);
                return;
            }

            RemoveRenderable(renderable);

        }


        public virtual bool ContainsRenderable(string name) => GetRenderableByName(name) != null;
    }
}
