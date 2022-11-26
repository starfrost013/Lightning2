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

        public SdlRenderer() : base()
        {
            FrameTimer = new Stopwatch();
            // Start the delta timer.
            FrameTimer.Start();
            ThisTime = 0;
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
                NCError.ShowErrorBox("Passed null WindowSettings to Window::Start method!", 7, "Window::Start windowSettings parameter was set to NULL!", NCErrorSeverity.FatalError);
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

            if (Settings.WindowHandle == IntPtr.Zero) NCError.ShowErrorBox($"Failed to create Window: {SDL_GetError()}", 8, 
                "Window::AddWindow - SDL_CreateWindow failed to create window", NCErrorSeverity.FatalError);

            // set the window ID 
            Settings.ID = SDL_GetWindowID(Settings.WindowHandle);

            // Create the renderer.
            Settings.RendererHandle = SDL_CreateRenderer(Settings.WindowHandle, (int)Settings.ID, Settings.RenderFlags);

            // Get the renderer driver name using our unofficial SDL function
            string realRenderDriverName = SDLu_GetRenderDriverName();

            if (realRenderDriverName != renderer) NCError.ShowErrorBox($"Specified renderer {renderer} is not supported. Using {realRenderDriverName} instead!", 123, 
                "Renderer not supported in current environment", NCErrorSeverity.Warning, null, true);

            if (Settings.RendererHandle == IntPtr.Zero) NCError.ShowErrorBox($"Failed to create Renderer: {SDL_GetError()}", 9, 
                "Window::AddWindow - SDL_CreateRenderer failed to create renderer", NCErrorSeverity.FatalError);

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
                        KeyPressed((Key)currentEvent.key);
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

            // this is actually fine for performance as it turns out (probably not a very big LINQ CALL)
            Renderables = Renderables.OrderBy(x => x.ZIndex).ToList();

            // Build a list of renderables to render from all asset managers.
            // Other stuff can be added "outside" so we simply remove and add to the list (todo: this isn't great)
            Cull();

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
        /// Clears the renderer and optionally sets the color to the Color <paramref name="clearColor"/>
        /// </summary>
        /// <param name="clearColor">The color to set the background to after clearing.</param>
        public override void Clear(Color clearColor = default)
        {
            // default(Color) is 0,0,0,0, no special case code needed
            SDL_SetRenderDrawColor(Settings.RendererHandle, clearColor.R, clearColor.G, clearColor.B, clearColor.A);
            SDL_RenderClear(Settings.RendererHandle);
            Settings.BackgroundColor = clearColor;
        }


        /// <summary>
        /// Sets the window to be fullscreen or windowed.
        /// </summary>
        /// <param name="fullscreen">A boolean determining if the window is fullscreen (TRUE) or windowed (FALSE)</param>
        public override void SetFullscreen(bool fullscreen) => SDL_SetWindowFullscreen(Settings.WindowHandle, fullscreen ? (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP : 0);

        #region Event handlers

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

                // --- THESE TASKS need to be performed ONLY when the renderable is on screen ---
                if (renderable.IsOnScreen
                    && renderable.OnRender != null)
                {
                    renderable.OnRender?.Invoke();
                    RenderedLastFrame++;
                }

                // --- THESE tasks need to be performed when the renderable is on AND off screen ---
                renderable.CurrentAnimation?.UpdateAnimationFor(renderable); // dont call if no
                renderable.OnUpdate?.Invoke();

                if (renderable.Children.Count > 0) RenderAll(renderable); 
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
                    || uiElement.CanReceiveEventsWhileUnfocused))
                {
                    uiElement.OnMousePressed?.Invoke(button);
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
                    || uiElement.CanReceiveEventsWhileUnfocused))
                {
                    uiElement.OnMouseReleased?.Invoke(button);
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
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnKeyPressed?.Invoke(key);
                }
            }
        }

        internal void KeyReleased(Key key)
        {
            foreach (Renderable renderable in Renderables)
            {
                // check if the UI element is focused.
                if ((renderable.Focused
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnKeyPressed?.Invoke(key);
                }
            }
        }

        #endregion
    }
}