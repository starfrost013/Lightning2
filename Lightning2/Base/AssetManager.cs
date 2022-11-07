namespace LightningGL
{
    /// <summary>
    /// Base class for Lightning asset managers.
    /// </summary>
    public abstract class AssetManager<T> where T : Renderable
    {
        public abstract T? AddAsset(T asset);

        public virtual void RemoveAsset(T asset)
        {
            
        }

        internal virtual void Update()
        {

        }
    }
}
