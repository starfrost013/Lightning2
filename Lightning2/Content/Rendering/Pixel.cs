using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// Pixel
    /// 
    /// February 11, 2022
    /// 
    /// A pixel, as part of a texture
    /// </summary>
    public class Pixel
    {
        /// <summary>
        /// The colour of this pixel.
        /// </summary>
        public Color4 Colour { get; set; }

        public Pixel()
        {

        }

        public Pixel(Color4 NColour) => Colour = NColour;
    }
}
