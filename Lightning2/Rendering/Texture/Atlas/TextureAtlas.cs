using LightningGL;
using NuCore.Utilities;
using System;
using System.Drawing;
using System.Numerics;

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
        public uint Index { get; set; }

        /// <summary>
        /// Optional name of this texture.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The size of an individual texture in this texture atlas.
        /// </summary>
        public Vector2 FrameSize { get; set; }
        
        /// <summary>
        /// The number of textures in this atlas texture.
        /// </summary>
        public Vector2 TextureCount { get; set; }

        public TextureAtlas(Window cWindow, Vector2 frameSize, Vector2 textureCount) : base(cWindow, new((frameSize.X * textureCount.X) + 1, (frameSize.Y * textureCount.Y) + 1)) // + 1 so that we do not set an out of bounds viewport
        {
            FrameSize = frameSize;
            TextureCount = textureCount;
        }

        public override void Load(Window cWindow)
        {
            if (FrameSize == default(Vector2)) _ = new NCException("Cannot load a texture with no texture frame size!", 45, "TextureAtlas's FrameSize property = (0,0)!", NCExceptionSeverity.FatalError);

            if (TextureCount.X < 1 || TextureCount.Y < 1) _ = new NCException($"A texture atlas must have at least one frame, set to {TextureCount.X},{TextureCount.Y}!", 46, "TextureAtlas::TextureCount::X or Y < 1!", NCExceptionSeverity.FatalError);

            NCLogging.Log($"Loading atlas texture at path {Path}...");

            base.Load(cWindow);
        }

        public override void Draw(Window cWindow)
        {
            if (Index < 0
                || Index > (FrameSize.X * FrameSize.Y)) _ = new NCException($"Cannot draw invalid TextureAtlas ({Name}) frame ({Index} specified, range (0,0 to {FrameSize.X},{FrameSize.Y})!)", 47, "TextureAtlas::LoadIndexed", NCExceptionSeverity.FatalError);

            int row = (int)(Index / FrameSize.Y);

            float startX = FrameSize.X * (Index / (row + 1));
            float startY = FrameSize.Y * row;
            float endX = startX + FrameSize.X;
            float endY = startY + FrameSize.Y;

            ViewportStart = new Vector2(startX, startY);
            ViewportEnd = new Vector2(endX, endY);

            base.Draw(cWindow);
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

        public override void SetPixel(int x, int y, Color colour, bool relative = false)
        {
            int row = (int)(Index / Size.Y) + 1;

            if (relative)
            {
                int relativeX = (int)(FrameSize.X * (Index / (row + 1)));
                int relativeY = (int)(FrameSize.Y * row);
                base.SetPixel(relativeX, relativeY, colour);
            }
            else
            {
                base.SetPixel(x, y, colour);
            }
        }
    }
}