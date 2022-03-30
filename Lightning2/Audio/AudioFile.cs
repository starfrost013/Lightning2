using NuCore.SDL2;
using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Numerics; 
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// AudioFile
    /// 
    /// March 24, 2022
    /// 
    /// Defines an audio file.
    /// </summary>
    public class AudioFile
    {
        /// <summary>
        /// Defines the unmanaged handle to this audio file.
        /// </summary>
        public IntPtr AudioHandle { get; private set; }

        /// <summary>
        /// A name used to describe this audio file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines if the current audio file is playing
        /// </summary>
        public bool Playing { get; set; }

        /// <summary>
        /// The number of times this audio file will repeat.
        /// 0 means the file will not repeat.
        /// -1 means the file will endlessly repeat.
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        /// The path to the audio file to be loaded.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Internal: Set by AudioManager. The SDL_mixer channel this sound uses.
        /// </summary>
        internal int Channel { get; set; }

        /// <summary>
        /// Determines if this sound is a positional sound. 
        /// </summary>
        public bool PositionalSound { get; set; }
        
        /// <summary>
        /// The position of this sound. If <see cref="PositionalSound"/> is set to <c>false</c>, this value is ignored.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The size of this sound. If <see cref="PositionalSound"/> is set to <c>false</c>, this value is ignored.
        /// </summary>
        public Vector2 Size { get; set; }

        internal void Load()
        {
            if (!File.Exists(Path)) throw new NCException($"Error loading audio file: The path {Path} does not exist!", 50, "AudioFile.Load", NCExceptionSeverity.FatalError);

            AudioHandle = SDL_mixer.Mix_LoadWAV(Path);

            if (AudioHandle == IntPtr.Zero) throw new NCException($"Error loading audio file at {Path}: {SDL_mixer.Mix_GetError()}", 51, "AudioFile.Load", NCExceptionSeverity.Error);
        }

        public void Play()
        {
            if (!PositionalSound)
            {
                SDL_mixer.Mix_PlayChannel(Channel, AudioHandle, Repeat); 
            }
        }

        public void Pause() => SDL_mixer.Mix_Pause(Channel);

        public void Stop() => SDL_mixer.Mix_HaltChannel(Channel);

        public void SetVolume(double Volume)
        {
            int real_volume = (int)Math.Clamp(Volume * 128, 0, 128);

            SDL_mixer.Mix_Volume(Channel, real_volume);

        }
    }
}
