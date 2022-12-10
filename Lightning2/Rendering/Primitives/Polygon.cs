namespace LightningGL
{
    /// <summary>
    /// Polygon
    /// 
    /// Primitive class for a polygon.
    /// </summary>
    public class Polygon : Primitive
    {
        /// <summary>
        /// The points of this polygon.
        /// </summary>
        public List<Vector2> Points { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"></param>
        public Polygon(string name) : base(name)
        {
            Points = new List<Vector2>();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        internal override void Draw()
        {
            // build a list of points
            // convert to make sdl2-gfx happy
            List<int> pointsListX = new();
            List<int> pointsListY = new();

            foreach (Vector2 point in Points)
            {
                pointsListX.Add(Convert.ToInt32(point.X));
                pointsListY.Add(Convert.ToInt32(point.Y));
            }

            int[] finalPointsX = pointsListX.ToArray();
            int[] finalPointsY = pointsListY.ToArray();

            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                int[] finalBorderPointsX = new int[finalPointsX.Length];
                int[] finalBorderPointsY = new int[finalPointsY.Length];

                Buffer.BlockCopy(finalPointsX, 0, finalBorderPointsX, 0, sizeof(int) * finalPointsX.Length);
                Buffer.BlockCopy(finalPointsY, 0, finalBorderPointsY, 0, sizeof(int) * finalPointsX.Length);

                // calculate the size
                int sizeX = default,
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

                sizeX = maxX - minY;
                sizeY = maxY - minY;
                
                // size for the renderer
                Size = new(sizeX, sizeY);

                // change the points so that the polygon is always larger
                for (int borderPointId = 0; borderPointId < finalBorderPointsX.Length; borderPointId++)
                {
                    // make it a reference so we always increment the actual value
                    ref int borderPointX = ref finalBorderPointsX[borderPointId];

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
                    ref int borderPointY = ref finalBorderPointsY[borderPointId];

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

                Lightning.Renderer.DrawPolygon(finalBorderPointsX, finalBorderPointsY, 
                    BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A, true);
            }

            // count will always be the same so use x count
            Lightning.Renderer.DrawPolygon(finalPointsX, finalPointsY,
                Color.R, Color.G, Color.B, Color.A, true);
        }
    }
}
