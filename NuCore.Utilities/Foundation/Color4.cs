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
            NewColour |= (uint)(Colour.R << 24);
            NewColour |= (uint)(Colour.G << 16);
            NewColour |= (uint)(Colour.B << 8);
            NewColour |= Colour.A;

            return NewColour;
        }

        public static explicit operator Color4(uint Colour)
        {
            return new Color4(
                (byte)(Colour & 0xFF),
                (byte)((Colour >> 8) & 0xFF),
                (byte)((Colour >> 16) & 0xFF),
                (byte)((Colour >> 24) & 0xFF)
                );
        }

        public static explicit operator Color4(SDL.SDL_Color Colour)
        {
            return new Color4(Colour.r, Colour.g, Colour.b, Colour.a);
        }

        public override string ToString()
        {
            return $"{R},{G},{B},{A}";
        }
    }
}
