namespace LightningGL
{
    public class Triangle : Primitive
    {
        public Triangle(string name) : base(name)
        {

        }

        public Vector2 Point1 { get; set; }

        public Vector2 Point2 { get; set; }

        public Vector2 Point3 { get; set; }

        internal override void Draw()
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

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

            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                // as we don't have a size, so calculate extents use a multiplication factor to approximate things
                // in 1.3 we will not use sdl2_gfx

                float minX = 0,
                      maxX = 0,
                      minY = 0,
                      maxY = 0,
                      sizeX = 0,
                      sizeY = 0;

                if (Point1.X > maxX) maxX = Point1.X;
                if (Point2.X > maxX) maxX = Point2.X;
                if (Point3.X > maxX) maxX = Point3.X;
                minX = maxX; // start at the highest and work down
                if (Point1.X < minX) minX = Point1.X;
                if (Point2.X < minX) minX = Point2.X;
                if (Point3.X < minX) minX = Point3.X; 
                if (Point1.Y > maxY) maxY = Point1.Y;
                if (Point2.Y > maxY) maxY = Point2.Y;
                if (Point3.Y > maxY) maxY = Point3.Y;
                minY = maxY; // start at the highest and work down
                if (Point1.Y < minY) minY = Point1.Y;
                if (Point2.Y < minY) minY = Point2.Y;
                if (Point3.Y < minY) minY = Point3.Y;

                sizeX = maxX - minX;
                sizeY = maxY - minY;

                Vector2 borderPoint1 = default,
                        borderPoint2 = default,
                        borderPoint3 = default;

                // oh dear
                // in all of these, the else if and not else is so that we do nothing if it is exactly (sizeX / 2)
                // oh dear
                if (Point1.X - minX > (sizeX / 2))
                {
                    borderPoint1.X = Point1.X + BorderSize.X;
                }
                else if (Point1.X - minX == (sizeX / 2))
                {
                    borderPoint1.X = Point1.X;
                }
                else
                {
                    borderPoint1.X = Point1.X - BorderSize.X;
                }

                if (Point1.Y - minY > (sizeY / 2))
                {
                    borderPoint1.Y = Point1.Y + BorderSize.Y;
                }
                else if (Point1.Y - minY == (sizeY / 2))
                {
                    borderPoint1.Y = Point1.Y;
                }
                else
                {
                    borderPoint1.Y = Point1.Y - BorderSize.Y;
                }

                if (Point2.X - minX > (sizeX / 2))
                {
                    borderPoint2.X = Point2.X + BorderSize.X;
                }
                else if (Point2.X - minX == (sizeX / 2))
                {
                    borderPoint2.X = Point2.X;
                }
                else
                {
                    borderPoint2.X = Point2.X - BorderSize.X;
                }

                if (Point2.Y - minY > (sizeY / 2))
                {
                    borderPoint2.Y = Point2.Y + BorderSize.Y;
                }
                else if (Point2.Y - minY == (sizeY / 2))
                {
                    borderPoint2.Y = Point2.Y;
                }
                else
                {
                    borderPoint2.Y = Point2.Y - BorderSize.Y;
                }

                if (Point3.X - minX > (sizeX / 2))
                {
                    borderPoint3.X = Point3.X + BorderSize.X;
                }
                else if (Point3.X - minX == (sizeX / 2))
                {
                    borderPoint3.X = Point3.X;
                }
                else
                {
                    borderPoint3.X = Point3.X - BorderSize.X;
                }

                if (Point3.Y - minY > (sizeY / 2))
                {
                    borderPoint3.Y = Point3.Y + BorderSize.Y;
                }
                else if (Point3.Y - minY == (sizeY / 2))
                {
                    borderPoint3.Y = Point3.Y;
                }
                else
                {
                    borderPoint3.Y = Point3.Y - BorderSize.Y;
                }

                filledTrigonRGBA(Lightning.Renderer.Settings.RendererHandle, (int)borderPoint1.X, (int)borderPoint1.Y, (int)borderPoint2.X, (int)borderPoint2.Y,
                    (int)borderPoint3.X, (int)borderPoint3.Y, BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
            }
            
            if (Filled)
            {
                filledTrigonRGBA(Lightning.Renderer.Settings.RendererHandle, (int)Point1.X, (int)Point1.Y, (int)Point2.X, (int)Point2.Y, (int)Point3.X, (int)Point3.Y, 
                    Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                trigonRGBA(Lightning.Renderer.Settings.RendererHandle, (int)Point1.X, (int)Point1.Y, (int)Point2.X, (int)Point2.Y, (int)Point3.X,
                    (int)Point3.Y, Color.R, Color.G, Color.B, Color.A);
            }
        }
    }
}
