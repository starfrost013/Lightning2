using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace NuCore.SDL2
{
    /// <summary>
    /// Defines a Freetype2 glyph slot.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe class FT_GlyphSlot // must be a class due to next
    {
        public IntPtr library;

        public IntPtr face;

        public FT_GlyphSlot next;

        uint glyph_index;

        private IntPtr generic;

        public IntPtr metrics; // temp

        double linearHoriAdvance;

        double linearVertAdvance;

        Vector2 advance; // temp

        IntPtr format;

        IntPtr bitmap;

        int bitmap_left;

        int bitmap_top;

        IntPtr outline;

        uint num_subglyphs;

        uint subglyphs;

        void* control_data;

        long control_len;

        Vector2 lsb_delta;  // temp

        Vector2 rsb_delta;  // temp

        void* other;

        IntPtr _internal; // internal is a reserved keyword
    }
}
