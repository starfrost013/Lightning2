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
    /// Light
    /// 
    /// April 7, 2022
    /// 
    /// Defines a light. 
    /// </summary>
    public class Light
    {
        public Vector2 Position { get; set; }

        public int Brightness { get; set; }

        public Color4 Colour { get; set; }

        public bool SnapToScreen { get; set; }
    }
}
