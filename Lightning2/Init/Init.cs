using LightningPackager;
using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LightningGL
{
    /// <summary>
    /// LightningGL
    /// A lightweight, easy-to-use, and elegantly designed C# game framework
    /// 
    /// © 2022 starfrost
    /// </summary>
    public class Lightning
    {
        /// <summary>
        /// Determines if the engine has been initialised correctly.
        /// </summary>
        public static bool Initialised { get; private set; }

        public static void Init(string[] args)
        {
            try
            {
                // Set culture to invariant so things like different decimal symbols don't choke
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                Init_InitLogging();

                // Log the sign-on message
                NCLogging.Log($"LightningGL {L2Version.LIGHTNING_VERSION_EXTENDED_STRING}");

                NCLogging.Log("Parsing command-line arguments...");
                if (!InitSettings.Parse(args)) _ = new NCException($"An error occurred while parsing command-line arguments.", 103, "InitSettings::Parse returned false", NCExceptionSeverity.FatalError);

                if (InitSettings.PackageFile != null)
                {
                    NCLogging.Log($"User specified package file {InitSettings.PackageFile} to load, loading it...");
                    
                    if (!Packager.LoadPackage(InitSettings.PackageFile, InitSettings.ContentFolder)) _ = new NCException($"An error occurred loading {InitSettings.PackageFile}. The game cannot be loaded.", 104, "Packager::LoadPackager returned false", NCExceptionSeverity.FatalError);
                }

                NCLogging.Log("Initialising SDL...");
                if (SDL.SDL_Init(SDL.SDL_InitFlags.SDL_INIT_EVERYTHING) < 0) _ = new NCException($"Error initialising SDL2: {SDL.SDL_GetError()}", 0, "Lightning::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising SDL_image...");
                if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_EVERYTHING) < 0) _ = new NCException($"Error initialising SDL2_image: {SDL.SDL_GetError()}", 1, "LightningGL::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising SDL_ttf...");
                if (SDL_ttf.TTF_Init() < 0) _ = new NCException($"Error initialising SDL2_ttf: {SDL.SDL_GetError()}", 2, "LightningGL.Init();", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising SDL_mixer...");
                if (SDL_mixer.Mix_Init(SDL_mixer.MIX_InitFlags.MIX_INIT_EVERYTHING) < 0) _ = new NCException($"Error initialising SDL2_mixer: {SDL.SDL_GetError()}", 3, "LightningGL::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Initialising audio device...");
                if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.Mix_AudioFormat.MIX_DEFAULT_FORMAT, 2, 2048) < 0) _ = new NCException($"Error initialising audio device: {SDL.SDL_GetError()}", 56, "LightningGL::Init", NCExceptionSeverity.FatalError);

                NCLogging.Log("Obtaining system information...");
                SystemInfo.Load();

                NCLogging.Log("Loading Engine.ini...");
                GlobalSettings.Load();

                NCLogging.Log("Validating system requirements...");
                GlobalSettings.Validate();

                NCLogging.Log("Initialising LocalisationManager...");
                LocalisationManager.Load();

                if (GlobalSettings.ProfilePerf)
                {
                    NCLogging.Log("Performance Profiler enabled, initialising profiler...");
                    PerformanceProfiler.Start();
                }

                if (GlobalSettings.LocalSettingsPath != null)
                {
                    NCLogging.Log($"Loading local settings from {GlobalSettings.LocalSettingsPath}");
                    LocalSettings.Load();
                }

                Initialised = true;
            }
            catch (Exception err)
            {
                _ = new NCException($"An unknown fatal error occurred during engine initialisation. The installation may be corrupted", 0x0000DEAD, "Fatal error occurred in LightningGL::Init!", NCExceptionSeverity.FatalError, err);
            }
        }

        private static void Init_InitLogging()
        {
            NCLogging.Settings = new NCLoggingSettings();
            NCLogging.Settings.WriteToLog = true;
            NCLogging.Init();
        }

        public static void Shutdown(Window cWindow)
        {
            if (!Initialised) _ = new NCException("Attempted to shutdown without starting! Please call LightningGL::Init!", 95, "LightningGL::Initialised false when calling Lightning2::Shutdown", NCExceptionSeverity.FatalError);
            NCLogging.Log("Shutdown requested.");

            NCLogging.Log("Calling UI shutdown events...");
            UIManager.Shutdown(cWindow);

            if (GlobalSettings.ProfilePerf)
            {
                NCLogging.Log("Shutting down performance profiler...");
                PerformanceProfiler.Shutdown();
            }

            NCLogging.Log("Destroying window and renderer...");
            cWindow.Shutdown();

            // create a list of fonts and audiofiles to unload
            // just foreaching through each font and audiofile doesn't work as collection is being modified 
            List<Font> fontsToUnload = new List<Font>();
            List<AudioFile> audioFilesToUnload = new List<AudioFile>();

            foreach (Font fontToUnload in FontManager.Fonts) fontsToUnload.Add(fontToUnload);
            foreach (AudioFile audioFileToUnload in AudioManager.AudioFiles) audioFilesToUnload.Add(audioFileToUnload);

            NCLogging.Log("Unloading fonts...");
            foreach (Font fontToUnload in fontsToUnload) FontManager.UnloadFont(fontToUnload);

            NCLogging.Log("Unloading audio...");
            foreach (AudioFile audioFileToUnload in audioFilesToUnload) AudioManager.UnloadFile(audioFileToUnload);

            // Shut down the light manager if it has been started.
            NCLogging.Log("Shutting down the Light Manager...");
            if (LightManager.Initialised) LightManager.Shutdown();

            // Shut down the particle manager.
            NCLogging.Log("Shutting down the Particle Manager...");
            ParticleManager.Shutdown();

            // Clear up any unpacked package data if Engine.ini specifies such
            NCLogging.Log("Cleaning up loaded package files, if any...");
            Packager.Shutdown(GlobalSettings.DeleteUnpackedFilesOnExit);

            // Shut all SDL subsystems down in reverse order.
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