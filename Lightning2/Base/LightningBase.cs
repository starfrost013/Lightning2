
namespace LightningGL
{
    /// <summary>
    /// LightningBase
    /// 
    /// The base class for the Lightning client and server.
    /// </summary>
    public class LightningBase
    {
        /// <summary>
        /// The current scene that is being run.
        /// </summary>
        public static Scene? CurrentScene { get; protected set; }

        /// <summary>
        /// The main renderer of the application.
        /// </summary>
        public static Renderer Renderer { get; protected set; }

        /// <summary>
        /// Determines if the scene manager is running.
        /// </summary>
        internal static bool Initialised { get; set; }

        /// <summary>
        /// The list of scenes.
        /// </summary>
        public static List<Scene> Scenes { get; protected set; }

        static LightningBase()
        {
            // Initialise SDL renderer as a default.
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

        public virtual void Init()
        {
            try
            {
                NCLogging.Log("Lightning initialising...");
                
                // Log the sign-on message
                NCLogging.Log($"Lightning Game Engine");
                NCLogging.Log($"Version {LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING}");

                NCLogging.Log("Initialising SDL...");
                if (SDL_Init(SDL_InitFlags.SDL_INIT_EVERYTHING) < 0) NCError.ShowErrorBox($"Error initialising SDL2: {SDL_GetError()}", 200,
                    "Failed to initialise SDL2 during Lightning::Init", NCErrorSeverity.FatalError);

                NCLogging.Log("Initialising SDL_image...");
                if (IMG_Init(IMG_InitFlags.IMG_INIT_EVERYTHING) < 0) NCError.ShowErrorBox($"Error initialising SDL2_image: {SDL_GetError()}", 201,
                    "Failed to initialise SDL2_image during Lightning::Init", NCErrorSeverity.FatalError);

                NCLogging.Log("Initialising SDL_ttf...");
                if (TTF_Init() < 0) NCError.ShowErrorBox($"Error initialising SDL2_ttf: {SDL_GetError()}", 202,
                    "Failed to initialise SDL2_ttf during Lightning::Init", NCErrorSeverity.FatalError);

                NCLogging.Log("Initialising SDL_mixer...");
                if (Mix_Init(MIX_InitFlags.MIX_INIT_EVERYTHING) < 0) NCError.ShowErrorBox($"Error initialising SDL2_mixer: {SDL_GetError()}", 203,
                    "Failed to initialise SDL2_mixer during Lightning::Init", NCErrorSeverity.FatalError);

                // this should always be the earliest step
                NCLogging.Log("Obtaining system information...");
                SystemInfo.Load();

                // this should always be the second earliest step
                NCLogging.Log("Loading global settings from Engine.ini...");
                GlobalSettings.Load();

                NCLogging.Log("Validating system requirements...");
                GlobalSettings.Validate();

                NCLogging.Log("Initialising LocalisationManager...");
                LocalisationManager.Load();

                // load global settings package file if init settings one was not specified
                if (GlobalSettings.GeneralPackageFile != null)
                {
                    NCLogging.Log($"User specified package file {GlobalSettings.GeneralPackageFile} to load, loading it...");

                    // set default content folder
                    GlobalSettings.GeneralContentFolder ??= "Content";
                    if (!Packager.LoadPackage(GlobalSettings.GeneralPackageFile, GlobalSettings.GeneralContentFolder)) NCError.ShowErrorBox($"An error occurred loading {GlobalSettings.GeneralPackageFile}. The game cannot be loaded.", 12, "Packager::LoadPackager returned false", NCErrorSeverity.FatalError);
                }

                // Load LocalSettings
                if (GlobalSettings.GeneralLocalSettingsPath != null)
                {
                    NCLogging.Log($"Loading local settings from {GlobalSettings.GeneralLocalSettingsPath}...");
                    LocalSettings.Load();
                }
            }
            catch (Exception err)
            {
                NCError.ShowErrorBox($"An unknown fatal error occurred. The installation may be corrupted", 0x0000DEAD, "A fatal error occurred in LightningGL::Init!", NCErrorSeverity.FatalError, err);
            }
        }

        public virtual void Shutdown()
        {
            if (!Initialised) NCError.ShowErrorBox("Attempted to shutdown without starting! Please call Lightning::Init!", 95, "Lightning::Initialised false when calling Lightning::Shutdown", NCErrorSeverity.FatalError);

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
        /// <exception cref="NCError">An error occurred initialising the Scene Manager.</exception>
        internal virtual void InitSceneManager(RendererSettings windowSettings)
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
                        NCError.ShowErrorBox($"Error initialising SceneManager: Failed to create scene instance!", 130,
                        "Scene initialisation failed in SceneManager::Init", NCErrorSeverity.FatalError);
                    }

                }
            }

            if (Scenes.Count == 0) NCError.ShowErrorBox($"There are no scenes defined.\n\nIf you tried to initialise Lightning without the Scene Manager," +
                $" this is no longer supported as of Lightning 1.2.0!", 131,
                "SceneManager::Scenes Count = 0!", NCErrorSeverity.FatalError);

            if (CurrentScene == null) NCError.ShowErrorBox($"Invalid startup scene {GlobalSettings.SceneStartupScene}", 132,
                "GlobalSettings::StartupScene did not correspond to a valid scene", NCErrorSeverity.FatalError);

            Initialised = true;
        }

        /// <summary>
        /// Internal: Main loop for the Scene Manager.
        /// </summary>
        internal virtual void Main()
        {
            Debug.Assert(Renderer != null);
            Debug.Assert(CurrentScene != null);

            while (Renderer.Run())
            {
                // Render the current scene.
                CurrentScene.Render();

                Renderer.Render();
            }
        }

        internal virtual void ShutdownAll()
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
        public virtual void SetCurrentScene(Scene newScene)
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
        public virtual void SetCurrentScene(string name)
        {
            Scene? scene = GetScene(name);

            if (scene == null)
            {
                NCError.ShowErrorBox($"Tried to set invalid scene {name}!", 133, "Called SceneManager::GetCurrentScene with an invalid scene name");
                return;
            }

            SetCurrentScene(scene);
        }

        /// <summary>
        /// Gets the current scene.
        /// </summary>
        /// <param name="name">The <see cref="Scene.Name"/> of the scene</param>
        /// <returns>A <see cref="Scene"/> object containing the first scene with the name <paramref name="name"/>, or <c>null</c> if there is no scene by that name.</returns>
        public virtual Scene? GetScene(string name)
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
