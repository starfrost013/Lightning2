﻿using NuCore.Utilities;
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

        public double Brightness { get; set; }

        public bool SnapToScreen { get; set; }
    }
}
