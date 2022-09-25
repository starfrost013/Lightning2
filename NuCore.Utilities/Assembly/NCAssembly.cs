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

        internal static string LIGHTNING_UTILITIES_PRESET_NAME = "NCMessageBoxPresets";
        #endregion

        public static bool IsAssemblyLoaded(string assemblyName)
        {
            // kludge warning
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.ToString() == assemblyName) return true;
            }

            return false;
        }

        public static Assembly GetLoadedAssembly(string assemblyName)
        {
            // kludge warning
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.ToString() == assemblyName) return assembly;
            }

            return null;
        }
    }
}
