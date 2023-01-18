using System.Runtime.CompilerServices;

namespace LightningBase
{
    public unsafe class FreeTypeFaceFacade
    {
        // A pointer to the wrapped FreeType2 face object.
        private readonly nint _Face;
        private readonly FT_FaceRec* _FaceRec;
        private readonly FreeTypeLibrary _Library;

        private bool Initialised { get; set; }
        /// <summary>
        /// Initialize a FreeTypeFaceFacade instance with a pointer to the Face instance.
        /// </summary>
        public FreeTypeFaceFacade(FreeTypeLibrary library, nint face)
        {
            _Library = library;
            _Face = face;
            _FaceRec = (FT_FaceRec*)_Face;
            Initialised = true;
        }

        /// <summary>
        /// Initialize a FreeTypeFaceFacade instance with font data.
        /// </summary>
        public FreeTypeFaceFacade(FreeTypeLibrary library, nint fontData, int dataLength, int faceIndex = 0)
        {
            _Library = library;

            var err = FreeTypeApi.FT_New_Memory_Face(_Library.Native, fontData, dataLength, faceIndex, out _Face);
            if (err != FT_Error.FT_Err_Ok)
                throw new FreeTypeException(err);

            _FaceRec = (FT_FaceRec*)_Face;

            Initialised = true;
        }

        #region Properties

        public nint Face { get { return _Face; } }
        public FT_FaceRec* FaceRec { get { return _FaceRec; } }

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_SCALABLE flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_SCALABLE flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasScalableFlag { get { return HasFaceFlag(FreeTypeApi.FT_FACE_FLAG_SCALABLE); } }

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_FIXED_SIZES flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_FIXED_SIZES flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasFixedSizes { get { return HasFaceFlag(FreeTypeApi.FT_FACE_FLAG_FIXED_SIZES); } }

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_COLOR flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_COLOR flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasColorFlag { get { return HasFaceFlag(FreeTypeApi.FT_FACE_FLAG_COLOR); } }

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_KERNING flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_KERNING flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasKerningFlag { get { return HasFaceFlag(FreeTypeApi.FT_FACE_FLAG_KERNING); } }

        /// <summary>
        /// Gets a value indicating whether the face has any bitmap strikes with fixed sizes.
        /// </summary>
        public bool HasBitmapStrikes { get { return (_FaceRec->num_fixed_sizes) > 0; } }

        /// <summary>
        /// Gets the current glyph bitmap.
        /// </summary>
        public FT_Bitmap GlyphBitmap { get { return _FaceRec->glyph->bitmap; } }
        public FT_Bitmap* GlyphBitmapPtr { get { return &_FaceRec->glyph->bitmap; } }

        /// <summary>
        /// Gets the left offset of the current glyph bitmap.
        /// </summary>
        public int GlyphBitmapLeft { get { return _FaceRec->glyph->bitmap_left; } }

        /// <summary>
        /// Gets the right offset of the current glyph bitmap.
        /// </summary>
        public int GlyphBitmapTop { get { return _FaceRec->glyph->bitmap_top; } }

        /// <summary>
        /// Gets the width in pixels of the current glyph.
        /// </summary>
        public int GlyphMetricWidth { get { return (int)_FaceRec->glyph->metrics.width >> 6; } }

        /// <summary>
        /// Gets the height in pixels of the current glyph.
        /// </summary>
        public int GlyphMetricHeight { get { return (int)_FaceRec->glyph->metrics.height >> 6; } }

        /// <summary>
        /// Gets the horizontal advance of the current glyph.
        /// </summary>
        public int GlyphMetricHorizontalAdvance { get { return (int)_FaceRec->glyph->metrics.horiAdvance >> 6; } }

        /// <summary>
        /// Gets the vertical advance of the current glyph.
        /// </summary>
        public int GlyphMetricVerticalAdvance { get { return (int)_FaceRec->glyph->metrics.vertAdvance >> 6; } }

        /// <summary>
        /// Gets the face's ascender size in pixels.
        /// </summary>
        public int Ascender { get { return (int)_FaceRec->size->metrics.ascender >> 6; } }

        /// <summary>
        /// Gets the face's descender size in pixels.
        /// </summary>
        public int Descender { get { return (int)_FaceRec->size->metrics.descender >> 6; } }

        /// <summary>
        /// Gets the face's line spacing in pixels.
        /// </summary>
        public int LineSpacing { get { return (int)_FaceRec->size->metrics.height >> 6; } }

        /// <summary>
        /// Gets the face's underline position.
        /// </summary>
        public int UnderlinePosition { get { return _FaceRec->underline_position >> 6; } }

        /// <summary>
        /// Gets a pointer to the face's glyph slot.
        /// </summary>
        public FT_GlyphSlotRec* GlyphSlot { get { return _FaceRec->glyph; } }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a value indicating whether the face has the specified flag defined.
        /// </summary>
        /// <param name="flag">The flag to evaluate.</param>
        /// <returns><see langword="true"/> if the face has the specified flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasFaceFlag(int flag)
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 248,
                    NCErrorSeverity.Warning);
                return false;
            }

            return (((int)_FaceRec->face_flags) & flag) != 0;
        }

        /// <summary>
        /// Selects the specified character size for the font face.
        /// </summary>
        /// <param name="sizeInPoints">The size in points to select.</param>
        /// <param name="dpiX">The horizontal pixel density.</param>
        /// <param name="dpiY">The vertical pixel density.</param>
        public void SelectCharSize(int sizeInPoints, uint dpiX, uint dpiY)
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 247, NCErrorSeverity.Warning);
                return;
            }

            var size = (nint)(sizeInPoints << 6);
            var err = FreeTypeApi.FT_Set_Char_Size(_Face, size, size, dpiX, dpiY);
            if (err != FT_Error.FT_Err_Ok)
            {
                NCError.ShowErrorBox($"FreeType Internal Error - Failed to set character size ({err})", 250, NCErrorSeverity.Warning);
            }
        }

        /// <summary>
        /// Selects the specified fixed size for the font face.
        /// </summary>
        /// <param name="ix">The index of the fixed size to select.</param>
        public void SelectFixedSize(int ix)
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 246, NCErrorSeverity.Warning);
                return;
            }

            var err = FreeTypeApi.FT_Select_Size(_Face, ix);
            if (err != FT_Error.FT_Err_Ok)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Error selecting fixed font size", 253, NCErrorSeverity.Error, default, true);
            }

        }

        /// <summary>
        /// Gets the glyph index of the specified character, if it is defined by this face.
        /// </summary>
        /// <param name="charCode">The character code for which to retrieve a glyph index.</param>
        /// <returns>The glyph index of the specified character, or 0 if the character is not defined by this face.</returns>
        public uint GetCharIndex(uint charCode)
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 242, NCErrorSeverity.FatalError);
                return 90000000; // fatal error but obvious nonsense value
            }

            return FreeTypeApi.FT_Get_Char_Index(_Face, charCode);
        }

        /// <summary>
        /// Marshals the face's family name to a C# string.
        /// </summary>
        /// <returns>The marshalled string.</returns>
        public string MarshalFamilyName()
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 243, NCErrorSeverity.FatalError);

                return string.Empty;
            }

            return Marshal.PtrToStringAnsi(_FaceRec->family_name);
        }

        /// <summary>
        /// Marshals the face's style name to a C# string.
        /// </summary>
        /// <returns>The marshalled string.</returns>
        public string MarshalStyleName()
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 244, NCErrorSeverity.FatalError);

                return string.Empty;
            }

            return Marshal.PtrToStringAnsi(_FaceRec->style_name);
        }

        /// <summary>
        /// Returns the specified character if it is defined by this face; otherwise, returns <see langword="null"/>.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns>The specified character, if it is defined by this face; otherwise, <see langword="null"/>.</returns>
        public char? GetCharIfDefined(Char c)
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 245, NCErrorSeverity.Warning);

                return null;
            }

            return FreeTypeApi.FT_Get_Char_Index(_Face, c) > 0 ? c : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetFixedSizeInPixels(FT_FaceRec* face, int ix)
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 241, NCErrorSeverity.FatalError);
                return 0;
            }

            return face->available_sizes[ix].height;
        }

        /// <summary>
        /// Returns the index of the fixed size which is the closest match to the specified pixel size.
        /// </summary>
        /// <param name="sizeInPixels">The desired size in pixels.</param>
        /// <param name="requireExactMatch">A value indicating whether to require an exact match.</param>
        /// <returns>The index of the closest matching fixed size.</returns>
        public int FindNearestMatchingPixelSize(int sizeInPixels, bool requireExactMatch = false)
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 239, NCErrorSeverity.Error, default, true);
                return sizeInPixels;
            }

            var numFixedSizes = _FaceRec->num_fixed_sizes;

            if (numFixedSizes == 0)
            {
                NCError.ShowErrorBox("FreeType Internal Error - tried to find a nearest matching ixed size on a proportional font!", 239, NCErrorSeverity.Error, default, true);
                return sizeInPixels;
            }

            var bestMatchIx = 0;
            var bestMatchDiff = Math.Abs(GetFixedSizeInPixels(_FaceRec, 0) - sizeInPixels);

            for (int i = 0; i < numFixedSizes; i++)
            {
                var size = GetFixedSizeInPixels(_FaceRec, i);
                var diff = Math.Abs(size - sizeInPixels);
                if (diff < bestMatchDiff)
                {
                    bestMatchDiff = diff;
                    bestMatchIx = i;
                }
            }

            if (bestMatchDiff != 0 && requireExactMatch)
            {
                NCError.ShowErrorBox("FreeType Internal Error - could not find matching pixel size", 251, NCErrorSeverity.Error, default, true);
                return sizeInPixels;
            }

            return bestMatchIx;
        }

        public bool EmboldenGlyphBitmap(int xStrength, int yStrength)
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 240, NCErrorSeverity.FatalError);
            }

            // 26.6 format
            var err = FreeTypeApi.FT_Bitmap_Embolden(_Library.Native, (nint)(GlyphBitmapPtr), xStrength << 6, yStrength << 6);

            if (err != FT_Error.FT_Err_Ok)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Failed to embolden font", 249, NCErrorSeverity.Warning, default, true);
                return false;
            }

            if ((int)_FaceRec->glyph->advance.x > 0)
                _FaceRec->glyph->advance.x += xStrength;
            if ((int)_FaceRec->glyph->advance.y > 0)
                _FaceRec->glyph->advance.x += yStrength;
            _FaceRec->glyph->metrics.width += xStrength;
            _FaceRec->glyph->metrics.height += yStrength;
            _FaceRec->glyph->metrics.horiBearingY += yStrength;
            _FaceRec->glyph->metrics.horiAdvance += xStrength;
            _FaceRec->glyph->metrics.vertBearingX -= xStrength / 2;
            _FaceRec->glyph->metrics.vertBearingY += yStrength;
            _FaceRec->glyph->metrics.vertAdvance += yStrength;

            _FaceRec->glyph->bitmap_top += (yStrength >> 6);

            return true;
        }

        public bool Unload()
        {
            FT_Error unloadError = FreeTypeApi.FT_Done_Face(_Face);

            if (unloadError != FT_Error.FT_Err_Ok)
            {
                NCError.ShowErrorBox($"FreeType Internal Error - Error unloading font {unloadError}", 254,
                     NCErrorSeverity.Error, default, true);

                return false;
            }

            Initialised = false;
            return true;
        }

        /// <summary>
        /// Get the bits per pixel for a pixel mode
        /// </summary>
        /// <returns>An integer indicating the bits per pixel (bit depth) for a pixel mode.</returns>
        public int GetBitsPerPixel()
        {
            if (!Initialised)
            {
                NCError.ShowErrorBox("FreeType Internal Error - Font not initialised", 255, NCErrorSeverity.FatalError);
            }

            return _FaceRec->glyph->bitmap.pixel_mode switch
            {
                FT_Pixel_Mode.FT_PIXEL_MODE_NONE => 0,
                FT_Pixel_Mode.FT_PIXEL_MODE_MONO => 1,
                FT_Pixel_Mode.FT_PIXEL_MODE_GRAY2 => 2,
                FT_Pixel_Mode.FT_PIXEL_MODE_GRAY4 => 4,
                FT_Pixel_Mode.FT_PIXEL_MODE_GRAY or FT_Pixel_Mode.FT_PIXEL_MODE_LCD or FT_Pixel_Mode.FT_PIXEL_MODE_LCD_V => 8,
                FT_Pixel_Mode.FT_PIXEL_MODE_BGRA => 32,
                _ => 8,// assume 8bit for everything else
            };
        }

        #endregion
    }
}