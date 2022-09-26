using static LightningBase.SDL;
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
        public static string Language { get; internal set; }

        /// <summary>
        /// The localisation folder to use 
        /// </summary>
        public static string LocalisationFolder { get; internal set; } 

        /// <summary>
        /// Determines whether the performance profiler will be loaded or not.
        /// </summary>
        public static bool ProfilePerformance { get; internal set; }

        /// <summary>
        /// Bring up the "About Lightning" messagebox when the Shift-F9 combination is pressed.
        /// </summary>
        public static bool EngineAboutScreenOnShiftF9 { get; internal set; }

        /// <summary>
        /// Delete files that have been uncompressed from the WAD on exit.
        /// </summary>
        public static bool DeleteUnpackedFilesOnExit { get; internal set; }

        /// <summary>
        /// Path to the LocalSettings.ini file.
        /// </summary>
        public static string LocalSettingsPath { get; internal set; }

        /// <summary>
        /// Path to the package file that is to be loaded.
        /// </summary>
        public static string PackageFile { get; internal set; }

        /// <summary>
        /// The content folder to use for the game.
        /// </summary>
        public static string ContentFolder { get; set; }

        /// <summary>
        /// Determines if the Scene Manager will be turned off.
        /// </summary>
        public static bool DontUseSceneManager { get; internal set; }

        #endregion

        #region Graphics settings
        /// <summary>
        /// The target FPS.
        /// </summary>
        public static int MaxFPS { get; internal set; }

        /// <summary>
        /// Determines if the FPS rate will be shown.
        /// </summary>
        public static bool ShowDebugInfo { get; internal set; }

        /// <summary>
        /// See <see cref="RendererSettings.WindowFlags"/>
        /// </summary>
        public static SDL_WindowFlags WindowFlags { get; internal set; }

        /// <summary>
        /// See <see cref="RendererSettings.RenderFlags"/>
        /// </summary>
        public static SDL_RendererFlags RenderFlags { get; internal set; }

        /// <summary>
        /// The X component of the window resolution.
        /// </summary>
        public static int ResolutionX { get; set; }

        /// <summary>
        /// The Y component of the window resolution.
        /// </summary>
        public static int ResolutionY { get; set; }

        /// <summary>
        /// Default window position X. Default is (screen resolution / 2) - size.
        /// </summary>
        public static int PositionX { get; set; }

        /// <summary>
        /// Default window position Y. Default is (screen resolution / 2) - size.
        /// </summary>
        public static int PositionY { get; set; }

        /// <summary>
        /// The title of the Window
        /// </summary>
        public static string WindowTitle { get; internal set; }

        /// <summary>
        /// The rendering backend to use. Default is <see cref="Renderer.OpenGL"/>
        /// </summary>
        public static RenderingBackend RendererType { get; internal set; }

        /// <summary>
        /// Delta-time / tick speed multiplier
        /// Default is 1.0
        /// </summary>
        public static int TickSpeed { get; internal set; }

        /// <summary>
        /// Determines if offscreen <see cref="Renderables"/> will be culled from the rendering or not.
        /// </summary>
        public static bool RenderOffScreenRenderables { get; internal set; }

        #endregion

        #region System requirements

        /// <summary>
        /// Minimum ram in MiB (mebibytes)
        /// </summary>
        public static int MinimumSystemRam { get; internal set; }

        /// <summary>
        /// The minimum number of logical processors. This is *not* CPU cores.
        /// If hyperthreading is enabled there will be two of these per core!
        /// </summary>
        public static int MinimumLogicalProcessors { get; internal set; }

        /// <summary>
        /// Required CPU features - <see cref="SystemInfoCPUCapabilities"/>
        /// </summary>
        public static SystemInfoCPUCapabilities MinimumCpuCapabilities { get; internal set; }

        /// <summary>
        /// Minimum operating system to run game.
        /// </summary>
        public static SystemInfoOperatingSystem MinimumOperatingSystem { get; internal set; }
        #endregion

        #region Scene settings

        /// <summary>
        /// The startup scene name.
        /// </summary>
        public static string StartupScene { get; set; }
        #endregion

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

            if (generalSection == null) _ = new NCException("Engine.ini must have a General section!", 41, 
                "GlobalSettings::Load call to NCINIFile::GetSection failed for General section", NCExceptionSeverity.FatalError);
            if (locSection == null) _ = new NCException("Engine.ini must have a Localisation section!", 29, 
                "GlobalSettings::Load call to NCINIFile::GetSection failed for Localisation section", NCExceptionSeverity.FatalError);

            // Load the Localisation section.
            string language = locSection.GetValue("Language");
            string localisationFolder = locSection.GetValue("LocalisationFolder");

            if (localisationFolder == null)
            {
                Language = @$"Content\Localisation\{language}.ini";
            }
            else
            {
                if (!Directory.Exists(localisationFolder)) _ = new NCException("Engine.ini must have a General section", 157,
                    "GlobalSettings::Load - the LocalisationFolder value does not correspond to an extant folder!", NCExceptionSeverity.FatalError);
                Language = @$"{localisationFolder}\{language}.ini";
            }
           

            if (!File.Exists(Language)) _ = new NCException("Engine.ini's Localisation section must have a valid Language value!", 30, 
                "GlobalSettings::Load call to NCINIFileSection::GetValue failed for Language value", NCExceptionSeverity.FatalError);

            // Load the General section.
            string generalMaxFps = generalSection.GetValue("MaxFPS");
            string generalShowDebugInfo = generalSection.GetValue("ShowDebugInfo");
            string generalProfilePerf = generalSection.GetValue("ProfilePerformance");
            string generalAboutScreenOnF9 = generalSection.GetValue("EngineAboutScreenOnShiftF9");
            string generalDeleteUnpackedFilesOnExit = generalSection.GetValue("DeleteUnpackedFilesOnExit");
            LocalSettingsPath = generalSection.GetValue("LocalSettingsPath");
            PackageFile = generalSection.GetValue("PackageFile");
            ContentFolder = generalSection.GetValue("ContentFolder");
            string generalDontUseSceneManager = generalSection.GetValue("DontUseSceneManager");

            // Convert will throw an exception, int.TryParse will return a boolean for simpler error checking

            _ = int.TryParse(generalMaxFps, out var generalMaxFpsValue);

            // we don't care about the values here
            _ = bool.TryParse(generalShowDebugInfo, out var generalShowDebugInfoValue);
            _ = bool.TryParse(generalProfilePerf, out var generalProfilePerfValue);
            if (!bool.TryParse(generalAboutScreenOnF9, out var generalAboutScreenOnF9Value)) generalAboutScreenOnF9Value = true; // force the default value, true for now
            _ = bool.TryParse(generalDeleteUnpackedFilesOnExit, out var generalDeleteUnpackedFilesOnExitValue);
            _ = bool.TryParse(generalDontUseSceneManager, out var generalDontUseSceneManagerValue);

            MaxFPS = generalMaxFpsValue;
            ShowDebugInfo = generalShowDebugInfoValue;
            ProfilePerformance = generalProfilePerfValue;
            EngineAboutScreenOnShiftF9 = generalAboutScreenOnF9Value;
            DeleteUnpackedFilesOnExit = generalDeleteUnpackedFilesOnExitValue;
            DontUseSceneManager = generalDontUseSceneManagerValue;

            // Load the Graphics section if it exists.
            if (graphicsSection != null)
            {
                string resolutionX = graphicsSection.GetValue("ResolutionX");
                string resolutionY = graphicsSection.GetValue("ResolutionY");
                string positionX = graphicsSection.GetValue("PositionX");
                string positionY = graphicsSection.GetValue("PositionY");
                string windowFlags = graphicsSection.GetValue("WindowFlags");
                string renderFlags = graphicsSection.GetValue("RenderFlags");
                WindowTitle = graphicsSection.GetValue("WindowTitle");
                string renderer = graphicsSection.GetValue("Renderer");
                string tickSpeed = graphicsSection.GetValue("TickSpeed");
                string renderOffScreenRenderables = graphicsSection.GetValue("RenderOffScreenRenderables");

                SDL_WindowFlags windowFlagsValue = 0;
                SDL_RendererFlags renderFlagsValue = 0;
                RenderingBackend rendererValue = 0;

                // inexplicably the overload i used isn't supported here
                _ = int.TryParse(resolutionX, out var resolutionXValue);
                _ = int.TryParse(resolutionY, out var resolutionYValue);
                _ = Enum.TryParse(windowFlags, true, out windowFlagsValue);
                _ = Enum.TryParse(renderFlags, true, out renderFlagsValue);
                _ = Enum.TryParse(renderer, true, out rendererValue);
                _ = int.TryParse(tickSpeed, out var tickSpeedValue);
                _ = bool.TryParse(renderOffScreenRenderables, out var renderOffscreenRenderablesValue);

                // Set those values.
                ResolutionX = resolutionXValue;
                ResolutionY = resolutionYValue;
                WindowFlags = windowFlagsValue;
                RenderFlags = renderFlagsValue;
                RendererType = rendererValue;
                RenderOffScreenRenderables = renderOffscreenRenderablesValue;

                // parse positionX/positionY
                _ = int.TryParse(positionX, out var positionXValue);
                _ = int.TryParse(positionY, out var positionYValue);

                // failed to load, set default values (middle of screen)
                if (positionXValue == 0 && positionYValue == 0)
                {
                    positionXValue = SystemInfo.ScreenResolutionX / 2 - (ResolutionX / 2);
                    positionYValue = SystemInfo.ScreenResolutionY / 2 - (ResolutionY / 2);
                }

                // set the default delta multiplier value
                if (tickSpeedValue == 0) tickSpeedValue = 1;

                TickSpeed = tickSpeedValue;
                PositionX = positionXValue;
                PositionY = positionYValue;
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
                _ = Enum.TryParse(minimumCpuCapabilities, out minimumCpuCapabilitiesValue);
                _ = Enum.TryParse(minimumOperatingSystem, out minimumOperatingSystemValue);

                MinimumSystemRam = minRamValue;
                MinimumLogicalProcessors = minLogicalProcessorsValue;
                MinimumCpuCapabilities = minimumCpuCapabilitiesValue;
                MinimumOperatingSystem = minimumOperatingSystemValue;
            }

            // load the scene section 
            if (!DontUseSceneManager)
            {
                if (sceneSection == null) _ = new NCException("DontUseSceneManager not specified, but no [Scene] section is present in Engine.ini!", 121, $"GlobalSettings::DontUseSceneManager not specified, but no [Scene] section in Engine.ini!", NCExceptionSeverity.FatalError);

                StartupScene = sceneSection.GetValue("StartupScene");
            }
        }

        /// <summary>
        /// Validates your computer's hardware against the game's system requirements
        /// </summary>
        public static void Validate()
        {
            // test system ram
            if (MinimumSystemRam > SystemInfo.SystemRam) _ = new NCException($"Insufficient RAM to run game. {MinimumSystemRam}MB required, you have {SystemInfo.SystemRam}MB!", 111, $"System RAM less than GlobalSettings::MinimumSystemRam!", NCExceptionSeverity.FatalError);

            // test threads
            if (MinimumLogicalProcessors > SystemInfo.Cpu.Threads) _ = new NCException($"Insufficient logical processors to run game. {MinimumLogicalProcessors} threads required, you have {SystemInfo.Cpu.Threads}!", 112, $"System logical processor count less than GlobalSettings::MinimumLogicalProcessors!", NCExceptionSeverity.FatalError);

            // test cpu functionality
            if (MinimumCpuCapabilities > SystemInfo.Cpu.Capabilities) _ = new NCException($"Insufficient CPU capabilities to run game. {MinimumCpuCapabilities} capabilities required, you have {SystemInfo.Cpu.Capabilities}!", 113, $"CPU capabilities less than GlobalSettings::MinimumCpuCapabilities!", NCExceptionSeverity.FatalError);

            bool failedOsCheck = false;

            // test windows compat
            if (SystemInfo.CurOperatingSystem < SystemInfoOperatingSystem.MacOS1013
                && MinimumOperatingSystem < SystemInfoOperatingSystem.MacOS1013)
            {
                if (MinimumOperatingSystem > SystemInfo.CurOperatingSystem) failedOsCheck = true;
            }
            // test macos compat
            else if (SystemInfo.CurOperatingSystem < SystemInfoOperatingSystem.Linux
                && MinimumOperatingSystem < SystemInfoOperatingSystem.Linux)
            {
                if (MinimumOperatingSystem > SystemInfo.CurOperatingSystem) failedOsCheck = true;
            }

            // test OS version
            if (failedOsCheck) _ = new NCException($"Insufficient OS version to run game. {MinimumOperatingSystem} must be used, you have {SystemInfo.CurOperatingSystem}!", 114, $"OS version less than GlobalSettings::MinimumOperatingSystem!", NCExceptionSeverity.FatalError);
        }

        public static void Write() => IniFile.Write(GLOBALSETTINGS_PATH);
    }
}