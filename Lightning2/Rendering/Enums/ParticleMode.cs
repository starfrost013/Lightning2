namespace LightningGL
{
    /// <summary>
    /// ParticleMode
    /// 
    /// July 31, 2022
    /// 
    /// Defines modes for particle effects.
    /// </summary>
    public enum ParticleMode
    {
        /// <summary>
        /// No alteration is done to the velocity.
        /// </summary>
        AbsoluteVelocity = 0,

        /// <summary>
        /// The velocity is put through a trigonometric function in order to create a single point where the particles emanate from in the centre.
        /// </summary>
        SinCos = 1,

        /// <summary>
        /// Normal particle mode. A toned down explosion mode
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Value used for the default particle mode.
        /// </summary>
        Default = Normal,

        /// <summary>
        /// Explosion mode. The particles explode around a specific point.
        /// </summary>
        Explode = 3
    }
}
