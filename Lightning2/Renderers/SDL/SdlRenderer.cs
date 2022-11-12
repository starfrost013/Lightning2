global using static LightningGL.Lightning;

namespace LightningGL
{
    /// <summary>
    /// Defines a LightningGL Window. 
    /// </summary>
    public class SdlRenderer : Renderer
    {
        /// <summary>
        /// The last processed SDL event. Only valid if .Update() is called.
        /// </summary>
        public SDL_Event LastEvent { get; set; }

        /// <summary>
        /// Determines if an event is waiting. 
        /// </summary>
        public bool EventWaiting { get; private set; }

        /// <summary>
        /// Number of renderables actually rendered this frame.
        /// </summary>
        internal int RenderedLastFrame { get; private set; }

        public SdlRenderer()
        {
            FrameTimer = new Stopwatch();
            // Start the delta timer.
            FrameTimer.Start();
            ThisTime = 0;
            Renderables = new List<Renderable>();
            Settings = new RendererSettings();
        }

        /// <summary>
        /// Starts this window.
        /// </summary>
        /// <param name="windowSettings">The window settings to use when starting this window - see <see cref="RendererSettings"/></param>
        internal override void Start(RendererSettings windowSettings)
        {
            // Check that the engine has been started.

            if (windowSettings == null)
            {
                _ = new NCException("Passed null WindowSettings to Window::Start method!", 7, "Window::Start windowSettings parameter null", NCExceptionSeverity.FatalError);
                return;
            }

            Settings = windowSettings;

            // localise the window title
            Settings.Title = LocalisationManager.ProcessString(Settings.Title);

            // set the renderer if the user specified one
            string renderer = SDLu_GetRenderDriverName();

            if (GlobalSettings.GraphicsSdlRenderingBackend != default)
            {
                // set the renderer
                renderer = GlobalSettings.GraphicsSdlRenderingBackend.ToString().ToLowerInvariant(); // needs to be lowercase
                SDL_SetHintWithPriority("SDL_HINT_RENDER_DRIVER", renderer, SDL_HintPriority.SDL_HINT_OVERRIDE);
            }

            NCLogging.Log($"Using renderer: {renderer}");

            // Create the window,
            Settings.WindowHandle = SDL_CreateWindow(Settings.Title, (int)Settings.Position.X, (int)Settings.Position.Y, (int)Settings.Size.X, (int)Settings.Size.Y, Settings.WindowFlags);

            if (Settings.WindowHandle == IntPtr.Zero) _ = new NCException($"Failed to create Window: {SDL_GetError()}", 8, 
                "Window::AddWindow - SDL_CreateWindow failed to create window", NCExceptionSeverity.FatalError);

            // set the window ID 
            Settings.ID = SDL_GetWindowID(Settings.WindowHandle);

            // Create the renderer.
            Settings.RendererHandle = SDL_CreateRenderer(Settings.WindowHandle, (int)Settings.ID, Settings.RenderFlags);

            // Get the renderer driver name using our unofficial SDL function
            string realRenderDriverName = SDLu_GetRenderDriverName();

            if (realRenderDriverName != renderer) _ = new NCException($"Specified renderer {renderer} is not supported. Using {realRenderDriverName} instead!", 123, 
                "Renderer not supported in current environment", NCExceptionSeverity.Warning, null, true);

            if (Settings.RendererHandle == IntPtr.Zero) _ = new NCException($"Failed to create Renderer: {SDL_GetError()}", 9, 
                "Window::AddWindow - SDL_CreateRenderer failed to create renderer", NCExceptionSeverity.FatalError);

            // Initialise the Light Manager.
            LightManager.Init();

            // maybe move this somewhere else
            if (!GlobalSettings.DebugDisabled) AddRenderable(new DebugViewer("DebugViewer"));
        }


        /// <summary>
        /// Runs the main loop at the start of each frame.
        /// </summary>
        /// <returns>A boolean determining if the window is to keep running or close.</returns>
        internal override bool Run()
        {
            // clear the renderet
            FrameTimer.Restart();
            SDL_RenderClear(Settings.RendererHandle);

            // Reset rendered this frame count
            RenderedLastFrame = 0;

            EventWaiting = (SDL_PollEvent(out var currentEvent) > 0);

            // default mainloop
            if (EventWaiting)
            {
                LastEvent = currentEvent;

                // default rendering loop.
                // Developers can choose to handle SDL events after this
                switch (currentEvent.type)
                {
                    case SDL_EventType.SDL_KEYDOWN:
                        Key curKey = (Key)currentEvent.key;

                        // Show a basic about screen on Shift-F9 (KMOD_SHIFT only triggers when both shift keys are held)
                        string curKeyString = curKey.KeySym.ToString();

                        if (curKeyString == "F9"
                            && (curKey.Modifiers.HasFlag(SDL_Keymod.KMOD_LSHIFT)
                            || curKey.Modifiers.HasFlag(SDL_Keymod.KMOD_RSHIFT))
                            && GlobalSettings.GeneralEngineAboutScreenOnShiftF9) ShowEngineAboutScreen();

                        KeyPressed(curKey);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN: // Mouse down event
                        MousePressed((MouseButton)currentEvent.button);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP: // Mouse up event
                        MouseReleased((MouseButton)currentEvent.button);
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION: // Mouse move event
                        MouseMove((MouseButton)currentEvent.motion);
                        break;
                    case SDL_EventType.SDL_WINDOWEVENT: // Window Event - check subtypes
                        switch (currentEvent.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
                                MouseEnter();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
                                MouseLeave();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                                FocusGained();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                                FocusLost();
                                break;
                        }

                        break;
                    case SDL_EventType.SDL_QUIT: // User requested a quit, so shut down
                        Lightning.Shutdown();
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Manages the render loop.
        /// </summary>
        internal override void Render()
        {
            Debug.Assert(CurrentScene != null);

            //TODO: Fix this (it doesn't work lmao)
            int renderableCount = Renderables.Count;

            // Build a list of renderables to render from all asset managers.
            // Other stuff can be added "outside" so we simply remove and add to the list (todo: this isn't great)
            Cull();

            if (renderableCount != Renderables.Count)
            {
                NCLogging.Log("Resorting renderable list by Z-index...");
                Renderables = (List<Renderable>)Renderables.OrderBy(x => x.ZIndex);
            }

            // Draw every object.
            RenderAll();

            // Update the primitive manager.
            PrimitiveManager.Update();

            // Render the lightmap.
            LightManager.Update();

            // Update audio.
            AudioManager.Update();

            // Update the text manager
            TextManager.Update();

            // Update camera (if it's not null)
            Settings.Camera?.Update();

            // Correctly draw the background
            SDL_SetRenderDrawColor(Settings.RendererHandle, Settings.BackgroundColor.R, Settings.BackgroundColor.G, Settings.BackgroundColor.B, Settings.BackgroundColor.A);

            SDL_RenderPresent(Settings.RendererHandle);

            int maxFps = GlobalSettings.GraphicsMaxFPS;

            // Delay for frame limiter
            if (maxFps > 0)
            {
                double targetFrameTime = 1000 / (double)maxFps;
                double actualFrameTime = DeltaTime;

                double delayTime = targetFrameTime - actualFrameTime;

                if (delayTime > 0) SDL_Delay((uint)delayTime);
            }

            // Update the internal FPS values.
            UpdateFps();

        }

        private void UpdateFps()
        {
            // Set the current frame time.
            ThisTime = FrameTimer.ElapsedTicks;

            CurFPS = 10000000 / ThisTime;

            DeltaTime = ((double)ThisTime / 10000);
            
            DeltaTime *= GlobalSettings.GraphicsTickSpeed;

            if (GlobalSettings.GeneralProfilePerformance) PerformanceProfiler.Update(this);
            FrameNumber++;
        }

        /// <summary>
        /// Internal - used as a part of LightningGL.Shutdown
        /// </summary>
        internal void Shutdown()
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


        private void Cull()
        {
            // if we haven't specified otherwise...
            if (!GlobalSettings.GraphicsRenderOffScreenRenderables)
            {
                // Cull stuff offscreen and move it with the camera
                for (int renderableId = 0; renderableId < Renderables.Count; renderableId++)
                {
                    Renderable renderable = Renderables[renderableId];

                    if (Settings.Camera != null)
                    {
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
                    }
                }
            }
            else
            {
                // just assume they are on screen
                foreach (Renderable renderable in Renderables) renderable.IsOnScreen = true;
            }
        }


        /// <summary>
        /// Clears the renderer and optionally sets the color to the Color <paramref name="color"/>
        /// </summary>
        /// <param name="color">The color to set the background to after clearing.</param>
        public void Clear(Color color = default)
        {
            // default(Color) is 0,0,0,0, no special case code needed
            SDL_SetRenderDrawColor(Settings.RendererHandle, color.R, color.G, color.B, color.A);
            SDL_RenderClear(Settings.RendererHandle);
            Settings.BackgroundColor = color;
        }

        /// <summary>
        /// Sets the window's current <see cref="Camera"/> to <paramref name="nCamera"/>.
        /// </summary>
        /// <param name="nCamera">The <see cref="Camera"/> instance to set the window's current camerat o</param>
        public void SetCurrentCamera(Camera nCamera) => Settings.Camera = nCamera;

        /// <summary>
        /// Sets the window to be fullscreen or windowed.
        /// </summary>
        /// <param name="fullscreen">A boolean determining if the window is fullscreen (TRUE) or windowed (FALSE)</param>
        public void SetFullscreen(bool fullscreen) => SDL_SetWindowFullscreen(Settings.WindowHandle, fullscreen ? (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP : 0);

        /// <summary>
        /// Adds a renderable..
        /// </summary>
        public void AddRenderable(Renderable renderable)
        {
            Renderables.Add(renderable);

            // guaranteed never null
            renderable.OnCreate();
        }

        /// <summary>
        /// Removes a renderable.
        /// </summary>
        public void RemoveRenderable(Renderable renderable)
        {
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

        #region Event handlers

        /// <summary>
        /// Renders the contents of the current scene.
        /// </summary>
        internal void RenderAll()
        {
            for (int renderableId = 0; renderableId < Renderables.Count; renderableId++)
            {
                Renderable renderable = Renderables[renderableId]; // prevent collection modified exception

                // --- THESE TASKS need to be performed ONLY when the renderable is on screen ---
                if (renderable.IsOnScreen
                    && renderable.OnRender != null)
                {
                    renderable.OnRender();
                    RenderedLastFrame++;
                }

                // --- THESE tasks need to be performed when the renderable is on AND off screen ---
                if (renderable.CurrentAnimation != null) renderable.CurrentAnimation.UpdateAnimationFor(renderable);
            }
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

        internal void MousePressed(MouseButton button)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Renderable uiElement in Renderables)
            {
                bool intersects = AABB.Intersects(uiElement, button.Position);

                // check if it is focused...
                uiElement.Focused = intersects;

                if ((intersects
                    || uiElement.CanReceiveEventsWhileUnfocused)
                    && uiElement.OnMousePressed != null)
                {
                    uiElement.OnMousePressed(button);
                }
            }
        }

        internal void MouseReleased(MouseButton button)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Renderable uiElement in Renderables)
            {
                bool intersects = AABB.Intersects(uiElement, button.Position);

                // check if it is focused...
                uiElement.Focused = intersects;

                if ((intersects 
                    || uiElement.CanReceiveEventsWhileUnfocused)
                    && (uiElement.OnMouseReleased != null))
                {
                    uiElement.OnMouseReleased(button);
                }
            }
        }

        internal void MouseEnter()
        {
            foreach (Renderable renderable in Renderables)
            {
                renderable.OnMouseEnter?.Invoke();
            }
        }

        internal void MouseLeave()
        {
            foreach (Renderable renderable in Renderables)
            {
                renderable.OnMouseLeave?.Invoke();
            }
        }

        internal void FocusGained()
        {
            foreach (Renderable renderable in Renderables)
            {
                renderable.OnFocusGained?.Invoke();
            }
        }

        internal void FocusLost()
        {
            foreach (Renderable renderable in Renderables)
            {
                renderable.OnFocusLost?.Invoke();
            }
        }

        internal void MouseMove(MouseButton button)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Renderable renderable in Renderables)
            {
                renderable.OnMouseMove?.Invoke(button);
            }
        }

        internal void KeyPressed(Key key)
        {
            foreach (Renderable renderable in Renderables)
            {
                // check if the UI element is focused.
                if ((renderable.Focused 
                    || renderable.CanReceiveEventsWhileUnfocused) 
                    && renderable.OnKeyPressed != null)
                {
                    renderable.OnKeyPressed(key);
                }
            }
        }

        internal void KeyReleased(Key key)
        {
            foreach (Renderable renderable in Renderables)
            {
                // check if the UI element is focused.
                if ((renderable.Focused
                    || renderable.CanReceiveEventsWhileUnfocused)
                    && renderable.OnKeyReleased != null)
                {
                    renderable.OnKeyReleased(key);
                }
            }
        }

        #endregion
    }
}