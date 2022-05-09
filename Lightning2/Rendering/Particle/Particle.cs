using System.Numerics;

namespace Lightning2
{
    /// <summary>
    /// Particle
    /// 
    /// April 28, 2022 (modified May 7, 2022)
    /// 
    /// Defines an individual particle as a part of a particle effect.
    /// </summary>
    public class Particle
    {
        public float Lifetime { get; set; }
        public Vector2 Position { get; set; }
    }
}