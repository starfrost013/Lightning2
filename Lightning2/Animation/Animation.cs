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
        public List<AnimationProperty> Properties { get; set; }

        public string Name { get; set; }

        public Animation()
        {
            Properties = new List<AnimationProperty>();
        }
    }
}
