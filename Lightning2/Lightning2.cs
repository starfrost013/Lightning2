using NuCore.SDL2;
using NuCore.Utilities;
using System.Collections.Generic;
using System.Globalization;

namespace Lightning2
{
    /// <summary>
    /// Lightning2 
    /// 
    /// © 2022 starfrost
    /// </summary>
    public class Lightning2
    {
        public static void Init()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Init_InitLogging();
            NCLogging.Log($"Lightning2 {L2Version.LIGHTNING2_VERSION_EXTENDED_STRING}");
            NCLogging.Log("Initialising SDL...");

            if (SDL.SDL_Init(SDL.SDL_InitFlags.SDL_INIT_EVERYTHING) < 0) throw new NCException($"Error initialising SDL2: {SDL.SDL_GetError()}", 0, "Lightning2.Init();", NCExceptionSeverity.FatalError);

            NCLogging.Log("Initialising SDL_image...");
            if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_EVERYTHING) < 0) throw new NCException($"Error initialising SDL2_image: {SDL.SDL_GetError()}", 1, "Lightning2.Init();", NCExceptionSeverity.FatalError);

            NCLogging.Log("Initialising SDL_ttf...");
            if (SDL_ttf.TTF_Init() < 0) throw new NCException($"Error initialising SDL2_ttf: {SDL.SDL_GetError()}", 2, "Lightning2.Init();", NCExceptionSeverity.FatalError);

            NCLogging.Log("Initialising SDL_mixer...");
            if (SDL_mixer.Mix_Init(SDL_mixer.MIX_InitFlags.MIX_INIT_EVERYTHING) < 0) throw new NCException($"Error initialising SDL2_mixer: {SDL.SDL_GetError()}", 3, "Lightning2.Init();", NCExceptionSeverity.FatalError);

            NCLogging.Log("Initialising audio device...");
            if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.Mix_AudioFormat.MIX_DEFAULT_FORMAT, 2, 2048) < 0) throw new NCException($"Error initialising audio device: {SDL.SDL_GetError()}", 56, "Lightning2.Init();", NCExceptionSeverity.FatalError);

            NCLogging.Log("Loading Engine.ini...");
            GlobalSettings.Load();

            NCLogging.Log("Loading localisation...");
            LocalisationManager.Load();
        }

        private static void Init_InitLogging()
        {
            NCLogging.Settings = new NCLoggingSettings();
            NCLogging.Settings.WriteToLog = true;
            NCLogging.Init();
        }

        public static void Shutdown(Window Win)
        {
            NCLogging.Log("Shutdown requested. Destroying renderer and window...");
            Win.Shutdown();

            // create a list of fonts and audiofiles to unload
            // just foreaching through each font and audiofile doesn't work as collection is being modified 
            List<Font> fonts_to_unload = new List<Font>();
            List<AudioFile> audio_files_to_unload = new List<AudioFile>();

            foreach (Font font_to_unload in TextManager.Fonts) fonts_to_unload.Add(font_to_unload);
            foreach (AudioFile audio_file_to_unload in AudioManager.AudioFiles) audio_files_to_unload.Add(audio_file_to_unload);

            NCLogging.Log("Unloading fonts...");
            foreach (Font font_to_unload in fonts_to_unload) TextManager.UnloadFont(font_to_unload);

            NCLogging.Log("Unloading loaded audio...");
            foreach (AudioFile audio_file_to_unload in audio_files_to_unload) AudioManager.UnloadFile(audio_file_to_unload);

            // Shut down the light manager if it has been started.
            NCLogging.Log("Shutting down the Light Manager...");
            if (LightManager.Initialised) LightManager.Shutdown();

            // Shut everything down in reverse order.

            NCLogging.Log("Shutting down SDL_ttf...");
            SDL_ttf.TTF_Quit();

            NCLogging.Log("Shutting down SDL_mixer...");
            SDL_mixer.Mix_Quit();

            NCLogging.Log("Shutting down SDL_image..");
            SDL_image.IMG_Quit();

            NCLogging.Log("Shutting down SDL...");
            SDL.SDL_Quit();
        }
    }
}