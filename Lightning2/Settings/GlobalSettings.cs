using NuCore.Utilities; 
using System;
using System.Collections.Generic;
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

            LocalisationFile = engine_section.GetValue("Language");
        }

    }
}
