namespace LightningGL
{
    /// <summary>
    /// Base class for Lightning asset managers.
    /// </summary>
    public abstract class AssetManager<T>
    {
        internal List<T> AssetList { get; set; }

        public AssetManager()
        {
            AssetList = new List<T>();
        }

        public abstract void AddAsset(Window cWindow,
            T asset);

        public virtual void RemoveAsset(Window cWindow,
            T asset)
        {
            AssetList.Remove(asset);
        }
    }
}
