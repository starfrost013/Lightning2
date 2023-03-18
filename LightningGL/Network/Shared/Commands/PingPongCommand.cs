﻿namespace LightningGL
{
    public class PingPongCommand : LNetCommand
    {
        public override string Name => "PingPongTest";

        public override byte Type => (byte)LNetCommandTypes.PingPong;

        public override void OnReceiveAsServer(NetworkServer server, NetworkClient sendingClient)
        {
            Logger.Log("Server Pong!");
        }

        public override void OnReceiveAsClient(NetworkClient client)
        {
            Logger.Log("Client Pong!");
        }
    }
}