
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
        /// The value of the animation property.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The keyframes of this animation property.
        /// </summary>
        public List<AnimationKeyframe> Keyframes { get; set; }  

        public AnimationProperty()
        {
            Keyframes = new List<AnimationKeyframe>();
        }
    }
}
