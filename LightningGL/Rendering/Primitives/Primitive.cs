
namespace LightningGL
{
    /// <summary>
    /// Primitive
    /// 
    /// Defines a Primitive. The base class for primitives
    /// </summary>
    public class Primitive : Renderable
    {
        /// <summary>
        /// The color of this primitive.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The border colour of this primitive.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// The size of this primitive's border.
        /// To achieve borders, a larger shape is drawn first (with the value of this property
        /// added to the size of the property) and then the original shape is drawn 'on top'.
        /// </summary>
        public Vector2 BorderSize { get; set; }

        /// <summary>
        /// Determines if this primitive is filled.
        /// </summary>
        public bool Filled { get; set; }

        public Primitive(string name) : base(name) { }
    }
}
