
namespace LightningGL
{
    /// <summary>
    /// LNetCommand
    /// 
    /// Defines an LNet command. 
    /// </summary>
    public abstract class LNetCommand
    {
        public LNetCommandHeader Header { get; set; }

        public abstract string Name { get; }

        /// <summary>
        /// Type of this command. Should always be equal to this command's value in <see cref="LNetCommandTypes"/>.
        /// </summary>
        public abstract byte Type { get; }

        public byte[] Data { get; set; }

        public virtual bool ReadCommandData(byte[] cmdData)
        {
            // no command data so ignore all data after packet info
            Data = cmdData;
            return true;
        }

        public virtual byte[] CommandDataToByteArray()
        {
            return Array.Empty<byte>(); 
        }

        public abstract void OnReceiveAsServer(NetworkServer server, NetworkClient sendingClient);

        public abstract void OnReceiveAsClient(NetworkClient client);

        private byte[] HeaderToByteArray()
        {
            byte[] array1 = Header.ToByteArray();
            byte[] array2 = new byte[1] { Type };

            return NCArray.Combine(array1, array2);
        }

        public virtual byte[] PacketToByteArray(LNetCommand packet)
        {
            byte[] array1 = HeaderToByteArray();
            byte[] array2 = packet.CommandDataToByteArray();

            return NCArray.Combine(array1, array2);
        }

        public LNetCommand()
        {
            Header = new LNetCommandHeader(); 
            Data = Array.Empty<byte>();
        }

    }
}
