using static LightningBase.SDL;
using static LightningBase.SDL_mixer;
using NuCore.Utilities;
using System;
using System.IO;

namespace LightningBase
{
    /// <summary>
    /// GlobalSettings
    /// 
    /// March 11, 2022
    /// 
    /// Defines the global settings, located in Engine.ini
    /// </summary>
    public static class GlobalSettings
    {
        /// <summary>
        /// Contains the path to the global settings INI file.
        /// </summary>
        internal static string GLOBALSETTINGS_PATH = @"Content\Engine.ini";

        /// <summary>
        /// The INI file object containing the Global Settings.
        /// </summary>
        internal static NCINIFile IniFile { get; private set; }

        #region General settings
        /// <summary>
        /// The file to use when loading localisation strings.
        /// </summary>
        public static string GeneralLanguage { get; internal set; }

        /// <summary>
        /// The localisation folder to use 
        /// </summary>
        public static string GeneralLocalisationFolder { get; internal set; } 

        /// <summary>
        /// Determines whether the performance profiler will be loaded or not.
        /// </summary>
        public static bool GeneralProfilePerformance { get; internal set; }

        /// <summary>
        /// Bring up the "About Lightning" messagebox when the Shift-F9 combination is pressed.
        /// </summary>
        public static bool GeneralEngineAboutScreenOnShiftF9 { get; internal set; }

        /// <summary>
        /// Delete files that have been uncompressed from the WAD on exit.
        /// </summary>
        public static bool GeneralDeleteUnpackedFilesOnExit { get; internal set; }

        /// <summary>
        /// Save the <see cref="LocalSettings"/> on engine shutdown if they have been changed.
        /// </summary>
        public static bool GeneralDontSaveLocalSettingsOnShutdown { get; internal set; }

        /// <summary>
        /// Path to the LocalSettings.ini file.
        /// </summary>
        public static string GeneralLocalSettingsPath { get; internal set; }

        /// <summary>
        /// Path to the package file that is to be loaded.
        /// </summary>
        public static string GeneralPackageFile { get; internal set; }

        /// <summary>
        /// The content folder to use for the game.
        /// </summary>
        public static string GeneralContentFolder { get; set; }

        /// <summary>
        /// Determines if the Scene Manager will be turned off.
        /// </summary>
        public static bool GeneralDontUseSceneManager { get; internal set; }

        /// <summary>
        /// Determines if the FPS rate will be shown.
        /// </summary>
        public static bool GeneralShowDebugInfo { get; internal set; }

        /// <summary>
        /// Enable the console. 
        /// </summary>
        public static bool GeneralEnableConsole { get; internal set; }

        #endregion

        #region Graphics settings
        /// <summary>
        /// The target FPS.
        /// </summary>
        public static int GraphicsMaxFPS { get; internal set; }

        /// <summary>
        /// See <see cref="RendererSettings.WindowFlags"/>
        /// </summary>
        public static SDL_WindowFlags GraphicsWindowFlags { get; internal set; }

        /// <summary>
        /// See <see cref="RendererSettings.RenderFlags"/>
        /// </summary>
        public static SDL_RendererFlags GraphicsRenderFlags { get; internal set; }

        /// <summary>
        /// The X component of the window resolution.
        /// </summary>
        public static int GraphicsResolutionX { get; set; }

        /// <summary>
        /// The Y component of the window resolution.
        /// </summary>
        public static int GraphicsResolutionY { get; set; }

        /// <summary>
        /// Default window position X. Default is (screen resolution / 2) - size.
        /// </summary>
        public static int GraphicsPositionX { get; set; }

        /// <summary>
        /// Default window position Y. Default is (screen resolution / 2) - size.
        /// </summary>
        public static int GraphicsPositionY { get; set; }

        /// <summary>
        /// The title of the Window
        /// </summary>
        public static string GraphicsWindowTitle { get; internal set; }

        /// <summary>
        /// The rendering backend to use. Default is <see cref="Renderer.OpenGL"/>
        /// </summary>
        public static RenderingBackend GraphicsRendererType { get; internal set; }

        /// <summary>
        /// Delta-time / tick speed multiplier
        /// Default is 1.0
        /// </summary>
        public static int GraphicsTickSpeed { get; internal set; }

        /// <summary>
        /// Determines if offscreen <see cref="Renderables"/> will be culled from the rendering or not.
        /// </summary>
        public static bool GraphicsRenderOffScreenRenderables { get; internal set; }

        #endregion

        #region System requirements

        /// <summary>
        /// Minimum ram in MiB (mebibytes)
        /// </summary>
        public static int RequirementsMinimumSystemRam { get; internal set; }

        /// <summary>
        /// The minimum number of logical processors. This is *not* CPU cores.
        /// If hyperthreading is enabled there will be two of these per core!
        /// </summary>
        public static int RequirementsMinimumLogicalProcessors { get; internal set; }

        /// <summary>
        /// Required CPU features - <see cref="SystemInfoCPUCapabilities"/>
        /// </summary>
        public static SystemInfoCPUCapabilities RequirementsMinimumCpuCapabilities { get; internal set; }

        /// <summary>
        /// Minimum operating system to run game.
        /// </summary>
        public static SystemInfoOperatingSystem RequirementsMinimumOperatingSystem { get; internal set; }
        #endregion

        #region Scene settings

        /// <summary>
        /// The startup scene name.
        /// </summary>
        public static string SceneStartupScene { get; set; }
        #endregion

        #region Audio settings

        /// <summary>
        /// <para>Default frequency of the audio device created for Lightning audio.</para>
        /// <para>The default value is 44100.</para>
        /// </summary>
        public static int AudioDeviceHz { get; set; }

        /// <summary>
        /// <para>The number of audio channels.</para>
        /// <para>The default value is 2.</para>
        /// </summary>
        public static int AudioChannels { get; set; }

        /// <summary>
        /// <para>The <see cref="Mix_AudioFormat"/> used by Lightning's audio device.</para>
        /// <para>The default value is <see cref="Mix_AudioFormat.MIX_DEFAULT_FORMAT"/></para>
        /// </summary>
        public static Mix_AudioFormat AudioFormat { get; set; }

        /// <summary>
        /// <para>The default audio chunk size.</para>
        /// <para>The default value is 2048.</para>
        /// </summary>
        public static int AudioChunkSize { get; set; }
        #endregion

        #region Network settings (to be moved to LightningServer.ini later)

        /// <summary>
        /// <para>The network master server IP address.</para>
        /// <para>The default value is https://lightningpowered.net:7801 - port 7800 is used for game servers and 7801 for the master server</para>
        /// </summary>
        public static string NetworkMasterServer { get; set; }

        /// <summary>
        /// <para>The network default port.</para>
        /// <para>The default value is 7800.</para>
        /// </summary>
        public static ushort NetworkDefaultPort { get; set; }

        /// <summary>
        /// <para>The number of milliseconds required before a client will timeout.</para>
        /// </summary>
        public static int NetworkKeepAliveMs { get; set; }

        #endregion

        #region Default values

        public static int DEFAULT_GRAPHICS_POSITION_X = SystemInfo.ScreenResolutionX / 2 - (GraphicsResolutionX / 2);

        public static int DEFAULT_GRAPHICS_POSITION_Y = SystemInfo.ScreenResolutionY / 2 - (GraphicsResolutionY / 2);

        public const int DEFAULT_GRAPHICS_TICK_SPEED = 1;

        public const int DEFAULT_AUDIO_DEVICE_HZ = 44100;

        public const int DEFAULT_AUDIO_CHANNELS = 2;

        public const Mix_AudioFormat DEFAULT_AUDIO_FORMAT = Mix_AudioFormat.MIX_DEFAULT_FORMAT;

        public const int DEFAULT_AUDIO_CHUNK_SIZE = 2048;

        public const string DEFAULT_NETWORK_MASTER_SERVER = "https://lightningpowered.net:7801";

        public const int DEFAULT_NETWORK_PORT = 7800;

        public const int DEFAULT_NETWORK_KEEP_ALIVE_MS = 500;

        #endregion

        #region GlobalSettings methods
        /// <summary>
        /// Loads the Global Settings.
        /// </summary>
        public static void Load()
        {
            // Possible todo: Serialise these to properties and get rid of the loader/sections
            // Consider this
            NCINIFile iniFile = NCINIFile.Parse(GLOBALSETTINGS_PATH);

            if (iniFile == null) _ = new NCException("Failed to load Engine.ini!", 28, "GlobalSettings::Load failed to load Engine.ini!", NCExceptionSeverity.FatalError);

            NCINIFileSection generalSection = iniFile.GetSection("General");
            NCINIFileSection graphicsSection = iniFile.GetSection("Graphics");
            NCINIFileSection locSection = iniFile.GetSection("Localisation");
            NCINIFileSection requirementsSection = iniFile.GetSection("Requirements");
            NCINIFileSection sceneSection = iniFile.GetSection("Scene");
            NCINIFileSection audioSection = iniFile.GetSection("Audio");
            NCINIFileSection networkSection = iniFile.GetSection("Network");

            if (generalSection == null) _ = new NCException("Engine.ini must have a General section!", 41, 
                "GlobalSettings::Load call to NCINIFile::GetSection failed for General section", NCExceptionSeverity.FatalError);
            if (locSection == null) _ = new NCException("Engine.ini must have a Localisation section!", 29, 
                "GlobalSettings::Load call to NCINIFile::GetSection failed for Localisation section", NCExceptionSeverity.FatalError);

            // Load the Localisation section.
            string language = locSection.GetValue("Language");
            string localisationFolder = locSection.GetValue("LocalisationFolder");

            if (localisationFolder == null)
            {
                GeneralLanguage = @$"Content\Localisation\{language}.ini";
            }
            else
            {
                if (!Directory.Exists(localisationFolder)) _ = new NCException("LocalisationFolder does not exist", 157,
                    "The LocalisationFolder GlobalSetting does not correspond to an extant folder. (GlobalSettings::Load)", NCExceptionSeverity.FatalError);
                GeneralLanguage = @$"{localisationFolder}\{language}.ini";
            }
           
            if (!File.Exists(GeneralLanguage)) _ = new NCException("Engine.ini's Localisation section must have a valid Language value!", 30, 
                "GlobalSettings::Load call to NCINIFileSection::GetValue failed for Language value", NCExceptionSeverity.FatalError);

            // Load the General section.
            string generalShowDebugInfo = generalSection.GetValue("ShowDebugInfo");
            string generalProfilePerf = generalSection.GetValue("ProfilePerformance");
            string generalAboutScreenOnF9 = generalSection.GetValue("EngineAboutScreenOnShiftF9");
            string generalDeleteUnpackedFilesOnExit = generalSection.GetValue("DeleteUnpackedFilesOnExit");
            GeneralLocalSettingsPath = generalSection.GetValue("LocalSettingsPath");
            GeneralPackageFile = generalSection.GetValue("PackageFile");
            GeneralContentFolder = generalSection.GetValue("ContentFolder");
            string generalDontUseSceneManager = generalSection.GetValue("DontUseSceneManager");
            string generalDontSaveLocalSettingsOnShutdown = generalSection.GetValue("DontSaveLocalSettingsOnShutdown");


            // we don't care about the values here
            _ = bool.TryParse(generalShowDebugInfo, out var generalShowDebugInfoValue);
            _ = bool.TryParse(generalProfilePerf, out var generalProfilePerfValue);
            if (!bool.TryParse(generalAboutScreenOnF9, out var generalAboutScreenOnF9Value)) generalAboutScreenOnF9Value = true; // force the default value, true for now
            _ = bool.TryParse(generalDeleteUnpackedFilesOnExit, out var generalDeleteUnpackedFilesOnExitValue);
            _ = bool.TryParse(generalDontUseSceneManager, out var generalDontUseSceneManagerValue);
            _ = bool.TryParse(generalDontSaveLocalSettingsOnShutdown, out var generalDontSaveLocalSettingsOnShutdownValue);

            GeneralShowDebugInfo = generalShowDebugInfoValue;
            GeneralProfilePerformance = generalProfilePerfValue;
            GeneralEngineAboutScreenOnShiftF9 = generalAboutScreenOnF9Value;
            GeneralDeleteUnpackedFilesOnExit = generalDeleteUnpackedFilesOnExitValue;
            GeneralDontUseSceneManager = generalDontUseSceneManagerValue;
            GeneralDontSaveLocalSettingsOnShutdown = generalDontSaveLocalSettingsOnShutdownValue;

            // Load the Graphics section if it exists.
            if (graphicsSection != null)
            {
                string maxFps = graphicsSection.GetValue("MaxFPS");
                string resolutionX = graphicsSection.GetValue("ResolutionX");
                string resolutionY = graphicsSection.GetValue("ResolutionY");
                string positionX = graphicsSection.GetValue("PositionX");
                string positionY = graphicsSection.GetValue("PositionY");
                string windowFlags = graphicsSection.GetValue("WindowFlags");
                string renderFlags = graphicsSection.GetValue("RenderFlags");
                GraphicsWindowTitle = graphicsSection.GetValue("WindowTitle");
                string renderer = graphicsSection.GetValue("Renderer");
                string tickSpeed = graphicsSection.GetValue("TickSpeed");
                string renderOffScreenRenderables = graphicsSection.GetValue("RenderOffScreenRenderables");

                SDL_WindowFlags windowFlagsValue = default;
                SDL_RendererFlags renderFlagsValue = default;
                RenderingBackend rendererValue = default;

                // Convert will throw an exception, int.TryParse will return a boolean for simpler error checking
                _ = int.TryParse(maxFps, out var graphicsMaxFpsValue);

                GraphicsMaxFPS = graphicsMaxFpsValue;

                // inexplicably the overload i used isn't supported for enum.tryparse
                _ = int.TryParse(resolutionX, out var resolutionXValue);
                _ = int.TryParse(resolutionY, out var resolutionYValue);
                _ = Enum.TryParse(windowFlags, true, out windowFlagsValue);
                _ = Enum.TryParse(renderFlags, true, out renderFlagsValue);
                _ = Enum.TryParse(renderer, true, out rendererValue);
                _ = int.TryParse(tickSpeed, out var tickSpeedValue);
                _ = bool.TryParse(renderOffScreenRenderables, out var renderOffscreenRenderablesValue);

                // Set those values.
                GraphicsResolutionX = resolutionXValue;
                GraphicsResolutionY = resolutionYValue;
                GraphicsWindowFlags = windowFlagsValue;
                GraphicsRenderFlags = renderFlagsValue;
                GraphicsRendererType = rendererValue;
                GraphicsRenderOffScreenRenderables = renderOffscreenRenderablesValue;

                // parse positionX/positionY
                _ = int.TryParse(positionX, out var positionXValue);
                _ = int.TryParse(positionY, out var positionYValue);

                // failed to load, set default values (middle of screen)
                if (positionXValue == 0 
                    && positionYValue == 0)
                {
                    positionXValue = DEFAULT_GRAPHICS_POSITION_X;
                    positionYValue = DEFAULT_GRAPHICS_POSITION_Y;
                }

                // set the default delta multiplier value
                if (tickSpeedValue == 0) tickSpeedValue = DEFAULT_GRAPHICS_TICK_SPEED;

                GraphicsTickSpeed = tickSpeedValue;
                GraphicsPositionX = positionXValue;
                GraphicsPositionY = positionYValue;
            }

            // Load the Requirements section if it exists.
            if (requirementsSection != null)
            {
                string minRam = requirementsSection.GetValue("MinimumSystemRam");
                string minLogicalProcessors = requirementsSection.GetValue("MinimumLogicalProcessors");
                string minimumCpuCapabilities = requirementsSection.GetValue("MinimumCpuCapabilities");
                string minimumOperatingSystem = requirementsSection.GetValue("MinimumOperatingSystem");

                SystemInfoCPUCapabilities minimumCpuCapabilitiesValue = 0;
                SystemInfoOperatingSystem minimumOperatingSystemValue = 0;

                _ = int.TryParse(minRam, out var minRamValue);
                _ = int.TryParse(minLogicalProcessors, out var minLogicalProcessorsValue);
                _ = Enum.TryParse(minimumCpuCapabilities, true, out minimumCpuCapabilitiesValue);
                _ = Enum.TryParse(minimumOperatingSystem, true, out minimumOperatingSystemValue);

                RequirementsMinimumSystemRam = minRamValue;
                RequirementsMinimumLogicalProcessors = minLogicalProcessorsValue;
                RequirementsMinimumCpuCapabilities = minimumCpuCapabilitiesValue;
                RequirementsMinimumOperatingSystem = minimumOperatingSystemValue;
            }

            // load the scene section 
            if (!GeneralDontUseSceneManager)
            {
                if (sceneSection == null) _ = new NCException("DontUseSceneManager not specified, but no [Scene] section is present in Engine.ini!", 121, 
                    $"GlobalSettings::DontUseSceneManager not specified, but no [Scene] section in Engine.ini!", NCExceptionSeverity.FatalError);

                SceneStartupScene = sceneSection.GetValue("StartupScene");

                if (SceneStartupScene == null) _ = new NCException("DontUseSceneManager not specified, but StartupScene not present in the [Scene] section of Engine.ini!", 164, 
                    $"GlobalSettings::DontUseSceneManager not specified, but no [Scene] section in Engine.ini!", NCExceptionSeverity.FatalError);
            }

            AudioDeviceHz = DEFAULT_AUDIO_DEVICE_HZ;
            AudioChannels = DEFAULT_AUDIO_CHANNELS;
            AudioFormat = DEFAULT_AUDIO_FORMAT;
            AudioChunkSize = DEFAULT_AUDIO_CHUNK_SIZE;

            // Load the audio settings
            if (audioSection != null)
            {
                string audioDeviceHz = audioSection.GetValue("DeviceHz");
                string audioChannels = audioSection.GetValue("Channels");
                string audioFormat = audioSection.GetValue("Format");
                string audioChunkSize = audioSection.GetValue("ChunkSize");

                Mix_AudioFormat audioFormatValue = DEFAULT_AUDIO_FORMAT;

                _ = int.TryParse(audioDeviceHz, out var audioDeviceHzValue);
                _ = int.TryParse(audioChannels, out var audioChannelsValue);
                if (Enum.TryParse(audioFormat, true, out audioFormatValue)) AudioFormat = audioFormatValue;
                _ = int.TryParse(audioChunkSize, out var audioChunkSizeValue);  

                AudioDeviceHz = audioDeviceHzValue;
                AudioChannels = audioChannelsValue;
                AudioFormat = audioFormatValue;
                AudioChunkSize = audioChunkSizeValue;

                if (AudioDeviceHz <= 0) AudioDeviceHz = DEFAULT_AUDIO_DEVICE_HZ;
                if (AudioChannels <= 0) AudioChannels = DEFAULT_AUDIO_CHANNELS;
                if (AudioChunkSize <= 0) AudioChunkSize = DEFAULT_AUDIO_CHUNK_SIZE;

            }

            // Load the network settings
            if (networkSection != null)
            {
                NetworkMasterServer = DEFAULT_NETWORK_MASTER_SERVER;
                NetworkDefaultPort = DEFAULT_NETWORK_PORT;
                NetworkKeepAliveMs = DEFAULT_NETWORK_KEEP_ALIVE_MS;

                string networkMasterServer = networkSection.GetValue("MasterServer");
                string networkDefaultPort = networkSection.GetValue("Port");
                string networkKeepAliveMs = networkSection.GetValue("KeepAliveMs");

                if (!string.IsNullOrWhiteSpace(networkMasterServer)) NetworkMasterServer = networkMasterServer;

                // don't block http or dns
                if (ushort.TryParse(networkDefaultPort, out var networkDefaultPortValue)
                    && networkDefaultPortValue != 53
                    && networkDefaultPortValue != 80
                    && networkDefaultPortValue != 443) NetworkDefaultPort = networkDefaultPortValue;
                if (int.TryParse(networkKeepAliveMs, out var networkKeepAliveMsValue)
                    && networkKeepAliveMsValue > 0) NetworkKeepAliveMs = networkKeepAliveMsValue;
            }
        }

        /// <summary>
        /// Validates your computer's hardware against the game's system requirements
        /// </summary>
        public static void Validate()
        {
            // test system ram
            if (RequirementsMinimumSystemRam > SystemInfo.SystemRam) _ = new NCException($"Insufficient RAM to run game. {RequirementsMinimumSystemRam}MB required, you have {SystemInfo.SystemRam}MB!", 111, $"System RAM less than GlobalSettings::MinimumSystemRam!", NCExceptionSeverity.FatalError);

            // test threads
            if (RequirementsMinimumLogicalProcessors > SystemInfo.Cpu.Threads) _ = new NCException($"Insufficient logical processors to run game. {RequirementsMinimumLogicalProcessors} threads required, you have {SystemInfo.Cpu.Threads}!", 112, $"System logical processor count less than GlobalSettings::MinimumLogicalProcessors!", NCExceptionSeverity.FatalError);

            // test cpu functionality
            if (RequirementsMinimumCpuCapabilities > SystemInfo.Cpu.Capabilities) _ = new NCException($"Insufficient CPU capabilities to run game. {RequirementsMinimumCpuCapabilities} capabilities required, you have {SystemInfo.Cpu.Capabilities}!", 113, $"CPU capabilities less than GlobalSettings::MinimumCpuCapabilities!", NCExceptionSeverity.FatalError);

            bool failedOsCheck = false;

            // test windows compat
            if (SystemInfo.CurOperatingSystem < SystemInfoOperatingSystem.MacOS1013
                && RequirementsMinimumOperatingSystem < SystemInfoOperatingSystem.MacOS1013)
            {
                if (RequirementsMinimumOperatingSystem > SystemInfo.CurOperatingSystem) failedOsCheck = true;
            }
            // test macos compat
            else if (SystemInfo.CurOperatingSystem < SystemInfoOperatingSystem.Linux
                && RequirementsMinimumOperatingSystem < SystemInfoOperatingSystem.Linux)
            {
                if (RequirementsMinimumOperatingSystem > SystemInfo.CurOperatingSystem) failedOsCheck = true;
            }

            // test OS version
            if (failedOsCheck) _ = new NCException($"Insufficient OS version to run game. {RequirementsMinimumOperatingSystem} must be used, you have {SystemInfo.CurOperatingSystem}!", 114, $"OS version less than GlobalSettings::MinimumOperatingSystem!", NCExceptionSeverity.FatalError);
        }

        public static void Write() => IniFile.Write(GLOBALSETTINGS_PATH);
        #endregion
    }
}