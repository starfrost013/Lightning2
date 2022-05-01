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
        
        public double Variance { get; set; }

        public List<Particle> Particles { get; set; }

        internal Texture Texture { get; private set; }

        private int CurFrame { get; set; }

        private Random rnd = new Random();

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

            for (int i = 0; i < Amount; i++)
            {
                Texture.Position = new Vector2(basePosition.X + dP, basePosition.Y + dP);

                Texture.Draw(cWindow);
            }
        }

        private void RenderCurrent(Window cWindow)
        {
            foreach (Particle particle in Particles)
            {

            }
        }
    }
}
