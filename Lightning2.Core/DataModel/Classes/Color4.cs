using NuCore.Utilities;
using NuRender;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization; 
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Color4
    /// 
    /// March 9, 2021 (modified December 8, 2021: Now passes through to NR class <see cref="Color4Internal"/>)
    /// 
    /// Represents ARGB colour.
    /// </summary>
    [TypeConverter(typeof(Color4Converter))]
    public class Color4 : Instance
    {
        internal override string ClassName => "Color4";

        /// <summary>
        /// The alpha component of this <see cref="Color4"/>.
        /// </summary>
        public byte A { get { return C4Internal.A; } set { C4Internal.A = value; } }

        /// <summary>
        /// The red component of this <see cref="Color4"/>.
        /// </summary>
        public byte R { get { return C4Internal.R; } set { C4Internal.R = value; } }

        /// <summary>
        /// The green component of this <see cref="Color4"/>.
        /// </summary>
        public byte G { get { return C4Internal.G; } set { C4Internal.G = value; } }

        /// <summary>
        /// The blue component of this <see cref="Color4"/>.
        /// </summary>
        public byte B { get { return C4Internal.B; } set { C4Internal.B = value; } }

        // result class?

        private Color4Internal C4Internal { get; set; }

        public Color4() // old code compat (July 12, 2021)
        {
            C4Internal = new Color4Internal(); 
        }

        public Color4(byte CA, byte CR, byte CG, byte CB)
        {
            C4Internal = new Color4Internal();
            // use for non-DataModel ONLY
            A = CA;
            R = CR;
            G = CG;
            B = CB;
        }

        /// <summary>
        /// Convert a relative colour string to a Color4 value.
        /// </summary>
        /// <param name="Colour"></param>
        /// <param name="AddToDataModel">If false, simply creates an object and returns. If true, adds to the DataModel</param>
        /// <returns></returns>
        public static Color4 FromRelative(string Colour, bool AddToDataModel = true, Instance Parent = null)
        {
            // December 9, 2021 (NuRender integration)
            Color4Internal C4I = Color4Internal.FromRelative(Colour);

            if (!AddToDataModel)
            {
                return new Color4(C4I.A, C4I.R, C4I.G, C4I.B);
            }
            else
            {
                Color4 NC4 = (Color4)DataModel.CreateInstance("Color4", Parent); // create a new Color3 and add it to the datamodel
                NC4.A = C4I.A;
                NC4.R = C4I.R;
                NC4.G = C4I.G;
                NC4.B = C4I.B;

                return NC4;
            }
        }

        /// <summary>
        /// Convert a hexadecimal colour string to a Color4 value.
        /// </summary>
        /// <param name="Colour">The HTML format colour to convert.</param>
        /// <param name="AddToDataModel">If false, simply creates an object and returns. If true, adds to the DataModel</param>
        /// <returns></returns>
        public static Color4 FromHex(string Colour, bool AddToDataModel = true, Instance Parent = null)
        {
            // December 9, 2021 (NuRender integration)
            Color4Internal C4I = Color4Internal.FromHex(Colour);

            if (!AddToDataModel)
            {
                return new Color4(C4I.A, C4I.R, C4I.G, C4I.B);
            }
            else
            {
                Color4 NC4 = (Color4)DataModel.CreateInstance("Color4", Parent); // create a new Color3 and add it to the datamodel
                NC4.A = C4I.A;
                NC4.R = C4I.R;
                NC4.G = C4I.G;
                NC4.B = C4I.B;

                return NC4;
            }
        }

        /// <summary>
        /// Converts a comma-separated list of denary RGB colour values to a Color4.
        /// </summary>
        /// <param name="Str">The comma-separated list of denary RGB colour values to convert to.</param>
        /// <param name="AddToDataModel">If false, simply creates an object and returns. If true, adds to the DataModel</param>
        /// <returns></returns>
        public static Color4 FromString(string Str, bool AddToDataModel = true, Instance Parent = null)
        {
            // December 9, 2021 (NuRender integration)
            Color4Internal C4I = Color4Internal.FromString(Str);

            if (!AddToDataModel)
            {
                return new Color4(C4I.A, C4I.R, C4I.G, C4I.B);
            }
            else
            {
                Color4 NC4 = (Color4)DataModel.CreateInstance("Color4", Parent); // create a new Color3 and add it to the datamodel
                NC4.A = C4I.A;
                NC4.R = C4I.R;
                NC4.G = C4I.G;
                NC4.B = C4I.B;

                return NC4;
            }
        }



        #region not really blending
        public static Color4 operator +(Color4 A, Color4 B) => new Color4((byte)(A.A + B.A), (byte)(A.R + B.R), (byte)(A.G + B.G), (byte)(A.B + B.B));
        public static Color4 operator +(double A, Color4 B) => new Color4((byte)(A + B.A), (byte)(A + B.R), (byte)(A + B.G), (byte)(A + B.B));
        public static Color4 operator +(Color4 A, double B) => new Color4((byte)(A.A + B), (byte)(A.R + B), (byte)(A.G + B), (byte)(A.B + B));
        public static Color4 operator -(Color4 A, Color4 B) => new Color4((byte)(A.A - B.A), (byte)(A.R - B.R), (byte)(A.G - B.G), (byte)(A.B - B.B));
        public static Color4 operator -(double A, Color4 B) => new Color4((byte)(A - B.A), (byte)(A - B.R), (byte)(A - B.G), (byte)(A - B.B));
        public static Color4 operator -(Color4 A, double B) => new Color4((byte)(A.A - B), (byte)(A.R - B), (byte)(A.G - B), (byte)(A.B - B));
        public static Color4 operator *(Color4 A, Color4 B) => new Color4((byte)(A.A * B.A), (byte)(A.R * B.R), (byte)(A.G * B.G), (byte)(A.B * B.B));
        public static Color4 operator *(double A, Color4 B) => new Color4((byte)(A * B.A), (byte)(A * B.R), (byte)(A * B.G), (byte)(A * B.B));
        public static Color4 operator *(Color4 A, double B) => new Color4((byte)(A.A * B), (byte)(A.R * B), (byte)(A.G * B), (byte)(A.B * B));
        public static Color4 operator /(Color4 A, Color4 B) => new Color4((byte)(A.A / B.A), (byte)(A.R / B.R), (byte)(A.G / B.G), (byte)(A.B / B.B));
        public static Color4 operator /(double A, Color4 B) => new Color4((byte)(A / B.A), (byte)(A / B.R), (byte)(A / B.G), (byte)(A / B.B));
        public static Color4 operator /(Color4 A, double B) => new Color4((byte)(A.A / B), (byte)(A.R / B), (byte)(A.G / B), (byte)(A.B / B));


        #endregion

        #region NuRender conversions 

        // New: Dec 15, 2021

        public static explicit operator Color4(Color4Internal C3I) => new Color4(C3I.A, C3I.R, C3I.G, C3I.B);

        public static explicit operator Color4Internal(Color4 C3) => new Color4Internal(C3.A, C3.R, C3.G, C3.B);

        #endregion

    }
}
