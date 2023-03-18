namespace LightningGL
{
    /// <summary>
    /// ReplicatedStorage
    /// 
    /// Storage to ensure synchronisation of the renderable list between client and server.
    /// Used for the SyncRenderables command.
    /// </summary>
    public class ReplicatedStorage
    {
        public List<Renderable> Renderables { get; set; }
        
        public ReplicatedStorage()
        {
            Renderables = new List<Renderable>();
        }
    }
}
