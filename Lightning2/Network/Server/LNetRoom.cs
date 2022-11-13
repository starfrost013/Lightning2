
namespace LightningNetwork
{
    /// <summary>
    /// LNetRoom
    /// 
    /// Defines a room.
    /// </summary>
    public class LNetRoom
    {
        public List<LNetClient> Clients { get; private set; }

        public LNetRoom()
        {
            Clients = new List<LNetClient>();
        }
    }
}
