
namespace LightningGL
{
    /// <summary>
    /// AnimationProperty
    /// 
    /// September 5, 2022
    /// 
    /// Defines an animation property.
    /// </summary>
    public class AnimationProperty
    {
        /// <summary>
        /// The name of the animation property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the animation property.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The keyframes of this animation property. See <see cref="AnimationKeyframe"/>
        /// </summary>
        public List<AnimationKeyframe> Keyframes = new List<AnimationKeyframe>();

        public AnimationProperty(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
