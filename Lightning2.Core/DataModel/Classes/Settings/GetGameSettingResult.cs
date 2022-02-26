using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Results class used for acquiring GameSettings.
    /// </summary>
    public class GetGameSettingResult : Result
    {
        public GameSetting Setting { get; set; }
    }
}
