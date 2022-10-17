using Newtonsoft.Json.Linq;

namespace LightningGL
{
    /// <summary>
    /// Animation
    /// 
    /// September 5, 2022
    /// 
    /// Defines an animation
    /// </summary>
    public class Animation : Renderable
    {
        /// <summary>
        /// The list of properties animated by this animation.
        /// </summary>
        public List<AnimationProperty> Properties { get; private set; }

        /// <summary>
        /// Backing field for <see cref="Name"/>
        /// </summary>
        private string _name;

        /// <summary>
        /// Name of this animation
        /// </summary>
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    string tempName = Path.Substring(0, Path.LastIndexOf('.'));
                    return tempName;
                }

                return _name;
            }
            set
            {
                _name = value;
            }
        }

        [JsonIgnore]
        /// <summary>
        /// Path to this animation's JSON file.
        /// </summary>
        public string Path { get; internal set; }

        [JsonProperty] // required for internal
        /// <summary>
        /// The length of this animation in milliseconds.
        /// </summary>
        public int Length { get; set; }

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

                if (validTypeNames.Contains(property.Type, StringComparison.InvariantCultureIgnoreCase)) isRealType = true;

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

        public void StartAnimationFor(Renderable renderable) => renderable.AnimationTimer.Restart();

        public void StopAnimationFor(Renderable renderable)
        {
            if (!renderable.AnimationTimer.IsRunning) return;

            renderable.AnimationTimer.Stop();
        }

        public void UpdateAnimationFor(Renderable renderable)
        {
            if (!renderable.AnimationTimer.IsRunning) return;

            if (renderable.AnimationTimer.ElapsedMilliseconds > Length)
            {
                if (Repeat <= 0 // <= used for endless repeat
                    || RepeatCount < Repeat)
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
                for (int keyframeId = 0; keyframeId < property.Keyframes.Count; keyframeId++)
                {
                    AnimationKeyframe thisKeyframe = property.Keyframes[keyframeId];
                    AnimationKeyframe nextKeyframe = null;
                    if (property.Keyframes.Count - keyframeId > 1) nextKeyframe = property.Keyframes[keyframeId + 1];

                    bool needToUpdate = nextKeyframe != null ? needToUpdate = (renderable.AnimationTimer.ElapsedMilliseconds >= thisKeyframe.Position
                        && renderable.AnimationTimer.ElapsedMilliseconds <= nextKeyframe.Position) : false;

                    if (needToUpdate)
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

                            long max = nextKeyframe.Position - thisKeyframe.Position;
                            long cur = renderable.AnimationTimer.ElapsedMilliseconds - thisKeyframe.Position;
                            object finalValue = null;

                            string propertyType = property.Type.ToLowerInvariant();

                            AnimationPropertyType animationPropertyType = (AnimationPropertyType)Enum.Parse(typeof(AnimationPropertyType), propertyType, true);

                            // various different renderable types
                            // we do this instead of using typeconverter so that we do not have to use the qualified type name and also to reduce code complexity
                            switch (animationPropertyType)
                            {
                                case AnimationPropertyType.Int32:
                                    int intValue1 = Convert.ToInt32(thisKeyframe.Value),
                                        intValue2 = Convert.ToInt32(nextKeyframe.Value);
                                    finalValue = AnimationPropertyFactory.GetIntValue(intValue1, intValue2, cur, max);
                                    break;
                                case AnimationPropertyType.Float: // single is same value
                                    float floatValue1 = Convert.ToSingle(thisKeyframe.Value),
                                          floatValue2 = Convert.ToSingle(nextKeyframe.Value);
                                    finalValue = AnimationPropertyFactory.GetFloatValue(floatValue1, floatValue2, cur, max);
                                    break;
                                case AnimationPropertyType.Double:
                                    double doubleValue1 = Convert.ToDouble(thisKeyframe.Value),
                                           doubleValue2 = Convert.ToDouble(nextKeyframe.Value);
                                    finalValue = AnimationPropertyFactory.GetDoubleValue(doubleValue1, doubleValue2, cur, max);
                                    break;
                                case AnimationPropertyType.Vector2:
                                    Vector2Converter vec2Converter = new Vector2Converter();
                                    Vector2 vector2Value1 = (Vector2)vec2Converter.ConvertFromInvariantString(thisKeyframe.Value),
                                            vector2Value2 = (Vector2)vec2Converter.ConvertFromInvariantString(nextKeyframe.Value);
                                    finalValue = AnimationPropertyFactory.GetVector2Value(vector2Value1, vector2Value2, cur, max);
                                    break;
                                case AnimationPropertyType.Boolean:
                                    bool boolValue1 = Convert.ToBoolean(thisKeyframe.Value),
                                         boolValue2 = Convert.ToBoolean(nextKeyframe.Value);

                                    // we do not, technically, get a boolean value technically here
                                    // instead we check if we are more than 50% to next keyframe and deem it the second value if true
                                    // remember, boolean, only two possible values
                                    finalValue = AnimationPropertyFactory.GetBooleanValue(boolValue1, boolValue2, cur, max);
                                    
                                    break;
                                default:
                                    _ = new NCException($"Invalid animation property type. Only int, float, double, boolean, and Vector2 are supported!", 148,
                                    "Animation::UpdateAnimationFor attempted to set a value of an unsupported animation property type!", NCExceptionSeverity.FatalError);
                                    break;
                            }

                            Type renderableType = renderable.GetType();

                            PropertyInfo propertyInfo = renderableType.GetProperty(property.Name);

                            if (propertyInfo == null) _ = new NCException($"Attempted to set value of invalid animation property {property.Name}", 152,
                                    $"Tried to get an invalid properrty of the type {renderableType.Name} in call to Animation::UpdateAnimationFor", NCExceptionSeverity.FatalError);

                            propertyInfo.SetValue(renderable, finalValue, null);  

                        }
                        catch (Exception ex)
                        {
                            _ = new NCException($"Error: An error occurred converting an animation property of {property.Type}, value {thisKeyframe.Value}: \n\n{ex}", 147, 
                                "An exception occurred in Animation::UpdateAnimationFor");
                        }
                    }
                }
            }
        }

        internal void Unload() => Properties.Clear();
    }
}