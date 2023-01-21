namespace LightningGL
{
    /// <summary>
    /// AnimatedTexture
    /// 
    /// March 19, 2022
    /// 
    /// Defines an animated texture loaded from a series of images.
    /// </summary>
    public class AnimatedTexture : Texture
    {
        /// <summary>
        /// The frames of this texture. A list of <see cref="Texture"/> objects.
        /// </summary>
        public List<Texture> Frames { get; private set; }

        /// <summary>
        /// The current animation cycle.for this animation - see <see cref="AnimationCycle"/>
        /// </summary>
        public AnimationCycle Cycle { get; set; }

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
        /// Stores the number of frames until the next frame.
        /// </summary>
        private int FramesUntilNextFrame { get; set; }

        /// <summary>
        /// Private: stores the current texture.
        /// </summary>
        private Texture? CurrentTexture { get; set; }

        public AnimatedTexture(string name, float sizeX, float sizeY, AnimationCycle cycle) : base(name, sizeX, sizeY)
        {
            Frames = new List<Texture>();
            FramePaths = new List<string>();
            Size = new Vector2(sizeX, sizeY);
            Cycle = cycle;
            // reasonable default
            FramesUntilNextFrame = Convert.ToInt32(Cycle.FrameLength / 8);
        }

        /// <summary>
        /// Loads this animated texture.
        /// </summary>
        public override void Create()
        {
            if (Size == default)
            {
                NCLogging.LogError("Cannot load an animated texture with no texture size", 44, NCLoggingSeverity.FatalError);
                return;
            }
            
            if (Cycle == null)
            {
                NCLogging.LogError("AnimatedTextures must have a valid Cycle property", 54, NCLoggingSeverity.FatalError);
                return;
            }

            for (int frameId = 0; frameId < FramePaths.Count; frameId++)
            {
                string texturePath = FramePaths[frameId];

                Texture newTexture = new($"{Name}Frame{frameId}", Size.X, Size.Y);
                newTexture.Path = texturePath;
                newTexture.Position = Position;
                newTexture.Repeat = Repeat;  // do this in the getter/setter?
                newTexture.OnCreate(); // set to create method at creation, but good practice

                if (newTexture.Handle != nint.Zero) Frames.Add(newTexture);
            }

            CurrentFrame = Cycle.StartFrame;
        }

        /// <summary>
        /// Draws this animated texture.
        /// </summary>
        public override void Draw()
        {
            bool reverseAnimation = (Cycle.StartFrame > Cycle.EndFrame);
            if (AnimationFinished) return;

            CurrentTexture = Frames[CurrentFrame];

            CurrentTexture.RenderPosition = RenderPosition;
            CurrentTexture.Repeat = Repeat;
            CurrentTexture.Size = Size;

            CurrentTexture.Draw();

            // decrement the frames remaining until the next frame
            FramesUntilNextFrame--;

            if (FramesUntilNextFrame == 0)
            {
                // set a new number of frames until the next frame
                // check to prevent division by zero because of lightning having just started
                if (Lightning.Renderer.DeltaTime > 0) FramesUntilNextFrame = Convert.ToInt32(Cycle.FrameLength / Lightning.Renderer.DeltaTime);

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

        /// <summary>
        /// Adds a frame to this AnimatedTexture.
        /// </summary>
        /// <param name="framePath">The path to the frame to add.</param>
        public void AddFrame(string framePath) => FramePaths.Add(framePath);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="x"><inheritdoc/></param>
        /// <param name="y"><inheritdoc/></param>
        /// <param name="unlockNow"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override Color GetPixel(int x, int y, bool unlockNow = false)
        {
            if (CurrentTexture == null)
            {
                NCLogging.LogError("Attempted to call GetPixel on a NULL texture!", 184, NCLoggingSeverity.Warning, null, true);
                return default;
            }

            return CurrentTexture.GetPixel(x, y, unlockNow);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="x"><inheritdoc/></param>
        /// <param name="y"><inheritdoc/></param>
        /// <param name="unlockNow"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override void SetPixel(int x, int y, Color color, bool unlockNow = false)
        {
            if (CurrentTexture == null)
            {
                NCLogging.LogError("Attempted to call GetPixel on a NULL texture!", 185, NCLoggingSeverity.Warning, null, true);
                return;
            }

            CurrentTexture.SetPixel(x, y, color, unlockNow);
        }

        /// <summary>
        /// Gets a pixel for every frame of this animated texture.
        /// </summary>
        /// <param name="x">The X position of the pixel.</param>
        /// <param name="y">The Y position of the pixel.</param>
        /// <param name="unlockNow">Unlocks the texture immediately - use this if you do not need to draw any more pixels</param>
        /// <exception cref="NCError">An invalid coordinate was supplied or the texture does not have a valid size.</exception>
        /// <returns>A <see cref="List{T}"/> object containing the color of the pixels corresponding to the <paramref name="x"/> and <paramref name="y"/> parameters for each frame of this AnimatedTexture.</returns>
        public List<Color> GetPixelGlobal(int x, int y, bool unlockNow = false)
        {
            List<Color> colours = new();

            foreach (Texture texture in Frames)
            {
                colours.Add(texture.GetPixel(x, y, unlockNow));
            }

            return colours;
        }

        /// <summary>
        /// Sets a pixel on every frame of this animated texture.
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to set.</param>
        /// <param name="y">The Y coordinate of the pixel to set.</param>
        /// <param name="unlockNow">Unlocks the texture immediately - use this if you do not need to draw any more pixels</param>
        /// <exception cref="NCError">An invalid coordinate was supplied or the texture does not have a valid size.</exception>
        public void SetPixelGlobal(int x, int y, Color color, bool unlockNow = false)
        {
            foreach (Texture texture in Frames) texture.SetPixel(x, y, color, unlockNow);
        }
    }
}
