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
        /// The settings of this window - see <see cref="RendererSettings"/>.
        /// </summary>
        public virtual RendererSettings Settings { get; protected set; }

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
        /// Adds a renderable..
        /// </summary>
        public void AddRenderable(Renderable renderable)
        {
            NCLogging.Log($"Adding renderable of type {renderable.GetType().Name} ({renderable.Name})");
            Renderables.Add(renderable);

            // guaranteed never null
            renderable.OnCreate();
        }

        /// <summary>
        /// Removes a renderable.
        /// </summary>
        public void RemoveRenderable(Renderable renderable)
        {
            NCLogging.Log($"Removing renderable of type {renderable.GetType().Name} ({renderable.Name})");
            renderable.OnDestroy();
            Renderables.Remove(renderable);
        }

        public Renderable? GetRenderableByName(string name)
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

        public void RemoveRenderableByName(string name)
        {
            Renderable? renderable = GetRenderableByName(name);

            if (renderable == null)
            {
                _ = new NCException($"Tried to remove nonexistent renderable name {name}", 190,
                    "Renderer::RemoveRenderableByName's name property did not correspond to a valid Renderable", NCExceptionSeverity.FatalError);
                return;
            }

            RemoveRenderable(renderable);

        }

        public bool ContainsRenderable(string name) => GetRenderableByName(name) != null;
    }
}
