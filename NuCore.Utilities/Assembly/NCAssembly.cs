using System;
using System.Reflection;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCAssembly
    /// 
    /// NuCore Assembly utilities.
    /// </summary>
    public static class NCAssembly
    {
        #region class and namespace names for NCException
        internal static string LIGHTNING_UTILITIES_NAME = "NuCore.Utilities.Lightning";

        internal static string LIGHTNING_UTILITIES_PRESET_NAME = $"NuCore.Utilities.NCMessageBoxPresets";

        /// <summary>
        /// NuCore.Utilities.Lightning assembly.
        /// </summary>
        internal static Assembly NCLightningAssembly { get; private set; }

        internal static bool NCLightningExists => (NCLightningAssembly != null);

        internal static void Init()
        {
            try
            {
                // try and load NuCore.Utilities.Lightning.
                // this is all kludge until we get a better msgbox api
                NCLightningAssembly = Assembly.Load(LIGHTNING_UTILITIES_NAME);
            }
            catch
            {
                // dont load
                NCLightningAssembly = null;
            }
        }

        #endregion
    }
}
