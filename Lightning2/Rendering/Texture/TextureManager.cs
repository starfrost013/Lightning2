﻿namespace LightningGL
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
        public override Texture AddAsset(Renderer cRenderer, Texture asset)
        {
            asset.Load(cRenderer);
            Assets.Add(asset);
            return asset;
        }

        public override void RemoveAsset(Renderer cRenderer, Texture asset)
        {
            asset.Unload();
            Assets.Remove(asset);
        }

        public Texture GetInstanceOfTexture(Renderer cRenderer, string path)
        {
            foreach (Texture texture in Assets)
            {
                if (texture.Path == path)
                {
                    return new Texture(cRenderer, texture.Size.X, texture.Size.Y)
                    {
                        Handle = texture.Handle,
                        FormatHandle = texture.FormatHandle
                    };
                }
            }

            return null;
        }
    }
}