using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCore.Utilities
{
    /// <summary>
    /// Defines a position in two-dimensional space. (using floating-point numbers)
    /// </summary>
    public class Vector2F
    {
        /// <summary>
        /// The X-coordinate of this vector.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// The Y-coordinate of this vector
        /// </summary>
        public double Y { get; set; }

        public Vector2F()
        {

        }

        public Vector2F(double NX, double NY)
        {
            X = NX;
            Y = NY;
        }

        public static explicit operator Vector2F(Vector2 V2F) => new Vector2F((double)V2F.X, (double)V2F.Y);
    }
}
