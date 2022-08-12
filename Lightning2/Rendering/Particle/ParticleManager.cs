using NuCore.Utilities;
using System.Collections.Generic;

namespace LightningGL
{
    /// <summary>
    /// ParticleManager
    /// 
    /// Basic manager class for <see cref="ParticleEffect"/>s.
    /// </summary>
    public static class ParticleManager
    {
        /// <summary>
        /// The list of particle effects currently loaded.
        /// </summary>
        private static List<ParticleEffect> Effects { get; set; }

        /// <summary>
        /// Constructor for the Particle Manager.
        /// </summary>
        static ParticleManager()
        {
            Effects = new List<ParticleEffect>();
        }

        /// <summary>
        /// Unloads all effects and shuts down the Particle Manager.
        /// </summary>
        internal static void Shutdown()
        {
            foreach (ParticleEffect effect in Effects)
            {
                NCLogging.Log($"Unloading particle effect at path {effect.Texture.Path}...");
                effect.Unload();
            }
        }

        /// <summary>
        /// Adds the particle effect <see cref="ParticleEffect"/> for the window <paramref name="cWindow"/>.
        /// </summary>
        /// <param name="cWindow">The window to add the particle effect for.</param>
        /// <param name="particle">The particle effect to add to the window.</param>
        public static void AddEffect(Window cWindow, ParticleEffect particle)
        {
            particle.Load(particle.Texture, cWindow);
            Effects.Add(particle);
        }

        /// <summary>
        /// Renders all particle effects to a window.
        /// </summary>
        /// <param name="cWindow">The window to render these particle effects to.</param>
        internal static void Render(Window cWindow)
        {
            foreach (ParticleEffect particleEffect in Effects)
            {
                particleEffect.Render(cWindow);
            }
        }
    }
}