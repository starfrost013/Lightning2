namespace LightningNetwork
{
    /// <summary>
    /// LNetPacket
    /// 
    /// Defines a packet.
    /// </summary>
    public class LNetPacket
    {
        public LNetPacketHeader Header { get; set; }

        public LNetPacket()
        {
            Header = new LNetPacketHeader();
        }
    }
}
