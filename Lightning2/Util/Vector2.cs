using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// Defines a position in two-dimensional space.
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// The X-coordinate of this vector.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The Y-coordinate of this vector
        /// </summary>
        public int Y { get; set; }

        public Vector2()
        {

        }

        public Vector2(int NX, int NY)
        {
            X = NX;
            Y = NY;
        }

        public static explicit operator Vector2(Vector2F V2F) => new Vector2(Convert.ToInt32(V2F.X), Convert.ToInt32(V2F.Y));


    }
}
