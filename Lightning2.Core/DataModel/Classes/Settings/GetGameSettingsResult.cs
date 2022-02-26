using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Result class used for GameSettings.
    /// </summary>
    public class GetGameSettingsResult : Result 
    {
        public GameSettings GameSettings { get; set; }
    }
}
