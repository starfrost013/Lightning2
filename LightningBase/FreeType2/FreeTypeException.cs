namespace LightningBase
{
    /// <summary>
    /// Represents an exception thrown as a result of a FreeType2 API error.
    /// </summary>
    public sealed class FreeTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeTypeException"/> class.
        /// </summary>
        public FreeTypeException(FT_Error err)
            : base(err.ToString().ToLowerInvariant().Replace("ft_err", "").Replace("_", " ")) { } // do some processing on the enum value to avoid a huge switch/case statement
    }
}