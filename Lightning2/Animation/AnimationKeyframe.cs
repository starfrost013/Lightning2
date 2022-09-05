
namespace LightningGL
{
    /// <summary>
    /// AnimationKeyframe
    /// 
    /// Defines an animation keyframe.
    /// </summary>
    public class AnimationKeyframe
    {
        public int Position { get; private set; }

        [JsonIgnore]
        public int Id { get; internal set; }

        public string Value { get; private set; }

        [JsonIgnore]
        public object RealValue { get; private set; }

        public AnimationKeyframe(int position, string value)
        {
            Position = position;
            Value = value;
        }
    }
}
