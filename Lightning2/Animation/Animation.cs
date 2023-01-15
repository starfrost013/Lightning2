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
        [JsonProperty]
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
        public override string Name
        {
            get
            {
                if (_name == null)
                {
                    string tempName = Path[..Path.LastIndexOf('.')]; // range operator
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
        public string Path { get; set; }

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
        public bool Reverse { get; set; }

        /// <summary>
        /// Constructor for the Animation class
        /// </summary>
        public Animation(string name, string path) : base(name)
        {
            Properties = new();
            Path = path;
            Name = name;
            _name = Name; // fix compile warnings
        }

        public override void Create()
        {
            if (!File.Exists(Path))
            {
                NCError.ShowErrorBox("Attempted to load a nonexistent animation file.", 138, NCErrorSeverity.FatalError);
                return;
            }

            NCLogging.Log($"Deserialising animation JSON from {Path}...");

            // try to deserialise
            try
            {
                Animation? tempAnimation = JsonConvert.DeserializeObject<Animation>(File.ReadAllText(Path));

                if (tempAnimation == null)
                {
                    NCError.ShowErrorBox($"A fatal error occurred while deserialising an animation JSON.", 140, NCErrorSeverity.FatalError);
                    return;
                }

                // set properties
                Properties = tempAnimation.Properties;
                Length = tempAnimation.Length;
                Repeat = tempAnimation.Repeat;
                Reverse = tempAnimation.Reverse;

                NCLogging.Log($"Validating animation JSON from {Path}...");

                // Validate
                Loaded = Validate(); 
            }
            catch (Exception err)
            {
                NCError.ShowErrorBox($"A fatal error occurred while deserialising an animation JSON. See base exception information for further information.", 139, NCErrorSeverity.FatalError, err);
                return;
            }
        }

        /// <summary>
        /// Validate this animation
        /// </summary>
        internal bool Validate()
        {
            if (Length <= 0)
            {
                NCError.ShowErrorBox($"An animation must have a length of at least 1 millisecond (value = {Length})",
                    142, NCErrorSeverity.Error);
                return false; 
            }

            List<string> validTypeNames = new();

            // Check for a valid type
            // only iterate once to increase speed
            foreach (Assembly name in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in name.GetTypes()) validTypeNames.Add(type.Name);
            }

            foreach (AnimationProperty property in Properties)
            {
                if (string.IsNullOrWhiteSpace(property.Name))
                {
                    NCError.ShowErrorBox($"All properties in an Animation JSON must have a name!",
                        144, NCErrorSeverity.Error);
                    return false; 
                }

                bool isRealType = validTypeNames.Contains(property.Type);

                if (!isRealType)
                {
                    NCError.ShowErrorBox($"Tried to use an AnimationProperty {property.Name}, type {property.Type} which is not loaded in the current AppDomain",
                        141, NCErrorSeverity.Error);
                    return false;
                }

                if (property.Keyframes.Count == 0)
                {
                    NCError.ShowErrorBox($"The property {property.Name} has no keyframes!",
                        145, NCErrorSeverity.Error);
                    return false; 
                }

                for (int keyframeId = 0; keyframeId < property.Keyframes.Count; keyframeId++)
                {
                    AnimationKeyframe keyframe = property.Keyframes[keyframeId];
                    keyframe.Id = keyframeId;

                    if (keyframe.Position < 0
                        || keyframe.Position > Length)
                    {
                        NCError.ShowErrorBox($"Keyframe {keyframe.Id} for property {property.Name} is not within the animation." +
                            $" The value is {keyframe.Position}, range is (0,{Length})!", 143, NCErrorSeverity.Error);
                        return false; 
                    }
                    
                }
            }

            return true;
        }

        internal void StartAnimationFor(Renderable renderable) => renderable.AnimationTimer.Restart();

        internal void StopAnimationFor(Renderable renderable)
        {
            if (!renderable.AnimationTimer.IsRunning) return;

            renderable.AnimationTimer.Stop();
        }

        internal void UpdateAnimationFor(Renderable renderable)
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
                    AnimationKeyframe? nextKeyframe = null;

                    if (property.Keyframes.Count - keyframeId > 1) nextKeyframe = property.Keyframes[keyframeId + 1];

                    if (nextKeyframe != null)
                    {
                        bool needToUpdate = (renderable.AnimationTimer.ElapsedMilliseconds >= thisKeyframe.Position
                            && renderable.AnimationTimer.ElapsedMilliseconds <= nextKeyframe.Position);

                        if (needToUpdate)
                        {
                            try
                            {
                                // todo: propertycache
                                // this is very inefficient

                                if (thisKeyframe.GetType()
                                    != nextKeyframe.GetType())
                                {
                                    NCError.ShowErrorBox($"All Keyframes of an animation property must be of the same type!", 146, NCErrorSeverity.FatalError);
                                }

                                long max = nextKeyframe.Position - thisKeyframe.Position;
                                long cur = renderable.AnimationTimer.ElapsedMilliseconds - thisKeyframe.Position;
                                object? finalValue = null;

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
                                        Vector2Converter vec2Converter = new();
                                        Vector2? vector2Value1 = (Vector2?)vec2Converter.ConvertFromInvariantString(thisKeyframe.Value),
                                                vector2Value2 = (Vector2?)vec2Converter.ConvertFromInvariantString(nextKeyframe.Value);

                                        if (vector2Value1 == null
                                            || vector2Value2 == null)
                                        {
                                            NCError.ShowErrorBox("Attempted to convert invalid Vector2 animation...", 182, 
                                                NCErrorSeverity.FatalError);
                                            break;
                                        }
                                        else
                                        {
                                            finalValue = AnimationPropertyFactory.GetVector2Value((Vector2)vector2Value1, (Vector2)vector2Value2, cur, max);
                                        }
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
                                        NCError.ShowErrorBox($"Invalid animation property type. Only int, float, double, boolean, and Vector2 are supported!", 148,
                                        NCErrorSeverity.FatalError);
                                        break;
                                }

                                Type renderableType = renderable.GetType();

                                PropertyInfo? propertyInfo = renderableType.GetProperty(property.Name);

                                if (propertyInfo == null)
                                {
                                    NCError.ShowErrorBox($"Attempted to set value of invalid animation property {property.Name}", 152,
                                       NCErrorSeverity.FatalError);
                                    return;
                                }

                                propertyInfo.SetValue(renderable, finalValue, null);

                            }
                            catch (Exception ex)
                            {
                                NCError.ShowErrorBox($"Error: An error occurred converting an animation property of {property.Type}, value {thisKeyframe.Value}: \n\n{ex}", 147,
                                    NCErrorSeverity.FatalError);
                            }
                        }
                    }
                }
            }
        }

        public override void Destroy()
        {
            Properties.Clear();
        }

    }
}