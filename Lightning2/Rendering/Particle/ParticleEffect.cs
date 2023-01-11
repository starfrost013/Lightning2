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
        public int Amount { get; set; }

        /// <summary>
        /// The lifetime of each particle in frames.
        /// </summary>
        public int Lifetime { get; set; }

        /// <summary>
        /// The variance on each particle's position in frames
        /// </summary>
        public float Variance { get; set; }

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
        public int MaxNumberCreatedEachFrame { get; set; }

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

        /// <summary>
        /// Private: Divisor for the final velocity after it has been multiplied by the delta-time.
        /// </summary>
        private readonly int FINAL_VELOCITY_DIVISOR = 100;

        /// <summary>
        /// Private: Value the amount of maximum particles is divided by when <see cref="MaxNumberCreatedEachFrame"/> has not been specified by the user.
        /// </summary>
        private readonly int DEFAULT_MAX_CREATED_EACH_FRAME_DIVISOR = 150;

        internal Texture Texture { get; private set; }

        /// <summary>
        /// The constructor for the <see cref="ParticleEffect"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        /// <param name="texture">The texture to use for particle effects</param>
        public ParticleEffect(string name, Texture texture) : base(name)
        {
            Mode = ParticleMode.SinCos;
            Texture = texture;
        }

        public Particle? AddParticle(Particle asset)
        {
            Lightning.Renderer.AddRenderable(asset, this);
            return asset;
        }

        public override void Create()
        {
            NCLogging.Log($"Loading particle effect at path {Texture.Path}...");
            // don't load it multiple times
            Texture.SnapToScreen = SnapToScreen;
            if (MaxNumberCreatedEachFrame <= 0) MaxNumberCreatedEachFrame = Amount / DEFAULT_MAX_CREATED_EACH_FRAME_DIVISOR;

            // this will load the texture
            Lightning.Renderer.AddRenderable(Texture, this);
        }

        public override void Draw()
        {
            if (Texture == null
                || !Texture.Loaded)
            {
                NCError.ShowErrorBox("Attempted to draw a particle effect without loading it!", 120, 
                    "ParticleEffect::Render called before ParticleEffect::Load!", NCErrorSeverity.FatalError);
                return;
            }

            List<Particle> particles = new();

            foreach (Renderable renderable in Children)
            {
                if (renderable is Particle) particles.Add((Particle)renderable);
            }

            // create a list of particles to remove
            List<Particle> particlesToRemove = new();

            // Check what particles have to be removed
            foreach (Particle particle in particles)
            {
                particle.Lifetime++;
                if (particle.Lifetime > Lifetime) particlesToRemove.Add(particle);
            }

            // remove all the particles we need to remove.
            foreach (Particle particleToRemove in particlesToRemove) Lightning.Renderer.RemoveRenderable(particleToRemove, this);

            // determine if a new particle set is to be created. check if under max AND if frame skip
            bool createNewParticleSet = (particles.Count < Amount);

            if (FrameSkipBetweenCreatingParticles > 0
                && Lightning.Renderer.FrameNumber % (FrameSkipBetweenCreatingParticles + 1) != 0) createNewParticleSet = false;

            if (createNewParticleSet) AddParticleSet();

            for (int curParticle = 0; curParticle < particles.Count; curParticle++)
            {
                Particle particle = particles[curParticle];

                // Check if absolute velocity mode is on.
                // This means we treat velocity as an absolute value and do not
                // vary particle positions. Use if we only want particles going in one direction
                if (Mode != ParticleMode.AbsoluteVelocity
                    && Mode != ParticleMode.ConstantVelocity)
                {
                    int particleAngleId = particle.Id;

                    double angleRads = 0;

                    switch (Mode)
                    {
                        case ParticleMode.SinCos:
                            angleRads = particleAngleId / (180 * Math.PI);
                            break;
                        case ParticleMode.Normal:
                        case ParticleMode.Explode:
                            // put it in degreeeeeeees mode
                            angleRads = particleAngleId;
                            break;
                    }

                    double xMul = Math.Sin(angleRads);
                    double yMul = Math.Cos(angleRads);

                    // set up the velocity
                    Vector2 finalVelocity = new(Convert.ToSingle((((Velocity.X * xMul) / FINAL_VELOCITY_DIVISOR) * particle.Lifetime) * Lightning.Renderer.DeltaTime),
                        Convert.ToSingle((((Velocity.Y * yMul) / FINAL_VELOCITY_DIVISOR) * particle.Lifetime) * (Lightning.Renderer.DeltaTime / 10)));

                    // Clamp velocity on "Normal" mode as opposed to explode
                    if (Mode != ParticleMode.Explode)
                    {
                        if (finalVelocity.X > Math.Abs(Velocity.X)) finalVelocity.X = Math.Abs(Velocity.X);
                        if (finalVelocity.Y > Math.Abs(Velocity.Y)) finalVelocity.Y = Math.Abs(Velocity.Y);

                        if (finalVelocity.X < -Math.Abs(Velocity.X)) finalVelocity.X = -Math.Abs(Velocity.X);
                        if (finalVelocity.Y < -Math.Abs(Velocity.Y)) finalVelocity.Y = -Math.Abs(Velocity.Y);
                    }

                    particle.Position += finalVelocity;
                }
                else
                {
                    Vector2 finalVelocity = new(Convert.ToSingle(Velocity.X * Lightning.Renderer.DeltaTime), Convert.ToSingle(Velocity.Y * Lightning.Renderer.DeltaTime));

                    if (Mode == ParticleMode.AbsoluteVelocity)
                    {
                        particle.Position += new Vector2((finalVelocity.X / FINAL_VELOCITY_DIVISOR) * particle.Lifetime, 
                            (finalVelocity.Y / FINAL_VELOCITY_DIVISOR) * particle.Lifetime);
                    }
                    else
                    {
                        particle.Position += new Vector2((finalVelocity.X / FINAL_VELOCITY_DIVISOR), (finalVelocity.Y / FINAL_VELOCITY_DIVISOR));
                    }
                }

                // IT might be better to put these as normal renderables
                Texture.Position = particle.Position;

                //Texture.Draw();
            }
        }

        private void AddParticleSet()
        {
            // save the number of particles to render this frame
            int numToAddThisFrame = MaxNumberCreatedEachFrame;

            int count = Children.Count;

            if (Amount - count < MaxNumberCreatedEachFrame) numToAddThisFrame = (Amount - count);

            for (int particleId = 0; particleId < numToAddThisFrame; particleId++) AddParticle();
        }

        internal void AddParticle()
        {
            // if needsmanualtrigger then don't play if we are not playing the particle effect
            if (NeedsManualTrigger
                && !Playing) return;

            Texture? tempTexture = TextureManager.GetInstanceOfTexture(Texture.Name);

            Debug.Assert(tempTexture != null); // should never be null

            // manually convert due to CS0553 (not directly convertable as particle is a derived class
            Particle particle = new(tempTexture.Name, (int)tempTexture.Size.X, (int)tempTexture.Size.Y, tempTexture.IsTarget) 
            { 
                Handle = tempTexture.Handle,
                FormatHandle = tempTexture.FormatHandle,
                Repeat = tempTexture.Repeat,
                SnapToScreen = tempTexture.SnapToScreen,
                ViewportStart = tempTexture.ViewportStart,
                ViewportEnd = tempTexture.ViewportEnd,
            };

            // easier to use doubles here so we don't use random.nextsingle
            float varX = Random.Shared.NextSingle() * (Variance - -Variance) + -Variance,
                  varY = Random.Shared.NextSingle() * (Variance - -Variance) + -Variance;

            particle.Position = Position + new Vector2(varX, varY);

            if (Mode == ParticleMode.Explode)
            {
                particle.Id = LastId % 360;
                LastId++;
            }
            else
            {
                particle.Id = Random.Shared.Next(0, 360);
            }

            particle.Name = $"Particle{particle.Id}";

            AddParticle(particle);
        }

        /// <summary>
        /// Plays this particle effect. Does nothing if <see cref="NeedsManualTrigger"/> is not set.
        /// </summary>
        public void Play()
        {
            if (NeedsManualTrigger
                && !Playing) Playing = true;
        }

        /// <summary>
        /// Stops this particle effect. Does nothing if <see cref="NeedsManualTrigger"/> is not set.
        /// </summary>
        public void Stop(bool forceStop = false)
        {
            if (NeedsManualTrigger
                && Playing) Playing = false;

            if (forceStop) Lightning.Renderer.RemoveAllChildren(this);
        }

        /// <summary>
        /// Unloads this particle effect.
        /// </summary>
        public override void Destroy()
        {
            Stop();
            Lightning.Renderer.RemoveRenderable(Texture);
        }
    }
}