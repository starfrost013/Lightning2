
namespace LightningNetwork
{
    public class PingPongCommand : LNetCommand
    {
        public override string Name => "PingPongTest";

        public override byte Id => (byte)LNetCommandTypes.LNET_COMMAND_TEST_PING_PONG;

        public override void OnSend()
        {
            NCLogging.Log("Ping!");
        }

        public override void OnReceive()
        {
            NCLogging.Log("Pong!");
        }
    }
}
