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
        /// The beginning frame
        /// </summary>
        public uint StartFrame { get; set; } 

        /// <summary>
        /// The end frame
        /// </summary>
        public uint EndFrame { get; set; }

        /// <summary>
        /// The number of frames between switching each animation frame. Default is 1.
        /// </summary>
        public uint FrameLength { get; set; }

        public AnimationCycle()
        {
            FrameLength = 1; 
        }

        public AnimationCycle(uint NStartFrame, uint NEndFrame, uint NFrameLength)
        {
            StartFrame = NStartFrame;
            EndFrame = NEndFrame;
            FrameLength = NFrameLength;
        }
    }
}