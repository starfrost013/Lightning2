using LightningGL;
using Newtonsoft.Json;
using NuCore.Utilities;
using System;
using System.DirectoryServices.ActiveDirectory;
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

        internal static bool IsPropertyValid(string propertyType, string propertyValue)
        {
            bool isValid = false;

            // temp until AnimPropertyTypes
            switch (propertyType.ToLowerInvariant())
            {
                case "int32":
                    isValid = int.TryParse(propertyValue, out _);
                    break;
                case "float":
                case "single":
                    isValid = float.TryParse(propertyValue, out _);
                    break;
                case "double":
                    isValid = float.TryParse(propertyValue, out _);
                    break;
                case "boolean":
                case "bool":
                    isValid = bool.TryParse(propertyValue, out _);
                    break;
                case "vector2":
                    Vector2Converter vector2Converter = new Vector2Converter();
                    isValid = (vector2Converter.ConvertFromInvariantString(propertyValue) != null);
                    break;
            }

            return isValid;
        }
    }
}
