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
        /// <summary>
        /// Lifetime of this particle in frames. Taken from its <see cref="ParticleEffect"/>.
        /// </summary>
        public float Lifetime { get; set; }

        /// <summary>
        /// Id of this particle. 
        /// We use this for angle calculations. Using the current ID in the effect renderer doesn't work
        /// </summary>
        public int Id { get; set; }
    }
}