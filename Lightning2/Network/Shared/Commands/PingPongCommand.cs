namespace LightningGL
{
    public class PingPongCommand : LNetCommand
    {
        public override string Name => "PingPongTest";

        public override byte Type => (byte)LNetCommandTypes.PingPong;

        public override void OnReceiveAsServer(LNetServer server, LNetClient sendingClient)
        {
            NCLogging.Log("Server Pong!");
        }

        public override void OnReceiveAsClient(LNetClient client)
        {
            NCLogging.Log("Client Pong!");
        }
    }
}
