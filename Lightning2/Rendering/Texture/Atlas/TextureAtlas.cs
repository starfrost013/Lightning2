namespace LightningGL
{
    /// <summary>
    /// TextureAtlas
    /// 
    /// March 19, 2022 (modified July 2, 2022: huge refactor)
    /// 
    /// Defines an animated texture loaded from a texture atlas. 
    /// A texture atlas is a single image containing multiple images designed to represent different frames or textures.
    /// </summary>
    public class TextureAtlas : Texture
    {
        /// <summary>
        /// The current index of this texture to draw.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The size of an individual texture in this texture atlas.
        /// </summary>
        public Vector2 FrameSize { get; init; }

        /// <summary>
        /// The number of textures in this atlas texture.
        /// </summary>
        public Vector2 TextureCount { get; init; }

        public TextureAtlas(string name, Vector2 frameSize, Vector2 textureCount) : base(name, (frameSize.X * textureCount.X) + 1, frameSize.Y * textureCount.Y + 1) // + 1 so that we do not set an out of bounds viewport
        {
            FrameSize = frameSize;
            TextureCount = textureCount;
        }

        public override void Create()
        {
            if (FrameSize == default) NCLogging.LogError("Cannot load a texture with no texture frame size!", 45, NCLoggingSeverity.FatalError);

            if (TextureCount.X < 1 
                || TextureCount.Y < 1) NCLogging.LogError($"A texture atlas must have at least one frame, set to {TextureCount.X},{TextureCount.Y}!", 
                    46, NCLoggingSeverity.FatalError);

            NCLogging.Log($"Loading atlas texture at path {Path}...");

            base.Create();
        }

        public void DrawFrame()
        {
            // save the maximum index
            int maxIndex = Convert.ToInt32(TextureCount.X * TextureCount.Y) - 1;

            if (Index < 0
                || Index > maxIndex)
            {
                NCLogging.LogError($"Cannot draw invalid TextureAtlas ({Name}, {Path}) frame ({Index} specified, range (0,0 to {FrameSize.X},{FrameSize.Y})!)",
                    47, NCLoggingSeverity.Error, null, false);
                return;
            }

            int row = (int)(Index / TextureCount.Y);

            // set the viewport
            float startX = FrameSize.X * (Index % TextureCount.X);
            float startY = FrameSize.Y * row;
            float endX = startX + FrameSize.X;
            float endY = startY + FrameSize.Y;

            ViewportStart = new Vector2(startX, startY);
            ViewportEnd = new Vector2(endX, endY);

            base.Draw();
        }

        public override void Draw()
        {
            // figure out a better way of "not drawing it once each frame"
        }

        public override Color GetPixel(int x, int y, bool relative = false)
        {
            int row = (int)(Index / Size.Y) + 1;

            if (relative)
            {
                int relativeX = (int)(FrameSize.X * (Index / (row + 1))) + x;
                int relativeY = (int)(FrameSize.Y * row) + y;
                return base.GetPixel(relativeX, relativeY);
            }
            else
            {
                return base.GetPixel(x, y);
            }
        }

        public override void SetPixel(int x, int y, Color color, bool relative = false)
        {
            int row = (int)(Index / Size.Y) + 1;

            if (relative)
            {
                int relativeX = (int)(FrameSize.X * (Index / (row + 1)));
                int relativeY = (int)(FrameSize.Y * row);
                base.SetPixel(relativeX, relativeY, color);
            }
            else
            {
                base.SetPixel(x, y, color);
            }
        }
    }
}