#if WINDOWS
using NuCore.NativeInterop.Win32;
#endif
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Vector2
    /// 
    /// March 9, 2021 (modified April 17, 2021: The great re-namespaceing).
    /// 
    /// Defines a two-dimensional vector.
    /// </summary>
    [TypeConverter(typeof(Vector2Converter))]
    public class Vector2 : Instance
    {
        internal override string ClassName => "Vector2";

        /// <summary>
        /// The X component of this Vector2.
        /// </summary>
        public double X { get { return V2Internal.X; } set { V2Internal.X = value; } }

        /// <summary>
        /// The Y component of this Vector2.. 
        /// </summary>
        public double Y { get { return V2Internal.Y; } set { V2Internal.Y = value; } }

        private Vector2Internal V2Internal { get; set; }

        public Vector2()
        {
            V2Internal = new Vector2Internal(); 
        }

        public Vector2(double NX, double NY)
        {
            V2Internal = new Vector2Internal();
            X = NX;
            Y = NY;
        }

        // July 25, 2021: These were totally busted, so I had to fix them. 
        public static Vector2 operator +(Vector2 A, Vector2 B) => new Vector2(A.X + B.X, A.Y + B.Y);
        public static Vector2 operator +(Vector2 A, double B) => new Vector2(A.X + B, A.Y + B);
        public static Vector2 operator -(Vector2 A, Vector2 B) => new Vector2(A.X - B.X, A.Y - B.Y);
        public static Vector2 operator -(Vector2 A, double B) => new Vector2(A.X - B, A.Y - B);
        public static Vector2 operator -(double A, Vector2 B) => new Vector2(A - B.X, A - B.Y);
        public static Vector2 operator -(Vector2 A) => new Vector2(-A.X, -A.Y);
        public static Vector2 operator *(Vector2 A, Vector2 B) => new Vector2(A.X * B.X, A.Y * B.Y);
        public static Vector2 operator *(Vector2 A, double B) => new Vector2(A.X * B, A.Y * B);
        
        public static Vector2 operator /(Vector2 A, Vector2 B) => new Vector2(A.X / B.X, A.Y / B.Y);
        public static Vector2 operator /(Vector2 A, double B) => new Vector2(A.X / B, A.Y / B);
        public static Vector2 operator /(double A, Vector2 B) => new Vector2(A / B.X, A / B.Y);

        public static bool operator ==(Vector2 A, Vector2 B)
        {
            // Prevent a stack overflow by upcasting
            object OA = (object)A;
            object OB = (object)B;

            if (OA != null && OB != null)
            {
                if (A.X == B.X && A.Y == B.Y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // One or the other is null...
                if (OA == null && OB == null)
                {
                    return true;
                }
                else
                {
                    return false; 
                }
            }
        }

        public static bool operator !=(Vector2 A, Vector2 B)
        {
            // Prevent a stack overflow by upcasting
            object OA = (object)A;
            object OB = (object)B;

            if (OA != null && OB != null)
            {
                if (A.X == B.X && A.Y == B.Y)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (OA == null && OB == null)
                {
                    return false; 
                }
                else
                {
                    return true; 
                }
            }


        }

        public static bool operator <(Vector2 A, Vector2 B) 
        {
            if (A == null || B == null)
            {
                return false; // not sure what to do here
            }
            else 
            {
                if ((A.X < B.X) && (A.Y < B.Y))
                {
                    return true;
                }
                else
                {
                    return false; 
                }
            }
        }

        public static bool operator <=(Vector2 A, Vector2 B) // January 14, 2022
        {
            if (A == null || B == null)
            {
                return false; // not sure what to do here
            }
            else
            {
                if ((A.X <= B.X) && (A.Y <= B.Y))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool operator >(Vector2 A, Vector2 B)
        {
            if (A == null || B == null)
            {
                return false;
            }
            else
            {
                if ((A.X > B.X) && (A.Y > B.Y))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool operator >=(Vector2 A, Vector2 B) // January 14, 2022
        {
            if (A == null || B == null)
            {
                return false; // not sure what to do here
            }
            else
            {
                if ((A.X >= B.X) && (A.Y >= B.Y))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public override bool Equals(object obj)
        {
            Type ObjType = obj.GetType();

            if (typeof(Vector2) != ObjType)
            {
                return false;
            }
            else
            {
                return (this == (Vector2)obj);
            }
            
        }

        public override int GetHashCode() => base.GetHashCode();

        public static Vector2 FromString(string Str, bool AddToDataModel = true, Instance Parent = null)
        {
            // December 9, 2021 (NuRender integration)
            Vector2Internal V2I = Vector2Internal.FromString(Str);

            if (!AddToDataModel)
            {
                return new Vector2(V2I.X, V2I.Y);
            }
            else
            {
                Vector2 NV2 = (Vector2)DataModel.CreateInstance("Vector2", Parent); // create a new Color3 and add it to the datamodel
                NV2.X = V2I.X;
                NV2.Y = V2I.Y;

                return NV2;
            }
        }

        #region Math operations
        public static double GetDotProduct(Vector2 A, Vector2 B) => ((A.X * B.X) + (B.Y * B.Y));

        public Vector2 GetAbs() => new Vector2(Math.Abs(X), Math.Abs(Y));

        /// <summary>
        /// Gets the smallest <see cref="Vector2"/> out of two.
        /// </summary>
        /// <param name="A">The first Vector2 you wish to compare.</param>
        /// <param name="B">The second Vector2 you wish to compare.</param>
        /// <returns>The smallest Vector2 out of <paramref name="A"/> and <paramref name="B"/>.</returns>
        public static Vector2 Min(Vector2 A, Vector2 B)
        {
            if (A < B)
            {
                return A; 
            }
            else
            {
                return B;
            }
        }

        /// <summary>
        /// Gets the largest <see cref="Vector2"/> out of two.
        /// </summary>
        /// <param name="A">The first Vector2 you wish to compare.</param>
        /// <param name="B">The second Vector2 you wish to compare.</param>
        /// <returns>The largest Vector2 out of <paramref name="A"/> and <paramref name="B"/>.</returns>
        public static Vector2 Max(Vector2 A, Vector2 B)
        {
            if (A > B)
            {
                return A;
            }
            else
            {
                return B; 
            }
        }

        #endregion

        #region NuRender conversions 

        // New: Dec 15, 2021

        public static explicit operator Vector2(Vector2Internal V2I) => new Vector2(V2I.X, V2I.Y);

        public static explicit operator Vector2Internal(Vector2 V2) => new Vector2Internal(V2.X, V2.Y);

        #endregion

    }
}
