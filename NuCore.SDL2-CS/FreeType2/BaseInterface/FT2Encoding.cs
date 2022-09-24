namespace LightningBase
{
    /// <summary>
    /// This is not an enum because it needs to be a set of strings.
    /// Enumerates FT2 encoding types.
    /// </summary>
    public static class FT2Encoding
    {
        public const string FT_ENCODING_NONE = "\0\0\0\0";

        public const string FT_ENCODING_MS_SYBMBOL = "symb";

        public const string FT_ENCODING_UNICODE = "unic";

        public const string FT_ENCODING_SJIS = "sjis";

        public const string FT_ENCODING_PRC = "gb  "; // trailing spaces REQUIRED!

        public const string FT_ENCODING_BIG5 = "big5";

        public const string FT_ENCODING_WANSUNG = "wans";

        public const string FT_ENCODING_JOHAB = "joha";

        public const string FT_ENCODING_ADOBE_STANDARD = "adob";

        public const string FT_ENCODING_ADOBE_EXPERT = "adbe";

        public const string FT_ENCODING_ADOBE_CUSTOM = "adbc";

        public const string FT_ENCODING_ADOBE_LATIN_1 = "lat1";

        public const string FT_ENCODING_ADOBE_LATIN_2 = "lat2";

        public const string FT_ENCODING_APPLE_ROMAN = "armn";

    }
}