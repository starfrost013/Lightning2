using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// Particle
    /// 
    /// April 28, 2022 (modified June 12, 2022)
    /// 
    /// Defines an individual particle as a part of a particle effect.
    /// </summary>
    public class Particle : Renderable
    {
        public float Lifetime { get; set; }
    }
}