namespace LightningGL
{
    /// <summary>
    /// AnimationCycle
    /// 
    /// March 20, 2022
    /// 
    /// Defines an animation cycle 
    /// </summary>
    public class AnimationCycle
    {
        /// <summary>
        /// The beginning frame of the animation cycle.
        /// </summary>
        public int StartFrame { get; set; }

        /// <summary>
        /// The end frame of the animation cycle.
        /// </summary>
        public int EndFrame { get; set; }

        /// <summary>
        /// The number of frames between switching each animation frame. Default is 1.
        /// </summary>
        public uint FrameLength { get; set; }

        /// <summary>
        /// Constructor for animation cycles.
        /// </summary>
        /// <param name="nStartFrame">The start frame of the animation cycle.</param>
        /// <param name="nEndFrame">The end frame of the animation cycle</param>
        /// <param name="nFrameLength">The length of each aniamtion cycle in frames.</param>
        public AnimationCycle(int nStartFrame, int nEndFrame, uint nFrameLength)
        {
            StartFrame = nStartFrame;
            EndFrame = nEndFrame;
            FrameLength = nFrameLength;
        }
    }
}