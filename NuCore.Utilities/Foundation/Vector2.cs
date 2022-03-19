using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCore.Utilities
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

        public static Vector2 operator +(Vector2 A, Vector2 B) => new Vector2(A.X + B.X, A.Y + B.Y);
        public static Vector2 operator -(Vector2 A, Vector2 B) => new Vector2(A.X - B.X, A.Y - B.Y);
        public static Vector2 operator *(Vector2 A, Vector2 B) => new Vector2(A.X * B.X, A.Y * B.Y);
        public static Vector2 operator /(Vector2 A, Vector2 B) => new Vector2(A.X / B.X, A.Y / B.Y);

        public static bool operator ==(Vector2 A, Vector2 B)
        {
            object obj_a = (object)A;
            object obj_b = (object)B;

            if (obj_a == null && obj_b == null) return true; // check both are null
            if (obj_a == null || obj_b == null) return false; // check that one or the other is null
            return (A.X == B.X && A.Y == B.Y);

        }

        public static bool operator !=(Vector2 A, Vector2 B)
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
