namespace LightningGL
{
    /// <summary>
    /// PrimitiveRenderer
    /// 
    /// As of October 29, 2022, this class is simply a forwarder to the various primitive classes.
    /// </summary>
    public class PrimitiveAssetManager : AssetManager<Primitive>
    {
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
        public Pixel AddPixel(Vector2 position, Color color, bool snapToScreen = false, Renderable? parent = null)
        {
            Pixel pixel = new("Pixel")
            {
                Position = position,
                Color = color,
                SnapToScreen = snapToScreen,
            };

            Lightning.Renderer.AddRenderable(pixel, parent);

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
        public Line AddLine(Vector2 start, Vector2 end, Color color, bool antiAliased = true, bool snapToScreen = false, Renderable? parent = null)
        {
            Line line = new("Line")
            {
                Color = color,
                Start = start,
                End = end,
                Position = start, // set the position to the start of the line
                Size = end - start, // size to the start
                Antialiased = antiAliased,
                SnapToScreen = snapToScreen,
            };

            Lightning.Renderer.AddRenderable(line, parent);
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
            Vector2 borderSize = default, bool snapToScreen = false, Renderable? parent = null)
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

            Lightning.Renderer.AddRenderable(rectangle, parent);
            return rectangle;
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
        public Ellipse AddEllipse(Vector2 position, Vector2 size, Color color, bool filled = false, bool antiAliased = false, bool snapToScreen = false,
            Vector2 borderSize = default, Color borderColor = default, Renderable? parent = null)
        {
            Ellipse circle = new("Circle")
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

            Lightning.Renderer.AddRenderable(circle, parent);
            return circle;
        }
    }
}