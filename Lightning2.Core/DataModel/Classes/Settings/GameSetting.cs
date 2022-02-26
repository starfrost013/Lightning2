using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// GameSetting
    /// 
    /// April 8, 2021 (modified April 15, 2021)
    /// 
    /// Defines a game setting. A game setting is a setting that is universal to the game that is being saved. 
    /// </summary>
    public class GameSetting : GameSettings // hack 
    {
        internal override InstanceTags Attributes => InstanceTags.Instantiable | InstanceTags.Destroyable | InstanceTags.ShownInIDE | InstanceTags.ShownInProperties;
        internal override string ClassName => "GameSetting";
        public string SettingName { get; set; }
        public Type SettingType { get; set; }
        public object SettingValue { get; set; }
    }
}
