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
        /// Contains the path to the input bindings INI file.
        /// </summary>
        public static readonly string INPUTBINDINGS_PATH = @"Content\InputBindings.ini";

        /// <summary>
        /// The INI file object containing the Global Settings.
        /// </summary>
        internal static IniFile IniFile { get; private set; }

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
        /// <para>Save the <see cref="GlobalSettings"/> on engine shutdown.</para>
        /// <para>This is done regardless of if the global settings have been changed because until the 2.5 rewrite of GlobalSettings there is no easy way to determine</para> 
        /// <para>if they have been changed.</para>
        /// </summary>
        public static bool GeneralDontSaveGlobalSettingsOnShutdown { get; internal set; }

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
        /// Determines if old performance profiler CSVs will be kept.
        /// </summary>
        public static bool GeneralKeepOldPerformanceProfilerCsvs { get; internal set; }

        /// <summary>
        /// The default input method to use.
        /// </summary>
        public static InputMethods GeneralDefaultInputMethod { get; internal set; }
        #endregion

        #region Debug settings

        /// <summary>
        /// Determines if the DebugViewer can be enabled by pressing a key.
        /// It is recommended to turn this OFF in release builds of your game!!
        /// </summary>
        public static DebugState DebugState { get; internal set; }

        /// <summary>
        /// The key to press to enable debug mode. The default value is F9.
        /// </summary>
        public static string DebugKey { get; internal set; }

        /// <summary>
        /// The font size for debug text.
        /// </summary>
        public static int DebugFontSize { get; internal set; }

        /// <summary>
        /// The font size for debug text.
        /// </summary>
        public static int DebugFontSizeLarge { get; internal set; }

        /// <summary>
        /// The default horizontal position of the debug position.
        /// </summary>
        public static float DebugPositionX { get; internal set; }

        /// <summary>
        /// The default vertical position of the debug position Y.
        /// </summary>
        public static float DebugPositionY { get; internal set; }

        /// <summary>
        /// The default key to be pressed for the debug console.
        /// </summary>
        public static string DebugConsoleKey { get; internal set; }

        /// <summary>
        /// The default horizontal position of the debug console.
        /// </summary>
        public static float DebugConsoleSizeX { get; internal set; }

        /// <summary>
        /// The default vertical position of the debug console.
        /// </summary>
        public static float DebugConsoleSizeY { get; internal set; }
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
        public static bool GraphicsDontCullRenderables { get; internal set; }

        /// <summary>
        /// The renderer that will be used
        /// </summary>
        public static Renderers GraphicsRenderer { get; internal set; }

        /// <summary>
        /// The spacing between linse. A multiplier of the font size.
        /// </summary>
        public static double GraphicsLineSpacing { get; internal set; }

        /// <summary>
        /// How much the bold font style horizontally emboldens font bitmaps.
        /// </summary>
        public static int GraphicsBoldFactorX { get; internal set; }

        /// <summary>
        /// How much the bold font style vertically emboldens font bitmaps.
        /// </summary>
        public static int GraphicsBoldFactorY { get; internal set; }

        /// <summary>
        /// The angle applied to italic text.
        /// </summary>
        public static int GraphicsItalicAngle { get; internal set; }

        /// <summary>
        /// The underline line thickness.
        /// </summary>
        public static int GraphicsUnderlineThickness { get; internal set; }

        /// <summary>
        /// The strikeout line thickness.
        /// </summary>
        public static int GraphicsStrikeoutThickness { get; internal set; }

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

        /// <summary>
        /// The number of simultaneous audio files that can be played.
        /// </summary>
        public static int AudioMaxSimultaneousAudioFiles { get; set; }

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

        private const DebugState DEFAULT_DEBUG_STATE = DebugState.Both;

        private const float DEFAULT_DEBUG_POSITION_X = 0;

        private const float DEFAULT_DEBUG_POSITION_Y = 12;

        private const int DEFAULT_DEBUG_FONT_SIZE = 11;

        private const int DEFAULT_DEBUG_FONT_SIZE_LARGE = 36;

        private const bool DEFAULT_SHOW_ABOUT_SCREEN_ON_SHIFT_F9 = true;

        private const double DEFAULT_LINE_SPACING = 1.2d;

        private const int DEFAULT_BOLD_FACTOR_X = 4;

        private const int DEFAULT_BOLD_FACTOR_Y = 4;

        private const int DEFAULT_ITALIC_ANGLE_DEGREES = 12;

        private const int DEFAULT_UNDERLINE_THICKNESS = 1;

        private const int DEFAULT_STRIKEOUT_THICKNESS = 1;

        private const string DEFAULT_DEBUG_CONSOLE_KEY = "F10";
        private static int DEFAULT_DEBUG_CONSOLE_SIZE_X => DEFAULT_GRAPHICS_RESOLUTION_X;
        private static int DEFAULT_DEBUG_CONSOLE_SIZE_Y => (int)(DEFAULT_GRAPHICS_RESOLUTION_Y / 2.5);

        private static int DEFAULT_MAX_SIMULTANEOUS_AUDIO_FILES = 16;

        private static InputMethods DEFAULT_INPUT_METHOD = InputMethods.KeyboardMouse;

        #endregion

        #region GlobalSettings methods
        /// <summary>
        /// Loads the Global Settings.
        /// </summary>
        public static void Load()
        {
            // Possible todo: Serialise these to properties and get rid of the loader/sections
            // Consider this
            IniFile iniFile = IniFile.Parse(GLOBALSETTINGS_PATH);

            if (iniFile == null) Logger.LogError("Failed to load Engine.ini!", 28, LoggerSeverity.FatalError);

            IniSection generalSection = iniFile.GetSection("General");
            IniSection graphicsSection = iniFile.GetSection("Graphics");
            IniSection locSection = iniFile.GetSection("Localisation");
            IniSection requirementsSection = iniFile.GetSection("Requirements");
            IniSection sceneSection = iniFile.GetSection("Scene");
            IniSection audioSection = iniFile.GetSection("Audio");
            IniSection networkSection = iniFile.GetSection("Network");
            IniSection debugSection = iniFile.GetSection("Debug");

            if (generalSection == null)
            {
                Logger.LogError("Engine.ini must have a General section!", 41, LoggerSeverity.FatalError);
                return;
            }

            if (locSection == null)
            {
                Logger.LogError("Engine.ini must have a Localisation section!", 29, LoggerSeverity.FatalError);
                return;
            }

            if (sceneSection == null)
            {
                Logger.LogError("Engine.ini must have a Scene section!", 121, LoggerSeverity.FatalError);
                return; 
            }

            if (graphicsSection == null)
            {
                Logger.LogError("Engine.ini must have a Graphics section!", 310, LoggerSeverity.FatalError);
                return;
            }

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
            _ = bool.TryParse(generalSection.GetValue("DontSaveGlobalSettingsOnShutdown"), out var generalDontSaveGlobalSettingsOnShutdownValue);
            _ = bool.TryParse(graphicsSection.GetValue("KeepOldPerformanceProfilerCsvs"), out var generalKeepOldPerformanceProfilerCsvsValue);
            if (!Enum.TryParse(typeof(InputMethods), graphicsSection.GetValue("DefaultInputMethod"), true, out var generalDefaultInputMethodValue)) generalDefaultInputMethodValue = DEFAULT_INPUT_METHOD;

            GeneralShowDebugInfo = generalShowDebugInfoValue;
            GeneralProfilePerformance = generalProfilePerfValue;
            GeneralEngineAboutScreenOnShiftF9 = generalAboutScreenOnF9Value;
            GeneralDeleteUnpackedFilesOnExit = generalDeleteUnpackedFilesOnExitValue;
            GeneralDontSaveLocalSettingsOnShutdown = generalDontSaveLocalSettingsOnShutdownValue;
            GeneralDontSaveGlobalSettingsOnShutdown = generalDontSaveGlobalSettingsOnShutdownValue;
            GeneralKeepOldPerformanceProfilerCsvs = generalKeepOldPerformanceProfilerCsvsValue;
            GeneralDefaultInputMethod = (InputMethods)generalDefaultInputMethodValue;

            // Load the Localisation section.
            string language = locSection.GetValue("Language");
            string localisationFolder = locSection.GetValue("LocalisationFolder");

            if (localisationFolder == null)
            {
                GeneralLanguage = @$"Content\Localisation\{language}.ini";
            }
            else
            {
                if (!Directory.Exists(localisationFolder)) Logger.LogError("LocalisationFolder does not exist", 157, LoggerSeverity.FatalError);
                GeneralLanguage = @$"{localisationFolder}\{language}.ini";
            }

            if (!File.Exists(GeneralLanguage)) Logger.LogError("Engine.ini's Localisation section must have a valid Language value!", 30, LoggerSeverity.FatalError);

            // Load the Graphics section, if it is present.
            if (graphicsSection != null)
            {
                // Convert will throw an exception, int.TryParse will return a boolean for simpler error checking
                _ = int.TryParse(graphicsSection.GetValue("MaxFPS"), out var graphicsMaxFpsValue);
                _ = int.TryParse(graphicsSection.GetValue("ResolutionX"), out var graphicsResolutionXValue);
                _ = int.TryParse(graphicsSection.GetValue("ResolutionY"), out var graphicsResolutionYValue);
                _ = Enum.TryParse(graphicsSection.GetValue("WindowFlags"), true, out SDL_WindowFlags graphicsWindowFlagsValue);
                _ = Enum.TryParse(graphicsSection.GetValue("RenderFlags"), true, out SDL_RendererFlags graphicsRenderFlagsValue);
                _ = int.TryParse(graphicsSection.GetValue("TickSpeed"), out var graphicsTickSpeedValue);
                _ = bool.TryParse(graphicsSection.GetValue("DontCullRenderables"), out var graphicsDontCullRenderablesValue);
                _ = int.TryParse(graphicsSection.GetValue("PositionX"), out var graphicsPositionXValue);
                _ = int.TryParse(graphicsSection.GetValue("PositionY"), out var graphicsPositionYValue);
                _ = Enum.TryParse(graphicsSection.GetValue("Renderer"), true, out Renderers graphicsRendererValue);
                _ = double.TryParse(graphicsSection.GetValue("LineSpacing"), out var graphicsLineSpacingValue);
                _ = int.TryParse(graphicsSection.GetValue("BoldFactorX"), out var graphicsBoldFactorXValue);
                _ = int.TryParse(graphicsSection.GetValue("BoldFactorY"), out var graphicsBoldFactorYValue);
                _ = int.TryParse(graphicsSection.GetValue("ItalicAngle"), out var graphicsItalicAngleValue);
                _ = int.TryParse(graphicsSection.GetValue("UnderlineThickness"), out var graphicsUnderlineThicknessValue);
                _ = int.TryParse(graphicsSection.GetValue("StrikeoutThickness"), out var graphicsStrikeoutThicknessValue);
                GraphicsWindowTitle = graphicsSection.GetValue("WindowTitle");

                if (graphicsMaxFpsValue <= 0) graphicsMaxFpsValue = DEFAULT_MAX_FPS;
                if (graphicsResolutionXValue <= 0) graphicsResolutionXValue = DEFAULT_GRAPHICS_RESOLUTION_X;
                if (graphicsResolutionYValue <= 0) graphicsResolutionYValue = DEFAULT_GRAPHICS_RESOLUTION_Y;

                // set the default tick speed value
                if (graphicsTickSpeedValue <= 0) graphicsTickSpeedValue = DEFAULT_GRAPHICS_TICK_SPEED;

                // set minimum spacing values
                if (graphicsLineSpacingValue <= 0) graphicsLineSpacingValue = DEFAULT_LINE_SPACING;
                if (graphicsBoldFactorXValue <= 0) graphicsBoldFactorXValue = DEFAULT_BOLD_FACTOR_X;
                if (graphicsBoldFactorYValue <= 0) graphicsBoldFactorYValue = DEFAULT_BOLD_FACTOR_Y;
                if (graphicsItalicAngleValue <= 0
                    || graphicsItalicAngleValue >= 360) graphicsItalicAngleValue = DEFAULT_ITALIC_ANGLE_DEGREES;
                if (graphicsUnderlineThicknessValue <= 0) graphicsUnderlineThicknessValue = DEFAULT_UNDERLINE_THICKNESS;
                if (graphicsStrikeoutThicknessValue <= 0) graphicsStrikeoutThicknessValue = DEFAULT_STRIKEOUT_THICKNESS;

                // Set the actual GlobalSettings values.
                GraphicsMaxFPS = graphicsMaxFpsValue;
                GraphicsResolutionX = graphicsResolutionXValue;
                GraphicsResolutionY = graphicsResolutionYValue;
                if (graphicsWindowFlagsValue != default) GraphicsWindowFlags = graphicsWindowFlagsValue;
                if (graphicsRenderFlagsValue != default) GraphicsRenderFlags = graphicsRenderFlagsValue;
                GraphicsDontCullRenderables = graphicsDontCullRenderablesValue;
                if (graphicsRendererValue != default) GraphicsRenderer = graphicsRendererValue;
                GraphicsLineSpacing = graphicsLineSpacingValue;
                GraphicsBoldFactorX = graphicsBoldFactorXValue;
                GraphicsBoldFactorY = graphicsBoldFactorYValue;
                GraphicsItalicAngle = graphicsItalicAngleValue;
                GraphicsUnderlineThickness = graphicsUnderlineThicknessValue;
                GraphicsStrikeoutThickness = graphicsStrikeoutThicknessValue;

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
                _ = Enum.TryParse(requirementsSection.GetValue("MinimumCpuCapabilities"), true, out SystemInfoCPUCapabilities minimumCpuCapabilitiesValue);
                _ = Enum.TryParse(requirementsSection.GetValue("MinimumOperatingSystem"), true, out SystemInfoOperatingSystem minimumOperatingSystemValue);

                RequirementsMinimumSystemRam = minRamValue;
                RequirementsMinimumLogicalProcessors = minLogicalProcessorsValue;
                if (minimumCpuCapabilitiesValue != default) RequirementsMinimumCpuCapabilities = minimumCpuCapabilitiesValue;
                if (minimumOperatingSystemValue != default) RequirementsMinimumOperatingSystem = minimumOperatingSystemValue;
            }

            // load the scene section (we checked for its presence earlier)

            SceneStartupScene = sceneSection.GetValue("StartupScene");

            if (SceneStartupScene == null) Logger.LogError("StartupScene not present in the [Scene] section of Engine.ini!", 164,
                LoggerSeverity.FatalError);

            AudioDeviceHz = DEFAULT_AUDIO_DEVICE_HZ;
            AudioChunkSize = DEFAULT_AUDIO_CHUNK_SIZE;
            AudioFormat = DEFAULT_AUDIO_FORMAT;
            AudioChannels = DEFAULT_AUDIO_CHANNELS;
            AudioMaxSimultaneousAudioFiles = DEFAULT_MAX_SIMULTANEOUS_AUDIO_FILES;

            // Load the audio settings, if it is present
            if (audioSection != null)
            {
                _ = int.TryParse(audioSection.GetValue("DeviceHz"), out var audioDeviceHzValue);
                _ = int.TryParse(audioSection.GetValue("Channels"), out var audioChannelsValue);
                _ = Enum.TryParse(audioSection.GetValue("Format"), true, out Mix_AudioFormat audioFormatValue);
                _ = int.TryParse(audioSection.GetValue("ChunkSize"), out var audioChunkSizeValue);
                _ = int.TryParse(audioSection.GetValue("MaxSimultaneousAudioFiles"), out var audioMaxSimultaneousAudioFilesValue);

                if (audioDeviceHzValue > 0) AudioDeviceHz = audioDeviceHzValue;
                if (audioChannelsValue > 0) AudioChannels = audioChannelsValue;
                if (audioFormatValue != default) AudioFormat = audioFormatValue;
                if (audioChunkSizeValue > 0) AudioChunkSize = audioChunkSizeValue;
                if (audioMaxSimultaneousAudioFilesValue > 0) AudioMaxSimultaneousAudioFiles = audioMaxSimultaneousAudioFilesValue;
            }

            NetworkMasterServer = DEFAULT_NETWORK_MASTER_SERVER;
            NetworkDefaultPort = DEFAULT_NETWORK_PORT;
            NetworkKeepAliveMs = DEFAULT_NETWORK_KEEP_ALIVE_MS;
            NetworkDefaultPort = DEFAULT_NETWORK_PORT;

            // Load the network settings, if they are present
            if (networkSection != null)
            {
                string networkMasterServer = networkSection.GetValue("MasterServer");
                string networkDefaultPort = networkSection.GetValue("Port");
                string networkKeepAliveMs = networkSection.GetValue("KeepAliveMs");

                if (!string.IsNullOrWhiteSpace(networkMasterServer)) NetworkMasterServer = networkMasterServer;

                // don't block http or dns
                _ = ushort.TryParse(networkDefaultPort, out var networkDefaultPortValue);

                // don't allow some common ports
                if (networkDefaultPortValue > 0 
                    && networkDefaultPortValue != 53
                    && networkDefaultPortValue != 80
                    && networkDefaultPortValue != 443) NetworkDefaultPort = networkDefaultPortValue;

                _ = int.TryParse(networkKeepAliveMs, out var networkKeepAliveMsValue);
                if (networkKeepAliveMsValue > 0) NetworkKeepAliveMs = networkKeepAliveMsValue;
            }

            // debugdisabled = false
            // Load the debug settings, if they are present
            if (debugSection != null)
            {
                DebugKey = sceneSection.GetValue("DebugKey");

                DebugState = (DebugState)Enum.Parse(typeof(DebugState), sceneSection.GetValue("DebugDisabled"));
                _ = int.TryParse(sceneSection.GetValue("DebugFontSize"), out var debugFontSizeValue);
                _ = int.TryParse(sceneSection.GetValue("DebugFontSizeLarge"), out var debugFontSizeLargeValue);
                _ = float.TryParse(sceneSection.GetValue("DebugPositionX"), out var debugPositionXValue);
                _ = float.TryParse(sceneSection.GetValue("DebugPositionY"), out var debugPositionYValue);
                DebugConsoleKey = sceneSection.GetValue("DebugConsoleKey");
                _ = float.TryParse(sceneSection.GetValue("DebugConsoleSizeX"), out var debugConsoleSizeXValue);
                _ = float.TryParse(sceneSection.GetValue("DebugConsoleSizeY"), out var debugConsoleSizeYValue);

                if (debugFontSizeValue > 0) DebugFontSize = debugFontSizeValue;

                DebugFontSizeLarge = debugFontSizeLargeValue;
                DebugPositionX = debugPositionXValue;
                DebugPositionY = debugPositionYValue;
                DebugConsoleSizeX = debugConsoleSizeXValue;
                DebugConsoleSizeY = debugConsoleSizeYValue;
            }

            if (string.IsNullOrWhiteSpace(DebugKey)) DebugKey = DEFAULT_DEBUG_KEY;
            if (DebugFontSize <= 0) DebugFontSize = DEFAULT_DEBUG_FONT_SIZE;
            if (DebugFontSizeLarge <= 0) DebugFontSizeLarge = DEFAULT_DEBUG_FONT_SIZE_LARGE;
            if (DebugPositionX <= 0) DebugPositionX = DEFAULT_DEBUG_POSITION_X;
            if (DebugPositionY <= 0) DebugPositionY = DEFAULT_DEBUG_POSITION_Y;
            if (string.IsNullOrWhiteSpace(DebugConsoleKey)) DebugConsoleKey = DEFAULT_DEBUG_CONSOLE_KEY;
            if (DebugConsoleSizeX <= 0) DebugConsoleSizeX = DEFAULT_DEBUG_CONSOLE_SIZE_X;
            if (DebugConsoleSizeY <= 0) DebugConsoleSizeY = DEFAULT_DEBUG_CONSOLE_SIZE_Y;
            if (DebugState == DebugState.NotLoaded) DebugState = DEFAULT_DEBUG_STATE;
        }

        /// <summary>
        /// Validates your computer's hardware against the game's system requirements
        /// </summary>
        public static void Validate()
        {
            // test system ram
            if (RequirementsMinimumSystemRam > SystemInfo.SystemRam) Logger.LogError($"Insufficient RAM to run game. " +
                $"{RequirementsMinimumSystemRam}MB required, you have {SystemInfo.SystemRam}MB!", 111, LoggerSeverity.FatalError);

            // test threads
            if (RequirementsMinimumLogicalProcessors > SystemInfo.Cpu.Threads) Logger.LogError($"Insufficient logical processors to run game. " +
                $"{RequirementsMinimumLogicalProcessors} threads required, you have {SystemInfo.Cpu.Threads}!", 112, LoggerSeverity.FatalError);

            // test cpu functionality
            if (RequirementsMinimumCpuCapabilities > SystemInfo.Cpu.Capabilities) Logger.LogError($"Insufficient CPU capabilities to run game. " +
                $"{RequirementsMinimumCpuCapabilities} capabilities required, you have {SystemInfo.Cpu.Capabilities}!", 113, LoggerSeverity.FatalError);

            bool failedOsCheck = false;

            // test windows compat
            if ((SystemInfo.CurOperatingSystem < SystemInfoOperatingSystem.MacOS1013
                && RequirementsMinimumOperatingSystem < SystemInfoOperatingSystem.MacOS1013)
                || SystemInfo.CurOperatingSystem != SystemInfoOperatingSystem.Linux)
            {
                if (RequirementsMinimumOperatingSystem > SystemInfo.CurOperatingSystem) failedOsCheck = true;
            }

            // test OS version
            if (failedOsCheck) Logger.LogError($"Insufficient OS version to run game. {RequirementsMinimumOperatingSystem} must be used, you have " +
                $"{SystemInfo.CurOperatingSystem}!", 114, LoggerSeverity.FatalError);
        }

        public static void Save() => IniFile.Save(GLOBALSETTINGS_PATH);
        #endregion
    }
}