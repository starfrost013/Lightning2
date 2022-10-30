using System.Drawing;

namespace LightningGL
{
    public class Triangle : Renderable
    {
        public bool Filled { get; set; }

        public Vector2 Point1 { get; set; }

        public Vector2 Point2 { get; set; }

        public Vector2 Point3 { get; set; }

        internal override void Draw(Renderer cRenderer)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cRenderer.Settings.Camera;

            if (currentCamera != null
                && !SnapToScreen)
            {
                Point1 = new(Point1.X - currentCamera.Position.X,
                    Point1.Y - currentCamera.Position.Y);
                Point2 = new(Point2.X - currentCamera.Position.X,
                    Point2.Y - currentCamera.Position.Y);
                Point3 = new(Point3.X - currentCamera.Position.X,
                    Point3.Y - currentCamera.Position.Y);
            }

            if (Filled)
            {
                filledTrigonRGBA(cRenderer.Settings.RendererHandle, (int)Point1.X, (int)Point1.Y, (int)Point2.X, (int)Point2.Y, (int)Point3.X,
                (int)Point3.Y, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                trigonRGBA(cRenderer.Settings.RendererHandle, (int)Point1.X, (int)Point1.Y, (int)Point2.X, (int)Point2.Y, (int)Point3.X,
                    (int)Point3.Y, Color.R, Color.G, Color.B, Color.A);
            }
        }
    }
}
