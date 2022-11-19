namespace LightningGL
{
    /// <summary>
    /// Base class for Lightning asset managers.
    /// </summary>
    public abstract class AssetManager<T> where T : Renderable
    {
        public abstract T? AddAsset(T asset);

        public virtual List<T> GetAssets()
        {
            List<T> assets = new List<T>();

            foreach (Renderable renderable in Lightning.Renderer.Renderables)
            {
                if (renderable is T)
                {
                    assets.Add((T)renderable);
                }
            }

            return assets;
        }

        public virtual void RemoveAsset(T asset)
        {
            
        }

        internal virtual void Update()
        {

        }
    }
}
