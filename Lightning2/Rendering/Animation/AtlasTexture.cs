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

        public void Load(Window Win, uint FramesX = 0, uint FramesY = 0)
        {
            if (FrameSize == default(Vector2)) throw new NCException("Cannot load a texture with no texture frame size!", 45, "AtlasTexture.LoadAtlas", NCExceptionSeverity.FatalError);

            if (FramesX < 1 || FramesY < 1) throw new NCException($"A texture atlas must have at least one frame, set to {FramesX},{FramesY}!", 46, "AtlasTexture.LoadAtlas", NCExceptionSeverity.FatalError);

            NCLogging.Log($"Loading atlas texture at path {Path}...");
            // +1 for safety purposes so that we don't set an out of bounds viewport
            Texture new_texture = new Texture(Win, new Vector2(FrameSize.X * FramesX + 1, FrameSize.Y * FramesY + 1));
            Size = new Vector2(FramesX, FramesY);

            new_texture.Path = Path;
            new_texture.Repeat = Repeat;
            new_texture.Position = Position;
            new_texture.Load(Win);

            if (new_texture.Handle != IntPtr.Zero) Atlas = new_texture;
        }

        public void DrawFrame(Window Win)
        {
            if (Index < 0
                || Index > (Size.X * Size.Y)) throw new NCException($"Cannot draw invalid AnimatedTexture ({Name}) frame ({Index} specified, range (0,0 to {FrameSize.X},{FrameSize.Y})!)", 47, "AnimatedTexture.LoadIndexed", NCExceptionSeverity.FatalError);

            int row = (int)(Index / Size.Y);

            float start_x = FrameSize.X * (Index / (row + 1)) - FrameSize.X;
            float start_y = FrameSize.Y * row;
            float end_x = start_x + FrameSize.X;
            float end_y = start_y + FrameSize.Y;

            Atlas.ViewportStart = new Vector2(start_x, start_y);
            Atlas.ViewportEnd = new Vector2(end_x, end_y);

            Atlas.Draw(Win);
        }

        public void GetPixel(int X, int Y, bool Relative = false)
        {
            int row = (int)(Index / Size.Y) + 1;

            if (Relative)
            {
                int relative_x = (int)(FrameSize.X * (Index / (row + 1)) - FrameSize.X);
                int relative_y = (int)(FrameSize.Y * row);
                Atlas.GetPixel(relative_x, relative_y);
            }
            else
            {
                Atlas.GetPixel(X, Y);
            }
        }

        public void SetPixel(int X, int Y, Color Colour, bool Relative = false)
        {
            int row = (int)(Index / Size.Y) + 1;

            if (Relative)
            {
                int relative_x = (int)(FrameSize.X * (Index / (row + 1)) - FrameSize.X);
                int relative_y = (int)(FrameSize.Y * row);
                Atlas.SetPixel(relative_x, relative_y, Colour);
            }
            else
            {
                Atlas.SetPixel(X, Y, Colour);
            }
        }

        public void Lock() => Atlas.Lock();
        public void Unlock() => Atlas.Unlock();
    }
}