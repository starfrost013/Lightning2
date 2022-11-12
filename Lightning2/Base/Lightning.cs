namespace LightningGL
{
    /// <summary>
    /// Lightning
    /// 
    /// March 14, 2022
    /// 
    /// Defines the Scene Manager, which manages scenes in Lightning (optional)
    /// </summary>
    public static class Lightning
    {
        /// <summary>
        /// The current scene that is being run.
        /// </summary>
        internal static Scene? CurrentScene { get; private set; }

        /// <summary>
        /// The main renderer of the application.
        /// </summary>
        public static SdlRenderer Renderer { get; private set; }

        /// <summary>
        /// Determines if the scene manager is running.
        /// </summary>
        internal static bool Initialised { get; private set; }

        /// <summary>
        /// The list of scenes.
        /// </summary>
        public static List<Scene> Scenes { get; private set; }

        static Lightning()
        {
            // Initialise Lightning2 window.
            Renderer = new SdlRenderer();
            Scenes = new List<Scene>();

            // Initialise all asset managers
            TextureManager = new TextureAssetManager();
            AudioManager = new AudioAssetManager();
            ParticleManager = new ParticleAssetManager();
            UIManager = new UIAssetManager();
            FontManager = new FontAssetManager();
            TextManager = new TextAssetManager();
            LightManager = new LightAssetManager();
            AnimationManager = new AnimationAssetManager();
            PrimitiveManager = new PrimitiveAssetManager();
        }

        #region Asset managers
        public static TextureAssetManager TextureManager { get; private set; } // init not valid on static members

        public static AudioAssetManager AudioManager { get; private set; }

        public static ParticleAssetManager ParticleManager { get; private set; }

        public static UIAssetManager UIManager { get; private set; }

        public static FontAssetManager FontManager { get; private set; }

        public static TextAssetManager TextManager { get; private set; }

        public static LightAssetManager LightManager { get; private set; }

        public static AnimationAssetManager AnimationManager { get; private set; }

        public static PrimitiveAssetManager PrimitiveManager { get; private set; }
        #endregion

        public static void Init(string[] args)
        {
            try
            {
                // Set culture to invariant so things like different decimal symbols don't crash
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                InitLogging();

                // Log the sign-on message
                NCLogging.Log($"Lightning Game Engine");
                NCLogging.Log($"Version {LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING}");

                NCLogging.Log("Initialising SDL...");
                if (SDL_Init(SDL_InitFlags.SDL_INIT_EVERYTHING) < 0) _ = new NCException($"Error initialising SDL2: {SDL_GetError()}", 0, "Failed to initialise SDL2 during Lightning::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising SDL_image...");
                if (IMG_Init(IMG_InitFlags.IMG_INIT_EVERYTHING) < 0) _ = new NCException($"Error initialising SDL2_image: {SDL_GetError()}", 1, "Failed to initialise SDL2_image during Lightning::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising SDL_ttf...");
                if (TTF_Init() < 0) _ = new NCException($"Error initialising SDL2_ttf: {SDL_GetError()}", 2, "Failed to initialise SDL2_ttf during Lightning::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising SDL_mixer...");
                if (Mix_Init(MIX_InitFlags.MIX_INIT_EVERYTHING) < 0) _ = new NCException($"Error initialising SDL2_mixer: {SDL_GetError()}", 3, "Failed to initialise SDL2_mixer during Lightning::Init", NCExceptionSeverity.FatalError);

                // this should always be the earliest step
                NCLogging.Log("Obtaining system information...");
                SystemInfo.Load();

                // this should always be the second earliest step
                NCLogging.Log("Loading global settings from Engine.ini...");
                GlobalSettings.Load();

                NCLogging.Log($"Initialising audio device ({GlobalSettings.AudioDeviceHz}Hz, {GlobalSettings.AudioChannels} channels, format {GlobalSettings.AudioFormat}, chunk size {GlobalSettings.AudioChunkSize})...");
                if (Mix_OpenAudio(GlobalSettings.AudioDeviceHz, GlobalSettings.AudioFormat, GlobalSettings.AudioChannels, GlobalSettings.AudioChunkSize) < 0) _ = new NCException($"Error initialising audio device: {SDL_GetError()}", 56, "Failed to initialise audio device during Lightning::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Validating system requirements...");
                GlobalSettings.Validate();

                NCLogging.Log("Initialising LocalisationManager...");
                LocalisationManager.Load();

                if (GlobalSettings.GeneralProfilePerformance)
                {
                    NCLogging.Log("Performance Profiler enabled, initialising profiler...");
                    PerformanceProfiler.Start();
                }

                // load global settings package file if init settings one was not specified
                if (GlobalSettings.GeneralPackageFile != null)
                {
                    NCLogging.Log($"User specified package file {GlobalSettings.GeneralPackageFile} to load, loading it...");

                    // set default content folder
                    if (GlobalSettings.GeneralContentFolder == null) GlobalSettings.GeneralContentFolder = "Content";
                    if (!Packager.LoadPackage(GlobalSettings.GeneralPackageFile, GlobalSettings.GeneralContentFolder)) _ = new NCException($"An error occurred loading {GlobalSettings.GeneralPackageFile}. The game cannot be loaded.", 12, "Packager::LoadPackager returned false", NCExceptionSeverity.FatalError);
                }

                // Load LocalSettings
                if (GlobalSettings.GeneralLocalSettingsPath != null)
                {
                    NCLogging.Log($"Loading local settings from {GlobalSettings.GeneralLocalSettingsPath}...");
                    LocalSettings.Load();
                }

                // Load the scene manager.
                InitSceneManager(new RendererSettings
                {
                    Position = new Vector2(GlobalSettings.GraphicsPositionX, GlobalSettings.GraphicsPositionY),
                    Size = new Vector2(GlobalSettings.GraphicsResolutionX, GlobalSettings.GraphicsResolutionY),
                    WindowFlags = GlobalSettings.GraphicsWindowFlags,
                    RenderFlags = GlobalSettings.GraphicsRenderFlags,
                    Title = GlobalSettings.GraphicsWindowTitle
                });

                // if scenemanager started successfully, run its main loop
                if (Initialised)
                {
                    Initialised = true;
                    Main();
                }
            }
            catch (Exception err)
            {
                _ = new NCException($"An unknown fatal error occurred. The installation may be corrupted", 0x0000DEAD, "A fatal error occurred in LightningGL::Init!", NCExceptionSeverity.FatalError, err);
            }
        }

        private static void InitLogging()
        {
            NCLogging.Settings = new NCLoggingSettings
            {
                WriteToLog = true,
            };

            NCLogging.Init();
        }

        public static void Shutdown()
        {
            if (!Initialised) _ = new NCException("Attempted to shutdown without starting! Please call Lightning::Init!", 95, "Lightning::Initialised false when calling Lightning::Shutdown", NCExceptionSeverity.FatalError);

            if (GlobalSettings.GeneralProfilePerformance)
            {
                NCLogging.Log("Stopping performance profiling...");
                PerformanceProfiler.Shutdown();
            }

            NCLogging.Log("Shutting down the Scene Manager...");
            ShutdownAll();

            NCLogging.Log("Destroying renderer...");
            Renderer.Shutdown();

            NCLogging.Log("Shutting down the Text Manager...");
            TextManager.Shutdown();

            // Shut down the light manager if it has been started.
            NCLogging.Log("Shutting down the Light Manager...");
            if (LightManager.Initialised) LightManager.Shutdown();


            // Clear up any unpacked package data if Engine.ini specifies to
            Packager.Shutdown(GlobalSettings.GeneralDeleteUnpackedFilesOnExit);

            // Save settings if we have to
            if (!GlobalSettings.GeneralDontSaveLocalSettingsOnShutdown
                && LocalSettings.WasChanged)
            {
                NCLogging.Log("Saving local settings as they were changed...");
                LocalSettings.Save();
            }

            // Shut all SDL libraries down in reverse order.
            NCLogging.Log("Shutting down SDL_ttf...");
            TTF_Quit();

            NCLogging.Log("Shutting down SDL_mixer...");
            Mix_Quit();

            NCLogging.Log("Shutting down SDL_image..");
            IMG_Quit();

            NCLogging.Log("Shutting down SDL...");
            SDL_Quit();
        }

        /// <summary>
        /// Initialises the Scene Manager.
        /// </summary>
        /// <param name="windowSettings">The window settings to use for the Scene Manager.</param>
        /// <exception cref="NCException">An error occurred initialising the Scene Manager.</exception>
        internal static void InitSceneManager(RendererSettings windowSettings)
        {
            Renderer.Start(windowSettings);

            // Initialise the scenes.
            Assembly? assembly = Assembly.GetEntryAssembly();

            Debug.Assert(assembly != null); // this should not happen

            foreach (Type t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(Scene)))
                {
                    Scene? scene = (Scene?)Activator.CreateInstance(t);

                    if (scene != null)
                    {
                        Scenes.Add(scene);

                        NCLogging.Log($"Initialising scene {scene.Name}...");

                        scene.Start();

                        if (GlobalSettings.SceneStartupScene == t.Name) CurrentScene = scene;
                    }
                    else
                    {
                        _ = new NCException($"Error initialising SceneManager: Failed to create scene instance!", 130,
                        "Scene initialisation failed in SceneManager::Init", NCExceptionSeverity.FatalError);
                    }

                }
            }

            if (Scenes.Count == 0) _ = new NCException($"There are no scenes defined.\n\nIf you tried to initialise Lightning without the Scene Manager," +
                $" this is no longer supported as of Lightning 1.2.0!", 131, 
                "SceneManager::Scenes Count = 0!", NCExceptionSeverity.FatalError);

            if (CurrentScene == null) _ = new NCException($"Invalid startup scene {GlobalSettings.SceneStartupScene}", 132, 
                "GlobalSettings::StartupScene did not correspond to a valid scene", NCExceptionSeverity.FatalError);

            Initialised = true;
        }

        /// <summary>
        /// Internal: Main loop for the Scene Manager.
        /// </summary>
        internal static void Main()
        {
            Debug.Assert(Renderer != null);
            Debug.Assert(CurrentScene != null);

            while (Renderer.Run())
            {
                // Only put events you need to GLOBALLY handle here.
                // Every other event will be handled by the renderer or dev
                if (Renderer.EventWaiting)
                {
                    switch (Renderer.LastEvent.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            ShutdownAll();
                            break;
                    }
                }

                // Render the current scene.
                CurrentScene.Render();

                Renderer.Render();
            }
        }

        internal static void ShutdownAll()
        {
            NCLogging.Log("Shutting down all scenes...");
            foreach (Scene scene in Scenes)
            {
                // shutdown every scene
                NCLogging.Log($"Shutting down scene {scene.Name}...");
                scene.Shutdown();
            }
        }

        /// <summary>
        /// Sets the current scene to the scene <see cref="Scene"/>.
        /// </summary>
        /// <param name="newScene">The new <see cref="Scene"/> to set to be the current scene.</param>
        public static void SetCurrentScene(Scene newScene)
        {
            Debug.Assert(CurrentScene != null); // you should never get here

            NCLogging.Log($"Setting scene to {newScene.Name}...");
            CurrentScene.SwitchAway(newScene);
            newScene.SwitchTo(CurrentScene);
            CurrentScene = newScene;
        }

        /// <summary>
        /// Sets the current scene to the scene <see cref="Scene"/>.
        /// </summary>
        /// <param name="newScene">The new <see cref="Scene"/> to set to be the current scene.</param>
        public static void SetCurrentScene(string name)
        {
            Scene? scene = GetScene(name);

            if (scene == null)
            {
                _ = new NCException($"Tried to set invalid scene {name}!", 133, "Called SceneManager::GetCurrentScene with an invalid scene name");
                return;
            }

            SetCurrentScene(scene);
        }

        /// <summary>
        /// Gets the current scene.
        /// </summary>
        /// <param name="name">The <see cref="Scene.Name"/> of the scene</param>
        /// <returns>A <see cref="Scene"/> object containing the first scene with the name <paramref name="name"/>, or <c>null</c> if there is no scene by that name.</returns>
        public static Scene? GetScene(string name)
        {
            foreach (Scene scene in Scenes)
            {
                if (scene.Name == name)
                {
                    return scene;
                }
            }

            return null;
        }
    }
}