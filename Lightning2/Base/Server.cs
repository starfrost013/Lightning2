namespace LightningGL
{
    /// <summary>
    /// Server
    /// 
    /// The Lightning server.
    /// </summary>
    public class Server : LightningBase
    {
        public LNetServer NetworkManager { get; set; }

        public Server() : base()
        {
            NetworkManager = new();
        }

        public override void Init()
        {
            NCLogging.Log("Lightning Server initialising...");
            base.Init();
            NCLogging.Log($"Server starting on port: {GlobalSettings.NetworkDefaultPort}", "Server");
            NetworkManager.BeginReceive();
        }
    }
}
