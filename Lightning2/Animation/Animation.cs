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
        /// The list of properties animated by this animation.
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
        internal string Path { get; set; }

        [JsonProperty] // required for internal
        /// <summary>
        /// The length of this animation in milliseconds.
        /// </summary>
        public int Length { get; internal set; }

        [JsonProperty] // required for internal
        /// <summary>
        /// Determines if this animation repeats. 0 does not repeat, -1 or lower repeats endlessly
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        /// The current number of times this animation has repeated.
        /// </summary>
        private int RepeatCount { get; set; }

        [JsonProperty] // required for internal
        /// <summary>
        /// Determines if the animation will play in reverse or not.
        /// </summary>
        public bool Reverse { get; internal set; }  

        [JsonIgnore]
        /// <summary>
        /// Determines if this animation has been loaded.
        /// </summary>
        internal bool Loaded { get; set; }

        /// <summary>
        /// Constructor for the Animation class
        /// </summary>
        public Animation(string path)
        {
            Properties = new List<AnimationProperty>();
            Path = path;
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

                    if (keyframe.Position < 0
                        || keyframe.Position > Length) _ = new NCException($"Keyframe {keyframe.Id} for property {property.Name} is not within the animation. The value is {keyframe.Position}, range is (0,{Length})!",
                    143, "AnimationKeyframe::Position", NCExceptionSeverity.FatalError);
                }
            }
        }

        public void StartAnimationFor(Renderable renderable)
        {
            renderable.AnimationTimer.Restart();
        }

        public void StopAnimationFor(Renderable renderable)
        {
            if (!renderable.AnimationTimer.IsRunning) return;

            renderable.AnimationTimer.Stop();
        }

        public void UpdateAnimationFor(Renderable renderable)
        {
            if (renderable.AnimationTimer.ElapsedMilliseconds > Length)
            {
                if (Repeat > 0
                    && RepeatCount < Repeat)
                {
                    RepeatCount++;
                    renderable.AnimationTimer.Restart();
                }
                else
                {
                    StopAnimationFor(renderable);
                }
            }

            foreach (AnimationProperty property in Properties)
            {
                Type renderableType = property.GetType();

                for (int keyframeId = 0; keyframeId < property.Keyframes.Count; keyframeId++)
                {
                    AnimationKeyframe thisKeyframe = property.Keyframes[keyframeId];
                    AnimationKeyframe nextKeyframe = null;
                    if (property.Keyframes.Count - keyframeId > 1) nextKeyframe = property.Keyframes[keyframeId + 1];

                    bool needToCheck = false;

                    if (nextKeyframe != null)
                    {
                        needToCheck = (renderable.AnimationTimer.ElapsedMilliseconds >= thisKeyframe.Position
                        && renderable.AnimationTimer.ElapsedMilliseconds <= nextKeyframe.Position);
                    }

                    if (needToCheck)
                    {
                        try
                        {
                            // todo: propertycache
                            // this is very inefficient

                            if (thisKeyframe.GetType() 
                                != nextKeyframe.GetType())
                            {
                                _ = new NCException($"All Keyframes of an animation property must be of the same type!", 148, "The value of thisKeyframe::GetType is not the same as nextKeyframe::GetType in a call to Animation::UpdateAnimationFor", 
                                    NCExceptionSeverity.FatalError);
                            }

                            // enumerate valid animation types
                            object value1 = TypeDescriptor.GetConverter(property.Type).ConvertFromInvariantString(thisKeyframe.Value);
                            object value2 = TypeDescriptor.GetConverter(property.Type).ConvertFromInvariantString(nextKeyframe.Value);
                            object finalValue = null;

                            long max = nextKeyframe.Position - thisKeyframe.Position;
                            long cur = nextKeyframe.Position - renderable.AnimationTimer.ElapsedMilliseconds;

                            // get various animation types
                            if (value1 is int
                                && value2 is int)
                            {
                                finalValue = AnimationPropertyFactory.GetIntValue(value1, value2, cur, max);
                            }
                            else if (value1 is float
                                && value2 is float)
                            {
                                finalValue = AnimationPropertyFactory.GetFloatValue(value1, value2, cur, max);
                            }
                            else if (value1 is double
                                && value2 is double)
                            {
                                finalValue = AnimationPropertyFactory.GetDoubleValue(value1, value2, cur, max);
                            }
                            else if (value1 is Vector2
                                && value2 is Vector2)
                            {
                                finalValue = AnimationPropertyFactory.GetVector2Value(value1, value2, cur, max);
                            }
                            else
                            {
                                _ = new NCException($"Invalid animation property type. Only int, float, double, and Vector2 are supported!", 148, "Animation::UpdateAnimationFor attempted to set a value of an unsupported animation property type ",
                                    NCExceptionSeverity.FatalError);
                            }

                            PropertyInfo propertyInfo = renderableType.GetProperty(property.Name);
                            propertyInfo.SetValue(renderable, finalValue, null);  

                        }
                        catch (Exception ex)
                        {
                            _ = new NCException($"Error: An error occurred converting an animation property of {property.Type}, value {thisKeyframe.Value}: \n\n{ex}", 147, "An exception occurred in Animation::UpdateAnimationFor");
                        }
                    }
                }
            }
        }
    }
}
