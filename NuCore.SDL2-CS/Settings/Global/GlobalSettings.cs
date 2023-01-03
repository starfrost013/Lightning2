using static LightningBase.SDL;
using static LightningBase.SDL_mixer;
using NuCore.Utilities;
using System;
using System.IO;
using System.Diagnostics;

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
        public static readonly string GLOBALSETTINGS_PATH = @"Content\Engine.ini";

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
        /// Determines if the FPS rate will be shown.
        /// </summary>
        public static bool GeneralShowDebugInfo { get; internal set; }

        /// <summary>
        /// Enable the console. 
        /// </summary>
        public static bool GeneralEnableConsole { get; internal set; }

        #endregion

        #region Debug settings

        /// <summary>
        /// Determines if the DebugViewer can be enabled by pressing a key.
        /// It is recommended to turn this OFF in release builds of your game!!
        /// </summary>
        public static bool DebugDisabled { get; internal set; }

        /// <summary>
        /// The key to press to enable debug mode. The default value is F9.
        /// </summary>
        public static string DebugKey { get; internal set; }

        /// <summary>
        /// The font size of debug.
        /// </summary>
        public static int DebugFontSize { get; internal set; }

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
        /// Delta-time / tick speed multiplier
        /// Default is 1.0
        /// </summary>
        public static int GraphicsTickSpeed { get; internal set; }

        /// <summary>
        /// Determines if offscreen <see cref="Renderable"/>s will be culled from the rendering or not.
        /// </summary>
        public static bool GraphicsRenderOffScreenRenderables { get; internal set; }

        /// <summary>
        /// The renderer that will be used
        /// </summary>
        public static Renderers GraphicsRenderer { get; internal set; }

        /// <summary>
        /// The default multiplier of the font size that is the minimum allowed value for spacing between characters
        /// </summary>
        public static double GraphicsMinimumCharacterSpacing { get; internal set; }

        /// <summary>
        /// The spacing between words. A multiplier of the font size.
        /// </summary>
        public static double GraphicsWordSpacing { get; internal set; }

        /// <summary>
        /// The spacing between linse. A multiplier of the font size.
        /// </summary>
        public static double GraphicsLineSpacing { get; internal set; }

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

        public static int DEFAULT_GRAPHICS_POSITION_X => SystemInfo.ScreenResolutionX / 2 - (GraphicsResolutionX / 2);

        public static int DEFAULT_GRAPHICS_POSITION_Y => SystemInfo.ScreenResolutionY / 2 - (GraphicsResolutionY / 2);

        public static int DEFAULT_GRAPHICS_RESOLUTION_X => SystemInfo.ScreenResolutionX;

        public static int DEFAULT_GRAPHICS_RESOLUTION_Y => SystemInfo.ScreenResolutionY;

        private const int DEFAULT_MAX_FPS = 60;

        private const int DEFAULT_GRAPHICS_TICK_SPEED = 1;

        private const int DEFAULT_AUDIO_DEVICE_HZ = 44100;

        private const int DEFAULT_AUDIO_CHANNELS = 2;

        private const Mix_AudioFormat DEFAULT_AUDIO_FORMAT = Mix_AudioFormat.MIX_DEFAULT_FORMAT;

        private const int DEFAULT_AUDIO_CHUNK_SIZE = 2048;

        private const string DEFAULT_NETWORK_MASTER_SERVER = "https://lightningpowered.net:7801";

        private const int DEFAULT_NETWORK_PORT = 7800;

        private const int DEFAULT_NETWORK_KEEP_ALIVE_MS = 500;

        private const string DEFAULT_DEBUG_KEY = "F9";

        private const int DEFAULT_DEBUG_FONT_SIZE = 11; 

        private const bool DEFAULT_SHOW_ABOUT_SCREEN_ON_SHIFT_F9 = true;

        private const double DEFAULT_MINIMUM_CHARACTER_SPACING = (5d / 11d); // 5 pixels for font size 11, just base it on that

        private const double DEFAULT_WORD_SPACING = 0.55d;

        private const double DEFAULT_LINE_SPACING = 1.2d;

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

            if (iniFile == null) NCError.ShowErrorBox("Failed to load Engine.ini!", 28, "GlobalSettings::Load failed to load Engine.ini!", NCErrorSeverity.FatalError);

            NCINIFileSection generalSection = iniFile.GetSection("General");
            NCINIFileSection graphicsSection = iniFile.GetSection("Graphics");
            NCINIFileSection locSection = iniFile.GetSection("Localisation");
            NCINIFileSection requirementsSection = iniFile.GetSection("Requirements");
            NCINIFileSection sceneSection = iniFile.GetSection("Scene");
            NCINIFileSection audioSection = iniFile.GetSection("Audio");
            NCINIFileSection networkSection = iniFile.GetSection("Network");
            NCINIFileSection debugSection = iniFile.GetSection("Debug");

            if (generalSection == null) NCError.ShowErrorBox("Engine.ini must have a General section!", 41, 
                "GlobalSettings::Load call to NCINIFile::GetSection failed for General section", NCErrorSeverity.FatalError);
            if (locSection == null) NCError.ShowErrorBox("Engine.ini must have a Localisation section!", 29, 
                "GlobalSettings::Load call to NCINIFile::GetSection failed for Localisation section", NCErrorSeverity.FatalError);
            if (sceneSection == null) NCError.ShowErrorBox("Engine.ini must have a Scene section!", 121,
                "GlobalSettings::Load call to NCINIFile::GetSection failed for Scene section", NCErrorSeverity.FatalError);

            // Load the General section.

            GeneralLocalSettingsPath = generalSection.GetValue("LocalSettingsPath");
            GeneralPackageFile = generalSection.GetValue("PackageFile");
            GeneralContentFolder = generalSection.GetValue("ContentFolder");

            // we don't care about boolean values unless we want to force true
            _ = bool.TryParse(generalSection.GetValue("ShowDebugInfo"), out var generalShowDebugInfoValue);
            _ = bool.TryParse(generalSection.GetValue("ProfilePerformance"), out var generalProfilePerfValue);
            if (!bool.TryParse(generalSection.GetValue("EngineAboutScreenOnShiftF9"), out var generalAboutScreenOnF9Value)) generalAboutScreenOnF9Value = DEFAULT_SHOW_ABOUT_SCREEN_ON_SHIFT_F9; // force the default value, true for now
            _ = bool.TryParse(generalSection.GetValue("DeleteUnpackedFilesOnExit"), out var generalDeleteUnpackedFilesOnExitValue);
            _ = bool.TryParse(generalSection.GetValue("DontSaveLocalSettingsOnShutdown"), out var generalDontSaveLocalSettingsOnShutdownValue);

            GeneralShowDebugInfo = generalShowDebugInfoValue;
            GeneralProfilePerformance = generalProfilePerfValue;
            GeneralEngineAboutScreenOnShiftF9 = generalAboutScreenOnF9Value;
            GeneralDeleteUnpackedFilesOnExit = generalDeleteUnpackedFilesOnExitValue;
            GeneralDontSaveLocalSettingsOnShutdown = generalDontSaveLocalSettingsOnShutdownValue;

            // Load the Localisation section.
            string language = locSection.GetValue("Language");
            string localisationFolder = locSection.GetValue("LocalisationFolder");

            if (localisationFolder == null)
            {
                GeneralLanguage = @$"Content\Localisation\{language}.ini";
            }
            else
            {
                if (!Directory.Exists(localisationFolder)) NCError.ShowErrorBox("LocalisationFolder does not exist", 157,
                    "The LocalisationFolder GlobalSetting does not correspond to an extant folder. (GlobalSettings::Load)", NCErrorSeverity.FatalError);
                GeneralLanguage = @$"{localisationFolder}\{language}.ini";
            }
           
            if (!File.Exists(GeneralLanguage)) NCError.ShowErrorBox("Engine.ini's Localisation section must have a valid Language value!", 30, 
                "GlobalSettings::Load call to NCINIFileSection::GetValue failed for Language value", NCErrorSeverity.FatalError);

            // Load the Graphics section, if it is present.
            if (graphicsSection != null)
            {
                // Convert will throw an exception, int.TryParse will return a boolean for simpler error checking
                _ = int.TryParse(graphicsSection.GetValue("MaxFPS"), out var graphicsMaxFpsValue);
                _ = int.TryParse(graphicsSection.GetValue("ResolutionX"), out var graphicsResolutionXValue);
                _ = int.TryParse(graphicsSection.GetValue("ResolutionY"), out var graphicsResolutionYValue);
                _ = Enum.TryParse(typeof(SDL_WindowFlags), graphicsSection.GetValue("WindowFlags"), true, out var graphicsWindowFlagsValue);
                _ = Enum.TryParse(typeof(SDL_RendererFlags), graphicsSection.GetValue("RenderFlags"), true, out var graphicsRenderFlagsValue);
                _ = int.TryParse(graphicsSection.GetValue("TickSpeed"), out var graphicsTickSpeedValue);
                _ = bool.TryParse(graphicsSection.GetValue("RenderOffScreenRenderables"), out var graphicsRenderOffscreenRenderablesValue);
                _ = int.TryParse(graphicsSection.GetValue("PositionX"), out var graphicsPositionXValue);
                _ = int.TryParse(graphicsSection.GetValue("PositionY"), out var graphicsPositionYValue);
                _ = Enum.TryParse(typeof(Renderers), graphicsSection.GetValue("Renderer"), true, out var graphicsRendererValue);
                _ = double.TryParse(graphicsSection.GetValue("MinimumCharacterSpacing"), out var graphicsMinimumCharacterSpacingValue);
                _ = double.TryParse(graphicsSection.GetValue("WordSpacing"), out var graphicsWordSpacingValue);
                _ = double.TryParse(graphicsSection.GetValue("LineSpacing"), out var graphicsLineSpacingValue);

                GraphicsWindowTitle = graphicsSection.GetValue("WindowTitle");

                if (graphicsMaxFpsValue <= 0) graphicsMaxFpsValue = DEFAULT_MAX_FPS;
                if (graphicsResolutionXValue <= 0) graphicsResolutionXValue = DEFAULT_GRAPHICS_RESOLUTION_X;
                if (graphicsResolutionYValue <= 0) graphicsResolutionYValue = DEFAULT_GRAPHICS_RESOLUTION_Y;

                // set the default tick speed value
                if (graphicsTickSpeedValue <= 0) graphicsTickSpeedValue = DEFAULT_GRAPHICS_TICK_SPEED;

                // set minimum spacing values
                if (graphicsMinimumCharacterSpacingValue <= 0) graphicsMinimumCharacterSpacingValue = DEFAULT_MINIMUM_CHARACTER_SPACING;
                if (graphicsWordSpacingValue <= 0) graphicsWordSpacingValue = DEFAULT_WORD_SPACING;
                if (graphicsLineSpacingValue <= 0) graphicsLineSpacingValue = DEFAULT_LINE_SPACING;  

                // Set the actual GlobalSettings values.
                GraphicsMaxFPS = graphicsMaxFpsValue;
                GraphicsResolutionX = graphicsResolutionXValue;
                GraphicsResolutionY = graphicsResolutionYValue;
                if (graphicsWindowFlagsValue != null) GraphicsWindowFlags = (SDL_WindowFlags)graphicsWindowFlagsValue;
                if (graphicsRenderFlagsValue != null) GraphicsRenderFlags = (SDL_RendererFlags)graphicsRenderFlagsValue;
                GraphicsRenderOffScreenRenderables = graphicsRenderOffscreenRenderablesValue;
                if (graphicsRendererValue != null) GraphicsRenderer = (Renderers)graphicsRendererValue;
                GraphicsMinimumCharacterSpacing = graphicsMinimumCharacterSpacingValue;
                GraphicsWordSpacing = graphicsWordSpacingValue;
                GraphicsLineSpacing = graphicsLineSpacingValue;

                // why the fuck do these have to be here???? this is fucked up
                // (because it uses other globalsettings like resolution so you need to load it after resolution. THIS IS A DESIGN PROBLEM, FIX IT)
                // failed to load, set default values (middle of screen)
                if (graphicsPositionXValue <= 0) graphicsPositionXValue = DEFAULT_GRAPHICS_POSITION_X;
                if (graphicsPositionYValue <= 0) graphicsPositionYValue = DEFAULT_GRAPHICS_POSITION_Y;

                GraphicsTickSpeed = graphicsTickSpeedValue;
                GraphicsPositionX = graphicsPositionXValue;
                GraphicsPositionY = graphicsPositionYValue;
            }

            // Load the Requirements section, if it is present.
            if (requirementsSection != null)
            {
                _ = int.TryParse(requirementsSection.GetValue("MinimumSystemRam"), out var minRamValue);
                _ = int.TryParse(requirementsSection.GetValue("MinimumLogicalProcessors"), out var minLogicalProcessorsValue);
                _ = Enum.TryParse(typeof(SystemInfoCPUCapabilities), requirementsSection.GetValue("MinimumCpuCapabilities"), true, out var minimumCpuCapabilitiesValue);
                _ = Enum.TryParse(typeof(SystemInfoOperatingSystem), requirementsSection.GetValue("MinimumOperatingSystem"), true, out var minimumOperatingSystemValue);

                RequirementsMinimumSystemRam = minRamValue;
                RequirementsMinimumLogicalProcessors = minLogicalProcessorsValue;
                if (minimumCpuCapabilitiesValue != null) RequirementsMinimumCpuCapabilities = (SystemInfoCPUCapabilities)minimumCpuCapabilitiesValue;
                if (minimumOperatingSystemValue != null) RequirementsMinimumOperatingSystem = (SystemInfoOperatingSystem)minimumOperatingSystemValue;
            }

            // load the scene section (we checked for its presence earlier)

            SceneStartupScene = sceneSection.GetValue("StartupScene");

            if (SceneStartupScene == null) NCError.ShowErrorBox("DontUseSceneManager not specified, but StartupScene not present in the [Scene] section of Engine.ini!", 164,
                $"GlobalSettings::DontUseSceneManager not specified, but no [Scene] section in Engine.ini!", NCErrorSeverity.FatalError);

            AudioFormat = DEFAULT_AUDIO_FORMAT;

            // Load the audio settings, if it is present
            if (audioSection != null)
            {
                _ = int.TryParse(audioSection.GetValue("DeviceHz"), out var audioDeviceHzValue);
                _ = int.TryParse(audioSection.GetValue("Channels"), out var audioChannelsValue);
                _ = Enum.TryParse(typeof(Mix_AudioFormat), audioSection.GetValue("Format"), true, out var audioFormatValue);
                _ = int.TryParse(audioSection.GetValue("ChunkSize"), out var audioChunkSizeValue);  

                AudioDeviceHz = audioDeviceHzValue;
                AudioChannels = audioChannelsValue;
                if (audioFormatValue != null) AudioFormat = (Mix_AudioFormat)audioFormatValue;
                AudioChunkSize = audioChunkSizeValue;

                if (AudioDeviceHz <= 0) AudioDeviceHz = DEFAULT_AUDIO_DEVICE_HZ;
                if (AudioChannels <= 0) AudioChannels = DEFAULT_AUDIO_CHANNELS;
                if (AudioChunkSize <= 0) AudioChunkSize = DEFAULT_AUDIO_CHUNK_SIZE;
            }

            NetworkMasterServer = DEFAULT_NETWORK_MASTER_SERVER;
            NetworkDefaultPort = DEFAULT_NETWORK_PORT;
            NetworkKeepAliveMs = DEFAULT_NETWORK_KEEP_ALIVE_MS;

            // Load the network settings, if they are present
            if (networkSection != null)
            {

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

            DebugKey = DEFAULT_DEBUG_KEY;
            DebugFontSize = DEFAULT_DEBUG_FONT_SIZE;
            // debugdisabled = false

            // Load the debug settings, if they are present
            if (debugSection != null)
            {
                DebugKey = sceneSection.GetValue("DebugKey");

                _ = bool.TryParse(sceneSection.GetValue("DebugDisabled"), out var debugDisabledValue);
                _ = int.TryParse(sceneSection.GetValue("DebugFontSize"), out var debugFontSizeValue);

                if (string.IsNullOrWhiteSpace(DebugKey)) DebugKey = DEFAULT_DEBUG_KEY;
                if (debugFontSizeValue <= 0) debugFontSizeValue = DEFAULT_DEBUG_FONT_SIZE;

                DebugDisabled = debugDisabledValue;
                DebugFontSize = debugFontSizeValue;

                if (DebugFontSize <= 0) DebugFontSize = DEFAULT_DEBUG_FONT_SIZE;
            }
        }

        /// <summary>
        /// Validates your computer's hardware against the game's system requirements
        /// </summary>
        public static void Validate()
        {
            // test system ram
            if (RequirementsMinimumSystemRam > SystemInfo.SystemRam) NCError.ShowErrorBox($"Insufficient RAM to run game. {RequirementsMinimumSystemRam}MB required, you have {SystemInfo.SystemRam}MB!", 111, $"System RAM less than GlobalSettings::MinimumSystemRam!", NCErrorSeverity.FatalError);

            // test threads
            if (RequirementsMinimumLogicalProcessors > SystemInfo.Cpu.Threads) NCError.ShowErrorBox($"Insufficient logical processors to run game. {RequirementsMinimumLogicalProcessors} threads required, you have {SystemInfo.Cpu.Threads}!", 112, $"System logical processor count less than GlobalSettings::MinimumLogicalProcessors!", NCErrorSeverity.FatalError);

            // test cpu functionality
            if (RequirementsMinimumCpuCapabilities > SystemInfo.Cpu.Capabilities) NCError.ShowErrorBox($"Insufficient CPU capabilities to run game. {RequirementsMinimumCpuCapabilities} capabilities required, you have {SystemInfo.Cpu.Capabilities}!", 113, $"CPU capabilities less than GlobalSettings::MinimumCpuCapabilities!", NCErrorSeverity.FatalError);

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
            if (failedOsCheck) NCError.ShowErrorBox($"Insufficient OS version to run game. {RequirementsMinimumOperatingSystem} must be used, you have {SystemInfo.CurOperatingSystem}!", 114, $"OS version less than GlobalSettings::MinimumOperatingSystem!", NCErrorSeverity.FatalError);
        }

        public static void Write() => IniFile.Write(GLOBALSETTINGS_PATH);
        #endregion
    }
}