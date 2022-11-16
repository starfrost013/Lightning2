namespace LightningGL
{
    /// <summary>
    /// Server
    /// 
    /// The Lightning server.
    /// </summary>
    public class Server : LightningBase
    {
        public bool Running { get; set; }
        public LNetServer NetworkManager { get; set; }

        public Server() : base()
        {
            NetworkManager = new();
        }

        public override void Init()
        {
            // figure out what shared code can be used here

            NCLogging.Log("Lightning Server initialising...");

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

            NCLogging.Log($"Initialising audio device ({GlobalSettings.AudioDeviceHz}Hz, {GlobalSettings.AudioChannels} channels, format {GlobalSettings.AudioFormat}, chunk size {GlobalSettings.AudioChunkSize})...");
            if (Mix_OpenAudio(GlobalSettings.AudioDeviceHz, GlobalSettings.AudioFormat, GlobalSettings.AudioChannels, GlobalSettings.AudioChunkSize) < 0) NCError.ShowErrorBox($"Error initialising audio device: {SDL_GetError()}", 56, "Failed to initialise audio device during Lightning::Init", NCErrorSeverity.FatalError);

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

            NCLogging.Log($"Server starting on port: {GlobalSettings.NetworkDefaultPort}", "Server");
            NetworkManager.BeginReceive();
        }

        internal override void Main()
        {
            while (Running)
            {
                // server is still alive
                NCLogging.Log("The server is still alive. wow.");
            }
        }

        public override void Shutdown()
        {
            Running = false;
            NetworkManager.Shutdown();
            
        }
    }
}
