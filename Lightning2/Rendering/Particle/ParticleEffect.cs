using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lightning2
{
    /// <summary>
    /// ParticleEffect
    /// 
    /// April 23, 2022
    /// 
    /// Defines a particle effect.
    /// </summary>
    public class ParticleEffect
    {
        /// <summary>
        /// The maximum number of particles this particle effect will support.
        /// </summary>
        public uint Amount { get; set; }

        /// <summary>
        /// The lifetime of each particle in frames.
        /// </summary>
        public double Lifetime { get; set; }
        
        /// <summary>
        /// The variance on each particle's lifetime in frames
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// The list of particles as a part of this particle effect
        /// </summary>
        private List<Particle> Particles { get; set; }

        internal Texture Texture { get; private set; }

        /// <summary>
        /// The velocity of this particle effect's particles
        /// </summary>
        public Vector2 Velocity { get; set; }

        private int CurFrame { get; set; }

        private Random rnd = new Random();

        public ParticleEffect(Texture nTexture)
        {
            Texture = nTexture;
        }

        public void Load(Texture nTexture, Window cWindow)
        {
            Particles = new List<Particle>();
            Texture = nTexture;
            Texture.Load(cWindow);
        }

        public void Render(Window cWindow)
        {
            if (CurFrame == 0)
            {
                InitRender(cWindow);
            } 
            else
            {
                RenderCurrent(cWindow); 
            }

            CurFrame++;

        }

        private void InitRender(Window cWindow)
        {
            Vector2 basePosition = Texture.Position;

            float dP = rnd.Next((int)-Variance, (int)Variance);

            foreach (Particle particle in Particles)
            {
                particle.Position = new Vector2(basePosition.X + dP, basePosition.Y + dP);
                Texture.Position = particle.Position;
                Texture.Draw(cWindow);
            }
        }

        private void RenderCurrent(Window cWindow)
        {
            // create a list of particles to remove
            List<Particle> particlesToRemove = new List<Particle>();
            
            foreach (Particle particle in Particles)
            {
                particle.Lifetime++;
                if (particle.Lifetime > Lifetime) particlesToRemove.Remove(particle);
            }

            foreach (Particle particleToRemove in particlesToRemove)
            {
                Particles.Remove(particleToRemove);
                if (Particles.Count <= Amount) AddParticle(particleToRemove);
            }
            
            foreach (Particle particle in Particles)
            {
                particle.Position += Velocity; 
                Texture.Position = particle.Position;
                Texture.Draw(cWindow);
            }
        }

        private void AddParticle(Particle particle)
        {
            Vector2 basePosition = Texture.Position;
            float dP = rnd.Next((int)-Variance, (int)Variance);

            particle.Position = new Vector2(basePosition.X + dP, basePosition.Y + dP);

            Particles.Add(particle);
        }

        public void Unload()
        {
            Particles.Clear();
        }
    }
}
