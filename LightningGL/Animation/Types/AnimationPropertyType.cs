

namespace LightningGL
{
    /// <summary>
    /// Enumerates supported animation property types.
    /// </summary>
    public enum AnimationPropertyType
    {
        /// <summary>
        /// An integer animation.
        /// </summary>
        Int32 = 0,

        /// <summary>
        /// A double-precision floating point animation.
        /// </summary>
        Double = 1,

        /// <summary>
        /// A single-precision floating point animation.
        /// </summary>
        Float = 2,

        /// <summary>
        /// See <see cref="Float"/> for documentation of this value.
        /// </summary>
        Single = Float,

        /// <summary>
        /// A Vector2 animation.
        /// </summary>
        Vector2 = 3,

        /// <summary>
        /// A boolean animation
        /// </summary>
        Boolean = 4,

        /// <summary>
        /// See <see cref="Boolean"/> for documentation of this value.
        /// </summary>
        Bool = Boolean,
    }
}
