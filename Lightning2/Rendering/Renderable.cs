using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// Renderable
    /// 
    /// June 12, 2022
    /// 
    /// Defines a renderable object. Simply introduced to reduce code reuse.
    /// </summary>
    public abstract class Renderable
    {
        /// <summary>
        /// The position of this texture.
        /// Must be valid to draw.
        /// </summary>
        public virtual Vector2 Position { get; set; }

        private Vector2 _renderPosition { get; set; }

        /// <summary>
        /// Internal value holding the position the item is actually rendered at.
        /// 
        /// HACK WARNIGN!
        /// </summary>
        internal Vector2 RenderPosition
        {
            get
            {
                // this MAY cause issues if the renderPosition is eve ractually (0,0)
                // will be fixed in GL version
                if (_renderPosition == default(Vector2)) return Position;
                return _renderPosition;

            }
            set
            {
                _renderPosition = value;
            }
        }

        /// <summary>
        /// The size of this texture. 
        /// Does not have to be equal to image size.
        /// </summary>
        public virtual Vector2 Size { get; set; }

        public bool SnapToScreen { get; set; }
        public virtual void Draw(Window cWindow)
        {

        }
    }
}
