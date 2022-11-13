namespace LightningNetwork
{
    /// <summary>
    /// LNetServer
    /// 
    /// The Lightning Network server.
    /// </summary>
    public class LNetServer
    {
        public List<LNetRoom> Rooms { get; init; }
        public LNetSessionManager SessionManager { get; init; }

        private IPEndPoint EndPoint { get; init; }

        public LNetServer()
        {
            Rooms = new List<LNetRoom>();
            // this is the server so we want to listen to everything
            EndPoint = new IPEndPoint(IPAddress.Any, GlobalSettings.NetworkDefaultPort);
            SessionManager = new LNetSessionManager();
        }

        public void Init()
        {
            NCLogging.Log($"Server starting on port: {GlobalSettings.NetworkDefaultPort}", "Server");
            Main();
        }

        private void Main()
        {
            SessionManager.UdpClient.BeginReceive
            (
                new AsyncCallback(OnReceivePacket),
                null
            );
        }

        private void OnReceivePacket(IAsyncResult result)
        {
            UdpState udpState = (UdpState)result.AsyncState;

            if (anyEndPoint == null) return;

            byte[] receivedData = SessionManager.UdpClient.EndReceive(result, ref anyEndPoint);

            // TEST - DEBUG - VERSION
            NCLogging.Log(receivedData.ToString());

            //todo: packet handling
            // Loop
            Main(); 
        }
    }
}
