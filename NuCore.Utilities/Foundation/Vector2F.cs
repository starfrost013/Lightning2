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

        public static Vector2F operator +(Vector2F A, Vector2F B) => new Vector2F(A.X + B.X, A.Y + B.Y);
        public static Vector2F operator -(Vector2F A, Vector2F B) => new Vector2F(A.X - B.X, A.Y - B.Y);
        public static Vector2F operator *(Vector2F A, Vector2F B) => new Vector2F(A.X * B.X, A.Y * B.Y);
        public static Vector2F operator /(Vector2F A, Vector2F B) => new Vector2F(A.X / B.X, A.Y / B.Y);

        public static bool operator ==(Vector2F A, Vector2F B)
        {
            object obj_a = (object)A;
            object obj_b = (object)B;

            if (obj_a == null && obj_b == null) return true; // check both are null
            if (obj_a == null || obj_b == null) return false; // check that one or the other is null
            return (A.X == B.X && A.Y == B.Y);

        }

        public static bool operator !=(Vector2F A, Vector2F B)
        {
            object obj_a = (object)A;
            object obj_b = (object)B;

            if (obj_a == null && obj_b == null) return true; // check both are null
            if (obj_a == null || obj_b == null) return false; // check that one or the other is null
            return (A.X != B.X || A.Y != B.Y);

        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
