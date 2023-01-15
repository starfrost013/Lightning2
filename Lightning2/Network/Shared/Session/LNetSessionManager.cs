
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

        /// <summary>
        /// The stack of packets to use.
        /// The most recent one is at the top.
        /// </summary>
        public Stack<LNetCommand> Commands { get; set; }

        private bool PacketsLeft => Commands.Count > 0;

        public LNetSessionManager()
        {
            UdpClient = new UdpClient();
            Commands = new Stack<LNetCommand>(); 
        }
        
        public void OnReceivePacket(byte[] data)
        {
            LNetCommand? latestCommand = ReadPacket(data);
            
            if (latestCommand != null)
            {
                Commands.Push(latestCommand);
            }
        }

        public void HandleAnyRemainingPackets()
        {
            while (PacketsLeft)
            {
                LNetCommand command = Commands.Pop();
            }
        }

        private LNetCommand? ReadPacket(byte[] data)
        {
            int curPointWithinPacket = 0;

            short protocolVersion = BitConverter.ToInt16(data, curPointWithinPacket);

            curPointWithinPacket += sizeof(short);

            if (protocolVersion != LNetProtocol.LNET_PROTOCOL_VERSION)
            {
                NCError.ShowErrorBox($"Received packet with invalid network  protocol version {protocolVersion}, expected {LNetProtocol.LNET_PROTOCOL_VERSION}", 181, 
                    NCErrorSeverity.Warning, default, false);
            }

            int packetId = BitConverter.ToInt32(data, curPointWithinPacket);

            curPointWithinPacket += sizeof(int);

            // packet type data
            byte packetType = data[curPointWithinPacket];

            LNetCommand? netCommand = PacketIdToCommand(packetType);

            if (netCommand == null)
            {
                NCError.ShowErrorBox($"Received invalid packet type {packetType}, maximum is {LNetCommandTypes.MaximumValidCommand}", 182,
                    NCErrorSeverity.Warning, null, false);
                return null;
            }

            // fill in global packet id
            netCommand.Header.Id = packetId;

            int numOfCommandBytes = data.Length - curPointWithinPacket;
            
            if (numOfCommandBytes <= 0)
            {
                // pass a zero byte array to ReceiveCommandData. 
                // This is a special value indicating there is no command data. So hopefully every command will detect this and try and parse it.
                netCommand.ReadCommandData(Array.Empty<byte>());
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
                    NCError.ShowErrorBox($"Tried to parse invalid packet ID {packetId}", 180,
                        NCErrorSeverity.Warning, null, false);
                    return null;
            }
            
        }
    }
}
