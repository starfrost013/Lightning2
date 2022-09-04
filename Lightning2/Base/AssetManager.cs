namespace LightningGL
{
    /// <summary>
    /// Base class for Lightning asset managers.
    /// </summary>
    public abstract class AssetManager<T>
    {
        internal List<T> Assets { get; set; }

        public AssetManager()
        {
            Assets = new List<T>();
        }

        public abstract void AddAsset(Window cWindow,
            T asset);

        public virtual void RemoveAsset(Window cWindow,
            T asset)
        {
            Assets.Remove(asset);
        }
    }
}
