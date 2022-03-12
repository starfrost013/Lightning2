using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void Load()
        {
            NCINIFile nc_ini = NCINIFile.Parse(GLOBALSETTINGS_FILE_PATH);

            if (nc_ini == null) throw new NCException("Failed to load Engine.ini!", 28, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            NCINIFileSection engine_section = nc_ini.GetSection("Localisation");

            if (engine_section == null) throw new NCException("Engine.ini must have a Localisation section!", 29, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);

            string loc_lang = engine_section.GetValue("Language");
            LocalisationFile = @$"Content\Localisation\{loc_lang}.ini";

            if (!File.Exists(LocalisationFile)) throw new NCException("Engine.ini's Localisation section must have a valid Language value!", 30, "GlobalSettings.Load()", NCExceptionSeverity.FatalError);
        }

    }
}
