namespace LightningGL
{
    public class ClientHelloCommand : LNetCommand
    {
        public override string Name => "ClientHello";
        public override byte Type => (byte)LNetCommandTypes.ClientHello;

        public string? ClientName { get; private set; }

        public string? ClientIp { get; private set; }

        public override bool ReadCommandData(byte[] cmdData)
        {
            return true; 
        }

        public override byte[] CommandDataToByteArray()
        {
            // just whitespace check because we already checked ip
            if (string.IsNullOrWhiteSpace(ClientName)
                || (string.IsNullOrWhiteSpace(ClientIp)))
            {
                return Array.Empty<byte>(); 
            }

            byte[] nameArray = ClientName.ToByteArrayWithLength();
            byte[] ipArray = ClientIp.ToByteArrayWithLength();

            return ArrayUtils.Combine(nameArray, ipArray);
        }

        public override void OnReceiveAsClient(NetworkClient client)
        {
            // This command can't be received as client only as server
            throw new NotImplementedException();
        }

        public override void OnReceiveAsServer(NetworkServer server, NetworkClient sendingClient)
        {
            // get client information here WAOW
            Logger.Log($"Client connected");
            
        }
    }
}
