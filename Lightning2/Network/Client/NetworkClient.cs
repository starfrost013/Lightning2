namespace LightningGL
{
    /// <summary>
    /// Lightning Network Services client
    /// 
    /// Copyright © 2023 starfrost.
    /// Development started     October 26, 2022
    /// </summary>
    public class NetworkClient
    {
        internal LNetSessionManager SessionManager { get; set; }

        private IPEndPoint ServerIP { get; set; }

        public ushort Port { get; set; }

        /// <summary>
        /// The IP Address of this client so the server knows where to send client messages.
        /// This is actually currently only stored by the server.
        /// </summary>
        private string? ClientIP { get; set; }

        /// <summary>
        /// Name of this client.
        /// </summary>
        public string Name { get; internal set; }

        internal bool Connected { get; set; }

        public NetworkClient(string name)
        {
            SessionManager = new LNetSessionManager();
            Port = GlobalSettings.NetworkDefaultPort;
            Name = name;
            ServerIP = new IPEndPoint(0, 0);
        }
        
        public void Connect(string ipAddress)
        {
            if (!IPAddress.TryParse(ipAddress, out var serverIp))
            {
                // do not connect
                NCError.ShowErrorBox($"Invalid server IP address {ipAddress}", 183, NCErrorSeverity.Warning,
                    null, false);
                return;
            }

            ServerIP = new(serverIp, Port);

            // Connect
            SessionManager.UdpClient.Connect(serverIp, Convert.ToInt32(Port));

            // send a Client Hello
        }

        public virtual void SendPacketToServer(LNetCommand command)
        {
            
        }

        internal void Main()
        {
            var ipEndpoint = ServerIP;

            SessionManager.UdpClient.BeginReceive
            (
                new AsyncCallback(OnReceivePacket),
                null
            );
        }

        /// <summary>
        /// Event handler for receiving packets.
        /// </summary>
        private void OnReceivePacket(IAsyncResult result)
        {
            IPEndPoint? ip = ServerIP;


            byte[] commandData = SessionManager.UdpClient.EndReceive(result, ref ip);

            SessionManager.OnReadPacket(commandData);
            Main(); // begin receive again
        }
    }
}
