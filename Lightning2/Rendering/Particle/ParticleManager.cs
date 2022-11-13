namespace LightningGL
{
    /// <summary>
    /// ParticleManager
    /// 
    /// Basic manager class for <see cref="ParticleEffect"/>s.
    /// </summary>
    public class ParticleAssetManager : AssetManager<ParticleEffect>
    {
        /// <summary>
        /// Adds the particle effect <see cref="ParticleEffect"/> for the window <paramref name="Lightning.Renderer"/>.
        /// </summary>
        /// <param name="Lightning.Renderer">The window to add the particle effect for.</param>
        /// <param name="asset">The particle effect to add to the window.</param>
        public override ParticleEffect AddAsset(ParticleEffect asset)
        {
            asset.Load();
            Lightning.Renderer.AddRenderable(asset);
            return asset;
        }

        /// <summary>
        /// Removes the particle effect <see cref="ParticleEffect"/> for the window <paramref name="Lightning.Renderer"/> from the Particle Manager.
        /// </summary>
        /// <param name="Lightning.Renderer">The window to remove the particle effect from.</param>
        /// <param name="asset">The particle effect to remove.</param>
        public override void RemoveAsset(ParticleEffect asset)
        {
            if (!Lightning.Renderer.ContainsRenderable(asset.Name)) NCError.ShowErrorBox($"Attempted to remove a particle effect (loaded from {asset.Texture.Path}) without loading it first!",
                136, "You must load a particle effect before trying to remove it!", NCErrorSeverity.Error);
            asset.Unload();
            Lightning.Renderer.RemoveRenderable(asset);
        }
    }
}