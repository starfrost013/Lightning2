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
        /// Unloads all effects and shuts down the Particle Manager.
        /// </summary>
        internal void Shutdown()
        {
            foreach (ParticleEffect effect in Assets)
            {
                NCLogging.Log($"Unloading particle effect at path {effect.Texture.Path}...");
                effect.Unload();
            }
        }

        /// <summary>
        /// Adds the particle effect <see cref="ParticleEffect"/> for the window <paramref name="cRenderer"/>.
        /// </summary>
        /// <param name="cRenderer">The window to add the particle effect for.</param>
        /// <param name="asset">The particle effect to add to the window.</param>
        public override void AddAsset(Renderer cRenderer, ParticleEffect asset)
        {
            asset.Load(cRenderer);
            Assets.Add(asset);
        }

        /// <summary>
        /// Removes the particle effect <see cref="ParticleEffect"/> for the window <paramref name="cRenderer"/> from the Particle Manager.
        /// </summary>
        /// <param name="cRenderer">The window to remove the particle effect from.</param>
        /// <param name="asset">The particle effect to remove.</param>
        public override void RemoveAsset(Renderer cRenderer, ParticleEffect asset)
        {
            if (!Assets.Contains(asset)) _ = new NCException($"Attempted to remove a particle effect (loaded from {asset.Texture.Path}) without loading it first!",
                136, "You must load a particle effect before trying to remove it!", NCExceptionSeverity.Error);
            asset.Unload();
            Assets.Remove(asset);
        }
    }
}