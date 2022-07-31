using LightningGL;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// ParticleEffect
    /// 
    /// April 23, 2022 (modified June 12, 2022: Renderable)
    /// 
    /// Defines a particle effect.
    /// </summary>
    public class ParticleEffect : Renderable
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
        /// The list of particles as a part of this particle effect
        /// </summary>
        private List<Particle> Particles { get; set; }

        /// <summary>
        /// Internal: The texture this effect is rendered to.
        /// </summary>
        internal Texture Texture { get; private set; }

        /// <summary>
        /// The velocity of this particle effect's particles
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Maximum number of particles created each frame.
        /// The default value is <see cref="Amount"/> divided by 100.
        /// </summary>
        public uint MaxNumberCreatedEachFrame { get; set; }

        /// <summary>
        /// The mode of this particle effect.
        /// 
        /// See <see cref="ParticleEffect"/>.
        /// </summary>
        public ParticleMode Mode { get; set; }

        /// <summary>
        /// Private field used for efficient generation of random numbers.
        /// </summary>
        private Random rnd = new Random();

        public ParticleEffect(Texture nTexture)
        {
            Texture = nTexture;
            Mode = ParticleMode.SinCosDeg;
        }

        public void Load(Texture nTexture, Window cWindow)
        {
            Particles = new List<Particle>();
            Texture = nTexture;
            Texture.Load(cWindow);
            if (MaxNumberCreatedEachFrame == 0) MaxNumberCreatedEachFrame = Amount / 100;
        }

        public void Render(Window cWindow)
        {
            // create a list of particles to remove
            List<Particle> particlesToRemove = new List<Particle>();

            // Check what particles have to be removed
            foreach (Particle particle in Particles)
            {
                particle.Lifetime++;
                if (particle.Lifetime > Lifetime) particlesToRemove.Add(particle);
            }

            foreach (Particle particleToRemove in particlesToRemove)
            {
                Particles.Remove(particleToRemove);
            }

            if (Particles.Count < Amount) AddParticleSet();

            Texture.Position = Position;

            for (int curParticle = 0; curParticle < Particles.Count; curParticle++)
            {
                Particle particle = Particles[curParticle];

                // Check if absolute velocity mode is on.
                // This means we treat velocity as an absolute value and do not
                // vary particle positions. Use if we only want particles going in one direction
                if (Mode != ParticleMode.AbsoluteVelocity)
                {
                    int particleAngleId = 0;

                    if (Mode == ParticleMode.SinCosExplode)
                    {
                        particleAngleId = curParticle % 360;
                    }
                    else
                    {
                        particleAngleId = particle.Id;
                    }

                    double angleRads = 0,
                        xMul = 0,
                        yMul = 0;

                    if (Mode == ParticleMode.SinCos)
                    {
                        angleRads = particleAngleId / (180 * Math.PI);

                    }
                    else if (Mode == ParticleMode.SinCosDeg)
                    {
                        // put it in degreeeeeeees mode
                        angleRads = particleAngleId;
                    }

                    xMul = Math.Sin(angleRads);
                    yMul = Math.Cos(angleRads);

                    // set up the velocity
                    Vector2 velocity = new Vector2(Convert.ToSingle(((Velocity.X * xMul) / 100) * particle.Lifetime),
                        Convert.ToSingle(((Velocity.Y * yMul) / 100) * particle.Lifetime));

                    particle.Position += velocity;
                }
                else
                {
                    particle.Position += new Vector2((Velocity.X / 100) * particle.Lifetime, (Velocity.Y / 100) * particle.Lifetime);
                }

                Texture.Position = particle.Position;
                Texture.Draw(cWindow);
            }
        }

        private void AddParticleSet()
        {
            // save the number of particles to render this frame
            uint numToAddThisFrame = MaxNumberCreatedEachFrame;

            if (Amount - Particles.Count < MaxNumberCreatedEachFrame) numToAddThisFrame = Convert.ToUInt32((Amount - Particles.Count));

            for (int i = 0; i < numToAddThisFrame; i++) AddParticle();
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