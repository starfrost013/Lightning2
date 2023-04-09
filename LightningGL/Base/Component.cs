
namespace LightningGL
{
    /// <summary>
    /// Component
    /// 
    /// A component of a Renderable in Lightning 2.0.
    /// </summary>
    internal class Component
    {
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
