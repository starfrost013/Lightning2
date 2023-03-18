namespace LightningGL
{
    /// <summary>
    /// LNetServer
    /// 
    /// The Lightning Network server.
    /// </summary>
    public class NetworkServer
    {
        public List<LNetRoom> Rooms { get; init; }
        public LNetSessionManager SessionManager { get; init; }

        private IPEndPoint EndPoint { get; init; }

        public NetworkServer()
        {
            Rooms = new List<LNetRoom>();
            // this is the server so we want to listen to everything
            EndPoint = new IPEndPoint(IPAddress.Any, GlobalSettings.NetworkDefaultPort);
            SessionManager = new LNetSessionManager();
        }
        internal void Init()
        {
            SessionManager.UdpClient = new UdpClient(GlobalSettings.NetworkDefaultPort);
            BeginReceive();
        }

        internal void BeginReceive()
        {
            Logger.Log("Preparing to receive data...");
            SessionManager.UdpClient.BeginReceive
            (
                new AsyncCallback(OnReceivePacket),
                null
            );
        }

        internal void SendPacket()
        {
            Logger.Log("Sending network commands is not implemented, please remind me to implement it");
        }

        private void OnReceivePacket(IAsyncResult result)
        {
            UdpState? udpState = (UdpState?)result.AsyncState;

            IPEndPoint? endPoint = EndPoint;

            if (udpState == null
                || endPoint == null) return;

            byte[] receivedData = SessionManager.UdpClient.EndReceive(result, ref endPoint);

            // TEST - DEBUG - VERSION
            Logger.Log($"CLIENT IP: {udpState.Ip} - DATA: {receivedData}");

            //todo: packet handling
            
            //BeginReceive again
            BeginReceive(); 
        }

        internal void Shutdown()
        {
            SessionManager.UdpClient.Close();
        }

    }
}
