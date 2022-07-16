using System.IO;

namespace NuCore.Utilities
{
    /// <summary>
    /// CRC32
    /// 
    /// Generates a Crc32
    /// 
    /// Stolen from my friend and slightly modified
    /// 
    /// TEMPORARY until .NET 7.0 
    /// </summary>
    public static class CRC32
    {
        /// <summary>
        /// The polynomial we use to XOR the result with in some cases.
        /// </summary>
        private const uint polynomial = 0xEDB88320;

        /// <summary>
        /// The result CRC32 value. 
        /// </summary>
        private static uint result = uint.MaxValue;
        public static uint Result => ~result;

        public static void NextByte(byte value)
        {
            // Iterate through all of the value's bits from the LSB to the MSB
            for (var i = 0; i < 8; i++)
            {
                // XOR the current value bit with the current bit from the eventual result
                var lsbXor = ((value ^ result) & 1) != 0;

                // Shift result to the right by one
                result >>= 1;

                // Shift value so we XOR with the next bit on the next iteration of the loop
                value >>= 1;

                // If the result of an XOR of the current value bit and the LSB of the eventual
                // result is a set bit we need to XOR the eventual result with the polynomial
                if (lsbXor) result ^= polynomial;

            }
        }

        public static void NextBytes(byte[] bytes)
        {
            for (var i = 0; i < bytes.Length; i++) NextByte(bytes[i]);
        }

        public static void NextBytes(BinaryReader br, long count)
        {
            for (long i = 0; i < count; i++) NextByte(br.ReadByte());

        }
    }
}
