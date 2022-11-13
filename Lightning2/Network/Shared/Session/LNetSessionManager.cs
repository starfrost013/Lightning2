
namespace LightningGL
{
    /// <summary>
    /// LNetSessionManager
    /// 
    /// The packet management class for the client and the server.
    /// Holds shared functionality.
    /// </summary>
    public class LNetSessionManager
    {
        public UdpClient UdpClient { get; set; }

        /// <summary>
        /// The last packet ID used for ordering and stuff.
        /// (todo: move this to global area?, do we even need this?)
        /// </summary>
        private int LastPacketId { get; set; }

        public LNetSessionManager()
        {
            UdpClient = new UdpClient();
        }
        
        public LNetCommand? ReadPacket(byte[] data)
        {
            int curPointWithinPacket = 0;

            short protocolVersion = BitConverter.ToInt16(data, curPointWithinPacket);

            curPointWithinPacket += sizeof(short);

            if (protocolVersion != LNetProtocol.LNET_PROTOCOL_VERSION)
            {
                NCError.ShowErrorBox($"Received packet with invalid LNet protocol version {protocolVersion}, expected {LNetProtocol.LNET_PROTOCOL_VERSION}", 181, 
                    "LNetSessionManager::ReadPacket - received packet with first two bytes not equal to LNetProtocol::LNET_PROTOCOL_VERSION!", NCErrorSeverity.Warning, null, false);
            }

            int packetId = BitConverter.ToInt32(data, curPointWithinPacket);

            curPointWithinPacket += sizeof(int);

            // packet type data
            byte packetType = data[curPointWithinPacket];

            LNetCommand? netCommand = PacketIdToCommand(packetType);

            if (netCommand == null)
            {
                NCError.ShowErrorBox($"Received invalid packet type {packetType}, maximum is {LNetCommandTypes.MaximumValidCommand}", 182,
                    "LNetSessionManager::ReadPacket - received an invalid packet type.", NCErrorSeverity.Warning, null, false);
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
                    NCError.ShowErrorBox($"Tried to parse invalid packet ID {packetId}", 180, "LNetSessionManager::PacketIdToCommand encountered invalid Packet ID!",
                        NCErrorSeverity.Warning, null, false);
                    return null;
            }
            
        }
    }
}
