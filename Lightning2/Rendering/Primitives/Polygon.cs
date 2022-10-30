
using System.Drawing;

namespace LightningGL
{
    internal class Polygon : Primitive
    {
        internal List<Vector2> Points { get; set; }

        public Polygon()
        {
            Points = new List<Vector2>();
        }

        internal override void Draw(Renderer cRenderer)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cRenderer.Settings.Camera;

            if (currentCamera != null
                && !SnapToScreen)
            {
                for (int curPoint = 0; curPoint < Points.Count; curPoint++)
                {
                    Vector2 point = Points[curPoint];

                    point.X -= currentCamera.Position.X;
                    point.Y -= currentCamera.Position.Y;

                    // Vector2s cannot be returned by reference so we have to do this terribleness
                    Points[curPoint] = point;
                }
            }

            // build a list of points
            // convert to make sdl2-gfx happy
            List<short> finalPointsX = new List<short>();
            List<short> finalPointsY = new List<short>();

            foreach (Vector2 point in Points)
            {
                finalPointsX.Add(Convert.ToInt16(point.X));
                finalPointsY.Add(Convert.ToInt16(point.Y));
            }

            // count will always be the same
            if (Filled)
            {
                filledPolygonRGBA(cRenderer.Settings.RendererHandle, finalPointsX.ToArray(), finalPointsY.ToArray(), finalPointsX.Count, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                if (Antialiased)
                {
                    aapolygonRGBA(cRenderer.Settings.RendererHandle, finalPointsX.ToArray(), finalPointsY.ToArray(), finalPointsX.Count, Color.R, Color.G, Color.B, Color.A);
                }
                else
                {
                    polygonRGBA(cRenderer.Settings.RendererHandle, finalPointsX.ToArray(), finalPointsY.ToArray(), finalPointsX.Count, Color.R, Color.G, Color.B, Color.A);
                }
            }
        }
    }
}
