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
            NCLogging.Init();
        }

        internal static void Load()
        {
            if (CurAnimation != null)
            {
                // you have to read all of the text here
                CurAnimation = JsonConvert.DeserializeObject<Animation>(File.ReadAllText(CurAnimation.Path));
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

            propertyType = propertyType.ToLowerInvariant();

            AnimationPropertyType animationPropertyType = (AnimationPropertyType)Enum.Parse(typeof(AnimationPropertyType), propertyType, true);

            // temp until AnimPropertyTypes
            switch (animationPropertyType)
            {
                case AnimationPropertyType.Int32:
                    isValid = int.TryParse(propertyValue, out _);
                    break;
                case AnimationPropertyType.Float:
                    isValid = float.TryParse(propertyValue, out _);
                    break;
                case AnimationPropertyType.Double:
                    isValid = double.TryParse(propertyValue, out _);
                    break;
                case AnimationPropertyType.Boolean:
                    isValid = bool.TryParse(propertyValue, out _);
                    break;
                case AnimationPropertyType.Vector2:
                    Vector2Converter vector2Converter = new Vector2Converter();
                    isValid = (vector2Converter.ConvertFromInvariantString(propertyValue) != null);
                    break;
            }

            return isValid;
        }
    }
}
