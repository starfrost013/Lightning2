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
    public static class TextureUtils 
    {
        /// <summary>
        /// If true, screen-space coordinates will be used for this layer instead of world-space coordinates.
        /// </summary>
        public static bool SnapToScreen { get; set; }

        public static Texture? CloneTexture(string name, bool clone = false)
        {
            try
            {
                Texture? texture = (Texture?)Lightning.Renderer.GetRenderableByName(name);

                if (texture == null)
                {
                    Logger.LogError($"Tried to get name of Texture not in hierarchy in call to TextureUtils::GetInstanceOfTexture!", 401, LoggerSeverity.Error, null, true);
                    return null;
                }
                else
                {
                    return CloneTexture(texture, clone);
                }
            }
            catch
            {
                Logger.LogError($"Unknown error in TextureUtils::GetInstanceOfTexture!", 402, LoggerSeverity.Error, null, true);
                return null;
            }
        }

        public static Texture? CloneTexture(Texture texture, bool clone = false)
        {
            // texture ID?
            int rTexture = Random.Shared.Next(10000000, 99999999);

            Texture clonedTexture = new($"{texture.Name}_{rTexture}", texture.Size.X, texture.Size.Y)
            {
                FormatHandle = texture.FormatHandle,
                Repeat = texture.Repeat,
                SnapToScreen = texture.SnapToScreen,
                ViewportStart = texture.ViewportStart,
                ViewportEnd = texture.ViewportEnd,
            };

            // not sure if this is the best solution
            if (clone)
            {
                clonedTexture.Handle = texture.Handle;
                clonedTexture.Loaded = true; // no need to reload
            }
            else
            {
                // especially this
                // don't allocate a new texture format here (we already did it)
                clonedTexture.Path = texture.Path;
            }

            Lightning.Renderer.AddRenderable(clonedTexture, texture.Parent); // this loads it
            return clonedTexture;
        }
    }
}