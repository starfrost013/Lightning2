using NuCore.Utilities;
using NuCore.SDL2;
using System;

namespace Lightning2
{
    /// <summary>
    /// Lightning2 Accelerated Rendering Pipeline
    /// 
    /// © 2022 starfrost
    /// </summary>
    public class Lightning2
    {
        public static void Init()
        {
            Init_InitLogging();
            NCLogging.Log("Initialising SDL...");

            if (SDL.SDL_Init(SDL.SDL_InitFlags.SDL_INIT_EVERYTHING) < 0) throw new NCException($"Error initialising SDL2: {SDL.SDL_GetError()}", 0, "Lightning2.Init();", NCExceptionSeverity.FatalError);

            NCLogging.Log("Initialising SDL_image...");
            if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_EVERYTHING) < 0) throw new NCException($"Error initialising SDL2_image: {SDL.SDL_GetError()}", 1, "Lightning2.Init();", NCExceptionSeverity.FatalError);

            NCLogging.Log("Initialising SDL_ttf...");
            if (SDL_ttf.TTF_Init() < 0) throw new NCException($"Error initialising SDL2_ttf: {SDL.SDL_GetError()}", 2, "Lightning2.Init();", NCExceptionSeverity.FatalError);

            NCLogging.Log("Initialising SDL_mixer...");
            if (SDL_mixer.Mix_Init(SDL_mixer.MIX_InitFlags.MIX_INIT_EVERYTHING) < 0) throw new NCException($"Error initialising SDL2_mixer: {SDL.SDL_GetError()}", 3, "Lightning2.Init();", NCExceptionSeverity.FatalError);

        }

        private static void Init_InitLogging()
        {
            NCLogging.Settings = new NCLoggingSettings();
            NCLogging.Settings.WriteToLog = true;
            NCLogging.Init();
        }
    }
}
