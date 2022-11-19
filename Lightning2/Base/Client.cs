
namespace LightningGL
{
    /// <summary>
    /// Client
    /// 
    /// The Lightning client.
    /// </summary>
    public class Client : LightningBase
    {
        public LNetClient NetworkClient { get; set; }

        public Client() : base()
        {
            NetworkClient = new LNetClient($"Test Client {Random.Shared.Next(100000, 999999)}");
        }

        public override void Init()
        {
            NCLogging.Log("Lightning Client initialising...");
            base.Init();
        }

        internal override void Main()
        {
            if (NetworkClient.Connected)
            {
                NetworkClient.Main();
            }

            base.Main();
        }

        internal void Connect(IPEndPoint endPoint)
        {
            
        }
    }
}
