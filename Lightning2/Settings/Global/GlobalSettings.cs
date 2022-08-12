using NuCore.Utilities;
using System;
using System.IO;
using static NuCore.SDL2.SDL;

namespace LightningGL
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
        public static string GLOBALSETTINGS_PATH = @"Content\Engine.ini";

        #region General settings
        /// <summary>
        /// The file to use when loading localisation strings.
        /// </summary>
        public static string LocalisationFile { get; internal set; }

        /// <summary>
        /// Determines whether the performance profiler will be loaded or not.
        /// </summary>
        public static bool ProfilePerf { get; internal set; }

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
        public static string ContentFolder { get; internal set; }

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
        /// See <see cref="WindowSettings.WindowFlags"/>
        /// </summary>
        public static SDL_WindowFlags WindowFlags { get; internal set; }

        /// <summary>
        /// See <see cref="WindowSettings.RenderFlags"/>
        /// </summary>
        public static SDL_RendererFlags RenderFlags { get; internal set; }

        /// <summary>
        /// The X component of the window resolution.
        /// </summary>
        public static uint ResolutionX { get; internal set; }

        /// <summary>
        /// The Y component of the window resolution.
        /// </summary>
        public static uint ResolutionY { get; internal set; }

        /// <summary>
        /// Default window position X. Default is (screen resolution / 2) - size.
        /// </summary>
        public static uint PositionX { get; internal set; }

        /// <summary>
        /// Default window position Y. Default is (screen resolution / 2) - size.
        /// </summary>
        public static uint PositionY { get; internal set; }

        /// <summary>
        /// The title of the Window
        /// </summary>
        public static string WindowTitle { get; internal set; }
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

        internal static void Load()
        {
            // Possible todo: Serialise these to properties and get rid of the loader/sections
            // Consider this
            NCINIFile ncIni = NCINIFile.Parse(GLOBALSETTINGS_PATH);

            if (ncIni == null) _ = new NCException("Failed to load Engine.ini!", 28, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            NCINIFileSection generalSection = ncIni.GetSection("General");
            NCINIFileSection graphicsSection = ncIni.GetSection("Graphics");
            NCINIFileSection locSection = ncIni.GetSection("Localisation");
            NCINIFileSection requirementsSection = ncIni.GetSection("Requirements");
            NCINIFileSection sceneSection = ncIni.GetSection("Scene");

            if (generalSection == null) _ = new NCException("Engine.ini must have a General section!", 41, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);
            if (locSection == null) _ = new NCException("Engine.ini must have a Localisation section!", 29, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            // Load the Localisation section.
            string locLang = locSection.GetValue("Language");
            LocalisationFile = @$"Content\Localisation\{locLang}.ini";

            if (!File.Exists(LocalisationFile)) _ = new NCException("Engine.ini's Localisation section must have a valid Language value!", 30, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            // Load the General section.
            string generalMaxFps = generalSection.GetValue("MaxFPS");
            string generalShowDebugInfo = generalSection.GetValue("ShowDebugInfo");
            string generalProfilePerf = generalSection.GetValue("PerformanceProfiler");
            string generalAboutScreenOnF9 = generalSection.GetValue("EngineAboutScreenOnShiftF9");
            string generalDeleteUnpackedFilesOnExit = generalSection.GetValue("DeleteUnpackedFilesOnExit");
            LocalSettingsPath = generalSection.GetValue("LocalSettingsPath");
            PackageFile = generalSection.GetValue("PackageFile");
            ContentFolder = generalSection.GetValue("ContentFolder");
            string generalDontUseSceneManager = generalSection.GetValue("DontUseSceneManager");

            // Convert will throw an exception, int.TryParse will return a boolean for simpler error checking
            int generalMaxFpsValue = 0;
            bool generalShowDebugInfoValue = false;
            bool generalProfilePerfValue = false;
            bool generalAboutScreenOnF9Value = false;
            bool generalDeleteUnpackedFilesOnExitValue = false;
            bool generalDontUseSceneManagerValue = false;

            _ = int.TryParse(generalMaxFps, out generalMaxFpsValue);

            // we don't care about the values here
            _ = bool.TryParse(generalShowDebugInfo, out generalShowDebugInfoValue);
            _ = bool.TryParse(generalProfilePerf, out generalProfilePerfValue);
            if (!bool.TryParse(generalAboutScreenOnF9, out generalAboutScreenOnF9Value)) generalAboutScreenOnF9Value = true; // force the default value, true for now
            _ = bool.TryParse(generalDeleteUnpackedFilesOnExit, out generalDeleteUnpackedFilesOnExitValue);
            _ = bool.TryParse(generalDontUseSceneManager, out generalDontUseSceneManagerValue);

            MaxFPS = generalMaxFpsValue;
            ShowDebugInfo = generalShowDebugInfoValue;
            ProfilePerf = generalProfilePerfValue;
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

                // Set default values up. 0 is not a valid SDL_WindowFlags value and nobody has a 0 x 0 display.
                uint resolutionXValue = 0;
                uint resolutionYValue = 0;

                SDL_WindowFlags windowFlagsValue = 0;
                SDL_RendererFlags renderFlagsValue = 0; 

                _ = uint.TryParse(resolutionX, out resolutionXValue);
                _ = uint.TryParse(resolutionY, out resolutionYValue);
                _ = Enum.TryParse(windowFlags, true, out windowFlagsValue);
                _ = Enum.TryParse(renderFlags, true, out renderFlagsValue);

                // Set those values.
                ResolutionX = resolutionXValue;
                ResolutionY = resolutionYValue;
                WindowFlags = windowFlagsValue;
                RenderFlags = renderFlagsValue;

                uint positionXValue = 0;
                uint positionYValue = 0;

                // parse positionX/positionY
                _ = uint.TryParse(positionX, out positionXValue);
                _ = uint.TryParse(positionY, out positionYValue);

                // failed to load, set default values (middle of screen)
                if (positionXValue == 0 && positionYValue == 0)
                {
                    positionXValue = Convert.ToUInt32(SystemInfo.ScreenResolutionX / 2) - ResolutionX / 2;
                    positionYValue = Convert.ToUInt32(SystemInfo.ScreenResolutionY / 2) - ResolutionY / 2;
                }

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

                int minRamValue = 0;
                int minLogicalProcessorsValue = 0;
                SystemInfoCPUCapabilities minimumCpuCapabilitiesValue = 0;
                SystemInfoOperatingSystem minimumOperatingSystemValue = 0;

                _ = int.TryParse(minRam, out minRamValue);
                _ = int.TryParse(minLogicalProcessors, out minLogicalProcessorsValue);
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

        internal static void Validate()
        {
            // test system ram
            if (MinimumSystemRam > SystemInfo.SystemRam)
            {
                _ = new NCException($"Insufficient RAM to run game. {MinimumSystemRam}MB required, you have {SystemInfo.SystemRam}MB!", 111, $"System RAM less than GlobalSettings::MinimumSystemRam!", NCExceptionSeverity.FatalError);
            }

            // test threads
            if (MinimumLogicalProcessors > SystemInfo.Cpu.Threads)
            {
                _ = new NCException($"Insufficient logical processors to run game. {MinimumLogicalProcessors} threads required, you have {SystemInfo.Cpu.Threads}!", 112, $"System logical processor count less than GlobalSettings::MinimumLogicalProcessors!", NCExceptionSeverity.FatalError);
            }

            // test cpu functionality
            if (MinimumCpuCapabilities > SystemInfo.Cpu.Capabilities)
            {
                _ = new NCException($"Insufficient CPU capabilities to run game. {MinimumCpuCapabilities} capabilities required, you have {SystemInfo.Cpu.Capabilities}!", 113, $"CPU capabilities less than GlobalSettings::MinimumCpuCapabilities!", NCExceptionSeverity.FatalError);
            }

            // test OS version
            if (MinimumOperatingSystem > SystemInfo.CurOperatingSystem)
            {
                _ = new NCException($"Insufficient OS version to run game. {MinimumOperatingSystem} must be used, you have {SystemInfo.CurOperatingSystem}!", 114, $"OS version less than GlobalSettings::MinimumOperatingSystem!", NCExceptionSeverity.FatalError);
            }
        }
    }
}