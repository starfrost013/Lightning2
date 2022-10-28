
namespace LightningNetwork
{
    /// <summary>
    /// LNetSessionManager
    /// 
    /// The packet management class for the client and the server.
    /// Holds shared functionality.
    /// </summary>
    public class LNetSessionManager
    {
        public UdpClient UdpState { get; set; }

        /// <summary>
        /// The last packet ID used for ordering and stuff.
        /// (todo: move this to global area?, do we even need this?)
        /// </summary>
        private int LastPacketId { get; set; }

        public LNetSessionManager()
        {
            UdpState = new UdpClient();
        }

        public byte[] PacketToByteArray(LNetCommand packet)
        {
            packet.Header.Id = LastPacketId;
            LastPacketId++;
            byte[] array1 = packet.ToByteArray();
            byte[] array2 = packet.CommandDataToByteArray();
            byte[] finalArray = new byte[array1.Length + array2.Length];

            Buffer.BlockCopy(array1, array1.Length, finalArray, 0, array1.Length);
            Buffer.BlockCopy(array2, array2.Length, finalArray, array1.Length, array2.Length);

            return finalArray;
        }
        
        public LNetCommand? ReadPacket(byte[] data)
        {
            int curPointWithinPacket = 0;

            short protocolVersion = BitConverter.ToInt16(data, curPointWithinPacket);

            curPointWithinPacket += sizeof(short);

            if (protocolVersion != LNetProtocol.LNET_PROTOCOL_VERSION)
            {
                _ = new NCException($"Received packet with invalid LNet protocol version {protocolVersion}, expected {LNetProtocol.LNET_PROTOCOL_VERSION}", 181, 
                    "LNetSessionManager::ReadPacket - received packet with first two bytes not equal to LNetProtocol::LNET_PROTOCOL_VERSION!", NCExceptionSeverity.Warning, null, false);
            }

            int packetId = BitConverter.ToInt32(data, curPointWithinPacket);

            curPointWithinPacket += sizeof(int);

            // packet type data
            byte packetType = data[curPointWithinPacket];

            LNetCommand? netCommand = PacketIdToCommand(packetType);

            if (netCommand == null)
            {
                _ = new NCException($"Received invalid packet type {packetType}, maximum is {LNetCommandTypes.LNET_MAXIMUM_VALID_COMMAND}", 182,
                    "LNetSessionManager::ReadPacket - received an invalid packet type.", NCExceptionSeverity.Warning, null, false);
                return null;
            }

            // fill in global packet id
            netCommand.Header.Id = packetId;

            int numOfCommandBytes = data.Length - curPointWithinPacket;
            
            if (numOfCommandBytes <= 0)
            {
                // pass a zero byte array to ReceiveCommandData. 
                // This is a special value indicating there is no command data. So hopefully every command will detect this and try and parse it.
                netCommand.ReadCommandData(new byte[0]);
                return netCommand;
            }
            else
            {
                byte[] cmdData = new byte[numOfCommandBytes];
                Buffer.BlockCopy(data, curPointWithinPacket, cmdData, 0, numOfCommandBytes);
                netCommand.ReadCommandData(cmdData);
                return netCommand;
            }

        }


        public virtual LNetCommand? PacketIdToCommand(byte packetId)
        {
            switch (packetId)
            {
                case 0x00:
                    return new PingPongCommand();
                default:
                    _ = new NCException($"Tried to parse invalid packet ID {packetId}", 180, "LNetSessionManager::PacketIdToCommand encountered invalid Packet ID!",
                        NCExceptionSeverity.Warning, null, false);
                    return null;
            }
            
        }


        /// <summary>
        /// Logs to NCLogging (in blue so you know it's the server)
        /// </summary>
        internal void LogAsServer(string logMessage)
        {
            NCLogging.Log($"[Server] {logMessage}", ConsoleColor.Blue);
        }

        /// <summary>
        /// Logs to NCLogging (in blue so you know it's the server)
        /// </summary>
        internal void LogAsClient(string logMessage)
        {
            NCLogging.Log($"[Client] {logMessage}", ConsoleColor.Blue);
        }
    }
}
