using System.Xml.Linq;

namespace LightningGL
{
    /// <summary>
    /// TextureManager
    /// 
    /// July 2, 2022
    /// 
    /// Manager for texture rendering.
    /// </summary>
    public class TextureAssetManager : AssetManager<Texture>
    {
        /// <summary>
        /// If true, screen-space coordinates will be used for this layer instead of world-space coordinates.
        /// </summary>
        public static bool SnapToScreen { get; set; }

        // this is here for reasons (so that it can be used from using static LightningGL.Lightning)
        public override Texture AddAsset(Texture asset)
        {
            asset.Load();
            Lightning.Renderer.AddRenderable(asset);
            return asset;
        }

        public override void RemoveAsset(Texture asset)
        {
            asset.Unload();
            Lightning.Renderer.RemoveRenderable(asset);
        }

        public Texture? GetInstanceOfTexture(string name)
        {
            try
            {
                Texture? texture = (Texture?)Lightning.Renderer.GetRenderableByName(name);

                if (texture == null) return null;

                // texture ID?
                int rTexture = Random.Shared.Next(10000000, 99999999);

                return new Texture($"{texture.Name}_{rTexture}", texture.Size.X, texture.Size.Y)
                {
                    Handle = texture.Handle,
                    FormatHandle = texture.FormatHandle
                };
            }
            catch
            {
                return null;
            }
        }

        public Texture? GetInstanceOfTexture(Texture texture, bool clone = false)
        {
            // texture ID?
            int rTexture = Random.Shared.Next(10000000, 99999999);

            Texture newTexture = new($"{texture.Name}_{rTexture}", texture.Size.X, texture.Size.Y)
            {
                FormatHandle = texture.FormatHandle
            };

            // not sure if this is the best solution
            if (clone)
            {
                newTexture.Handle = texture.Handle;
            }
            else
            {
                // especially this
                // don't allocate a new texture format here (we already did it)
                newTexture.Path = texture.Path;
                newTexture.Load();
            }

            Lightning.Renderer.AddRenderable(newTexture);
            return newTexture;
        }
    }
}