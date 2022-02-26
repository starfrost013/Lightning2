using NuCore.Utilities;
using NuCore.NativeInterop.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core
{
    /// <summary>
    /// LaunchArgs
    /// 
    /// April 8, 2021 (modified December 8, 2021: Use NuCore)
    /// 
    /// Defines launch arguments for the DataModel.
    /// </summary>
    public class LaunchArgs
    {

        /// <summary>
        /// Application name used for console logging.
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// The path to the GameXML to launch.
        /// </summary>
        public string GameXMLPath { get; set; }

        /// <summary>
        /// Determines if services will be initialised.
        /// </summary>
        public bool InitServices { get; set; }

        public static LaunchArgsResult HandleArgs(string[] Args)
        {
            LaunchArgsResult LAR = new LaunchArgsResult();

            switch (Args.Length)
            {
                case 0:
                    if (LAR.Arguments.AppName == null || // fix crash
                    (!LAR.Arguments.AppName.Contains("Polaris")
                    && !LAR.Arguments.AppName.Contains("LightningSDK")))
                    {
                        MessageBox.Show("Lightning [GameXML]\nGameXML: path to the LGX (Lightning Game XML) file you wish to load.", "Lightning Game Engine", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    LAR.Action = LaunchArgsAction.DoNothing;

                    return LAR;
                default:

                    bool AmOverridingAppId = false;

                    LAR.Action = LaunchArgsAction.LaunchGameXML;
                    // only one arg so we can be dumb but we're not going to for extensibility
                    foreach (string Argument in Args)
                    {
                        switch (Argument)
                        {
                            case "-noservices":
                                LAR.Action = LaunchArgsAction.InitNoServices;
                                continue;
                            case "-overrideappid":
                                AmOverridingAppId = true; // is this simpler than getting the next arg and checking?
                                continue;
                            default:

                                if (AmOverridingAppId)
                                {
                                    LAR.Arguments.AppName = Argument;
                                }

                                LAR.Arguments.GameXMLPath = Argument;
                                LAR.Arguments.InitServices = true;
                                continue;
                        }
                    }

                    if (LAR.Arguments.AppName == null && AmOverridingAppId) ErrorManager.ThrowError("DataModel", "OverrideAppIdSpecifiedWithNoAppId");

                    return LAR;
            }
        }
    }
}
