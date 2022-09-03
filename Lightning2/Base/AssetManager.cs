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

        public abstract T Load(Window cWindow,
            T asset);

        public virtual void AddAsset(Window cWindow,
            T asset)
        {
            Load(cWindow, asset);
            AssetList.Add(asset);
        }
    }
}
