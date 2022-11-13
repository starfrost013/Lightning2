namespace LightningGL
{
    /// <summary>
    /// Lightning Network Services client
    /// 
    /// Copyright © 2023 starfrost.
    /// Development started     October 26, 2022
    /// </summary>
    public class LNetClient
    {
        public LNetSessionManager SessionManager { get; set; }

        public string? ServerIP { get; set; }

        public ushort ServerPort { get; set; }

        /// <summary>
        /// The IP Address of this client so the server knows where to send client messages.
        /// This is actually currently only stored by the server.
        /// </summary>
        public string? IP { get; internal set; }

        /// <summary>
        /// Name of this client.
        /// </summary>
        public string Name { get; internal set; }

        public LNetClient(string name)
        {
            SessionManager = new LNetSessionManager();
            ServerPort = GlobalSettings.NetworkDefaultPort;
            Name = name;
        }
        
        public void Connect()
        {
            IPAddress? serverIp = new IPAddress(0);

            if (!IPAddress.TryParse(ServerIP, out serverIp))
            {
                // do not connect
                NCError.Throw($"Invalid server IP address {ServerIP}", 183, "LNetClient::Connect tried to connect to invalid server IP", NCErrorSeverity.Warning,
                    null, false);
                return;
            }

            // Connect
            SessionManager.UdpClient.Connect(serverIp, Convert.ToInt32(ServerPort));

            // send a Client Hello

        }

        public virtual void SendPacketToServer(LNetCommand lnc)
        {

        }

        private void Main()
        {
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
            
        }
    }
}
