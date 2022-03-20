using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics; 
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// AnimatedTexture
    /// 
    /// March 19, 2022
    /// 
    /// Defines an animated texture loaded from a series of images.
    /// 
    /// awaiting refactor 3/20/22
    /// </summary>
    public class AnimatedTexture
    {
        public List<Texture> Frames { get; set; }

        public Texture CurrentFrame { get; set; }

        public string Name { get; set; }

        public List<string> FramePath { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Repeat { get; set; }

        public Vector2 Size { get; set; }

        public AnimatedTexture()
        {
            Frames = new List<Texture>();
            FramePath = new List<string>();
        }

        public void Load(Window Win, string[] Path)
        {
            if (Size == default(Vector2)) throw new NCException("Cannot load a texture with no texture size!", 44, "AnimatedTexture.LoadIndexed", NCExceptionSeverity.FatalError);

            foreach (string TexturePath in Path)
            {
                Texture new_texture = new Texture(Size.X, Size.Y);
                new_texture.Path = TexturePath;
                new_texture.Position = Position;
                new_texture.Repeat = Repeat;  // do this in the getter/setter?
                // Texture will only load current or throw fatal error. Maybe add Loaded attribute that checks if TextureHandle isn't a nullptr?
                new_texture.Load(Win);

                if (new_texture.TextureHandle != IntPtr.Zero) Frames.Add(CurrentFrame);

            }
        }

        public void SetCurrentFrame(int Index)
        {
            if (Index < 0 ||
                Index > Frames.Count) throw new NCException($"Cannot draw invalid AnimatedTexture frame! ({Index}, max {Frames.Count}!)", 48, "AnimatedTexture.LoadIndexed", NCExceptionSeverity.FatalError);

            CurrentFrame = Frames[Index];
        }

        public void DrawCurrentFrame(Window Win)
        {
            CurrentFrame.Draw(Win);
        }

        public Color4 GetPixel(int X, int Y) => CurrentFrame.GetPixel(X, Y);

        public void SetPixel(int X, int Y, Color4 Colour) => CurrentFrame.SetPixel(X, Y, Colour);

        public void Lock() => CurrentFrame.Lock();

        public void Unlock() => CurrentFrame.Unlock();  
    }
}
