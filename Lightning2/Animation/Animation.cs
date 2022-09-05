using System.Reflection;

namespace LightningGL
{
    /// <summary>
    /// Animation
    /// 
    /// September 5, 2022
    /// 
    /// Defines an animation
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// The list of animation 
        /// </summary>
        public List<AnimationProperty> Properties { get; private set; }

        /// <summary>
        /// Name of this animation
        /// </summary>
        public string Name { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Path to this animation's JSON file.
        /// </summary>
        public string Path { get; internal set; }

        /// <summary>
        /// The length of this animation in milliseconds.
        /// </summary>
        public int Length { get; internal set; }

        [JsonIgnore]
        /// <summary>
        /// Determines if this animation has been loaded.
        /// </summary>
        internal bool Loaded { get; set; }

        /// <summary>
        /// Constructor for the Animation class
        /// </summary>
        public Animation()
        {
            Properties = new List<AnimationProperty>();
        }
        
        /// <summary>
        /// Validate this animation
        /// </summary>
        public void Validate()
        {
            if (Length <= 0)
            {
                _ = new NCException($"An animation must have a length of at least 1 millisecond (value = {Length})",
                    142, "Animation::Length was less than or equal to 0 during Animation::Validate call", NCExceptionSeverity.FatalError);
            }

            List<string> validTypeNames = new List<string>();

            // Check for a valid type
            // only iterate once to increase speed
            foreach (Assembly name in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in name.GetTypes()) validTypeNames.Add(type.Name);
            }

            foreach (AnimationProperty property in Properties)
            {
                if (string.IsNullOrWhiteSpace(property.Name)) _ = new NCException($"All properties in an Animation JSON must have a name!", 
                    144, "AnimationProperty::Name failed a string.IsNullOrWhiteSpace check", NCExceptionSeverity.FatalError);

                bool isRealType = false;

                if (validTypeNames.Contains(property.Type)) isRealType = true;

                if (!isRealType) _ = new NCException($"Tried to use an AnimationProperty {property.Name}, type {property.Type} which is not loaded in the current AppDomain",
                    141, "Tried to instantiate a type for an AnimationProperty from an unloaded assembly.", NCExceptionSeverity.FatalError);

                if (property.Keyframes.Count == 0) _ = new NCException($"The property {property.Name} has no keyframes!", 
                    145, "AnimationProperty::Keyframes::Count was 0 during call to Animation::Validate", NCExceptionSeverity.FatalError);

                for (int keyframeId = 0; keyframeId < property.Keyframes.Count; keyframeId++)
                {
                    AnimationKeyframe keyframe = property.Keyframes[keyframeId];
                    keyframe.Id = keyframeId;

                    if (keyframe.Position <= 0
                        || keyframe.Position > Length) _ = new NCException($"Keyframe {keyframe.Id} for property {property.Name} is not within the animation. The value is {keyframe.Position}, range is (0,{Length})!",
                    143, "AnimationKeyframe::Position", NCExceptionSeverity.FatalError);
                }
            }
        }

    }
}
