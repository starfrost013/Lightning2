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
            List<short> pointsListX = new List<short>();
            List<short> pointsListY = new List<short>();

            foreach (Vector2 point in Points)
            {
                pointsListX.Add(Convert.ToInt16(point.X));
                pointsListY.Add(Convert.ToInt16(point.Y));
            }

            short[] finalPointsX = pointsListX.ToArray();
            short[] finalPointsY = pointsListY.ToArray();

            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                short[] finalBorderPointsX = new short[finalPointsX.Length];
                short[] finalBorderPointsY = new short[finalPointsY.Length];

                Buffer.BlockCopy(finalPointsX, 0, finalBorderPointsX, 0, sizeof(short) * finalPointsX.Length);
                Buffer.BlockCopy(finalPointsY, 0, finalBorderPointsY, 0, sizeof(short) * finalPointsX.Length);

                // calculate the size
                short sizeX = default,
                      sizeY = default,
                      minX = default,
                      minY = default,
                      maxX = default,
                      maxY = default;

                // find size of polygon 
                foreach (short finalBorderPointX in finalBorderPointsX)
                {
                    if (finalBorderPointX > maxX)
                    {
                        maxX = finalBorderPointX;
                        // start at the highest and then work dwon
                        if (minX == default) minX = maxX;
                    }

                    if (finalBorderPointX < minX) minX = finalBorderPointX;
                }

                foreach (short finalBorderPointY in finalBorderPointsY)
                {
                    if (finalBorderPointY > maxY)
                    {
                        maxY = finalBorderPointY;
                        // start at the highest and then work dwon
                        if (minY == default) minY = maxY;
                    }

                    if (finalBorderPointY < minY) minY = finalBorderPointY;
                }

                sizeX = Convert.ToInt16(maxX - sizeX);
                sizeY = Convert.ToInt16(maxY - sizeY);

                // change the points so that the polygon is always larger
                for (int borderPointId = 0; borderPointId < finalBorderPointsX.Length; borderPointId++)
                {
                    // make it a reference so we always increment the actual value
                    ref short borderPointX = ref finalBorderPointsX[borderPointId];

                    // check for halfway point
                    if (borderPointX - minX > (sizeX / 2))
                    {
                        borderPointX += Convert.ToInt16(BorderSize.X);
                    }
                    // we don't want to do anything if it is exactly (sizeY / 2)
                    else if (borderPointX - minX < (sizeX / 2))
                    {
                        borderPointX -= Convert.ToInt16(BorderSize.X);
                    }
                }

                for (int borderPointId = 0; borderPointId < finalBorderPointsY.Length; borderPointId++)
                {
                    // make it a reference so we always increment the actual value
                    ref short borderPointY = ref finalBorderPointsY[borderPointId];

                    // check for halfway point
                    if (borderPointY - minY > (sizeY / 2))
                    {
                        borderPointY += Convert.ToInt16(BorderSize.Y);
                    }
                    // we don't want to do anything if it is exactly (sizeY / 2)
                    else if (borderPointY - minY < (sizeY / 2))
                    {
                        borderPointY -= Convert.ToInt16(BorderSize.Y);
                    }
                }

                filledPolygonRGBA(cRenderer.Settings.RendererHandle, finalBorderPointsX, finalBorderPointsY, finalBorderPointsX.Length, 
                    BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
            }

            // count will always be the same so use x count
            if (Filled)
            {
                filledPolygonRGBA(cRenderer.Settings.RendererHandle, finalPointsX, finalPointsY, finalPointsX.Length, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                if (Antialiased)
                {
                    aapolygonRGBA(cRenderer.Settings.RendererHandle, finalPointsX, finalPointsY, finalPointsX.Length, Color.R, Color.G, Color.B, Color.A);
                }
                else
                {
                    polygonRGBA(cRenderer.Settings.RendererHandle, finalPointsX, finalPointsY, finalPointsX.Length, Color.R, Color.G, Color.B, Color.A);
                }
            }
        }
    }
}
