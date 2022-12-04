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
        /// <inheritdoc/>
        /// </summary>
        public override SdlRendererSettings Settings => (SdlRendererSettings)base.Settings;

        public SdlRenderer() : base()
        {
            FrameTimer = new Stopwatch();
            // Start the delta timer.
            FrameTimer.Start();
            ThisTime = 0;
            Settings = new SdlRendererSettings();
        }

        /// <summary>
        /// Starts this window.
        /// </summary>
        /// <param name="windowSettings">The window settings to use when starting this window - see <see cref="RendererSettings"/></param>
        internal override void Start()
        {
            // Check that we provided RendererSettings

            if (Settings == null)
            {
                NCError.ShowErrorBox("Tried to run SdlRenderer::Start without specifying RendererSettings! Please use the Settings property of SdlRenderer to specify " +
                    "renderer settings!", 7, "SdlRenderer:Start windowSettings parameter was set to NULL!", NCErrorSeverity.FatalError);
                return;
            }

            NCLogging.Log("Initialising SDL...");
            if (SDL_Init(SDL_InitFlags.SDL_INIT_EVERYTHING) < 0) NCError.ShowErrorBox($"Error initialising SDL2: {SDL_GetError()}", 200,
                "Failed to initialise SDL2 during SdlRenderer::Init", NCErrorSeverity.FatalError);

            NCLogging.Log("Initialising SDL_image...");
            if (IMG_Init(IMG_InitFlags.IMG_INIT_EVERYTHING) < 0) NCError.ShowErrorBox($"Error initialising SDL2_image: {SDL_GetError()}", 201,
                "Failed to initialise SDL2_image during SdlRenderer::Init", NCErrorSeverity.FatalError);

            NCLogging.Log("Initialising SDL_ttf...");
            if (TTF_Init() < 0) NCError.ShowErrorBox($"Error initialising SDL2_ttf: {SDL_GetError()}", 202,
                "Failed to initialise SDL2_ttf during SdlRenderer::Init", NCErrorSeverity.FatalError);

            NCLogging.Log("Initialising SDL_mixer...");
            if (Mix_Init(MIX_InitFlags.MIX_INIT_EVERYTHING) < 0) NCError.ShowErrorBox($"Error initialising SDL2_mixer: {SDL_GetError()}", 203,
                "Failed to initialise SDL2_mixer during SdlRenderer::Init", NCErrorSeverity.FatalError);

            NCLogging.Log($"Initialising audio device ({GlobalSettings.AudioDeviceHz}Hz, {GlobalSettings.AudioChannels} channels, format {GlobalSettings.AudioFormat}, chunk size {GlobalSettings.AudioChunkSize})...");
            if (Mix_OpenAudio(GlobalSettings.AudioDeviceHz, GlobalSettings.AudioFormat, GlobalSettings.AudioChannels, GlobalSettings.AudioChunkSize) < 0) NCError.ShowErrorBox(
                $"Error initialising audio device: {SDL_GetError()}", 56, "Failed to initialise audio device during SdlRenderer::Init", NCErrorSeverity.FatalError);

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
                        EventManager.FireKeyPressed((Key)currentEvent.key);
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        EventManager.FireKeyReleased((Key)currentEvent.key);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN: // Mouse down event
                        EventManager.FireMousePressed((MouseButton)currentEvent.button);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP: // Mouse up event
                        EventManager.FireMouseReleased((MouseButton)currentEvent.button);
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION: // Mouse move event
                        EventManager.FireMouseMove((MouseButton)currentEvent.motion);
                        break;
                    case SDL_EventType.SDL_WINDOWEVENT: // Window Event - check subtypes
                        switch (currentEvent.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
                                EventManager.FireMouseEnter();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
                                EventManager.FireMouseLeave();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                                EventManager.FireFocusGained();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                                EventManager.FireFocusLost();
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
            // only render if we have a scene
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

        internal override void Shutdown()
        {
            base.Shutdown();

            SDL_DestroyRenderer(Settings.RendererHandle);
            SDL_DestroyWindow(Settings.WindowHandle);
        }
    }
}