using NuCore.Utilities;
using System;
using System.Drawing;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// AtlasTexture
    /// 
    /// March 19, 2022
    /// 
    /// Defines an animated texture loaded from a texture atlas. 
    /// A texture atlas is a single image containing multiple images designed to represent different frames or textures.
    /// </summary>
    public class AtlasTexture : Renderable
    {
        public uint Index { get; set; }

        public Texture Atlas { get; set; }

        public string Name { get; set; }

        private Vector2 _position { get; set; }

        public override Vector2 Position
        {
            get
            {
                if (Atlas != null) return Atlas.Position;
                return _position;
            }
            set
            {
                _position = value;
                if (Atlas != null) Atlas.Position = value;
            }
        }

        public Vector2 FrameSize { get; set; }

        private Vector2 _repeat { get; set; }

        public Vector2 Repeat
        {
            get
            {
                if (Atlas != null) return Atlas.Repeat;
                return _repeat;
            }
            set
            {
                _repeat = value;
                if (Atlas != null) Atlas.Repeat = value;
            }
        }

        private string _path { get; set; }

        /// <summary>
        /// Path to the texture 
        /// </summary>
        public string Path
        {
            get
            {
                if (_path == null)
                {
                    return "<<<CREATED ATLAS TEXTURE>>>";
                }
                else
                {
                    return _path;
                }
            }
            set
            {
                _path = value;
            }
        }

        public void Load(Window cWindow, uint framesX = 0, uint framesY = 0)
        {
            if (FrameSize == default(Vector2)) throw new NCException("Cannot load a texture with no texture frame size!", 45, "AtlasTexture.LoadAtlas", NCExceptionSeverity.FatalError);

            if (framesX < 1 || framesY < 1) throw new NCException($"A texture atlas must have at least one frame, set to {framesX},{framesY}!", 46, "AtlasTexture.LoadAtlas", NCExceptionSeverity.FatalError);

            NCLogging.Log($"Loading atlas texture at path {Path}...");
            // +1 for safety purposes so that we don't set an out of bounds viewport
            Texture newTexture = new Texture(cWindow, new Vector2(FrameSize.X * framesX + 1, FrameSize.Y * framesY + 1));
            Size = new Vector2(framesX, framesY);

            newTexture.Path = Path;
            newTexture.Repeat = Repeat;
            newTexture.Position = Position;
            newTexture.Load(cWindow);

            if (newTexture.Handle != IntPtr.Zero) Atlas = newTexture;
        }

        public override void Draw(Window cWindow)
        {
            if (Index < 0
                || Index > (Size.X * Size.Y)) throw new NCException($"Cannot draw invalid AnimatedTexture ({Name}) frame ({Index} specified, range (0,0 to {FrameSize.X},{FrameSize.Y})!)", 47, "AnimatedTexture.LoadIndexed", NCExceptionSeverity.FatalError);

            int row = (int)(Index / Size.Y);

            float startX = FrameSize.X * (Index / (row + 1)) - FrameSize.X;
            float startY = FrameSize.Y * row;
            float endX = startX + FrameSize.X;
            float endY = startY + FrameSize.Y;

            Atlas.ViewportStart = new Vector2(startX, startY);
            Atlas.ViewportEnd = new Vector2(endX, endY);

            Atlas.Draw(cWindow);
        }

        public void GetPixel(int x, int y, bool relative = false)
        {
            int row = (int)(Index / Size.Y) + 1;

            if (relative)
            {
                int relativeX = (int)(FrameSize.X * (Index / (row + 1)) - FrameSize.X);
                int relativeY = (int)(FrameSize.Y * row);
                Atlas.GetPixel(relativeX, relativeY);
            }
            else
            {
                Atlas.GetPixel(x, y);
            }
        }

        public void SetPixel(int x, int y, Color colour, bool relative = false)
        {
            int row = (int)(Index / Size.Y) + 1;

            if (relative)
            {
                int relativeX = (int)(FrameSize.X * (Index / (row + 1)) - FrameSize.X);
                int relativeY = (int)(FrameSize.Y * row);
                Atlas.SetPixel(relativeX, relativeY, colour);
            }
            else
            {
                Atlas.SetPixel(x, y, colour);
            }
        }

        public void Lock() => Atlas.Lock();
        public void Unlock() => Atlas.Unlock();
    }
}