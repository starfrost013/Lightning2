
namespace LightningGL
{
    /// <summary>
    /// Component
    /// 
    /// A component of a Renderable in Lightning 2.0.
    /// </summary>
    public abstract class Component
    {
        public virtual Vector2 Size { get; set; }

        public virtual Vector2 Position { get; set; }   

        public virtual Vector2 RenderPosition { get; internal set; }

        public bool Loaded { get; protected set; }

        /// <summary>
        /// Called when the renderable is created.
        /// </summary>
        public virtual void Create()
        {

        }

        /// <summary>
        /// Draws this Renderable.
        /// NOT RUN if the component is not rendered.
        /// </summary>
        public virtual void Draw()
        {
            // temp?
        }

        /// <summary>
        /// Run each frame REGARDLESS of if the component is rendered or not.
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Called when the component is destroyed.
        /// </summary>
        public virtual void Destroy()
        {

        }
    }
}
