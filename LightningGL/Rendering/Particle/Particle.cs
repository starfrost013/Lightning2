namespace LightningGL
{
    /// <summary>
    /// Particle
    /// 
    /// April 28, 2022 (modified June 12, 2022)
    /// 
    /// Defines an individual particle as a part of a particle effect.
    /// </summary>
    public class Particle : Texture
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

        public Particle(string name, int sizeX, int sizeY, bool isTarget = false) : base(name, sizeX, sizeY) { }

        public override void Destroy()
        {
            // this does nothing
            // all particles share the same texture
            // so we don't want to unload it
            // but particle inherits from texture and texture::destroy unloads the texture
            // therefore we have this stupid method
            //
            // my precious hack...


        }
    }
}