
namespace LightningGL
{
    /// <summary>
    /// Client
    /// 
    /// The lightning client.
    /// </summary>
    public class Client : LightningBase
    {
        public LNetClient NetworkManager { get; set; }

        public Client() : base()
        {
            NetworkManager = new LNetClient($"Test Client {Random.Shared.Next(100000, 999999)}");
        }

        public override void Init()
        {
            NCLogging.Log("Lightning Client initialising...");
            base.Init();
        }

        internal override void Main()
        {
            if (NetworkManager.Connected)
            {
                NetworkManager.Main();
            }

            base.Main();
        }
    }
}
