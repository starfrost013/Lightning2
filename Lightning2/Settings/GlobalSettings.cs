using static NuCore.SDL2.SDL;
using NuCore.Utilities;
using System;
using System.IO;

namespace LightningGL
{
    /// <summary>
    /// GlobalSettings
    /// 
    /// March 11, 2022
    /// 
    /// Defines the (serialised) global settings, located in Engine.ini
    /// </summary>
    public static class GlobalSettings
    {
        public static string GLOBALSETTINGS_FILE_PATH = @"Content\Engine.ini";

        /// <summary>
        /// The file to use when loading localisation strings.
        /// </summary>
        public static string LocalisationFile { get; private set; }

        /// <summary>
        /// The target FPS.
        /// </summary>
        public static int MaxFPS { get; set; }

        /// <summary>
        /// Determines if the FPS rate will be shown.
        /// </summary>
        public static bool ShowFPS { get; set; }

        /// <summary>
        /// Determines whether the performance profiler will be loaded or not.
        /// </summary>
        public static bool ProfilePerf { get; set; }

        /// <summary>
        /// Bring up the "About Lightning" messagebox when the Shift-F9 combination is pressed.
        /// </summary>
        public static bool EngineAboutScreenOnShiftF9 { get; set; }

        #region Graphics settings

        /// <summary>
        /// See <see cref="WindowSettings.WindowFlags"/>
        /// </summary>
        public static SDL_WindowFlags WindowFlags { get; set; }

        /// <summary>
        /// The X component of the window resolution.
        /// </summary>
        public static uint ResolutionX { get; set; }

        /// <summary>
        /// The Y component of the window resolution.
        /// </summary>
        public static uint ResolutionY { get; set; }

        #endregion

        internal static void Load()
        {
            // Possible todo: Serialise these to properties and get rid of the loader/sections
            // Consider this
            NCINIFile ncIni = NCINIFile.Parse(GLOBALSETTINGS_FILE_PATH);

            if (ncIni == null) _ = new NCException("Failed to load Engine.ini!", 28, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            NCINIFileSection engineSection = ncIni.GetSection("Engine");
            NCINIFileSection graphicsSection = ncIni.GetSection("Graphics");
            NCINIFileSection locSection = ncIni.GetSection("Localisation");

            if (engineSection == null) _ = new NCException("Engine.ini must have an Engine section!", 41, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);
            if (locSection == null) _ = new NCException("Engine.ini must have a Localisation section!", 29, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            // Load the Localisation section.
            string locLang = locSection.GetValue("Language");
            LocalisationFile = @$"Content\Localisation\{locLang}.ini";

            if (!File.Exists(LocalisationFile)) _ = new NCException("Engine.ini's Localisation section must have a valid Language value!", 30, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            // Load the Engine section.
            string engineMaxFps = engineSection.GetValue("MaxFPS");
            string engineShowFps = engineSection.GetValue("ShowFPS");
            string engineProfilePerf = engineSection.GetValue("PerformanceProfiler");
            string engineAboutScreenOnF9 = engineSection.GetValue("EngineAboutScreenOnShiftF9");

            // Convert will throw an exception, int.TryParse will return a boolean for simpler error checking
            int engineMaxFpsValue = 0;
            bool engineShowFpsValue = false;
            bool engineProfilePerfValue = false;
            bool engineAboutScreenOnF9Value = true; // default true for now

            _ = int.TryParse(engineMaxFps, out engineMaxFpsValue);

            // we don't care about the values here
            _ = bool.TryParse(engineShowFps, out engineShowFpsValue);
            _ = bool.TryParse(engineProfilePerf, out engineProfilePerfValue);
            if (!bool.TryParse(engineAboutScreenOnF9, out engineAboutScreenOnF9Value)) engineAboutScreenOnF9Value = true; // force the default value

            MaxFPS = engineMaxFpsValue;
            ShowFPS = engineShowFpsValue;
            ProfilePerf = engineProfilePerfValue;
            EngineAboutScreenOnShiftF9 = engineAboutScreenOnF9Value;

            // Load the Graphics section.
            string resolutionX = graphicsSection.GetValue("ResolutionX");
            string resolutionY = graphicsSection.GetValue("ResolutionY");
            string windowFlags = graphicsSection.GetValue("WindowFlags");

            // Set default values up. 0 is not a valid SDL_WindowFlags value and nobody has a 0 x 0 display.
            uint resolutionXValue = 0;
            uint resolutionYValue = 0;
            SDL_WindowFlags windowFlagsValue = 0;

            _ = uint.TryParse(resolutionX, out resolutionXValue);
            _ = uint.TryParse(resolutionY, out resolutionYValue);
            _ = Enum.TryParse(windowFlags, true, out windowFlagsValue);

            ResolutionX = resolutionXValue;
            ResolutionY = resolutionYValue;
            WindowFlags = windowFlagsValue;
        }
    }
}
