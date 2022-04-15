using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lightning2
{
    /// <summary>
    /// AudioManager
    /// 
    /// March 24, 2022
    /// 
    /// Defines APIs for playing an arbitrary number of audio files at once.
    /// </summary>
    public static class AudioManager
    {
        public static List<AudioFile> AudioFiles { get; set; }

        static AudioManager()
        {
            AudioFiles = new List<AudioFile>();
        }

        /// <summary>
        /// Loads the audio file at path <see cref="Path"/>, if it exists.
        /// </summary>
        /// <param name="FPath">The path of the file to load.</param>
        /// <param name="Name">A name to assign to the audio file. Optional, will be automatically generated from the file path (extension and directory removed) if not supplied.</param>
        /// <exception cref="NCException">An error occurred loading the file.</exception>
        public static void LoadFile(string FPath, string Name = null)
        {
            if (!File.Exists(FPath)) if (!File.Exists(FPath)) throw new NCException($"Error loading audio file: The path {FPath} does not exist!", 52, "AudioFile.Load", NCExceptionSeverity.FatalError);

            AudioFile temp_audio = new AudioFile();

            temp_audio.Path = FPath;

            // remove the extension and path from the name 

            string[] path_extension_split = FPath.Split('.');

            if (Name == null)
            {
                if (path_extension_split.Length != 0)
                {
                    temp_audio.Name = path_extension_split[0];
                }

                string[] path_dir_split = temp_audio.Name.Split(Path.DirectorySeparatorChar);

                if (path_dir_split.Length == 0)
                {
                    temp_audio.Name = FPath;
                }
                else
                {
                    temp_audio.Name = path_dir_split[path_dir_split.Length - 1];
                }

                string file_extension = path_extension_split[1];

                temp_audio.Path = Path.Combine(path_dir_split);
                temp_audio.Path = $"{temp_audio.Path}.{file_extension}";
            }
            else
            {
                temp_audio.Name = Name;
            }

            temp_audio.Load();

            if (temp_audio.AudioHandle != IntPtr.Zero)
            {
                temp_audio.Channel = AudioFiles.Count;
                NCLogging.Log($"Loaded audio file at {FPath} to channel {temp_audio.Channel}");
                AudioFiles.Add(temp_audio);
            }
        }

        public static void UnloadFile(AudioFile File)
        {
            NCLogging.Log($"Unloading audio file {File.Name}...");
            File.Unload();
            AudioFiles.Remove(File);
        }

        /// <summary>
        /// Acquires the <see cref="AudioFile"/> with the name <see cref="Name"/>.
        /// </summary>
        /// <param name="Name">The name of the audio file to obtain.</param>
        /// <returns>The first instance of <see cref="AudioFile"/> in <see cref="AudioFiles"/> with the name <see cref="Name"/>, or <c>null</c> if there is no audio file with that name.</returns>
        public static AudioFile GetFileWithName(string Name)
        {
            foreach (AudioFile file in AudioFiles)
            {
                if (file.Name == Name)
                {
                    return file;
                }
            }

            return null;
        }

        /// <summary>
        /// Acquires the <see cref="AudioFile"/> with path <see cref="Path"/>.
        /// </summary>
        /// <param name="Name">The path of the audio file to obtain.</param>
        /// <returns>The first instance of <see cref="AudioFile"/> in <see cref="AudioFiles"/> with the path <see cref="Name"/>, or <c>null</c> if there is no audio file with that path.</returns>
        public static AudioFile GetFileWithPath(string Name)
        {
            foreach (AudioFile file in AudioFiles)
            {
                if (file.Path == Name)
                {
                    return file;
                }
            }

            return null;
        }
    }
}
