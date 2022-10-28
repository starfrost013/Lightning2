
namespace LightningNetwork
{
    public class PingPongCommand : LNetCommand
    {
        public override string Name => "PingPongTest";

        public override byte Type => (byte)LNetCommandTypes.LNET_COMMAND_TEST_PING_PONG;

        public override void OnReceiveAsServer(LNetServer server)
        {
            NCLogging.Log("Server Pong!");
        }

        public override void OnReceiveAsClient(LNetClient client)
        {
            NCLogging.Log("Client Pong!");
        }
    }
}
