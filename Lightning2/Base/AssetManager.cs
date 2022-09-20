namespace LightningGL
{
    /// <summary>
    /// Base class for Lightning asset managers.
    /// </summary>
    public abstract class AssetManager<T> where T : Renderable
    {
        internal List<T> Assets { get; set; }

        public AssetManager()
        {
            Assets = new List<T>();
        }

        public abstract void AddAsset(Renderer cRenderer, T asset);

        public virtual void RemoveAsset(Renderer cRenderer, T asset)
        {
            Assets.Remove(asset);
        }

        internal virtual void Update(Renderer cRenderer)
        {
            // Tasks that apply to all renderables.
            foreach (T renderable in Assets)
            {
                renderable.Draw(cRenderer);
            }
        }
    }
}
