
namespace LightningNetwork
{
    /// <summary>
    /// LNetPacketHeader
    /// 
    /// Defines a packet header for LNet.
    /// </summary>
    public class LNetCommandHeader
    {
        /// <summary>
        /// this is here so the contents of the header are clearly defined in the code
        /// </summary>
        public const short PROTOCOL_VERSION = LNetProtocol.LNET_PROTOCOL_VERSION;

        /// <summary>
        /// Global ID of a packet so that we know when to resend packets
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Packet command type.
        /// </summary>
        public byte Command { get; set; }

        /// <summary>
        /// Converts this <see cref="LNetCommandHeader"/> to a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            byte[] bytes = BitConverter.GetBytes(PROTOCOL_VERSION);
            byte[] id = BitConverter.GetBytes(Id);
            byte[] finalArray = new byte[sizeof(short) + sizeof(int) + 1];

            // Build a final array
            Buffer.BlockCopy(bytes, 0, finalArray, 0, sizeof(short));
            Buffer.BlockCopy(id, sizeof(short), finalArray, sizeof(short), sizeof(int));
            finalArray[finalArray.Length - 1] = Command;

            return finalArray;
        }

    }
}
