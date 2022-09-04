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
        public override void AddAsset(Window cWindow, AudioFile asset)
        {
            LoadFile(asset.Path, asset.Name);
        }

        /// <summary>
        /// Loads the audio file at path <see cref="Path"/>, if it exists.
        /// </summary>
        /// <param name="path">The path of the file to load.</param>
        /// <param name="name">A name to assign to the audio file. Optional, will be automatically generated from the file path (extension and directory removed) if not supplied.</param>
        /// <exception cref="NCException">An error occurred loading the file.</exception>
        public void LoadFile(string path, string name = null)
        {
            if (!File.Exists(path)) _ = new NCException($"Error loading audio file: The path {path} does not exist!", 52, "AudioManager::Load path parameter does not exist!", NCExceptionSeverity.FatalError);

            AudioFile tempAudio = new AudioFile();

            tempAudio.Path = path;

            // remove the extension and path from the name 

            string[] pathExtensionSplit = path.Split('.');

            if (name == null)
            {
                if (pathExtensionSplit.Length != 0)
                {
                    tempAudio.Name = pathExtensionSplit[0];
                }

                string[] pathDirectorySplit = tempAudio.Name.Split(Path.DirectorySeparatorChar);

                if (pathDirectorySplit.Length == 0)
                {
                    tempAudio.Name = path;
                }
                else
                {
                    tempAudio.Name = pathDirectorySplit[pathDirectorySplit.Length - 1];
                }

                string fileExtension = pathExtensionSplit[1];

                tempAudio.Path = Path.Combine(pathDirectorySplit);
                tempAudio.Path = $"{tempAudio.Path}.{fileExtension}";
            }
            else
            {
                tempAudio.Name = name;
            }

            tempAudio.Load();

            if (tempAudio.AudioHandle != IntPtr.Zero)
            {
                tempAudio.Channel = Assets.Count;
                NCLogging.Log($"Loaded audio file at {path} to channel {tempAudio.Channel}");
                Assets.Add(tempAudio);
            }
        }

        /// <summary>
        /// Unloads the AudioFile <paramref name="file"/> and removes it from the internal audio file list.
        /// </summary>
        /// <param name="file">The <see cref="AudioFile"/> to unload</param>
        public void UnloadFile(AudioFile file)
        {
            NCLogging.Log($"Unloading audio file {file.Name}...");

            if (!Assets.Contains(file))
            {
                _ = new NCException($"Attempted to load an audio file {file.Name} (path {file.Path}) that is not present in the audio files list and therefore has not been loaded!", 135,
                    "AudioManager::UnloadFile file parameter is not a loaded AudioFile present within AudioManager::AudioFiles!", NCExceptionSeverity.Warning, null, true);
                return;
            }

            file.Unload();
            Assets.Remove(file);
        }

        /// <summary>
        /// Acquires the <see cref="AudioFile"/> with the name <see cref="Name"/>.
        /// </summary>
        /// <param name="name">The name of the audio file to obtain.</param>
        /// <returns>The first instance of <see cref="AudioFile"/> in <see cref="AudioFiles"/> with the name <see cref="Name"/>, or <c>null</c> if there is no audio file with that name.</returns>
        public AudioFile GetFileWithName(string name)
        {
            foreach (AudioFile file in Assets)
            {
                if (file.Name == name)
                {
                    return file;
                }
            }

            return null;
        }

        /// <summary>
        /// Acquires the <see cref="AudioFile"/> with path <see cref="Path"/>.
        /// </summary>
        /// <param name="name">The path of the audio file to obtain.</param>
        /// <returns>The first instance of <see cref="AudioFile"/> in <see cref="AudioFiles"/> with the path <paramref name="name"/>, or <c>null</c> if there is no audio file with that path.</returns>
        public AudioFile GetFileWithPath(string name)
        {
            foreach (AudioFile file in Assets)
            {
                if (file.Path == name)
                {
                    return file;
                }
            }

            return null;
        }

        /// <summary>
        /// Internal: Updates all loaded and playing audio files.
        /// </summary>
        /// <param name="cWindow">The window to update audio on.</param>
        internal void Update(Window cWindow)
        {
            foreach (AudioFile file in Assets)
            {
                if (file.Playing) file.Update(cWindow);
            }
        }
    }
}