
namespace LightningGL
{
    /// <summary>
    /// LNetRoom
    /// 
    /// Defines a room.
    /// </summary>
    public class LNetRoom
    {
        public List<NetworkClient> Clients { get; private set; }

        public LNetRoom()
        {
            Clients = new List<NetworkClient>();
        }
    }
}
