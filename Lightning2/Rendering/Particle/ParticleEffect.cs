using NuCore.Utilities;
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
        /// The velocity of this particle effect's particles.
        /// 
        /// Partially ignored in the case of <see cref="Mode"/> being set to <see cref="ParticleMode.Explode"/>.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Maximum number of particles created each frame.
        /// The default value is <see cref="Amount"/> divided by 150.
        /// </summary>
        public uint MaxNumberCreatedEachFrame { get; set; }

        /// <summary>
        /// The mode of this particle effect.
        /// 
        /// See <see cref="ParticleEffect"/>.
        /// </summary>
        public ParticleMode Mode { get; set; }

        /// <summary>
        /// The number of frames to wait between creating particles.
        /// </summary>
        public int FrameSkipBetweenCreatingParticles { get; set; }

        /// <summary>
        /// Private field used for efficient generation of random numbers.
        /// </summary>
        private Random Random = new Random();
        
        /// <summary>
        /// Last particle ID created. Used only when <see cref="Mode"/> is set to <see cref="ParticleMode.Explode"/>
        /// </summary>
        private int LastId { get; set; }

        /// <summary>
        /// Determines if this particle effect is playing always or needs to be manually triggered.
        /// </summary>
        public bool NeedsManualTrigger { get; set; }

        /// <summary>
        /// Determines if this particle effect is playing.
        /// </summary>
        private bool Playing { get; set; }

        public ParticleEffect(Texture nTexture)
        {
            Texture = nTexture;
            Mode = ParticleMode.SinCos;
        }

        public void Load(Texture nTexture, Window cWindow)
        {
            NCLogging.Log($"Loading particle effect at path {nTexture.Path}...");
            Particles = new List<Particle>();
            Texture = nTexture;
            Texture.Load(cWindow);
            Texture.SnapToScreen = SnapToScreen;
            if (MaxNumberCreatedEachFrame == 0) MaxNumberCreatedEachFrame = Amount / 150;
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

            // determine if a new particle set is to be created. check if under max AND if frame skip
            bool createNewParticleSet = (Particles.Count < Amount);

            if (FrameSkipBetweenCreatingParticles > 0)
            {
                if (cWindow.FrameNumber % (FrameSkipBetweenCreatingParticles + 1) != 0) createNewParticleSet = false;
            }

            if (createNewParticleSet) AddParticleSet();

            Texture.Position = Position;

            for (int curParticle = 0; curParticle < Particles.Count; curParticle++)
            {
                Particle particle = Particles[curParticle];

                // Check if absolute velocity mode is on.
                // This means we treat velocity as an absolute value and do not
                // vary particle positions. Use if we only want particles going in one direction
                if (Mode != ParticleMode.AbsoluteVelocity)
                {
                    int particleAngleId = particle.Id;

                    double angleRads = 0;

                    if (Mode == ParticleMode.SinCos)
                    {
                        angleRads = particleAngleId / (180 * Math.PI);
                    }
                    else if (Mode == ParticleMode.Normal
                        || Mode == ParticleMode.Explode)
                    {
                        // put it in degreeeeeeees mode
                        angleRads = particleAngleId;
                    }

                    double xMul = Math.Sin(angleRads);
                    double yMul = Math.Cos(angleRads);

                    // set up the velocity
                    Vector2 velocity = new Vector2(Convert.ToSingle(((Velocity.X * xMul) / 100) * particle.Lifetime),
                        Convert.ToSingle(((Velocity.Y * yMul) / 100) * particle.Lifetime));
                    
                    // Clamp velocity on "Normal" mode as opposed to explode
                    if (Mode != ParticleMode.Explode)
                    {
                        if (velocity.X > Math.Abs(Velocity.X)) velocity.X = Math.Abs(Velocity.X);
                        if (velocity.Y > Math.Abs(Velocity.Y)) velocity.Y = Math.Abs(Velocity.Y);

                        if (velocity.X < -Math.Abs(Velocity.X)) velocity.X = -Math.Abs(Velocity.X);
                        if (velocity.Y < -Math.Abs(Velocity.Y)) velocity.Y = -Math.Abs(Velocity.Y);
                    }

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
            // if needsmanualtrigger then don't play if we are not playing the particle effect
            if (NeedsManualTrigger
                && !Playing) return;

            Particle particle = new Particle();

            // This is a bit hacky but it's less code than making
            // Rnd.NextDouble generate a negative number
            int nVariance = (int)(Variance * 10000000);

            float varX = Random.Next(-nVariance, nVariance);
            float varY = Random.Next(-nVariance, nVariance);

            varX /= 10000000;
            varY /= 10000000;

            particle.Position = Position + new Vector2(varX, varY);

            if (Mode == ParticleMode.Explode)
            {
                particle.Id = LastId % 360;
                LastId++;
            }
            else
            {
                particle.Id = Random.Next(0, 360);
            }

            Particles.Add(particle);
        }

        public void Play()
        { 
            if (NeedsManualTrigger && !Playing) Playing = true;
        }

        public void Stop()
        {
            if (NeedsManualTrigger && Playing) Playing = false;
        }

        public void Unload()
        {
            Particles.Clear();
        }
        
    }
}