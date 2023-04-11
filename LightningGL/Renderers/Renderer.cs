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
            Sort();

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

        private void Sort(Renderable? parent = null)
        {
            Renderables = Renderables.OrderBy(x => x.ZIndex).ToList();

            if (parent == null)
            {
                foreach (Renderable renderable in Lightning.Renderer.Renderables)
                {
                    if (renderable.Children.Count > 0)
                    {
                        renderable.Children = renderable.Children.OrderBy(x => x.ZIndex).ToList();
                        Sort(renderable);
                    }
                }
            }
            else
            {
                foreach (Renderable renderable in parent.Children)
                {
                    if (renderable.Children.Count > 0)
                    {
                        renderable.Children = renderable.Children.OrderBy(x => x.ZIndex).ToList();
                        Sort(renderable);
                    }
                }
            }
            
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
                Tree.RemoveRenderable(renderable);
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
                    renderable.DrawComponents();
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
                            renderable.RenderPosition = new(renderable.Position.X - curCamera.Position.X + curCamera.FocusDelta.X,
                                renderable.Position.Y - curCamera.Position.Y + curCamera.FocusDelta.Y);
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

            DeltaTime = (double)ThisTime / 10000 * GlobalSettings.GraphicsTickSpeed;

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

        internal virtual Texture? TextureFromFreetypeBitmap(FT_Bitmap bitmap, Texture texture, Color foregroundColor)
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

        internal virtual void DrawTexture(params object[] args)
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
