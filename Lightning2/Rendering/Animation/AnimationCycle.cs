using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
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

        public AnimationCycle()
        {
            FrameLength = 1; 
        }

        public AnimationCycle(int NStartFrame, int NEndFrame, uint NFrameLength)
        {
            StartFrame = NStartFrame;
            EndFrame = NEndFrame;
            FrameLength = NFrameLength;
        }
    }
}