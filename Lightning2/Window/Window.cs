﻿using NuCore.Utilities;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using static NuCore.SDL2.SDL;

namespace Lightning2
{
    /// <summary>
    /// Defines a Lightning2 Window. 
    /// </summary>
    public class Window
    {
        public WindowSettings Settings { get; set; }

        private long LastTime { get; set; }

        public double DeltaTime { get; set; }

        private long ThisTime { get; set; }

        private Stopwatch DeltaTimer { get; set; }

        internal double CurFPS { get; set; }

        internal int FrameNumber { get; private set; }

        /// <summary>
        /// The last processed SDL event. Only valid if .Update() is called.
        /// </summary>
        public SDL_Event LastEvent { get; set; }

        /// <summary>
        /// Determines if an event is waiting. 
        /// </summary>
        public bool EventWaiting { get; set; }

        public Window()
        {
            DeltaTimer = new Stopwatch();
            // Start the delta timer.
            DeltaTimer.Start();
            LastTime = 0;
            ThisTime = 0;
        }

        public void Start(WindowSettings Ws)
        {
            if (Ws == null) throw new NCException("Passed null WindowSettings to Window init method!", 7, "Window.AddWindow", NCExceptionSeverity.FatalError);

            Settings = Ws;

            Settings.WindowHandle = SDL_CreateWindow(Settings.Title, (int)Settings.Position.X, (int)Settings.Position.Y, (int)Settings.Size.X, (int)Settings.Size.Y, Settings.WindowFlags);

            if (Settings.WindowHandle == IntPtr.Zero) throw new NCException($"Failed to create Window: {SDL_GetError()}", 8, "Window.AddWindow", NCExceptionSeverity.FatalError);

            Settings.RendererHandle = SDL_CreateRenderer(Settings.WindowHandle, Settings.ID, Settings.RenderFlags);

            if (Settings.RendererHandle == IntPtr.Zero) throw new NCException($"Failed to create Renderer: {SDL_GetError()}", 9, "Window.AddWindow", NCExceptionSeverity.FatalError);
        }

        private void Update()
        {
            // Set the last frame time.
            LastTime = DeltaTimer.ElapsedMilliseconds;
            SDL_RenderClear(Settings.RendererHandle);
        }

        public bool Run()
        {
            Update();

            EventWaiting = (SDL_PollEvent(out var current_event) > 0);

            // default mainloop
            if (EventWaiting)
            {
                LastEvent = current_event;

                switch (current_event.type)
                {
                    case SDL_EventType.SDL_QUIT: // User requested a quit, so shut down
                        Lightning2.Shutdown(this);
                        return false;
                }
            }

            return true;
        }

        public void Render()
        {
            // Correctly draw the background
            SDL_SetRenderDrawColor(Settings.RendererHandle, Settings.Background.R, Settings.Background.G, Settings.Background.B, Settings.Background.A);

            // Set the current frame time.
            ThisTime = DeltaTimer.ElapsedMilliseconds;

            DeltaTime = (double)(ThisTime - LastTime) / 1000;

            CurFPS = 1 / DeltaTime;

            FrameNumber++;

            // Render all of the particle effects.
            ParticleManager.Render(this);

            // Render the lightmap.
            if (LightManager.Initialised) LightManager.RenderLightmap(this);

            // draw fps on top always (by drawing it last. we don't have zindex, but we will later). Also snap it to the screen like a hud element. 
            // check the showfps global setting first
            if (GlobalSettings.ShowFPS)
            {
                PrimitiveRenderer.DrawText(this, $"FPS: {(int)CurFPS} ({(int)(1000 / CurFPS)}ms)", new Vector2(0, 0), Color.FromArgb(255, 255, 255, 255), true);

                if (CurFPS < GlobalSettings.TargetFPS) PrimitiveRenderer.DrawText(this, $"Running under target FPS ({GlobalSettings.TargetFPS})!", new Vector2(0, 12), Color.FromArgb(255, 255, 0, 0), true);
            }

            SDL_RenderPresent(Settings.RendererHandle);
        }

        /// <summary>
        /// Internal - used as a part of Lightning2.Shutdown
        /// </summary>
        internal void Shutdown()
        {
            SDL_DestroyRenderer(Settings.RendererHandle);
            SDL_DestroyWindow(Settings.WindowHandle);
        }

        /// <summary>
        /// Clears the renderer and optionally sets the colour to the Color <paramref name="colour"/>
        /// </summary>
        /// <param name="colour"></param>
        public void Clear(Color colour = default(Color))
        {
            if (colour == default(Color))
            {
                SDL_SetRenderDrawColor(Settings.RendererHandle, 0, 0, 0, 0);
            }
            else
            {
                SDL_SetRenderDrawColor(Settings.RendererHandle, colour.R, colour.G, colour.B, colour.A);
            }

            SDL_RenderClear(Settings.RendererHandle);
            Settings.Background = colour;
        }

        /// <summary>
        /// Sets the window's current <see cref="Camera"/> to <paramref name="ncamera"/>.
        /// </summary>
        /// <param name="ncamera">The <see cref="Camera"/> instance to set the window's current camerat o</param>
        public void SetCurrentCamera(Camera ncamera)
        {
            if (ncamera.Type == CameraType.Chase)
            {
                if (ncamera.FocusDelta == default(Vector2)) ncamera.FocusDelta = new Vector2(-(Settings.Size.X / 2), 0);
            }

            Settings.Camera = ncamera;
        }
    }
}