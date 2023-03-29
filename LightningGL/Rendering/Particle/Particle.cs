namespace LightningGL
{
    /// <summary>
    /// Particle
    /// 
    /// Defines an individual particle as a part of a particle effect.
    /// </summary>
    internal class Particle : Renderable
    {
        /// <summary>
        /// Lifetime of this particle in frames. Taken from its <see cref="ParticleEffect"/>.
        /// </summary>
        internal float Lifetime { get; set; }

        /// <summary>
        /// Id of this particle. 
        /// We use this for angle calculations. Using the current ID in the effect renderer doesn't work
        /// </summary>
        internal int Id { get; set; }

        internal Particle(string name, Vector2 position, int id) : base(name)
        {
            Position = position;
            Id = id;    
        }
    }
}