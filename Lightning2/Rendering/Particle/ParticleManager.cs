﻿using NuCore.Utilities;
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
        private static List<ParticleEffect> Effects { get; set; }

        static ParticleManager()
        {
            Effects = new List<ParticleEffect>();
        }

        public static void Shutdown()
        {
            foreach (ParticleEffect effect in Effects)
            {
                NCLogging.Log($"Unloading particle effect at path {effect.Texture.Path}...");
                effect.Unload();
            }
        }

        public static void AddParticleEffect(Window cWindow, ParticleEffect particle)
        {
            particle.Load(particle.Texture, cWindow);
            Effects.Add(particle);
        }

        public static void Render(Window cWindow)
        {
            foreach (ParticleEffect particleEffect in Effects)
            {
                particleEffect.Render(cWindow);
            }
        }
    }
}
