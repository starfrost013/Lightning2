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
        public int Lifetime { get; set; }
        
        /// <summary>
        /// The variance on each particle's lifetime in frames
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// The position of this particle effect.
        /// </summary>
        public Vector2 Position { get; set; }

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
            for (int i = 0; i < Amount; i++)
            {
                AddParticle();

                Texture.Draw(cWindow);
            }
        }

        private void RenderCurrent(Window cWindow)
        {
            // create a list of particles to remove
            List<Particle> particlesToRemove = new List<Particle>();
            
            foreach (Particle particle in Particles)
            {
                if (particle.Lifetime > Lifetime) particlesToRemove.Add(particle);
                particle.Lifetime++;
            }

            foreach (Particle particleToRemove in particlesToRemove)
            {
                Particles.Remove(particleToRemove);

                if (Particles.Count <= Amount)
                {
                    AddParticle();
                }
            }

            Texture.Position = Position;

            foreach (Particle particle in Particles)
            {
                // This is a bit hacky but it's less code than making
                // Rnd.NextDouble generate a negative number
                int nVariance = (int)(Variance * 100000);

                float varX = rnd.Next(-nVariance, nVariance);
                float varY = rnd.Next(-nVariance, nVariance);

                varX /= 100000;
                varY /= 100000;

                Texture.Position += new Vector2(varX, varY);
                Texture.Position += new Vector2((Velocity.X / 500) * Lifetime, (Velocity.Y / 500) * Lifetime);
                Texture.Draw(cWindow);
            }
        }

        public void AddParticle()
        {
            Particle particle = new Particle();
            particle.Position = Position;
            Particles.Add(particle);
        }

        public void Unload()
        {
            Particles.Clear();
        }
    }
}