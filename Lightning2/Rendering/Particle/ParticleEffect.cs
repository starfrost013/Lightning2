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

        private bool _snaptoscreen { get; set; }

        public bool SnapToScreen
        {
            // Check that our texture is valid and if so set its snaptoscreen value
            get
            {
                if (Texture != null) return Texture.SnapToScreen;
                return _snaptoscreen;
            }
            set
            {
                _snaptoscreen = value;
                if (Texture != null) Texture.SnapToScreen = value;
            }
        }

        public bool AbsoluteVelocity { get; set; }

        /// <summary>
        /// Private field used for efficient generation of random numbers.
        /// </summary>
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
            
            // Check what particles have to be removed
            foreach (Particle particle in Particles)
            {
                if (particle.Lifetime > Lifetime) particlesToRemove.Add(particle);
                particle.Lifetime++;
            }

            foreach (Particle particleToRemove in particlesToRemove)
            {
                Particles.Remove(particleToRemove);

                if (Particles.Count <= Amount) AddParticle();
            }

            Texture.Position = Position;

            for (int i = 0; i < Particles.Count; i++)
            {
                Particle particle = Particles[i];

                // Check if absolute velocity mode is on.
                // This means we treat velocity as an absolute value and do not
                // vary particle positions. Use if we only want particles going wrong way
                if (!AbsoluteVelocity)
                {
                    if (i % 4 == 0)
                    {
                        particle.Position += new Vector2((Velocity.X / 500) * particle.Lifetime, (Velocity.Y / 500) * particle.Lifetime);
                    }
                    else if (i % 4 == 1)
                    {
                        particle.Position += new Vector2(((Velocity.X / 500) * particle.Lifetime), -((Velocity.Y / 500) * particle.Lifetime));
                    }
                    else if (i % 4 == 2)
                    {
                        particle.Position += new Vector2(-((Velocity.X / 500) * particle.Lifetime), ((Velocity.Y / 500) * particle.Lifetime));
                    }
                    else if (i % 4 == 3)
                    {
                        particle.Position += new Vector2(-((Velocity.X / 500) * particle.Lifetime), -((Velocity.Y / 500) * particle.Lifetime));
                    }
                }
                else
                {
                    particle.Position += new Vector2((Velocity.X / 500) * particle.Lifetime, (Velocity.Y / 500) * particle.Lifetime);
                }
                
                Texture.Position = particle.Position;
                Texture.Draw(cWindow);
            }
        }

        public void AddParticle()
        {
            Particle particle = new Particle();

            // This is a bit hacky but it's less code than making
            // Rnd.NextDouble generate a negative number
            int nVariance = (int)(Variance * 100000);

            float varX = rnd.Next(-nVariance, nVariance);
            float varY = rnd.Next(-nVariance, nVariance);

            varX /= 100000;
            varY /= 100000;

            particle.Position = Position + new Vector2(varX, varY);
            
            Particles.Add(particle);
        }

        public void Unload()
        {
            Particles.Clear();
        }
    }
}