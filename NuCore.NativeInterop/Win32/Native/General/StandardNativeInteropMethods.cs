#if WINDOWS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Lightning
/// 
/// 2021-03-05
/// 
/// Provides native interop services for Windows-based Lightning applications
/// 
/// March 5, 2021   Move from Emerald to Lightning
/// </summary>
namespace NuCore.NativeInterop
{
    public static class NativeMethodsWin32
    {

        /// <summary>
        /// Imported from Tiralen 2022/01/03
        /// 
        /// Acquires a system metric
        /// </summary>
        /// <param name="SysMetric">The <see cref="SystemMetric"/> to acquire.</param>
        /// <returns>The value of the system metric specified by the parameter <paramref name="SysMetric"/>.</returns>
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric SysMetric); // temp
    }
}
#endif