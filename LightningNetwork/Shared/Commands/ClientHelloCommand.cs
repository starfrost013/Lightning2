﻿

namespace LightningNetwork
{
    public class ClientHelloCommand : LNetCommand
    {
        public override string Name => "ClientHello";
        public override byte Type => (byte)LNetCommandTypes.ClientHello;

        public string? ClientName { get; private set; }

        public string? ClientIp { get; private set; }

        public override bool ReadCommandData(byte[] cmdData)
        {
            return base.ReadCommandData(cmdData);
        }

        public override byte[] CommandDataToByteArray()
        {
            byte[] nameArray = ClientName.ToByteArrayWithLength();
            byte[] ipArray = ClientIp.ToByteArrayWithLength();

            return NCArray.Combine(nameArray, ipArray);
        }

        public override void OnReceiveAsClient(LNetClient client)
        {
            // This command can't be received as client only as server
            throw new NotImplementedException();
        }

        public override void OnReceiveAsServer(LNetServer server, LNetClient sendingClient)
        {
            // get client information here WAOW
            
        }
    }
}
