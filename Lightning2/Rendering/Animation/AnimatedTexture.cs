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
        /// <summary>
        /// The frames of this texture. A list of <see cref="Texture"/> objects.
        /// </summary>
        public List<Texture> Frames { get; set; }

        /// <summary>
        /// The current frame in the animationcycle.
        /// </summary>
        internal int CurrentFrame { get; set; }

        /// <summary>
        /// The name of this texture.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of paths to load the string from.
        /// </summary>
        public List<string> FramesPath { get; set; }

        /// <summary>
        /// The position of this texture.
        /// Must be valid to draw.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Determines if this texture repeats, and if so
        /// by how many tiles. 
        /// 
        /// If NULL or zero, the texture will be drawn only once.
        /// </summary>
        public Vector2 TextureRepeat { get; set; }

        /// <summary>
        /// Determines the number of times the animation repeats.
        /// 0 makes the animation repeat infintiely.
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        /// Internal: the number of times the animation has repeated.
        /// </summary>
        private int CurRepeats { get; set; }

        /// <summary>
        /// Internal: determines if the animatin is finished.
        /// </summary>
        private bool AnimationFinished { get; set; }

        /// <summary>
        /// The size of this texture.
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// The current animation cycle.that is being used
        /// </summary>
        public AnimationCycle Cycle { get; set; }

        public AnimatedTexture()
        {
            Frames = new List<Texture>();
            FramesPath = new List<string>();
        }

        public void Load(Window Win)
        {
            if (Size == default(Vector2)) throw new NCException("Cannot load an animated texture with no texture size!", 44, "AnimatedTexture.LoadIndexed", NCExceptionSeverity.FatalError);
            if (Cycle == null) throw new NCException("AnimatedTextures must have a cycle!", 54, "AnimatedTexture.LoadIndexed", NCExceptionSeverity.FatalError);

            foreach (string TexturePath in FramesPath)
            {
                Texture new_texture = new Texture(Win, Size.X, Size.Y);
                new_texture.Path = TexturePath;
                new_texture.Position = Position;
                new_texture.Repeat = TextureRepeat;  // do this in the getter/setter?
                // Texture will only load current or throw fatal error. Maybe add Loaded attribute that checks if TextureHandle isn't a nullptr?
                new_texture.Load(Win);

                if (new_texture.TextureHandle != IntPtr.Zero) Frames.Add(new_texture);
            }

            CurrentFrame = Cycle.StartFrame;
        }

        public void DrawCurrentFrame(Window Win)
        {
            bool reverse_animation = (Cycle.StartFrame > Cycle.EndFrame);
            if (AnimationFinished) return;

            Texture cur_frame = Frames[(int)CurrentFrame];
            cur_frame.Draw(Win);

            if (Win.FrameNumber % Cycle.FrameLength == 0)
            {
                // will be set to true if the cycle is to end.
                bool end_cycle = false;

                if (reverse_animation)
                {
                    CurrentFrame--; 
                }
                else
                {
                    CurrentFrame++; 
                }

                if (reverse_animation)
                {
                    if (CurrentFrame < Cycle.EndFrame) CurrentFrame = Cycle.StartFrame;
                }
                else
                {
                    if (CurrentFrame > Cycle.EndFrame) CurrentFrame = Cycle.StartFrame;
                }

                if (end_cycle)
                {
                    CurRepeats++;
                    if (CurRepeats > Repeat && (Repeat != 0)) AnimationFinished = true;
                    end_cycle = false; 
                }
            }
        }
    }
}
