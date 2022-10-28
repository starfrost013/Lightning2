namespace LightningNetwork
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
        /// </summary>
        public string IP { get; set; }

        public LNetClient()
        {
            SessionManager = new LNetSessionManager();
            ServerPort = GlobalSettings.NetworkDefaultPort;
        }
        
        public void Connect()
        {
            IPAddress? serverIp = new IPAddress(0);

            if (!IPAddress.TryParse(ServerIP, out serverIp))
            {
                // do not connect
                _ = new NCException($"Invalid server IP address {ServerIP}", 183, "LNetClient::Connect tried to connect to invalid server IP", NCExceptionSeverity.Warning,
                    null, false);
                return;
            }

            // Connect
            SessionManager.UdpState.Connect(serverIp, Convert.ToInt32(ServerPort));

            // send a Client Hello

        }

        private void Main()
        {

        }
    }
}
