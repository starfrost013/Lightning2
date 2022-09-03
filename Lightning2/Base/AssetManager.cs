using System.Collections.Generic;

namespace LightningGL
{
    /// <summary>
    /// Base class for Lightning asset managers.
    /// </summary>
    public abstract class AssetManager<T>
    {
        public List<T> AssetList { get; internal set; }

        public abstract T Load(T asset);

        public virtual void AddElement(T asset)
        {
            AssetList.Add(asset);
        }
    }
}
