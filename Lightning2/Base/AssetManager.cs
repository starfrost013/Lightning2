using System.Collections.Generic;

namespace LightningGL
{
    /// <summary>
    /// Base class for Lightning asset managers.
    /// </summary>
    internal abstract class AssetManager<T>
    {
        internal List<T> AssetList { get; set; }

        internal abstract T Load(T asset);

        internal virtual void AddElement(T asset)
        {
            AssetList.Add(asset);
        }
    }
}
