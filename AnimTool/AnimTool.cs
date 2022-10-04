using LightningGL;
using Newtonsoft.Json;
using System;
using System.Reflection;

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

        /// <summary>
        /// The current property being edited.
        /// </summary>
        internal static AnimationProperty? CurProperty { get; set; }

        static AnimTool()
        {
            CurAnimation = new Animation("Untitled Animation");

            Type renderableType = typeof(Renderable);

            foreach (PropertyInfo property in renderableType.GetProperties())
            {
                // don't add delegates or properties with internal/private get methods

                if (!typeof(Delegate).IsAssignableFrom(property.PropertyType)
                    && property.PropertyType.IsPublic)
                {
                    CurAnimation.Properties.Add(new AnimationProperty(property.Name, property.PropertyType.Name));
                }
                
            }
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
