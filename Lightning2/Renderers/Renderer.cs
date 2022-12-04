﻿namespace LightningGL
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
            Renderables = new List<Renderable>();
        }

        internal virtual void Start()
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
        public virtual void AddRenderable(Renderable renderable, Renderable? parent = null)
        {
            string parentName = (parent == null) ? "Root" : parent.Name;

            NCLogging.Log($"Adding renderable of type {renderable.GetType().Name} ({renderable.Name}) - parent {parentName}");

            if (parent == null)
            {
                Lightning.Renderer.Renderables.Add(renderable);

                // guaranteed never null
                renderable.OnCreate();
            }
            else
            {
                // check that it contains the renderable
                if (!ContainsRenderable(parent.Name)) NCError.ShowErrorBox($"Tried to add a renderable with a parent that is not in the object hierarchy!", 194, 
                    "Renderer::AddRenderable parent parameter is not in the renderer object hierarchy", NCErrorSeverity.FatalError);

                renderable.Parent = parent;

                parent.Children.Add(renderable);

                // guaranteed never null
                renderable.OnCreate();
            }
        }

        /// <summary>
        /// Removes a renderable.
        /// </summary>
        public virtual void RemoveRenderable(Renderable renderable, Renderable? parent = null)
        {
            string parentName = (parent == null) ? "Root" : parent.Name;

            NCLogging.Log($"Removing renderable of type {renderable.GetType().Name} ({renderable.Name}) - parent {parentName}");

            if (renderable.Children.Count > 0)
            {
                foreach (Renderable child in renderable.Children)
                {
                    child.OnDestroy();

                    if (child.Children.Count > 0) RemoveRenderable(child);

                    renderable.Children.Remove(child);
                }
            }

            renderable.OnDestroy();

            // if there's no parent...
            if (parent == null)
            {
                Lightning.Renderer.Renderables.Remove(renderable);
            }
            else
            {
                parent.Children.Remove(renderable);
            }
            
        }

        public virtual void RemoveAllChildren(Renderable renderable)
        {
            NCLogging.Log($"Removing all children of renderable of type {renderable.GetType().Name} ({renderable.Name})");

            if (renderable.Children.Count > 0)
            {
                foreach (Renderable child in renderable.Children)
                {
                    child.OnDestroy();

                    if (child.Children.Count > 0) RemoveRenderable(child);

                    renderable.Children.Remove(child);
                }
            }
        }

        public virtual Renderable? GetRenderableByName(string name, Renderable? parent = null)
        {
            // iterate through either the root or the child list depending on if the parent paremter was provided
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            Renderable? foundRenderable = null;

            foreach (Renderable renderable in renderables)
            {
                // kind of a stupid hack but it's better than using break in a foreach lol
                if (renderable.Children.Count > 0)
                {
                    Renderable? newRenderable = GetRenderableByName(name, renderable);
                    if (newRenderable != null) foundRenderable = newRenderable; 
                }

                if (renderable.Name == name) foundRenderable = renderable;
            }

            return foundRenderable;
        }

        public virtual void RemoveRenderableByName(string name)
        {
            Renderable? renderable = GetRenderableByName(name);

            if (renderable == null)
            {
                NCError.ShowErrorBox($"Tried to remove nonexistent renderable name {name}", 190,
                    "Renderer::RemoveRenderableByName's name property did not correspond to a valid Renderable", NCErrorSeverity.FatalError);
                return;
            }

            RemoveRenderable(renderable);
        }

        public virtual bool ContainsRenderable(string name) => GetRenderableByName(name) != null;

        internal int CountRenderables(Renderable? parent = null, int initialCount = 0)
        {
            List<Renderable> renderables = (parent == null) ? Renderables : parent.Children;

            int count = initialCount;

            foreach (Renderable renderable in renderables)
            {
                count++;
                if (renderable.Children.Count > 0) count = CountRenderables(renderable, count);
            }

            return count;
        }

        /// <summary>
        /// Renders the contents of the current scene.
        /// </summary>
        internal void RenderAll(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Renderables : parent.Children;

            // TODO: separate render/update? so we can use a foreach loop here
            for (int renderableId = 0; renderableId < renderables.Count; renderableId++)
            {
                Renderable renderable = renderables[renderableId]; // prevent collection modified exception

                // --- THESE TASKS need to be performed ONLY when the renderable is actually being drawn ---
                if (renderable.IsOnScreen
                    && !renderable.IsNotRendering
                    && renderable.OnRender != null)
                {
                    renderable.OnRender?.Invoke();
                    RenderedLastFrame++;
                }

                // --- THESE tasks need to be performed when the renderable exists, regardless of if it is being drawn or not ---
                renderable.CurrentAnimation?.UpdateAnimationFor(renderable); // dont call if no
                renderable.OnUpdate?.Invoke();

                if (renderable.Children.Count > 0) RenderAll(renderable);
            }
        }

        protected void Cull(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Renderables : parent.Children;

            // if we haven't specified otherwise...
            if (!GlobalSettings.GraphicsRenderOffScreenRenderables)
            {
                // Cull stuff offscreen and move it with the camera
                for (int renderableId = 0; renderableId < renderables.Count; renderableId++)
                {
                    if (Settings.Camera != null)
                    {
                        Renderable renderable = renderables[renderableId];

                        // transform the position if there is a camera.
                        Camera curCamera = Lightning.Renderer.Settings.Camera;

                        if (curCamera != null
                            && !renderable.SnapToScreen)
                        {
                            renderable.RenderPosition = new(renderable.Position.X - curCamera.Position.X,
                                renderable.Position.Y - curCamera.Position.Y);
                        }

                        renderable.IsOnScreen = (renderable.NotCullable
                            || (renderable.RenderPosition.X >= -renderable.Size.X
                            && renderable.RenderPosition.X <= GlobalSettings.GraphicsResolutionX + renderable.Size.X
                            && renderable.RenderPosition.Y >= -renderable.Size.Y
                            && renderable.RenderPosition.Y <= GlobalSettings.GraphicsResolutionY + renderable.Size.Y));

                        if (renderable.Children.Count > 0) Cull(renderable);
                    }
                }
            }
            else
            {
                // just assume they are on screen
                foreach (Renderable renderable in Renderables)
                {
                    renderable.IsOnScreen = true;
                    if (renderable.Children.Count > 0) Cull(renderable);
                }
            }
        }

        protected void UpdateFps()
        {
            // Set the current frame time.
            ThisTime = FrameTimer.ElapsedTicks;

            CurFPS = 10000000 / ThisTime;

            DeltaTime = ((double)ThisTime / 10000);

            DeltaTime *= GlobalSettings.GraphicsTickSpeed;

            if (GlobalSettings.GeneralProfilePerformance) PerformanceProfiler.Update(this);
            FrameNumber++;
        }

    }
}
