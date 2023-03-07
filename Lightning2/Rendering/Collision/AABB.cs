namespace LightningGL
{
    /// <summary>
    /// AABB
    /// 
    /// June 15, 2022
    /// 
    /// A simple AABB class. As physics is not yet in the engine,
    /// we do not presently implement center points or half-widths. 
    /// 
    /// These will be implemented later.
    /// </summary>
    public class AABB
    {
        /// <summary>
        /// Tests if two <see cref="Renderable"/>s are intersecting.
        /// </summary>
        /// <param name="render1">The first renderable to test for intersection.</param>
        /// <param name="render2">The second renderable to test for intersection.</param>
        /// <returns>A boolean determining if the two renderables are intersecting or not</returns>
        public static bool Intersects(Renderable render1, Renderable render2)
        {
            Vector2 render1Min = render1.RenderPosition;
            Vector2 render2Min = render2.RenderPosition;
            Vector2 render1Max = render1.RenderPosition + render1.Size;
            Vector2 render2Max = render2.RenderPosition + render2.Size;

            return (render1Min.X <= render2Max.X
                && render1Min.Y <= render2Max.Y
                && render1Max.X >= render2Min.X
                && render1Max.Y >= render2Min.Y);
        }

        /// <summary>
        /// Tests if a <see cref="Renderable"/> is intersecting with the position <paramref name="render2"/>.
        /// </summary>
        /// <param name="render1">The first renderable to test for intersection.</param>
        /// <param name="render2">The position to test for intersection with <paramref name="render1"/>.</param>
        /// <returns>A boolean determining if the two renderables are intersecting or not</returns>
        public static bool Intersects(Renderable render1, Vector2 render2)
        {
            Vector2 render1Min = render1.Position;
            Vector2 render1Max = render1.Position + render1.Size;

            return (render1Min.X <= render2.X
                && render1Min.Y <= render2.Y
                && render1Max.X >= render2.X
                && render1Max.Y >= render2.Y);
        }
    }
}
