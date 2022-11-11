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

            Vector2 renderPoint1 = Point1,
                    renderPoint2 = Point2,
                    renderPoint3 = Point3;

            if (currentCamera != null
                && !SnapToScreen)
            {
                renderPoint1 = new(Point1.X - currentCamera.Position.X,
                    Point1.Y - currentCamera.Position.Y);
                renderPoint2 = new(Point2.X - currentCamera.Position.X,
                    Point2.Y - currentCamera.Position.Y);
                renderPoint3 = new(Point3.X - currentCamera.Position.X,
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

                if (renderPoint1.X > maxX) maxX = renderPoint1.X;
                if (renderPoint2.X > maxX) maxX = renderPoint2.X;
                if (renderPoint3.X > maxX) maxX = renderPoint3.X;
                minX = maxX; // start at the highest and work down
                if (renderPoint1.X < minX) minX = renderPoint1.X;
                if (renderPoint2.X < minX) minX = renderPoint2.X;
                if (renderPoint3.X < minX) minX = renderPoint3.X; 
                if (renderPoint1.Y > maxY) maxY = renderPoint1.Y;
                if (renderPoint2.Y > maxY) maxY = renderPoint2.Y;
                if (renderPoint3.Y > maxY) maxY = renderPoint3.Y;
                minY = maxY; // start at the highest and work down
                if (renderPoint1.Y < minY) minY = renderPoint1.Y;
                if (renderPoint2.Y < minY) minY = renderPoint2.Y;
                if (renderPoint3.Y < minY) minY = renderPoint3.Y;

                sizeX = maxX - minX;
                sizeY = maxY - minY;

                // set the size for the renderer
                Size = new(sizeX, sizeY);

                Vector2 borderPoint1 = default,
                        borderPoint2 = default,
                        borderPoint3 = default;

                // oh dear
                // in all of these, the else if and not else is so that we do nothing if it is exactly (sizeX / 2)
                // oh dear
                if (renderPoint1.X - minX > (sizeX / 2))
                {
                    borderPoint1.X = renderPoint1.X + BorderSize.X;
                }
                else if (renderPoint1.X - minX == (sizeX / 2))
                {
                    borderPoint1.X = renderPoint1.X;
                }
                else
                {
                    borderPoint1.X = renderPoint1.X - BorderSize.X;
                }

                if (renderPoint1.Y - minY > (sizeY / 2))
                {
                    borderPoint1.Y = renderPoint1.Y + BorderSize.Y;
                }
                else if (renderPoint1.Y - minY == (sizeY / 2))
                {
                    borderPoint1.Y = renderPoint1.Y;
                }
                else
                {
                    borderPoint1.Y = renderPoint1.Y - BorderSize.Y;
                }

                if (renderPoint2.X - minX > (sizeX / 2))
                {
                    borderPoint2.X = renderPoint2.X + BorderSize.X;
                }
                else if (renderPoint2.X - minX == (sizeX / 2))
                {
                    borderPoint2.X = renderPoint2.X;
                }
                else
                {
                    borderPoint2.X = renderPoint2.X - BorderSize.X;
                }

                if (renderPoint2.Y - minY > (sizeY / 2))
                {
                    borderPoint2.Y = renderPoint2.Y + BorderSize.Y;
                }
                else if (renderPoint2.Y - minY == (sizeY / 2))
                {
                    borderPoint2.Y = renderPoint2.Y;
                }
                else
                {
                    borderPoint2.Y = renderPoint2.Y - BorderSize.Y;
                }

                if (renderPoint3.X - minX > (sizeX / 2))
                {
                    borderPoint3.X = renderPoint3.X + BorderSize.X;
                }
                else if (renderPoint3.X - minX == (sizeX / 2))
                {
                    borderPoint3.X = renderPoint3.X;
                }
                else
                {
                    borderPoint3.X = renderPoint3.X - BorderSize.X;
                }

                if (renderPoint3.Y - minY > (sizeY / 2))
                {
                    borderPoint3.Y = renderPoint3.Y + BorderSize.Y;
                }
                else if (renderPoint3.Y - minY == (sizeY / 2))
                {
                    borderPoint3.Y = renderPoint3.Y;
                }
                else
                {
                    borderPoint3.Y = renderPoint3.Y - BorderSize.Y;
                }

                filledTrigonRGBA(Lightning.Renderer.Settings.RendererHandle, (int)borderPoint1.X, (int)borderPoint1.Y, (int)borderPoint2.X, (int)borderPoint2.Y,
                    (int)borderPoint3.X, (int)borderPoint3.Y, BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
            }
            
            if (Filled)
            {
                filledTrigonRGBA(Lightning.Renderer.Settings.RendererHandle, (int)renderPoint1.X, (int)renderPoint1.Y, (int)renderPoint2.X, (int)renderPoint2.Y, 
                    (int)renderPoint3.X, (int)renderPoint3.Y, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                trigonRGBA(Lightning.Renderer.Settings.RendererHandle, (int)renderPoint1.X, (int)renderPoint1.Y, (int)renderPoint2.X, (int)renderPoint2.Y, 
                    (int)renderPoint3.X, (int)renderPoint3.Y, Color.R, Color.G, Color.B, Color.A);
            }
        }
    }
}
