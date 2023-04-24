namespace LightningGL
{
    /// <summary>
    /// TextureAtlas 
    /// 
    /// Defines an animated texture loaded from a texture atlas. 
    /// A texture atlas is a single image containing multiple images designed to represent different frames or textures.
    /// 
    /// This API is terrible and will be redesigned in 2.5.
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

        public TextureAtlas(Vector2 frameSize, Vector2 textureCount) : base((frameSize.X * textureCount.X) + 1, frameSize.Y * textureCount.Y + 1) // + 1 so that we do not set an out of bounds viewport
        {
            FrameSize = frameSize;
            TextureCount = textureCount;
        }

        public TextureAtlas(Vector2 frameSize, Vector2 textureCount, string path) : base((frameSize.X * textureCount.X) + 1, frameSize.Y * textureCount.Y + 1) // + 1 so that we do not set an out of bounds viewport
        {
            FrameSize = frameSize;
            TextureCount = textureCount;
            Path = path; 
        }

        public override void Create()
        {
            if (FrameSize == default) Logger.LogError("Cannot load a texture with no texture frame size!", 45, LoggerSeverity.FatalError);

            if (TextureCount.X < 1 
                || TextureCount.Y < 1) Logger.LogError($"A texture atlas must have at least one frame, set to {TextureCount.X},{TextureCount.Y}!", 
                    46, LoggerSeverity.FatalError);

            Logger.Log($"Loading texture atlas at path {Path}...");

            base.Create();
        }

        public void DrawFrame()
        {
            // save the maximum index
            int maxIndex = Convert.ToInt32(TextureCount.X * TextureCount.Y) - 1;

            if (Index < 0
                || Index > maxIndex)
            {
                Logger.LogError($"Cannot draw invalid TextureAtlas ({Path}) frame ({Index} specified, range (0,0 to {FrameSize.X},{FrameSize.Y})!)",
                    47, LoggerSeverity.Error, null, false);
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

            // Horrendous special case code
            // TODO: REDESIGN THIS ENTIRE API
            //
            // This is horrendously hacky code and will almost certainly break again, but this API will be killed as soon as 2.5 starts,
            // so i don't care
            Camera camera = Lightning.Renderer.Settings.Camera;

            RenderPosition = new(Position.X - camera.Position.X,
                    Position.Y - camera.Position.Y);

            // END HACK 
            // WHY DOES THIS EVEN WORK

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