﻿using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using static NuCore.SDL2.SDL;

namespace LightningGL
{
    /// <summary>
    /// Defines a LightningGL Window. 
    /// </summary>
    public class Window
    {
        /// <summary>
        /// The settings of this window - see <see cref="WindowSettings"/>.
        /// </summary>
        public WindowSettings Settings { get; internal set; }

        private long LastTime { get; set; }

        private long ThisTime { get; set; }

        /// <summary>
        /// Delta-time used for ensuring objects move at the same speed regardless of framerate.
        /// </summary>
        public double DeltaTime { get; set; }

        private Stopwatch FrameTimer { get; set; }

        /// <summary>
        /// Internal: The current rate of frames rendered per second.
        /// </summary>
        internal double CurFPS { get; private set; }

        /// <summary>
        /// Internal: The number of frames since the engine started rendering. 
        /// </summary>
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
            FrameTimer = new Stopwatch();
            // Start the delta timer.
            FrameTimer.Start();
            LastTime = 0;
            ThisTime = 0;
        }

        public void Start(WindowSettings windowSettings)
        {
            // Check that the engine has been started.
            if (!Lightning.Initialised) _ = new NCException("You cannot start a window without initialising the engine - call Lightning::Init first!", 0x0001DEAD, "Window::Start called before Lightning::Init!", NCExceptionSeverity.FatalError);

            if (windowSettings == null) _ = new NCException("Passed null WindowSettings to Window init method!", 7, "Window::AddWindow windowSettings parameter null", NCExceptionSeverity.FatalError);

            Settings = windowSettings;

            // localise the window title
            Settings.Title = LocalisationManager.ProcessString(Settings.Title);

            // Create the window,
            Settings.WindowHandle = SDL_CreateWindow(Settings.Title, (int)Settings.Position.X, (int)Settings.Position.Y, (int)Settings.Size.X, (int)Settings.Size.Y, Settings.WindowFlags);

            if (Settings.WindowHandle == IntPtr.Zero) _ = new NCException($"Failed to create Window: {SDL_GetError()}", 8, "Window::AddWindow - SDL_CreateWindow failed to create window", NCExceptionSeverity.FatalError);

            // set the window ID 
            Settings.ID = SDL_GetWindowID(Settings.WindowHandle);

            // Create the renderer.
            Settings.RendererHandle = SDL_CreateRenderer(Settings.WindowHandle, (int)Settings.ID, Settings.RenderFlags);

            if (Settings.Camera == null)
            {
                Camera camera = new Camera(CameraType.Follow);
                Settings.Camera = camera;
            }

            if (Settings.RendererHandle == IntPtr.Zero) _ = new NCException($"Failed to create Renderer: {SDL_GetError()}", 9, "Window::AddWindow - SDL_CreateRenderer failed to create renderer", NCExceptionSeverity.FatalError);
        }

        private void Update()
        {
            // Set the last frame time.
            LastTime = FrameTimer.ElapsedTicks;
            FrameTimer.Restart();
            SDL_RenderClear(Settings.RendererHandle);
        }

        public bool Run()
        {
            Update();

            EventWaiting = (SDL_PollEvent(out var currentEvent) > 0);

            // default mainloop
            if (EventWaiting)
            {
                LastEvent = currentEvent;

                // default rendering loop.
                // Developers can choose to handle SDL events after this
                switch (currentEvent.type)
                {
                    case SDL_EventType.SDL_KEYDOWN:
                        Key curKey = (Key)currentEvent.key;

                        // Show a basic about screen on Shift-F9 (KMOD_SHIFT only triggers when both shift keys are held)
                        string curKeyString = curKey.KeySym.ToString();

                        if (curKeyString == "F9"
                            && (curKey.Modifiers.HasFlag(SDL_Keymod.KMOD_LSHIFT) 
                            || curKey.Modifiers.HasFlag(SDL_Keymod.KMOD_RSHIFT))
                            && GlobalSettings.EngineAboutScreenOnShiftF9) ShowEngineAboutScreen();

                        UIManager.KeyPressed(curKey);
                        return true; 
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN: // Mouse down event
                        UIManager.MousePressed(this, currentEvent.button.button, new Vector2(currentEvent.button.x, currentEvent.button.y));
                        return true;
                    case SDL_EventType.SDL_MOUSEBUTTONUP: // Mouse up event
                        UIManager.MouseReleased(this, currentEvent.button.button, new Vector2(currentEvent.button.x, currentEvent.button.y));
                        return true;
                    case SDL_EventType.SDL_MOUSEMOTION: // Mouse move event
                        UIManager.MouseMove(this, new Vector2(currentEvent.motion.x, currentEvent.motion.y),
                            new Vector2(currentEvent.motion.xrel, currentEvent.motion.yrel),
                            (SDL_MouseButton)currentEvent.motion.state);
                        return true; 
                    case SDL_EventType.SDL_WINDOWEVENT: // Window Event - check subtypes
                        switch (currentEvent.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
                                UIManager.MouseEnter(this);
                                return true;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
                                UIManager.MouseLeave(this);
                                return true;
                        }

                        return true; 
                    case SDL_EventType.SDL_QUIT: // User requested a quit, so shut down
                        Lightning.Shutdown(this);
                        return false;
                }
            }

            return true;
        }

        public void Render()
        {

            // Call all render events
            UIManager.Render(this);

            // Render all textures.
            TextureManager.Render(this);

            // Render all of the particle effects.
            ParticleManager.Render(this);

            // Render the lightmap.
            if (LightManager.Initialised) LightManager.RenderLightmap(this);

            // Render the UI.
            UIManager.Render(this);

            // Render audio.
            AudioManager.Update(this);

            // draw fps on top always (by drawing it last. we don't have zindex, but we will later). Also snap it to the screen like a hud element. 
            // check the showfps global setting first
            // do this BEFORE present. then measure frametime in Render_MeasureFps, this makes it accurate.
            if (GlobalSettings.ShowFPS) Render_DrawDebugInformation();

            // Correctly draw the background
            SDL_SetRenderDrawColor(Settings.RendererHandle, Settings.Background.R, Settings.Background.G, Settings.Background.B, Settings.Background.A);

            SDL_RenderPresent(Settings.RendererHandle);

            int maxFps = GlobalSettings.MaxFPS;

            // Delay for frame limiter
            if (maxFps > 0)
            {
                double targetFrameTime = (1000) / (double)maxFps;
                double actualFrameTime = DeltaTime;

                double delayTime = targetFrameTime - actualFrameTime;

                if (delayTime > 0) SDL_Delay((uint)delayTime);
            }

            // Update the internal FPS values.
            Render_UpdateFps();
        }

        private void Render_DrawDebugInformation()
        {
            int currentY = (int)Settings.Camera.Position.Y;
            PrimitiveRenderer.DrawText(this, $"FPS: {CurFPS.ToString("F1")} ({DeltaTime.ToString("F2")}ms)", new Vector2(Settings.Camera.Position.X, currentY), Color.FromArgb(255, 255, 255, 255), true);

            if (CurFPS < GlobalSettings.MaxFPS)
            {
                currentY += 12;

                int maxFps = GlobalSettings.MaxFPS;

                if (maxFps == 0) maxFps = 60;

                PrimitiveRenderer.DrawText(this, $"Running under target FPS ({maxFps})!", new Vector2(Settings.Camera.Position.X, currentY), Color.FromArgb(255, 255, 0, 0), true);
            }

            currentY += 12;
            PrimitiveRenderer.DrawText(this, FrameNumber.ToString(), new Vector2(Settings.Camera.Position.X, currentY), Color.FromArgb(255, 255, 255, 255), true);
        }

        private void Render_UpdateFps()
        {
            // Set the current frame time.
            ThisTime = FrameTimer.ElapsedTicks;

            CurFPS = 10000000 / ThisTime;

            DeltaTime = ((double)ThisTime / 10000);

            if (GlobalSettings.ProfilePerf) PerformanceProfiler.Update(this);
            FrameNumber++;
        }
        
        /// <summary>
        /// Sets up a simple message box displaying engine version information.
        /// </summary>
        private void ShowEngineAboutScreen()
        {
            NCMessageBox messageBox = new()
            {
                Message = $"Powered by LightningGL\n" +
                $"Version {L2Version.LIGHTNING_VERSION_EXTENDED_STRING}",
                Title = "About",
                Icon = SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION
            };

            messageBox.AddButton("OK", SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT | SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT);
            messageBox.Show();
        }

        /// <summary>
        /// Internal - used as a part of LightningGL.Shutdown
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
            // default(Color) is 0,0,0,0, no special case code needed
            SDL_SetRenderDrawColor(Settings.RendererHandle, colour.R, colour.G, colour.B, colour.A);
            SDL_RenderClear(Settings.RendererHandle);
            Settings.Background = colour;
        }

        /// <summary>
        /// Sets the window's current <see cref="Camera"/> to <paramref name="nCamera"/>.
        /// </summary>
        /// <param name="nCamera">The <see cref="Camera"/> instance to set the window's current camerat o</param>
        public void SetCurrentCamera(Camera nCamera)
        {
            if (nCamera.Type == CameraType.Chase)
            {
                if (nCamera.FocusDelta == default(Vector2)) nCamera.FocusDelta = new Vector2(-(Settings.Size.X / 2), 0);
            }

            Settings.Camera = nCamera;
        }
    }
}