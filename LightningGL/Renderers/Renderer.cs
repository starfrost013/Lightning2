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
        public virtual RendererSettings Settings { get; internal set; }

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
        internal Stopwatch FrameTimer { get; init; }

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

        /// <summary>
        /// Determine if events will be run.
        /// </summary>
        protected bool EventsRunning { get; set; }

        /// <summary>
        /// The library used for freetype.
        /// </summary>
        internal FreeTypeLibrary? FreeTypeLibrary { get; set; }

        public Renderer()
        {
            Settings = new();
            Renderables = new List<Renderable>();

            FrameTimer = new();
            // Start the delta/frame timer.
            FrameTimer.Start();
            ThisTime = 0;
        }

        internal virtual void Start()
        {
            // Check that we provided RendererSettings
            Logger.Log("Initialising FreeType...");

            FreeTypeLibrary = new FreeTypeLibrary();

            if (Settings == null)
            {
                Logger.LogError("Tried to run Renderer::Start without specifying RendererSettings! Please use the Settings property of Renderer to specify " +
                    "renderer settings!", 7, LoggerSeverity.FatalError);
                return;
            }

        }

        internal virtual bool Run()
        {
            return false;
        }

        internal virtual void Render()
        {
            // this is actually fine for performance as it turns out (probably not a very big LINQ call)
            Renderables = Renderables.OrderBy(x => x.ZIndex).ToList();

#if PROFILING
            ProfilingTimers.Order.Stop();
            ProfilingTimers.Cull.Start();
#endif
            // Build a list of renderables to render from all asset managers.
            // Other stuff can be added "outside" so we simply remove and add to the list (todo: this isn't great)
            Cull();

#if PROFILING
            ProfilingTimers.Cull.Stop();
            ProfilingTimers.UpdateRenderables.Start();
#endif
            // Draw every object.
            RenderAll();

#if PROFILING
            ProfilingTimers.UpdateRenderables.Stop();
            ProfilingTimers.UpdateLightmap.Start();
#endif
            // Render the lightmap.
            LightManager.Update();

#if PROFILING
            ProfilingTimers.UpdateLightmap.Stop();
            ProfilingTimers.Purge.Start();
#endif
            // purge the text manager glyph cache
            GlyphCache.PurgeUnusedEntries();

#if PROFILING
            ProfilingTimers.Purge.Stop();
            ProfilingTimers.UpdateCamera.Start();
#endif
            // Update camera (if it's not null)
            Settings.Camera?.Update();
        }

        /// <summary>
        /// Sets the window's current <see cref="Camera"/> to <paramref name="nCamera"/>.
        /// </summary>
        /// <param name="nCamera">The <see cref="Camera"/> instance to set the window's current camerat o</param>
        public void SetCurrentCamera(Camera nCamera) => Settings.Camera = nCamera;

        public virtual void SetFullscreen(bool fullscreen)
        {

        }

        /// <summary>
        /// Clears the renderer.
        /// </summary>
        /// <param name="clearColor">The colour to clear the renderer.</param>
        public virtual void Clear(Color clearColor)
        {

        }

        /// <summary>
        /// Internal - used as a part of LightningGL.Shutdown
        /// </summary>
        internal virtual void Shutdown()
        {
            Logger.Log("Renderer destruction requested. Calling shutdown events...");
            NotifyShutdown();

            EventsRunning = false;

            Logger.Log("Destroying all renderables...");

            for (int renderableId = 0; renderableId < Renderables.Count; renderableId++)
            {
                Renderable renderable = Renderables[renderableId];
                RemoveRenderable(renderable);
                renderableId--;
            }

            Logger.Log("Shutting down FreeType...");
            FreeTypeLibrary?.Dispose();
        }

        private void NotifyShutdown()
        {
            foreach (Renderable uiElement in Renderables)
            {
                if (uiElement.OnShutdown != null) uiElement.OnShutdown();
            }
        }

        /// <summary>
        /// Adds a renderable to the renderer hierarchy.
        /// </summary>
        public virtual T AddRenderable<T>(T renderable, Renderable? parent = null) where T : Renderable
        {
            string parentName = (parent == null) ? "Root" : parent.Name;

            Logger.Log($"Adding renderable of type {renderable.GetType().Name} ({renderable.Name}) - parent {parentName}");

            if (parent == null)
            {
                Lightning.Renderer.Renderables.Add(renderable);

                // guaranteed never null
                renderable.OnCreate();
            }
            else
            {
                // check that it contains the renderable
                if (!ContainsRenderable(parent.Name)) Logger.LogError($"Tried to add a renderable with a parent " +
                    $"that is not in the object hierarchy!", 194, LoggerSeverity.FatalError);

                renderable.Parent = parent;

                parent.Children.Add(renderable);

                // guaranteed never null
                renderable.OnCreate();
            }

            return renderable;
        }

        /// <summary>
        /// Removes a renderable.
        /// </summary>
        public virtual void RemoveRenderable(Renderable renderable, Renderable? parent = null)
        {
            string parentName = (parent == null) ? "Root" : parent.Name;

            Logger.Log($"Removing renderable of type {renderable.GetType().Name} ({renderable.Name}) - parent {parentName}");


            for (int childId = 0; childId < renderable.Children.Count; childId++)
            {
                Renderable child = renderable.Children[childId];
                child.StopCurrentAnimation();
                child.OnDestroy();

                if (child.Children.Count > 0) RemoveRenderable(child);

                renderable.Children.Remove(child);
                childId--;
            }

            renderable.StopCurrentAnimation();
            renderable.OnDestroy();

            // if there's no parent...
            // how do we take into account the case where it's not actually in its parent?
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
            Logger.Log($"Removing all children of renderable of type {renderable.GetType().Name} ({renderable.Name})");

            for (int renderableId = 0; renderableId < renderable.Children.Count; renderableId++)
            {
                Renderable child = renderable.Children[renderableId];
                RemoveRenderable(child, renderable);
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
                Logger.LogError($"Tried to remove nonexistent renderable name {name}", 190, LoggerSeverity.FatalError);
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
            if (!GlobalSettings.GraphicsDontCullRenderables)
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
                // just set every renderable to be on screen
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

            CurFPS = Stopwatch.Frequency / ThisTime;

            DeltaTime = ((double)ThisTime / 10000);

            DeltaTime *= GlobalSettings.GraphicsTickSpeed;

            if (GlobalSettings.GeneralProfilePerformance) PerformanceProfiler.Update(this);
            FrameNumber++;
        }

        #region Backend-specific primitives

        internal virtual void DrawPixel(int x, int y, byte r, byte g, byte b, byte a)
        {
            Logger.LogError($"DrawPixel not implemented for renderer {GetType().Name!}", 205, LoggerSeverity.FatalError);
        }

        internal virtual void DrawLine(int x1, int y1, int x2, int y2, byte r, byte g, byte b, byte a)
        {
            Logger.LogError($"DrawLine not implemented for renderer {GetType().Name!}", 206, LoggerSeverity.FatalError);
        }

        internal virtual void DrawEllipse(int x, int y, int rx, int ry, byte r, byte g, byte b, byte a, bool filled)
        {
            Logger.LogError($"DrawCircle not implemented for renderer {GetType().Name!}", 208, LoggerSeverity.FatalError);
        }

        internal virtual void DrawRectangle(Vector2 position, Vector2 size, byte r, byte g, byte b, byte a, bool filled = false)
        {
            Logger.LogError($"DrawRectangle not implemented for renderer {GetType().Name!}", 209, LoggerSeverity.FatalError);
        }

        #endregion

        #region Backend-specific texture code

        internal virtual nint CreateTexture(int sizeX, int sizeY, bool isTarget = false)
        {
            Logger.LogError($"AllocTexture not implemented for renderer {GetType().Name!}", 223, LoggerSeverity.FatalError);
            return default;
        }

        internal virtual Texture? TextureFromFreetypeBitmap(FT_Bitmap bitmap, Texture texture, Color foregroundColor, FontStyle style)
        {
            Logger.LogError($"TextureFromFreeTypeBitmap not implemented for renderer {GetType().Name!}", 254, LoggerSeverity.FatalError);
            return default;
        }

        internal virtual nint LoadTexture(string path)
        {
            Logger.LogError($"LoadTexture not implemented for renderer {GetType().Name!}", 220, LoggerSeverity.FatalError);
            return default;
        }

        internal virtual nint AllocTextureFormat() // probably SDL only
        {
            Logger.LogError($"AllocTextureFormat not implemented for renderer {GetType().Name!}", 221, LoggerSeverity.FatalError);
            return default;
        }

        internal virtual void LockTexture(nint handle, Vector2 start, Vector2 size, out nint pixels, out int pitch) // probably SDL only
        {
            Logger.LogError($"LockTexture not implemented for renderer {GetType().Name!}", 224, LoggerSeverity.FatalError);
            pixels = default;
            pitch = 0;
        }

        internal virtual void UnlockTexture(nint handle) // probably SDL only
        {
            Logger.LogError($"UnlockTexture not implemented for renderer {GetType().Name!}", 225, LoggerSeverity.FatalError);
        }

        internal virtual void DrawTexture(Texture texture)
        {
            Logger.LogError($"DrawTexture not implemented for renderer {GetType().Name!}", 222, LoggerSeverity.FatalError);
        }

        internal virtual void SetTextureBlendMode(params object[] args) // probably SDL only 
        {
            Logger.LogError($"SetTextureBlendMode not implemented for renderer {GetType().Name!}", 230, LoggerSeverity.FatalError);
        }

        internal  virtual nint DestroyTexture(nint handle)
        {
            Logger.LogError($"DestroyTexture not implemented for renderer {GetType().Name!}", 232, LoggerSeverity.FatalError);
            return default; 
        }

        #endregion
    }
}
