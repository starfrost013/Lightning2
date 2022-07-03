using NuCore.Utilities;
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

        public static string LocalisationFile { get; private set; }

        public static int TargetFPS { get; set; }

        public static bool ShowFPS { get; set; }

        public static bool ProfilePerf { get; set; }

        public static void Load()
        {
            NCINIFile ncIni = NCINIFile.Parse(GLOBALSETTINGS_FILE_PATH);

            if (ncIni == null) new NCException("Failed to load Engine.ini!", 28, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            NCINIFileSection engineSection = ncIni.GetSection("Engine");
            NCINIFileSection locSection = ncIni.GetSection("Localisation");

            if (engineSection == null) new NCException("Engine.ini must have an Engine section!", 41, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);
            if (locSection == null) new NCException("Engine.ini must have a Localisation section!", 29, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            string locLang = locSection.GetValue("Language");
            LocalisationFile = @$"Content\Localisation\{locLang}.ini";

            if (!File.Exists(LocalisationFile)) new NCException("Engine.ini's Localisation section must have a valid Language value!", 30, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            string engineTargetFps = engineSection.GetValue("TargetFPS");
            string engineShowFps = engineSection.GetValue("ShowFPS");
            string engineProfilePerf = engineSection.GetValue("PerformanceProfiler");

            // Convert will throw an exception, int.TryParse will return a boolean for simpler error checking
            int engineTargetFpsValue = 0;
            bool engineShowFpsValue = false;
            bool engineProfilePerfValue = false;

            if (!int.TryParse(engineTargetFps, out engineTargetFpsValue)) new NCException($"Invalid TargetFPS setting ({engineTargetFps}) in Engine.ini Engine section, setting to 60!", 42, "GlobalSettings.Load()", NCExceptionSeverity.Error);
            if (!bool.TryParse(engineShowFps, out engineShowFpsValue)) engineShowFpsValue = false;
            if (!bool.TryParse(engineProfilePerf, out engineProfilePerfValue)) engineProfilePerfValue = false;

            if (engineTargetFpsValue <= 0)
            {
                engineTargetFpsValue = 60;
                new NCException($"The TargetFPS setting ({engineTargetFps}) must be a positive number, setting to 60!", 43, "GlobalSettings.Load()", NCExceptionSeverity.Error);
            }

            TargetFPS = engineTargetFpsValue;
            ShowFPS = engineShowFpsValue;
            ProfilePerf = engineProfilePerfValue;
        }
    }
}
