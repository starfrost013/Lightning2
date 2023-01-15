namespace LightningGL
{
    /// <summary>
    /// Server
    /// 
    /// The Lightning server.
    /// </summary>
    public class Server : LightningBase
    {
        public bool Running { get; set; }
        public NetworkServer NetworkServer { get; set; }

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

            NCLogging.Log("Lightning Server starting...");
            
            NCLogging.Log($"Server starting on port: {GlobalSettings.NetworkDefaultPort}", "Server");
            NetworkServer.Init();

            Running = true;

            Initialised = true;
            NCLogging.Log("Server initialisation successful!", ConsoleColor.Green);
            Main();
        }

        internal override void Main()
        {
            while (Running)
            {
                // server is still alive
                NCLogging.Log("The server is still alive!!!");
            }
        }

        public override void Shutdown()
        {
            Running = false;
            NetworkServer.Shutdown();
            
        }
    }
}
