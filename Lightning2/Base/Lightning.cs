namespace LightningGL
{
    /// <summary>
    /// Lightning
    /// A lightweight, easy-to-use, and elegantly designed C# game framework
    /// 
    /// © 2022 starfrost
    /// </summary>
    public class Lightning
    {
        /// <summary>
        /// Determines if the engine has been initialised correctly.
        /// </summary>
        public static bool Initialised { get; private set; }

        #region Asset managers
        public static TextureAssetManager TextureManager { get; private set; } // init not valid on static members

        public static AudioAssetManager AudioManager { get; private set; }

        public static ParticleAssetManager ParticleManager { get; private set; }

        public static UIAssetManager UIManager { get; private set; }

        public static FontAssetManager FontManager { get; private set; }

        public static LightAssetManager LightManager { get; private set; }

        public static SceneAssetManager SceneManager { get; private set; }

        public static AnimationAssetManager AnimationManager { get; private set; }
        #endregion

        static Lightning()
        {
            // Initialise all asset managers
            TextureManager = new TextureAssetManager();
            AudioManager = new AudioAssetManager();
            ParticleManager = new ParticleAssetManager();
            UIManager = new UIAssetManager();
            FontManager = new FontAssetManager();
            LightManager = new LightAssetManager();
            SceneManager = new SceneAssetManager();
            AnimationManager = new AnimationAssetManager();
        }

        public static void Init(string[] args)
        {
            try
            {
                // Set culture to invariant so things like different decimal symbols don't crash
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                Init_InitLogging();

                // Log the sign-on message
                NCLogging.Log($"Lightning Game Engine");
                NCLogging.Log($"Version {LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING}");

                NCLogging.Log("Parsing command-line arguments...");
                if (!InitSettings.Parse(args)) _ = new NCException($"An error occurred while parsing command-line arguments.", 103, "InitSettings::Parse returned false", NCExceptionSeverity.FatalError);

                if (InitSettings.PackageFile != null)
                {
                    NCLogging.Log($"User specified package file {InitSettings.PackageFile} to load, loading it...");

                    if (!Packager.LoadPackage(InitSettings.PackageFile, InitSettings.ContentFolder)) _ = new NCException($"An error occurred loading {InitSettings.PackageFile}. The game cannot be loaded.", 104, "Packager::LoadPackager returned false", NCExceptionSeverity.FatalError);
                }

                NCLogging.Log("Initialising SDL...");
                if (SDL_Init(SDL_InitFlags.SDL_INIT_EVERYTHING) < 0) _ = new NCException($"Error initialising SDL2: {SDL_GetError()}", 0, "Failed to initialise SDL2 during Lightning::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising SDL_image...");
                if (IMG_Init(IMG_InitFlags.IMG_INIT_EVERYTHING) < 0) _ = new NCException($"Error initialising SDL2_image: {SDL_GetError()}", 1, "Failed to initialise SDL2_image during Lightning::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising SDL_ttf...");
                if (TTF_Init() < 0) _ = new NCException($"Error initialising SDL2_ttf: {SDL_GetError()}", 2, "Failed to initialise SDL2_ttf during Lightning::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising SDL_mixer...");
                if (Mix_Init(MIX_InitFlags.MIX_INIT_EVERYTHING) < 0) _ = new NCException($"Error initialising SDL2_mixer: {SDL_GetError()}", 3, "Failed to initialise SDL2_mixer during Lightning::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising audio device (44Khz, stereo)...");
                if (Mix_OpenAudio(44100, Mix_AudioFormat.MIX_DEFAULT_FORMAT, 2, 2048) < 0) _ = new NCException($"Error initialising audio device: {SDL_GetError()}", 56, "Failed to initialise audio device during Lightning::Init", NCExceptionSeverity.FatalError);

                // this should always be the earliest step
                NCLogging.Log("Obtaining system information...");
                SystemInfo.Load();

                // this should always be the second earliest step
                NCLogging.Log("Loading Engine.ini...");
                GlobalSettings.Load();

                NCLogging.Log("Validating system requirements...");
                GlobalSettings.Validate();

                NCLogging.Log("Initialising LocalisationManager...");
                LocalisationManager.Load();

                if (GlobalSettings.ProfilePerformance)
                {
                    NCLogging.Log("Performance Profiler enabled, initialising profiler...");
                    PerformanceProfiler.Start();
                }

                // load global settings package file if init settings one was not specified
                if (InitSettings.PackageFile == null
                    && GlobalSettings.PackageFile != null)
                {
                    NCLogging.Log($"User specified package file {GlobalSettings.PackageFile} to load, loading it...");

                    // set default content folder
                    if (GlobalSettings.ContentFolder == null) GlobalSettings.ContentFolder = "Content";
                    if (!Packager.LoadPackage(GlobalSettings.PackageFile, GlobalSettings.ContentFolder)) _ = new NCException($"An error occurred loading {GlobalSettings.PackageFile}. The game cannot be loaded.", 12, "Packager::LoadPackager returned false", NCExceptionSeverity.FatalError);
                }

                // Load LocalSettings
                if (GlobalSettings.LocalSettingsPath != null)
                {
                    NCLogging.Log($"Loading local settings from {GlobalSettings.LocalSettingsPath}...");
                    LocalSettings.Load();
                }

                // Load the Scene Manager
                // This should ALWAYS be the last thing initialised
                if (!GlobalSettings.DontUseSceneManager)
                {
                    Initialised = true;
                    SceneManager.Init(new RendererSettings
                    {
                        Position = new Vector2(GlobalSettings.PositionX, GlobalSettings.PositionY),
                        Size = new Vector2(GlobalSettings.ResolutionX, GlobalSettings.ResolutionY),
                        WindowFlags = GlobalSettings.WindowFlags,
                        RenderFlags = GlobalSettings.RenderFlags,
                        Title = GlobalSettings.WindowTitle
                    });
                    SceneManager.Main();
                }
                else
                {
                    NCLogging.Log("Warning: Initialising Lightning without the Scene Manager is deprecated and will be removed in Lightning 1.2.0.", ConsoleColor.Yellow);
                    Initialised = true;
                }

            }
            catch (Exception err)
            {
                _ = new NCException($"An unknown fatal error occurred during engine initialisation. The installation may be corrupted", 0x0000DEAD, "A fatal error occurred in LightningGL::Init!", NCExceptionSeverity.FatalError, err);
            }
        }

        private static void Init_InitLogging()
        {
            NCLogging.Settings = new NCLoggingSettings
            {
                WriteToLog = true,
            };
            
            NCLogging.Init();
        }

        public static void Shutdown(Renderer cRenderer)
        {
            if (!Initialised) _ = new NCException("Attempted to shutdown without starting! Please call LightningGL::Init!", 95, "LightningGL::Initialised false when calling Lightning2::Shutdown", NCExceptionSeverity.FatalError);
            NCLogging.Log("Shutdown requested.");

            if (GlobalSettings.ProfilePerformance)
            {
                NCLogging.Log("Stopping performance profiling...");
                PerformanceProfiler.Shutdown();
            }

            NCLogging.Log("Destroying renderer...");
            cRenderer.Shutdown();

            NCLogging.Log("Shutting down the Font Manager...");
            FontManager.Shutdown();

            // create a list of fonts and audiofiles to unload
            // just foreaching through each font and audiofile doesn't work as collection is being modified 
            List<AudioFile> audioFilesToUnload = new();
            foreach (AudioFile audioFileToUnload in AudioManager.Assets) audioFilesToUnload.Add(audioFileToUnload);

            NCLogging.Log("Unloading all audio files...");
            foreach (AudioFile audioFileToUnload in audioFilesToUnload) AudioManager.UnloadFile(audioFileToUnload);

            // Shut down the light manager if it has been started.
            NCLogging.Log("Shutting down the Light Manager...");
            if (LightManager.Initialised) LightManager.Shutdown();

            // Shut down the particle manager.
            NCLogging.Log("Shutting down the Particle Manager...");
            ParticleManager.Shutdown();

            // Clear up any unpacked package data if Engine.ini specifies such
            NCLogging.Log("Cleaning up loaded package files, if any...");
            Packager.Shutdown(GlobalSettings.DeleteUnpackedFilesOnExit);

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
    }
}