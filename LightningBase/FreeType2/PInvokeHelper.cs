namespace LightningBase
{
    /// <summary>
    /// Helpful methods to make marshalling simpler.
    /// </summary>
    public static class PInvokeHelper
    {
        /// <summary>
        /// A generic wrapper for <see cref="Marshal.PtrToStructure(nint, Type)"/>.
        /// </summary>
        /// <typeparam name="T">The type to cast to.</typeparam>
        /// <param name="reference">The pointer that holds the struct.</param>
        /// <returns>A marshalled struct.</returns>
        public static T PtrToStructure<T>(nint reference)
        {
            return Marshal.PtrToStructure<T>(reference);
        }

        /// <summary>
        /// A method to copy data from one pointer to another, byte by byte.
        /// </summary>
        /// <param name="source">The source pointer.</param>
        /// <param name="sourceOffset">An offset into the source buffer.</param>
        /// <param name="destination">The destination pointer.</param>
        /// <param name="destinationOffset">An offset into the destination buffer.</param>
        /// <param name="count">The number of bytes to copy.</param>
        public static unsafe void Copy(nint source, int sourceOffset, nint destination, int destinationOffset, int count)
        {
            byte* src = (byte*)source + sourceOffset;
            byte* dst = (byte*)destination + destinationOffset;
            byte* end = dst + count;

            while (dst != end)
                *dst++ = *src++;
        }

        /// <summary>
        /// A common pattern in SharpFont is to pass a pointer to a memory address inside of a struct. This method
        /// works for all cases and provides a generic API.
        /// </summary>
        /// <see cref="Marshal.OffsetOf"/>
        /// <typeparam name="T">The type of the struct to take an offset from.</typeparam>
        /// <param name="start">A pointer to the start of a struct.</param>
        /// <param name="fieldName">The name of the field to get an offset to.</param>
        /// <returns><code>start</code> + the offset of the <code>fieldName</code> field in <code>T</code>.</returns>
        public static nint AbsoluteOffsetOf<T>(nint start, string fieldName)
        {
            return new nint(start.ToInt64() + Marshal.OffsetOf<T>(fieldName).ToInt64());
        }
    }
}
