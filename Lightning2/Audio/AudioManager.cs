using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace LightningGL
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
        internal static List<AudioFile> AudioFiles { get; set; }

        static AudioManager()
        {
            AudioFiles = new List<AudioFile>();
        }

        /// <summary>
        /// Loads the audio file at path <see cref="Path"/>, if it exists.
        /// </summary>
        /// <param name="path">The path of the file to load.</param>
        /// <param name="name">A name to assign to the audio file. Optional, will be automatically generated from the file path (extension and directory removed) if not supplied.</param>
        /// <exception cref="NCException">An error occurred loading the file.</exception>
        public static void LoadFile(string path, string name = null)
        {
            if (!File.Exists(path)) if (!File.Exists(path)) _ = new NCException($"Error loading audio file: The path {path} does not exist!", 52, "AudioFile.Load", NCExceptionSeverity.FatalError);

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
                tempAudio.Channel = AudioFiles.Count;
                NCLogging.Log($"Loaded audio file at {path} to channel {tempAudio.Channel}");
                AudioFiles.Add(tempAudio);
            }
        }

        public static void UnloadFile(AudioFile file)
        {
            NCLogging.Log($"Unloading audio file {file.Name}...");
            file.Unload();
            AudioFiles.Remove(file);
        }

        /// <summary>
        /// Acquires the <see cref="AudioFile"/> with the name <see cref="Name"/>.
        /// </summary>
        /// <param name="name">The name of the audio file to obtain.</param>
        /// <returns>The first instance of <see cref="AudioFile"/> in <see cref="AudioFiles"/> with the name <see cref="Name"/>, or <c>null</c> if there is no audio file with that name.</returns>
        public static AudioFile GetFileWithName(string name)
        {
            foreach (AudioFile file in AudioFiles)
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
        /// <returns>The first instance of <see cref="AudioFile"/> in <see cref="AudioFiles"/> with the path <see cref="Name"/>, or <c>null</c> if there is no audio file with that path.</returns>
        public static AudioFile GetFileWithPath(string name)
        {
            foreach (AudioFile file in AudioFiles)
            {
                if (file.Path == name)
                {
                    return file;
                }
            }

            return null;
        }

        internal static void Update(Window cWindow)
        {
            foreach (AudioFile file in AudioFiles)
            {
                if (file.Playing) file.Update(cWindow);
            }
        }
    }
}
