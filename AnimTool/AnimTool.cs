using LightningGL;
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
        }

        internal static void Load()
        {

        }

        internal static void Save()
        {

        }
    }
}
