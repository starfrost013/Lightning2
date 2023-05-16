
namespace LightningGL
{
    /// <summary>
    /// Component
    /// 
    /// A component of a Renderable in Lightning 2.0.
    /// </summary>
    public abstract class Component : LightningObject
    {
        [JsonIgnore]
        /// <summary>
        /// The current animation of this Renderable
        /// </summary>
        public Animation? CurrentAnimation { get; private set; }
            
        public virtual void StartCurrentAnimation()
        {
            if (CurrentAnimation == null
                || !CurrentAnimation.Loaded)
            {
                Logger.LogError("You must load an animation before playing it! The animation will not be set.", 151, LoggerSeverity.Error);
                return;
            }

            CurrentAnimation.StartAnimationFor(this);
        }

        public virtual void StopCurrentAnimation()
        {
            if (CurrentAnimation == null
                || !CurrentAnimation.Loaded)
            {
                return;
            }

            CurrentAnimation.StopAnimationFor(this);
        }
    }
}
