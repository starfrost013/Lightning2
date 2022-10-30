global using static LightningGL.Lightning;

namespace LightningGL
{
    /// <summary>
    /// Defines a LightningGL Window. 
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// The settings of this window - see <see cref="RendererSettings"/>.
        /// </summary>
        public RendererSettings Settings { get; internal set; }

        /// <summary>
        /// Private: The time the current frame took. Used to measure FPS.
        /// </summary>
        private long ThisTime { get; set; }

        /// <summary>
        /// Delta-time used for ensuring objects move at the same speed regardless of framerate.
        /// </summary>
        public double DeltaTime { get; set; }

        /// <summary>
        /// Private: Frame-timer used for measuring frametime.
        /// </summary>
        private Stopwatch FrameTimer { get; set; }

        /// <summary>
        /// Internal: The current rate of frames rendered per second.
        /// </summary>
        public double CurFPS { get; private set; }

        /// <summary>
        /// Internal: The number of frames since the engine started rendering. 
        /// </summary>
        internal int FrameNumber { get; private set; }

        /// <summary>
        /// The last processed SDL event. Only valid if .Update() is called.
        /// </summary>
        public SDL_Event LastEvent { get; set; }

        /// <summary>
        /// Determines if an event is waiting. 
        /// </summary>
        public bool EventWaiting { get; set; }

        /// <summary>
        /// Composed renderable list
        /// </summary>
        private List<Renderable> Renderables { get; set; }

        public Renderer()
        {
            FrameTimer = new Stopwatch();
            // Start the delta timer.
            FrameTimer.Start();
            ThisTime = 0;
        }

        /// <summary>
        /// Starts this window.
        /// </summary>
        /// <param name="windowSettings">The window settings to use when starting this window - see <see cref="RendererSettings"/></param>
        public void Start(RendererSettings windowSettings)
        {
            // Check that the engine has been started.
            if (!Initialised) _ = new NCException("You cannot start a window without initialising the engine - call Lightning::Init first!", 134, "Window::Start called before Lightning::Init!", NCExceptionSeverity.FatalError);

            if (!GlobalSettings.DontUseSceneManager
                && SceneManager.Initialised) _ = new NCException("GlobalSettings::DontUseSceneManager needs to be set to initialise Windows without using the Scene Manager!", 127, "Window::Init called when GlobalSettings::DontUseSceneManager is FALSE and SceneManager::Initialised is TRUE", NCExceptionSeverity.FatalError);
            if (windowSettings == null) _ = new NCException("Passed null WindowSettings to Window::Start method!", 7, "Window::Start windowSettings parameter null", NCExceptionSeverity.FatalError);

            Settings = windowSettings;

            // localise the window title
            Settings.Title = LocalisationManager.ProcessString(Settings.Title);

            // set the renderer if the user specified one
            string renderer = SDLu_GetRenderDriverName();

            if (GlobalSettings.RendererType != default(RenderingBackend))
            {
                // set the renderer
                renderer = GlobalSettings.RendererType.ToString().ToLowerInvariant(); // needs to be lowercase
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

            // Create a default camera if the developer did not create one during init.
            if (Settings.Camera == null)
            {
                Camera defaultCamera = new Camera(CameraType.Follow);
                SetCurrentCamera(defaultCamera);
            }

            if (Settings.RendererHandle == IntPtr.Zero) _ = new NCException($"Failed to create Renderer: {SDL_GetError()}", 9, 
                "Window::AddWindow - SDL_CreateRenderer failed to create renderer", NCExceptionSeverity.FatalError);

            // Initialise the Light Manager.
            LightManager.Init(this);
        }

        /// <summary>
        /// Run at the end of each frame.
        /// </summary>
        private void Update()
        {
            FrameTimer.Restart();
            SDL_RenderClear(Settings.RendererHandle);
        }

        /// <summary>
        /// Runs the main loop.
        /// </summary>
        /// <returns>A boolean determining if the window is to keep running or close.</returns>
        public bool Run()
        {
            Update();

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
                            && GlobalSettings.EngineAboutScreenOnShiftF9) ShowEngineAboutScreen();

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
                        Lightning.Shutdown(this);
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Manages the render loop.
        /// </summary>
        public void Render()
        {
            // Build a list of renderables to render from all asset managers.
            List<Renderable> renderables = BuildRenderableList();

            // temporary compromise - we loaded new renderables
            if (Renderables == null
                || renderables.Count != Renderables.Count)
            {
                // we have to resort by z-index.
                Renderables = renderables;
                NCLogging.Log("Resorting renderable list by Z-index...");
                Renderables.OrderBy(x => x.ZIndex);
            }
            
            RenderAll();

            // Render the lightmap.
            LightManager.Update(this);

            // Update audio.
            AudioManager.Update(this);

            // Update the font manager
            FontManager.Update(this);

            // draw fps on top always (by drawing it last. we don't have zindex, but we will later). Also snap it to the screen like a hud element. 
            // check the showfps global setting first
            // do this BEFORE present. then measure frametime in Render_MeasureFps, this makes it accurate.
            if (GlobalSettings.ShowDebugInfo) DrawDebugInformation();

            // Update camera
            if (Settings.Camera != null) Settings.Camera.Update();

            // Correctly draw the background
            SDL_SetRenderDrawColor(Settings.RendererHandle, Settings.BackgroundColor.R, Settings.BackgroundColor.G, Settings.BackgroundColor.B, Settings.BackgroundColor.A);

            SDL_RenderPresent(Settings.RendererHandle);

            int maxFps = GlobalSettings.MaxFPS;

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

        /// <summary>
        /// Draws debug information to the screen if the <see cref="GlobalSettings.ShowDebugInfo"/> setting is enabled.
        /// </summary>
        private void DrawDebugInformation()
        {
            int currentY = (int)Settings.Camera.Position.Y;
            int debugLineDistance = 12;

            PrimitiveRenderer.DrawText(this, $"FPS: {CurFPS.ToString("F1")} ({DeltaTime.ToString("F2")}ms)", new Vector2(Settings.Camera.Position.X, currentY), Color.FromArgb(255, 255, 255, 255), true);

            if (CurFPS < GlobalSettings.MaxFPS)
            {
                currentY += debugLineDistance;

                int maxFps = GlobalSettings.MaxFPS;

                if (maxFps == 0) maxFps = 60;

                PrimitiveRenderer.DrawText(this, $"Running under target FPS ({maxFps})!", new Vector2(Settings.Camera.Position.X, currentY), Color.FromArgb(255, 255, 0, 0), true);
            }

            currentY += debugLineDistance;
            PrimitiveRenderer.DrawText(this, FrameNumber.ToString(), new Vector2(Settings.Camera.Position.X, currentY), Color.FromArgb(255, 255, 255, 255), true);
        }

        private void UpdateFps()
        {
            // Set the current frame time.
            ThisTime = FrameTimer.ElapsedTicks;

            CurFPS = 10000000 / ThisTime;

            DeltaTime = ((double)ThisTime / 10000);
            
            DeltaTime *= GlobalSettings.TickSpeed;

            if (GlobalSettings.ProfilePerformance) PerformanceProfiler.Update(this);
            FrameNumber++;
        }

        /// <summary>
        /// Sets up a simple message box displaying engine version information.
        /// </summary>
        private void ShowEngineAboutScreen()
        {
            NCMessageBox messageBox = new()
            {
                Text = $"Powered by the Lightning Game Engine\n" +
                $"Version {LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING}",
                Title = "About",
                Icon = SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION
            };

            messageBox.AddButton("OK", SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT | SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT);
            messageBox.Show();
        }

        /// <summary>
        /// Internal - used as a part of LightningGL.Shutdown
        /// </summary>
        internal void Shutdown()
        {
            NCLogging.Log("Renderer destruction requested. Calling shutdown events...");
            NotifyShutdown();
            SDL_DestroyRenderer(Settings.RendererHandle);
            SDL_DestroyWindow(Settings.WindowHandle);
        }

        /// <summary>
        /// Clears the renderer and optionally sets the color to the Color <paramref name="color"/>
        /// </summary>
        /// <param name="color"></param>
        public void Clear(Color color = default(Color))
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

        private List<Renderable> BuildRenderableList()
        {
            // lightmanager doesn't draw each frame and audiomanager needs to be updated on its own
            List<Renderable> renderables = new List<Renderable>();

            foreach (Renderable renderable in UIManager.Assets) renderables.Add(renderable);
            foreach (Renderable renderable in TextureManager.Assets) renderables.Add(renderable);
            foreach (Renderable renderable in ParticleManager.Assets) renderables.Add(renderable);
            foreach (Renderable renderable in TextManager.Assets) renderables.Add(renderable);

            // if we haven't specified otherwise...
            if (!GlobalSettings.RenderOffScreenRenderables)
            {
                // Cull stuff offscreen and move it with the camera
                for (int renderableId = 0; renderableId < renderables.Count; renderableId++)
                {
                    Renderable renderable = renderables[renderableId];

                    if (Settings.Camera != null)
                    {
                        if (!renderable.SnapToScreen)
                        {
                            renderable.IsOnScreen = (renderable.RenderPosition.X >= Settings.Camera.Position.X - renderable.Size.X
                                && renderable.RenderPosition.Y >= Settings.Camera.Position.Y - renderable.Size.Y
                                && renderable.RenderPosition.X <= Settings.Camera.Position.X + GlobalSettings.ResolutionX
                                && renderable.RenderPosition.Y <= Settings.Camera.Position.Y + GlobalSettings.ResolutionY);
                        }
                        else
                        {
                            renderable.IsOnScreen = (renderable.RenderPosition.X >= 0
                                && renderable.RenderPosition.X <= GlobalSettings.ResolutionX
                                && renderable.RenderPosition.Y >= 0
                                && renderable.RenderPosition.Y <= GlobalSettings.ResolutionY);
                        }
                    }
                }
            }
            else
            {
                // just assume they are on screen
                foreach (Renderable renderable in renderables) renderable.IsOnScreen = true;
            }

            return renderables;
        }


        #region Event handlers

        /// <summary>
        /// Renders all UI elements.
        /// </summary>
        /// <param name="cRenderer">The UI element to render.</param>
        internal void RenderAll()
        {
            foreach (Renderable renderable in Renderables)
            {
                // --- THESE TASKS need to be performed ONLY when the renderable is on screen ---
                if (renderable.IsOnScreen)
                {
                    if (renderable.OnRender != null) renderable.OnRender(this);
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
                    uiElement.OnShutdown(this);
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

                if (intersects
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

                if (intersects
                    && uiElement.OnMouseReleased != null)
                {
                    uiElement.OnMouseReleased(button);
                }
            }
        }

        internal void MouseEnter()
        {
            foreach (Renderable renderable in Renderables)
            {
                if (renderable.OnMouseEnter != null)
                {
                    renderable.OnMouseEnter();
                }
            }
        }

        internal void MouseLeave()
        {
            foreach (Renderable renderable in Renderables)
            {
                if (renderable.OnMouseLeave != null)
                {
                    renderable.OnMouseLeave();
                }
            }
        }

        internal void FocusGained()
        {
            foreach (Renderable renderable in Renderables)
            {
                if (renderable.OnFocusGained != null)
                {
                    renderable.OnFocusGained();
                }
            }
        }

        internal void FocusLost()
        {
            foreach (Renderable renderable in Renderables)
            {
                if (renderable.OnFocusLost != null)
                {
                    renderable.OnFocusLost();
                }
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
                if (renderable.OnMouseMove != null) // this one is passed regardless of intersection for things like button highlighting
                {
                    renderable.OnMouseMove(button);
                }
            }
        }

        internal void KeyPressed(Key key)
        {
            foreach (Renderable renderable in Renderables)
            {
                // check if the UI element is focused.
                if (renderable.Focused
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
                if (renderable.Focused
                    && renderable.OnKeyReleased != null)
                {
                    renderable.OnKeyReleased(key);
                }
            }
        }

        #endregion
    }
}