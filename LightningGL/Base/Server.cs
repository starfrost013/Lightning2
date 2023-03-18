namespace LightningGL
{
    /// <summary>
    /// Server
    /// 
    /// The Lightning server.
    /// </summary>
    public class Server : LightningCore
    {
        public bool Running { get; set; }
        public NetworkServer NetworkServer { get; set; }

        private const string LOGGING_PREFIX = "Server";

        public Server() : base()
        {
            NetworkServer = new();
        }

        /// <summary>
        /// Server initialisation code
        /// 
        /// Cuts out everything we don't need frmo client.
        /// TODO: Get as much of this in lightningbase as possible (CoreInit(), then ClientInit()), here for staging/testing
        /// </summary>
        internal override void Init()
        {
            base.Init();

            Logger.Log("Lightning Server starting...");
            
            Logger.Log(LOGGING_PREFIX, $"Server starting on port: {GlobalSettings.NetworkDefaultPort}");
            NetworkServer.Init();

            Running = true;

            Initialised = true;
            Logger.Log(LOGGING_PREFIX, "Server initialisation successful!", ConsoleColor.Green);
            Main();
        }

        internal override void Main()
        {
            while (Running)
            {
                // server is still alive
                Logger.Log("The server is still alive!!!");
            }
        }

        public override void Shutdown()
        {
            Running = false;
            NetworkServer.Shutdown();
            
        }
    }
}
