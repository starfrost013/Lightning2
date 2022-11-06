using LightningBase;

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

        public static TextAssetManager TextManager { get; private set; }

        public static LightAssetManager LightManager { get; private set; }

        public static SceneAssetManager SceneManager { get; private set; }

        public static AnimationAssetManager AnimationManager { get; private set; }

        public static PrimitiveAssetManager PrimitiveManager { get; private set; } 
        #endregion

        static Lightning()
        {
            // Initialise all asset managers
            TextureManager = new TextureAssetManager();
            AudioManager = new AudioAssetManager();
            ParticleManager = new ParticleAssetManager();
            UIManager = new UIAssetManager();
            FontManager = new FontAssetManager();
            TextManager = new TextAssetManager();
            LightManager = new LightAssetManager();
            SceneManager = new SceneAssetManager();
            AnimationManager = new AnimationAssetManager();
            PrimitiveManager = new PrimitiveAssetManager();
        }

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
                SceneManager.Init(new RendererSettings
                {
                    Position = new Vector2(GlobalSettings.GraphicsPositionX, GlobalSettings.GraphicsPositionY),
                    Size = new Vector2(GlobalSettings.GraphicsResolutionX, GlobalSettings.GraphicsResolutionY),
                    WindowFlags = GlobalSettings.GraphicsWindowFlags,
                    RenderFlags = GlobalSettings.GraphicsRenderFlags,
                    Title = GlobalSettings.GraphicsWindowTitle
                });

                // if scenemanager started successfully, run its main loop
                if (SceneManager.Initialised)
                {
                    Initialised = true;
                    SceneManager.Main();
                }
            }
            catch (Exception err)
            {
                _ = new NCException($"An unknown fatal error occurred during engine initialisation. The installation may be corrupted", 0x0000DEAD, "A fatal error occurred in LightningGL::Init!", NCExceptionSeverity.FatalError, err);
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

        public static void Shutdown(Renderer cRenderer)
        {
            if (!Initialised) _ = new NCException("Attempted to shutdown without starting! Please call Lightning::Init!", 95, "Lightning::Initialised false when calling Lightning::Shutdown", NCExceptionSeverity.FatalError);

            if (GlobalSettings.GeneralProfilePerformance)
            {
                NCLogging.Log("Stopping performance profiling...");
                PerformanceProfiler.Shutdown();
            }

            NCLogging.Log("Shutting down the Scene Manager...");
            SceneManager.ShutdownAll();

            NCLogging.Log("Destroying renderer...");
            cRenderer.Shutdown();

            NCLogging.Log("Shutting down the Text Manager...");
            TextManager.Shutdown();

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
    }
}