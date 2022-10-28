
using System.Runtime.CompilerServices;

namespace LightningNetwork
{
    /// <summary>
    /// LNetServer
    /// 
    /// The Lightning Network server.
    /// </summary>
    public class LNetServer
    {
        public List<LNetRoom> Rooms { get; init; }
        public LNetSessionManager SessionManager { get; init; }

        private IPEndPoint EndPoint { get; init; }

        public LNetServer()
        {
            Rooms = new List<LNetRoom>();
            // this is the server so we want to listen to everything
            EndPoint = new IPEndPoint(IPAddress.Any, GlobalSettings.NetworkDefaultPort);
            SessionManager = new LNetSessionManager();
        }

        public void Init()
        {
            SessionManager.LogAsServer($"Server starting on port: {GlobalSettings.NetworkDefaultPort}");
        }


    }
}
