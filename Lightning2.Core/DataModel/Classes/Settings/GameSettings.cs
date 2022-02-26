using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// A setting that can be created for a game.
    /// </summary>
    public class GameSettings : Instance
    {
        internal override string ClassName => "GameSettings";

        internal override InstanceTags Attributes => InstanceTags.Instantiable | InstanceTags.Destroyable | InstanceTags.ShownInIDE | InstanceTags.ShownInProperties;
        public GetGameSettingResult GetSetting(string Name)
        {
            GetGameSettingResult GGSR = new GetGameSettingResult();

            foreach (GameSetting Setting in Children)
            {
                if (Setting.Name == Name)
                {
                    GGSR.Successful = true;
                    GGSR.Setting = Setting;
                    return GGSR;
                }    
            }

            GGSR.FailureReason = $"Cannot find the GameSetting with the name {Name}! in this game's GameSettings!";
            return GGSR; 
        }

    }
}
