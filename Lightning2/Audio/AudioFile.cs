using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.IO;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// AudioFile
    /// 
    /// March 24, 2022 (modified  June 12, 2022: inherits from Renderable)
    /// 
    /// Defines an audio file.
    /// </summary>
    public class AudioFile : Renderable
    {
        /// <summary>
        /// Defines the unmanaged handle to this audio file.
        /// </summary>
        public IntPtr AudioHandle { get; private set; }

        /// <summary>
        /// A name used to describe this audio file.
        /// </summary>
        public string Name { get; internal set; }

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

        /// <summary>
        /// Loads this audio file.
        /// </summary>
        internal void Load()
        {
            AudioHandle = SDL_mixer.Mix_LoadWAV(Path);

            if (AudioHandle == IntPtr.Zero) _ = new NCException($"Error loading audio file at {Path}: {SDL_mixer.Mix_GetError()}", 51, "An SDL_mixer error occurred in AudioFile::Load", NCExceptionSeverity.Error);

            RealVolume = 1; // make sure there is always a value
        }

        /// <summary>
        /// Internal: Used by AudioManager ONLY to unload file
        /// </summary>
        internal void Unload() => SDL_mixer.Mix_FreeChunk(AudioHandle); // loadmus used therefore we are freeing chunks

        /// <summary>
        /// Plays this audio file until <see cref="Stop"/> is called.
        /// </summary>
        public void Play()
        {
            SDL_mixer.Mix_PlayChannel(Channel, AudioHandle, Repeat);
            Playing = true;
        }

        /// <summary>
        /// Update positional sound volume based on current main camera position.
        /// </summary>
        /// <param name="cWindow"></param>
        internal void Update(Window cWindow)
        {
            if (!PositionalSound) return;

            Vector2 cameraPosition = cWindow.Settings.Camera.Position;
            Vector2 audioPosition = RenderPosition;

            // faster than math.pow
            double magnitude = Vector2.Distance(audioPosition, cameraPosition);

            if (magnitude > 0)
            {
                // /12 to make sounds fade out slower.
                int volumeToSet = (int)(RealVolume / (magnitude / 12) * 128);
                SDL_mixer.Mix_Volume(Channel, volumeToSet);
            }
            else // set to (realvolume * 128) if <= 0
            {
                int volumeToSet = (int)(RealVolume * 128);
                SDL_mixer.Mix_Volume(Channel, volumeToSet);
            }
        }

        /// <summary>
        /// Pause the current audio file.
        /// </summary>
        public void Pause()
        {
            SDL_mixer.Mix_Pause(Channel);
            Playing = false;
        }

        /// <summary>
        /// Stop the current audio file.
        /// </summary>
        public void Stop()
        {
            SDL_mixer.Mix_HaltChannel(Channel);
            Playing = false;
        }

        /// <summary>
        /// Set the current audio volume.
        /// </summary>
        /// <param name="volume">A double, range 0 to 1, setting the volume of this audio file.</param>
        public void SetVolume(double volume)
        {
            RealVolume = volume;
            SDL_mixer.Mix_Volume(Channel, (int)(RealVolume * 128));
        }
    }
}