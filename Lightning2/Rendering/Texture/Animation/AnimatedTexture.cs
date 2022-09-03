﻿namespace LightningGL
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
    public class AnimatedTexture : Texture
    {
        /// <summary>
        /// The frames of this texture. A list of <see cref="Texture"/> objects.
        /// </summary>
        public List<Texture> Frames { get; private set; }

        /// <summary>
        /// The current frame in the animationcycle.
        /// </summary>
        internal int CurrentFrame { get; set; }

        /// <summary>
        /// The list of paths to load the string from.
        /// </summary>
        private List<string> FramePaths { get; set; }

        /// <summary>
        /// Determines the number of times the animation repeats.
        /// 0 makes the animation repeat infintiely.
        /// </summary>
        public int AnimationRepeat { get; set; }

        /// <summary>
        /// Internal: the number of times the animation has repeated.
        /// </summary>
        private int CurRepeats { get; set; }

        /// <summary>
        /// Internal: determines if the animatin is finished.
        /// </summary>
        private bool AnimationFinished { get; set; }

        /// <summary>
        /// The current animation cycle.for this animation - see <see cref="AnimationCycle"/>
        /// </summary>
        public AnimationCycle Cycle { get; set; }

        public AnimatedTexture(Window cWindow, float sizeX, float sizeY) : base(cWindow, sizeX, sizeY)
        {
            Frames = new List<Texture>();
            FramePaths = new List<string>();
            Size = new Vector2(sizeX, sizeY);
        }

        /// <summary>
        /// Loads this animated texture.
        /// </summary>
        /// <param name="cWindow">The window to load this animated texture to.</param>
        public override void Load(Window cWindow)
        {
            if (Size == default(Vector2)) _ = new NCException("Cannot load an animated texture with no texture size", 44, "AnimatedTexture::Size property = (0,0)", NCExceptionSeverity.FatalError);
            if (Cycle == null) _ = new NCException("AnimatedTextures must have a valid Cycle property", 54, "AnimatedTexture::Cycle property = null", NCExceptionSeverity.FatalError);

            foreach (string texturePath in FramePaths)
            {
                Texture newTexture = new Texture(cWindow, Size.X, Size.Y);
                newTexture.Path = texturePath;
                newTexture.Position = Position;
                newTexture.Repeat = Repeat;  // do this in the getter/setter?
                // Texture will only load current or throw fatal error. Maybe add Loaded attribute that checks if TextureHandle isn't a nullptr?
                newTexture.Load(cWindow);

                if (newTexture.Handle != IntPtr.Zero) Frames.Add(newTexture);
            }

            CurrentFrame = Cycle.StartFrame;
        }

        /// <summary>
        /// Draws this animated texture.
        /// </summary>
        /// <param name="cWindow">The window to draw this animated texture to.</param>
        public override void Draw(Window cWindow)
        {
            bool reverseAnimation = (Cycle.StartFrame > Cycle.EndFrame);
            if (AnimationFinished) return;

            Texture curFrame = Frames[CurrentFrame];

            curFrame.RenderPosition = Position;
            curFrame.Repeat = Repeat;
            curFrame.Size = Size;

            curFrame.Draw(cWindow);

            if (cWindow.FrameNumber % Cycle.FrameLength == 0)
            {
                // will be set to true if the cycle is to end.
                bool endCycle = false;

                if (reverseAnimation)
                {
                    CurrentFrame--;
                }
                else
                {
                    CurrentFrame++;
                }

                if (reverseAnimation)
                {
                    // if we reach the start, restart
                    if (CurrentFrame < Cycle.StartFrame) CurrentFrame = Cycle.StartFrame;
                }
                else
                {
                    if (CurrentFrame > Cycle.EndFrame) CurrentFrame = Cycle.StartFrame;
                }

                if (endCycle)
                {
                    CurRepeats++;
                    if (CurRepeats > AnimationRepeat
                        && (AnimationRepeat != 0)) AnimationFinished = true;
                    endCycle = false;
                }
            }
        }

        public void AddFrame(string framePath) => FramePaths.Add(framePath);
    }
}
