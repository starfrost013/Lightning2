using NuCore.Utilities;
using System.IO;

namespace Lightning2
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
        public static void Load()
        {
            NCINIFile nc_ini = NCINIFile.Parse(GLOBALSETTINGS_FILE_PATH);

            if (nc_ini == null) throw new NCException("Failed to load Engine.ini!", 28, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            NCINIFileSection engine_section = nc_ini.GetSection("Engine");
            NCINIFileSection loc_section = nc_ini.GetSection("Localisation");

            if (engine_section == null) throw new NCException("Engine.ini must have an Engine section!", 41, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);
            if (loc_section == null) throw new NCException("Engine.ini must have a Localisation section!", 29, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            string loc_lang = loc_section.GetValue("Language");
            LocalisationFile = @$"Content\Localisation\{loc_lang}.ini";

            if (!File.Exists(LocalisationFile)) throw new NCException("Engine.ini's Localisation section must have a valid Language value!", 30, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            string engine_target = engine_section.GetValue("TargetFPS");
            string engine_show = engine_section.GetValue("ShowFPS");

            // Convert will throw an exception, int.TryParse will return a boolean for simpler error checking
            int engine_target_fps = 0;
            bool engine_show_fps = false;

            if (!int.TryParse(engine_target, out engine_target_fps)) throw new NCException($"Invalid TargetFPS setting ({engine_target}) in Engine.ini Engine section, setting to 60!", 42, "GlobalSettings.Load()", NCExceptionSeverity.Error);
            if (!bool.TryParse(engine_show, out engine_show_fps)) engine_show_fps = false;

            if (engine_target_fps <= 0)
            {
                engine_target_fps = 60;
                throw new NCException($"The TargetFPS setting ({engine_target}) must be a positive number, setting to 60!", 43, "GlobalSettings.Load()", NCExceptionSeverity.Error);
            }

            TargetFPS = engine_target_fps;
            ShowFPS = engine_show_fps;
        }
    }
}
