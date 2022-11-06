namespace LightningGL
{
    /// <summary>
    /// PrimitiveRenderer
    /// 
    /// As of October 29, 2022, this class is simply a forwarder
    /// </summary>
    public class PrimitiveAssetManager : AssetManager<Primitive>
    {
        public override Primitive AddAsset(Renderer cRenderer, Primitive asset)
        {
            Assets.Add(asset);
            return asset;
        }

        internal override void Update(Renderer cRenderer)
        {
            // This is done for 1.2 ONLY for API compatibility with 1.0/1.1
            Assets.Clear();
        }

        /// <summary>
        /// Draws a pixel on the screen.
        /// </summary>
        /// <param name="cRenderer">The window to draw the pixel to.</param>
        /// <param name="position">The position of the pixel to draw.</param>
        /// <param name="color">The <see cref="Color"/> of the pixel to draw.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public void AddPixel(Renderer cRenderer, Vector2 position, Color color, bool snapToScreen = false)
        {
            Assets.Add(new Pixel
            {
                Position = position,
                Color = color,
                SnapToScreen = snapToScreen,
            });
        }

        /// <summary>
        /// Draws a line to the screen.
        /// </summary>
        /// <param name="cRenderer">The window to draw this line to.</param>
        /// <param name="start">The start point of this line.</param>
        /// <param name="end">The endpoint of this line.</param>
        /// <param name="thickness">The thickness of this line.</param>
        /// <param name="color">The color of this line.</param>
        /// <param name="antiAliased">Determines if this line will be anti-aliased or not.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public void AddLine(Renderer cRenderer, Vector2 start, Vector2 end, short thickness, Color color, bool antiAliased = true, bool snapToScreen = false)
        {
            Assets.Add(new Line
            {
                Color = color,
                Start = start,
                End = end,
                Thickness = thickness,
                Antialiased = antiAliased,
                SnapToScreen = snapToScreen,
            });
        }

        /// <summary>
        /// Draws a rectangle to the screen.
        /// </summary>
        /// <param name="cRenderer">The window to draw this line to.</param>
        /// <param name="position">The position of the rectangle to draw</param>
        /// <param name="size">The size of the rectangle to draw.</param>
        /// <param name="color">The color of the rectangle to draw.</param>
        /// <param name="filled">Determines if this rectangle will be filled or not.</param>
        /// <param name="borderColor">The color of this rectangle's border.</param>
        /// <param name="borderSize">The size of this rectangle's border.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space (false) or screen-relative space (true).</param>
        public void AddRectangle(Renderer cRenderer, Vector2 position, Vector2 size, Color color, bool filled = false, Color borderColor = default(Color),
            Vector2 borderSize = default(Vector2), bool snapToScreen = false)
        {
            Assets.Add(new Rectangle
            {
                Position = position,
                Size = size,
                Color = color,
                Filled = filled,
                BorderColor = borderColor,
                BorderSize = borderSize,
                SnapToScreen = snapToScreen,
            });
        }

        /// <summary>
        /// Draws a rounded rectangle to the screen.
        /// </summary>
        /// <param name="cRenderer">The window to draw this rectangle to.</param>
        /// <param name="position">The position of the rectangle to draw</param>
        /// <param name="size">The size of the rectangle to draw.</param>
        /// <param name="color">The color of the rectangle to draw.</param>
        /// <param name="cornerRadius">The radius, in pixels, of this rectangle's corners.</param>
        /// <param name="filled">Determines if this rectangle will be filled or not.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public void AddRoundedRectangle(Renderer cRenderer, Vector2 position, Vector2 size, Color color, int cornerRadius, bool filled = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default)
        {
            Assets.Add(new RoundedRectangle
            {
                Position = position,
                Size = size,
                Color = color,
                Filled = filled,
                CornerRadius = cornerRadius,
                BorderSize = borderSize,
                BorderColor = borderColor,
                SnapToScreen = snapToScreen
            });

        }

        /// <summary>
        /// Draws a triangle to the screen
        /// </summary>
        /// <param name="cRenderer">The Window to draw the triangle to.</param>
        /// <param name="point1">The first point of the triangle.</param>
        /// <param name="point2">The second point of the triangle.</param>
        /// <param name="point3">The third point of the triangle.</param>
        /// <param name="color">The color of the triangle - see <see cref="Color"/></param>
        /// <param name="filled">Determines if the triangle will be filled.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public void AddTriangle(Renderer cRenderer, Vector2 point1, Vector2 point2, Vector2 point3, Color color, bool filled = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default)
        {
            Assets.Add(new Triangle
            {
                Point1 = point1,
                Point2 = point2,
                Point3 = point3,
                Color = color,
                Filled = filled,
                BorderSize = borderSize,
                BorderColor = borderColor,
                SnapToScreen = snapToScreen
            });
        }

        /// <summary>
        /// Draws a polygon to the screen.
        /// </summary>
        /// <param name="cRenderer">The window to draw the polygon to.</param>
        /// <param name="points">The points of the polygon.</param>
        /// <param name="color">The color of the polygon.</param>
        /// <param name="filled">Determines if the polygon will be filled or not.</param>
        /// <param name="antiAliased">Determines if the polygon will be anti-aliased - UNFILLED POLYGONS ONLY</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public void AddPolygon(Renderer cRenderer, List<Vector2> points, Color color, bool filled = false, bool antiAliased = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default)
        {
            Assets.Add(new Polygon
            {
                Points = points,
                Color = color,
                Filled = filled,
                Antialiased = antiAliased,
                BorderColor = borderColor,
                BorderSize = borderSize,
                SnapToScreen = snapToScreen
            });
        }

        /// <summary>
        /// Draws a circle to the screen.
        /// </summary>
        /// <param name="cRenderer">The window to draw this circle to.</param>
        /// <param name="position">The position of the circle to draw</param>
        /// <param name="size">The size of the circle to draw.</param>
        /// <param name="color">The color of the circle to draw.</param>
        /// <param name="filled">Determines if this circle is filled.</param>
        /// <param name="antiAliased">Determines if this circle is anti-aliased. Only has an effect on unfilled circles for now</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public void AddCircle(Renderer cRenderer, Vector2 position, Vector2 size, Color color, bool filled = false, bool antiAliased = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default)
        {
            Assets.Add(new Circle
            {
                Position = position,
                Size = size,
                Color = color,
                Filled = filled,
                Antialiased = antiAliased,
                BorderSize = borderSize,
                BorderColor = borderColor,
                SnapToScreen = snapToScreen
            });
        }

        /// <summary>
        /// Draws simple text to the screen using a debug font.
        /// </summary>
        /// <param name="cRenderer">The Window to draw the text to.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text to. </param>
        /// <param name="color">The color to draw the text as.</param>
        /// <param name="localise">If true, the text will be localised with <see cref="LocalisationManager"/> before being drawn.</param>
        ///  <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public void AddText(Renderer cRenderer, string text, Vector2 position, Color color, bool localise = true, bool snapToScreen = false)
        {
            Assets.Add(new BasicText
            {
                Position = position,
                Text = text,
                Localise = localise,
                Color = color,
                SnapToScreen = snapToScreen
            });
        }
    }
}