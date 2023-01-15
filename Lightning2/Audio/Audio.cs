namespace LightningGL
{
    /// <summary>
    /// AudioFile
    /// 
    /// March 24, 2022 (modified  June 12, 2022: inherits from Renderable)
    /// 
    /// Defines an audio file.
    /// </summary>
    public class Audio : Renderable
    {
        /// <summary>
        /// Defines the unmanaged handle to this audio file.
        /// </summary>
        public nint AudioHandle { get; private set; }

        /// <summary>
        /// Determines if the current audio file is playing
        /// </summary>
        public bool Playing { get; internal set; }

        /// <summary>
        /// The number of times this audio file will repeat.
        /// 0 means the file will not repeat.
        /// -1 means the file will endlessly repeat.
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        /// The path to the audio file to be loaded.
        /// </summary>
        public string Path { get; internal set; }

        /// <summary>
        /// Internal: Set by AudioManager. The SDL_mixer channel this sound uses.
        /// </summary>
        internal int Channel { get; set; }

        /// <summary>
        /// Determines if this sound is a positional sound. 
        /// </summary>
        public bool PositionalSound { get; set; }

        /// <summary>
        /// Private: The real volume of this sound.
        /// 
        /// Used for positional sound calculations.
        /// </summary>
        private double RealVolume { get; set; }

        private const double DEFAULT_REAL_VOLUME = 1d;

        public Audio(string name, string path) : base(name)
        {
            Path = path;
        }

        /// <summary>
        /// Loads this audio file.
        /// </summary>
        public override void Create()
        {
            AudioHandle = Mix_LoadWAV(Path);
                
            if (AudioHandle == nint.Zero) NCError.ShowErrorBox($"Error loading audio file at {Path}: {Mix_GetError()}", 51, NCErrorSeverity.Error);

            RealVolume = DEFAULT_REAL_VOLUME; // make sure there is always a value

            if (!File.Exists(Path))
            {
                NCError.ShowErrorBox($"Error loading audio file: The path {Path} does not exist!", 52, NCErrorSeverity.FatalError);
                return;
            }

            if (Path.Contains(".mod", StringComparison.InvariantCultureIgnoreCase))
            {
                NCError.ShowErrorBox(".mod file loading is completely broken in SDL_mixer 2.6.2 and causes memory leaks. Sorry, not my code.", 167, NCErrorSeverity.Error);
                return;
            }

            Channel = AudioManager.LastChannelId;
            AudioManager.LastChannelId++;

            NCLogging.Log($"Loaded audio file at {Path} to channel {Channel}");
            Loaded = true;
        }

        /// <summary>
        /// Internal: Used by AudioManager ONLY to unload file
        /// </summary>
        public override void Destroy() => Mix_FreeChunk(AudioHandle); // loadmus used therefore we are freeing chunks

        /// <summary>
        /// Plays this audio file until <see cref="Stop"/> is called.
        /// </summary>
        public void Play()
        {
            Mix_PlayChannel(Channel, AudioHandle, Repeat);
            Playing = true;
        }

        /// <summary>
        /// Internal: Update positional sound volume based on current main camera position.
        /// </summary>
        /// <param name="SceneManager.Renderer"></param>
        public override void Update()
        {
            if (!PositionalSound) return;

            Vector2 cameraPosition = Lightning.Renderer.Settings.Camera.Position;
            Vector2 audioPosition = RenderPosition;

            // faster than math.pow
            double magnitude = Vector2.Distance(audioPosition, cameraPosition);

            if (magnitude > 0)
            {
                // /12 to make sounds fade out slower.
                int volumeToSet = (int)(RealVolume / (magnitude / 12) * 128);
                Mix_Volume(Channel, volumeToSet);
            }
            else // set to (realvolume * 128) if <= 0
            {
                int volumeToSet = (int)(RealVolume * 128);
                Mix_Volume(Channel, volumeToSet);
            }
        }

        /// <summary>
        /// Pause the current audio file.
        /// </summary>
        public void Pause()
        {
            Mix_Pause(Channel);
            Playing = false;
        }

        /// <summary>
        /// Stop the current audio file.
        /// </summary>
        public void Stop()
        {
            Mix_HaltChannel(Channel);
            Playing = false;
        }

        /// <summary>
        /// Set the current audio volume.
        /// </summary>
        /// <param name="volume">A double, range 0 to 1, setting the volume of this audio file.</param>
        public void SetVolume(double volume)
        {
            RealVolume = volume;
            Mix_Volume(Channel, (int)(RealVolume * 128));
        }
    }
}