namespace LightningGL
{
    /// <summary>
    /// AudioManager
    /// 
    /// March 24, 2022 (modified August 22, 2022: add error check for unloadfile)
    /// 
    /// Defines APIs for playing an arbitrary number of audio files at once.
    /// </summary>
    public class AudioAssetManager : AssetManager<AudioFile>
    {
        private int LastChannelId = 0;

        public override AudioFile AddAsset(AudioFile asset)
        {
            LoadFile(asset.Path, asset.Name);
            return asset;
        }

        /// <summary>
        /// Loads the audio file at path <see cref="Path"/>, if it exists.
        /// </summary>
        /// <param name="path">The path of the file to load.</param>
        /// <param name="name">A name to assign to the audio file. Optional, will be automatically generated from the file path (extension and directory removed) if not supplied.</param>
        /// <exception cref="NCException">An error occurred loading the file.</exception>
        internal void LoadFile(string path, string name)
        {
            if (!File.Exists(path))
            {
                _ = new NCException($"Error loading audio file: The path {path} does not exist!", 52, "AudioManager::Load path parameter does not exist!", NCExceptionSeverity.FatalError);
                return;
            }

            if (path.Contains(".mod", StringComparison.InvariantCultureIgnoreCase))
            {
                _ = new NCException(".mod file loading is completely broken in SDL_mixer 2.6.2 and causes memory leaks. Sorry, not my code.", 167, 
                    "AudioAssetManager::LoadFile path parameter has a .mod extension", NCExceptionSeverity.Error);
            }

            AudioFile tempAudio = new AudioFile(name, path);

            // 11/7/22 As all renderables now have to have a name we don't have to correct for this eventuality
            tempAudio.Name = name;

            tempAudio.Load();

            if (tempAudio.AudioHandle != IntPtr.Zero)
            {
                tempAudio.Channel = LastChannelId;
                LastChannelId++;
                NCLogging.Log($"Loaded audio file at {path} to channel {tempAudio.Channel}");
                Lightning.Renderer.AddRenderable(tempAudio);
            }
        }

        /// <summary>
        /// Unloads the AudioFile <paramref name="file"/> and removes it from the internal audio file list.
        /// </summary>
        /// <param name="file">The <see cref="AudioFile"/> to unload</param>
        public void UnloadFile(AudioFile file)
        {
            NCLogging.Log($"Unloading audio file {file.Name}...");

            if (!Lightning.Renderer.ContainsRenderable(file.Name))
            {
                _ = new NCException($"Attempted to load an audio file {file.Name} (path {file.Path}) that is not present in the audio files list and therefore has not been loaded!", 135,
                    "AudioManager::UnloadFile file parameter is not a loaded AudioFile present within AudioManager::AudioFiles!", NCExceptionSeverity.Warning, null, true);
                return;
            }

            file.Unload();
            Lightning.Renderer.RemoveRenderable(file);
        }
    }
}