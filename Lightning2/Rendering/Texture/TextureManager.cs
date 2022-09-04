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

        // this is here for reasons (so that it can be used from using static LightningGL.Window)
        public override void AddAsset(Window cWindow, Texture asset)
        {
            if (asset.Path != Texture.CREATED_TEXTURE_PATH) asset.Load(cWindow);
            Assets.Add(asset);
        }

        public override void RemoveAsset(Window cWindow, Texture asset)
        {
            asset.Unload();
            Assets.Remove(asset);
        }

        /// <summary>
        /// Renders all of the textures in the texture manager.
        /// </summary>
        /// <param name="cWindow">The window to render the textures to.</param>
        internal void Render(Window cWindow)
        {
            Camera curCamera = cWindow.Settings.Camera;

            foreach (Texture texture in Assets)
            {
                if (curCamera != null
                    && !texture.SnapToScreen)
                {
                    texture.RenderPosition = new Vector2
                        (
                            texture.Position.X - curCamera.Position.X,
                            texture.Position.Y - curCamera.Position.Y
                        );
                }
                else
                {
                    texture.RenderPosition = texture.Position;
                }

                texture.Draw(cWindow);
            }
        }

    }
}
