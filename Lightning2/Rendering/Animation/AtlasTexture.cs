﻿using NuCore.Utilities;
using System;
using System.Drawing;
using System.Numerics;

namespace Lightning2
{
    /// <summary>
    /// AtlasTexture
    /// 
    /// March 19, 2022
    /// 
    /// Defines an animated texture loaded from a texture atlas. 
    /// A texture atlas is a single image containing multiple images designed to represent different frames or textures.
    /// </summary>
    public class AtlasTexture
    {
        public Vector2 Index { get; set; }

        public Texture Atlas { get; set; }

        public string Name { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 FrameSize { get; set; }

        public Vector2 Repeat { get; set; }

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


        public void Load(Window Win, int FramesX = 0, int FramesY = 0)
        {
            if (FrameSize == default(Vector2)) throw new NCException("Cannot load a texture with no texture frame size!", 45, "AtlasTexture.LoadAtlas", NCExceptionSeverity.FatalError);

            if (FramesX < 1 || FramesY < 1) throw new NCException($"A texture atlas must have at least one frame, set to {FramesX},{FramesY}!", 46, "AtlasTexture.LoadAtlas", NCExceptionSeverity.FatalError);

            // +1 for safety purposes so that we don't set an out of bounds viewport
            Texture new_texture = new Texture(Win, FrameSize.X * FramesX + 1, FrameSize.Y * FramesY + 1);

            new_texture.Path = Path;
            new_texture.Repeat = Repeat;
            new_texture.Position = Position;
            new_texture.Load(Win);

            if (new_texture.TextureHandle != IntPtr.Zero) Atlas = new_texture;

        }

        public void DrawFrame(Window Win, bool SnapToScreen = false)
        {
            Camera cur_camera = Win.Settings.Camera;

            if (Index.X < 0
                || Index.Y < 0
                || Index.X > FrameSize.X
                || Index.Y > FrameSize.Y) throw new NCException($"Cannot draw invalid AnimatedTexture ({Name}) frame ({Index} specified, range (0,0 to {FrameSize.X},{FrameSize.Y})!)", 47, "AnimatedTexture.LoadIndexed", NCExceptionSeverity.FatalError);

            float start_x = FrameSize.X * Index.X;
            float start_y = FrameSize.Y * Index.Y;
            float end_x = (FrameSize.X * Index.X) + FrameSize.X;
            float end_y = (FrameSize.Y * Index.Y) + FrameSize.Y;

            if (!SnapToScreen)
            {
                start_x -= cur_camera.Position.X;
                start_y -= cur_camera.Position.Y;
                end_x -= cur_camera.Position.X;
                end_y -= cur_camera.Position.Y;
            }

            Atlas.ViewportStart = new Vector2(start_x, start_y);
            Atlas.ViewportEnd = new Vector2(end_x, end_y);

            Atlas.Draw(Win);
        }

        public void GetPixel(int X, int Y, bool Relative = false)
        {
            if (Relative)
            {
                Atlas.GetPixel((int)(FrameSize.X * Index.X) + X, (int)(FrameSize.Y * Index.Y) + Y);
            }
            else
            {
                Atlas.GetPixel(X, Y);
            }
        }

        public void SetPixel(int X, int Y, Color Colour, bool Relative = false)
        {
            if (Relative)
            {
                Atlas.SetPixel((int)(FrameSize.X * Index.X) + X, (int)(FrameSize.Y * Index.Y) + Y, Colour);
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
