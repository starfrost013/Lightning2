using System;
using System.Runtime.InteropServices;

namespace LightningBase
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

        FT_Vector advance; // temp

        IntPtr format;

        IntPtr bitmap;

        int bitmap_left;

        int bitmap_top;

        IntPtr outline;

        uint num_subglyphs;

        uint subglyphs;

        void* control_data;

        long control_len;

        FT_Vector lsb_delta;  // temp

        FT_Vector rsb_delta;  // temp

        void* other;

        IntPtr _internal; // internal is a reserved keyword in c#
    }
}
