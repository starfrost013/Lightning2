
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

        private int Length => Header.ToByteArray().Length + sizeof(byte);

        public virtual bool ReadCommandData(byte[] cmdData)
        {
            // no command data so ignore all data after packet info
            return true;
        }

        public virtual byte[] CommandDataToByteArray()
        {
            return new byte[0];
        }

        public abstract void OnReceiveAsServer(LNetServer server, LNetClient sendingClient);

        public abstract void OnReceiveAsClient(LNetClient client);

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
        }

    }
}
