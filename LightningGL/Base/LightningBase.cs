
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
            LightManager = new LightAssetManager();
        }

        #region Asset managers
        public static TextureAssetManager TextureManager { get; private set; } // init not valid on static members
        public static LightAssetManager LightManager { get; private set; }

        #endregion

        internal virtual void Init()
        {
            try
            {
                Logger.Log("Initialising core engine...");
                
                // Log the sign-on message
                Logger.Log("Lightning Game Engine", ConsoleColor.Blue);
                Logger.Log($"Version {LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING}", ConsoleColor.Blue);

#if DEBUG
                Logger.Log("Debug build (Pre-release - do not distribute!)", ConsoleColor.Yellow);
#elif RELEASE
                Logger.Log("Release build (Pre-release - do not distribute!)", ConsoleColor.Yellow);
#elif PROFILING
                Logger.Log("Release build with Enhanced Profiling (Pre-release - do not distribute!)", ConsoleColor.Magenta);
#elif FINAL
                Logger.Log("Final build!", ConsoleColor.Green);
#endif

                // we use sdl for non-rendering tasks in all cases
                Logger.Log("Initialising SDL...");
                if (SDL_Init(SDL_InitFlags.SDL_INIT_EVERYTHING) < 0) Logger.LogError($"Error initialising SDL2: {SDL_GetError()}", 200, LoggerSeverity.FatalError);

                // this should always be the earliest step
                Logger.Log("Obtaining system information...");
                SystemInfo.Load();

                // this should always be the second earliest step
                Logger.Log("Loading global settings from Engine.ini...");
                GlobalSettings.Load();

                Logger.Log("Performing device detection for available input methods...");
                InputMethodManager.ScanAvailableInputMethods(true);

                Logger.Log("Validating system requirements...");
                GlobalSettings.Validate();

                Logger.Log("Initialising LocalisationManager...");
                LocalisationManager.Load();

                // load global settings package file if init settings one was not specified
                if (GlobalSettings.GeneralPackageFile != null)
                {
                    Logger.Log($"User specified package file {GlobalSettings.GeneralPackageFile} to load, loading it...");

                    // set default content folder
                    GlobalSettings.GeneralContentFolder ??= "Content";
                    if (!Packager.LoadPackage(GlobalSettings.GeneralPackageFile, GlobalSettings.GeneralContentFolder)) Logger.LogError($"An error occurred loading " +
                        $"{GlobalSettings.GeneralPackageFile}. The game cannot be loaded.", 12, LoggerSeverity.FatalError);
                }

                // Load LocalSettings
                if (GlobalSettings.GeneralLocalSettingsPath != null)
                {
                    Logger.Log($"Loading local settings from {GlobalSettings.GeneralLocalSettingsPath}...");
                    LocalSettings.Load();
                }
            }
            catch (Exception err)
            {
                Logger.LogError($"An unknown fatal error occurred. The engine installation may be corrupted!", 0x0000DEAD, LoggerSeverity.FatalError, err);
            }
        }

        public virtual void Shutdown()
        {
            if (!Initialised) Logger.LogError("Attempted to shutdown without starting! Please call Client::Init or Server::Init!", 95, LoggerSeverity.FatalError);

            if (GlobalSettings.GeneralProfilePerformance)
            {
                Logger.Log("Stopping performance profiling...");
                PerformanceProfiler.Shutdown();
            }

            Logger.Log("Shutting down the Scene Manager...");
            ShutdownAll();

            Logger.Log("Freeing GlyphCache glyphs (Text Manager)...");
            GlyphCache.Shutdown();

            Logger.Log("Shutting down renderer...");
            Renderer.Shutdown();

            // Shut down the light manager if it has been started.
            Logger.Log("Shutting down the Light Manager...");
            if (LightManager.Initialised) LightManager.Shutdown();

            // Clear up any unpacked package data if Engine.ini specifies to
            Packager.Shutdown(GlobalSettings.GeneralDeleteUnpackedFilesOnExit);

            // Save settings if we have to
            if (!GlobalSettings.GeneralDontSaveLocalSettingsOnShutdown
                && LocalSettings.WasChanged)
            {
                Logger.Log("Saving local settings as they were changed...");
                LocalSettings.Save();
            }

            if (!GlobalSettings.GeneralDontSaveLocalSettingsOnShutdown)
            {
                Logger.Log("Saving global settings...");
                GlobalSettings.Save();
            }
            
            Logger.Log("Shutting down SDL...");
            SDL_Quit();
        }

        /// <summary>
        /// Initialises the Scene Manager.
        /// </summary>
        /// <param name="windowSettings">The window settings to use for the Scene Manager.</param>
        internal virtual void InitSceneManager(RendererSettings windowSettings)
        {
            Logger.Log("Initialising renderer...");
            Renderer.Settings = windowSettings;
            Renderer.Start();

            // Initialise the scenes.
            Assembly? assembly = Assembly.GetEntryAssembly();

            Debug.Assert(assembly != null); // this should not happen

            Scene? startupScene = null;

            foreach (Type t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(Scene)))
                {
                    Scene? scene = (Scene?)Activator.CreateInstance(t);

                    if (scene != null)
                    {
                        Scenes.Add(scene);

                        Logger.Log($"Initialising scene {scene.Name}...");

                        scene.Start();

                        if (GlobalSettings.SceneStartupScene == t.Name) startupScene = scene;
                    }
                    else
                    {
                        Logger.LogError($"Error initialising SceneManager: Failed to create scene instance!", 130, LoggerSeverity.FatalError);
                    }

                }
            }

            if (Scenes.Count == 0)
            {
                Logger.LogError($"There are no scenes defined.\n\nIf you tried to initialise Lightning without the Scene Manager," +
                $" this is no longer supported as of Lightning 2.0.0!", 131, LoggerSeverity.FatalError);
                return;
            }

            if (startupScene == null)
            {
                Logger.LogError($"Invalid startup scene {GlobalSettings.SceneStartupScene}", 132, LoggerSeverity.FatalError);
                return;
            }

            SetCurrentScene(startupScene);
            
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
            Logger.Log("Shutting down all scenes...");
            foreach (Scene scene in Scenes)
            {
                // shutdown every scene
                Logger.Log($"Shutting down scene {scene.Name}...");
                scene.Shutdown();
            }
        }

        /// <summary>
        /// Sets the current scene to the scene <see cref="Scene"/>.
        /// </summary>
        /// <param name="newScene">The new <see cref="Scene"/> to set to be the current scene.</param>
        public virtual void SetCurrentScene(Scene newScene)
        {
            Logger.Log($"Switching to scene {newScene.Name}...");

            if (CurrentScene != null)
            {
                CurrentScene.SwitchFrom(newScene);
                EventManager.FireOnSwitchFromScene(CurrentScene, newScene);
                newScene.SwitchTo(CurrentScene);
            }
            else // startup scene, so no scene to switch from
            {
                newScene.SwitchTo(null);
                EventManager.FireOnSwitchToScene(null, newScene);
            }

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
                Logger.LogError($"Tried to set invalid scene {name}!", 133, LoggerSeverity.FatalError);
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
