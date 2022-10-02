using LightningGL;
using Newtonsoft.Json;
using System;

namespace AnimTool
{
    /// <summary>
    /// AnimTool
    /// 
    /// September 25, 2022
    /// 
    /// Animation state.
    /// </summary>
    public static class AnimTool
    {
        internal static Animation? CurAnimation { get; set; } 

        static AnimTool()
        {
            CurAnimation = new Animation("Untitled Animation");
            //TODO: ADD RENDERABLE PROPERTIES
        }

        internal static void Load()
        {
            if (CurAnimation != null)
            {
                CurAnimation = (Animation?)JsonConvert.DeserializeObject(CurAnimation.Path);
            }
            
        }

        internal static void Save()
        {
            if (CurAnimation != null)
            {
                string json = JsonConvert.SerializeObject(CurAnimation);
                File.WriteAllText(CurAnimation.Path, json);
            }
        }
    }
}
