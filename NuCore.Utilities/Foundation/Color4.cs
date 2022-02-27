using NuCore.SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCore.Utilities
{
    /// <summary>
    /// Defines an RGBA colour.
    /// </summary>
    public class Color4
    {
        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public byte A { get; set; }

        public Color4()
        {

        }

        public Color4(byte NR, byte NG, byte NB, byte NA)
        {
            R = NR;
            G = NG;
            B = NB;
            A = NA;
        }

        public static explicit operator uint(Color4 Colour)
        {
            uint NewColour = 0;
            NewColour &= Colour.R;
            NewColour &= Colour.G;
            NewColour &= Colour.B;
            NewColour &= Colour.A;

            return NewColour;
        }

        public static explicit operator Color4(uint Colour)
        {
            return new Color4(
                (byte)Colour,
                (byte)(Colour >> 8),
                (byte)(Colour >> 16),
                (byte)(Colour >> 24)
                );
        }

    }
}
