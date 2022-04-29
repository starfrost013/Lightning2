using System;
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
        public uint Amount { get; set; }

        public double Lifetime { get; set; }
        
        public double Variance { get; set; }

        public Particle Particle { get; set; }

        private Random rnd = new Random();

        public void Load(Particle particle, Window cWindow)
        {
            Particle = particle;
            particle.Load(cWindow);
        }

        public void Render(Window cWindow)
        {
            Vector2 basePosition = Particle.Texture.Position;

            float dP = rnd.Next((int)-Variance, (int)Variance);

            for (int i = 0; i < Amount; i++)
            {
                Particle.Texture.Position = new Vector2(basePosition.X + dP, basePosition.Y + dP);

                Particle.Texture.Draw(cWindow);
            }
        }
    }
}
