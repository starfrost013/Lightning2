namespace LightningGL
{
    /// <summary>
    /// PrimitiveRenderer
    /// 
    /// As of October 29, 2022, this class is simply a forwarder to the various primitive classes.
    /// </summary>
    public class PrimitiveAssetManager : AssetManager<Primitive>
    {
        /// <summary>
        /// The size of the SDL2_gfx monospace font.
        /// </summary>
        private readonly static int MONOSPACE_FONT_SIZE = 7;

        public override Primitive AddAsset(Primitive asset)
        {
            Lightning.Renderer.AddRenderable(asset);
            return asset;
        }

        /// <summary>
        /// Draws a pixel on the screen.
        /// </summary>
        /// <param name="position">The position of the pixel to draw.</param>
        /// <param name="color">The <see cref="Color"/> of the pixel to draw.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public Pixel AddPixel(Vector2 position, Color color, bool snapToScreen = false)
        {
            Pixel pixel = new("Pixel")
            {
                Position = position,
                Color = color,
                SnapToScreen = snapToScreen,
            };

            Lightning.Renderer.AddRenderable(pixel);

            return pixel;
        }

        /// <summary>
        /// Draws a line to the screen.
        /// </summary>
        /// <param name="start">The start point of this line.</param>
        /// <param name="end">The endpoint of this line.</param>
        /// <param name="thickness">The thickness of this line.</param>
        /// <param name="color">The color of this line.</param>
        /// <param name="antiAliased">Determines if this line will be anti-aliased or not.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public Line AddLine(Vector2 start, Vector2 end, short thickness, Color color, bool antiAliased = true, bool snapToScreen = false)
        {
            Line line = new("Line")
            {
                Color = color,
                Start = start,
                End = end,
                Position = start, // set the position to the start of the line
                Size = end - start, // size to the start
                Thickness = thickness,
                Antialiased = antiAliased,
                SnapToScreen = snapToScreen,
            };

            Lightning.Renderer.AddRenderable(line);
            return line; 
        }

        /// <summary>
        /// Draws a rectangle to the screen.
        /// </summary>
        /// <param name="position">The position of the rectangle to draw</param>
        /// <param name="size">The size of the rectangle to draw.</param>
        /// <param name="color">The color of the rectangle to draw.</param>
        /// <param name="filled">Determines if this rectangle will be filled or not.</param>
        /// <param name="borderColor">The color of this rectangle's border.</param>
        /// <param name="borderSize">The size of this rectangle's border.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space (false) or screen-relative space (true).</param>
        public Rectangle AddRectangle(Vector2 position, Vector2 size, Color color, bool filled = false, Color borderColor = default,
            Vector2 borderSize = default, bool snapToScreen = false)
        {
            Rectangle rectangle = new("Rectangle")
            {
                Position = position,
                Size = size,
                Color = color,
                Filled = filled,
                BorderColor = borderColor,
                BorderSize = borderSize,
                SnapToScreen = snapToScreen,
            };

            Lightning.Renderer.AddRenderable(rectangle);
            return rectangle;
        }

        /// <summary>
        /// Draws a rounded rectangle to the screen.
        /// </summary>
        /// <param name="position">The position of the rectangle to draw</param>
        /// <param name="size">The size of the rectangle to draw.</param>
        /// <param name="color">The color of the rectangle to draw.</param>
        /// <param name="cornerRadius">The radius, in pixels, of this rectangle's corners.</param>
        /// <param name="filled">Determines if this rectangle will be filled or not.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public RoundedRectangle AddRoundedRectangle(Vector2 position, Vector2 size, Color color, int cornerRadius, bool filled = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default)
        {
            RoundedRectangle roundedRectangle = new("RoundedRectangle")
            {
                Position = position,
                Size = size,
                Color = color,
                Filled = filled,
                CornerRadius = cornerRadius,
                BorderSize = borderSize,
                BorderColor = borderColor,
                SnapToScreen = snapToScreen
            };

            Lightning.Renderer.AddRenderable(roundedRectangle);
            return roundedRectangle;
        }

        /// <summary>
        /// Draws a triangle to the screen
        /// </summary>
        /// <param name="point1">The first point of the triangle.</param>
        /// <param name="point2">The second point of the triangle.</param>
        /// <param name="point3">The third point of the triangle.</param>
        /// <param name="color">The color of the triangle - see <see cref="Color"/></param>
        /// <param name="filled">Determines if the triangle will be filled.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public Triangle AddTriangle(Vector2 point1, Vector2 point2, Vector2 point3, Color color, bool filled = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default)
        {
            Triangle triangle = new("Triangle")
            {
                Point1 = point1,
                Point2 = point2,
                Point3 = point3,
                Position = point1, // for now
                Color = color,
                Filled = filled,
                BorderSize = borderSize,
                BorderColor = borderColor,
                SnapToScreen = snapToScreen
            };

            Lightning.Renderer.AddRenderable(triangle);
            return triangle;
        }

        /// <summary>
        /// Draws a polygon to the screen.
        /// </summary>
        /// <param name="points">The points of the polygon.</param>
        /// <param name="color">The color of the polygon.</param>
        /// <param name="filled">Determines if the polygon will be filled or not.</param>
        /// <param name="antiAliased">Determines if the polygon will be anti-aliased - UNFILLED POLYGONS ONLY</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public Polygon AddPolygon(List<Vector2> points, Color color, bool filled = false, bool antiAliased = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default)
        {
            Polygon polygon = new("Polygon")
            {
                Points = points,
                Position = points[0], // for now
                Color = color,
                Filled = filled,
                Antialiased = antiAliased,
                BorderColor = borderColor,
                BorderSize = borderSize,
                SnapToScreen = snapToScreen
            };

            Lightning.Renderer.AddRenderable(polygon);
            return polygon;
        }

        /// <summary>
        /// Draws a circle to the screen.
        /// </summary>
        /// <param name="position">The position of the circle to draw</param>
        /// <param name="size">The size of the circle to draw.</param>
        /// <param name="color">The color of the circle to draw.</param>
        /// <param name="filled">Determines if this circle is filled.</param>
        /// <param name="antiAliased">Determines if this circle is anti-aliased. Only has an effect on unfilled circles for now</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public Circle AddCircle(Vector2 position, Vector2 size, Color color, bool filled = false, bool antiAliased = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default)
        {
            Circle circle = new("Circle")
            {
                Position = position,
                Size = size,
                Color = color,
                Filled = filled,
                Antialiased = antiAliased,
                BorderSize = borderSize,
                BorderColor = borderColor,
                SnapToScreen = snapToScreen
            };

            Lightning.Renderer.AddRenderable(circle);
            return circle;
        }

        /// <summary>
        /// Draws simple text to the screen using a debug font.
        /// </summary>
        /// <param name="Lightning.Renderer">The Window to draw the text to.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text to. </param>
        /// <param name="color">The color to draw the text as.</param>
        /// <param name="localise">If true, the text will be localised with <see cref="LocalisationManager"/> before being drawn.</param>
        ///  <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public BasicText AddText(string text, Vector2 position, Color color, bool localise = true, bool snapToScreen = false)
        {
            BasicText basicText = new("BasicText")
            {
                Position = position,
                Text = text,
                Localise = localise,
                Color = color,
                // the font is monospace so all characters are the same size
                Size = new(MONOSPACE_FONT_SIZE * text.Length, MONOSPACE_FONT_SIZE),
                SnapToScreen = snapToScreen
            };

            Lightning.Renderer.AddRenderable(basicText);
            return basicText;
        }
    }
}