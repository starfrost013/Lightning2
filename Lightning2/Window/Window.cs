using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

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

        public double CurFPS { get; set; }

        public Window()
        {
            DeltaTimer = new Stopwatch();
            // Start the delta timer.
            DeltaTimer.Start();
            LastTime = 0;
            ThisTime = 0; 
        }

        public void AddWindow(WindowSettings Ws)
        {
            if (Ws == null) throw new NCException("Passed null WindowSettings to Window init method!", 7, "Window.AddWindow", NCExceptionSeverity.FatalError);

            Settings = Ws;

            Settings.WindowHandle = SDL.SDL_CreateWindow(Settings.Title, Settings.Position.X, Settings.Position.Y, Settings.Size.X, Settings.Size.Y, Settings.Flags);
            
            if (Settings.WindowHandle == IntPtr.Zero) throw new NCException($"Failed to create Window: {SDL.SDL_GetError}", 8, "Window.AddWindow", NCExceptionSeverity.FatalError);

            Settings.RendererHandle = SDL.SDL_CreateRenderer(Settings.WindowHandle, Settings.ID, Settings.RenderFlags);

            if (Settings.RendererHandle == IntPtr.Zero) throw new NCException($"Failed to create Renderer: {SDL.SDL_GetError}", 9, "Window.AddWindow", NCExceptionSeverity.FatalError);

        }

        public void Update()
        {
            // Set the last frame time.
            LastTime = DeltaTimer.ElapsedMilliseconds;
            SDL.SDL_RenderClear(Settings.RendererHandle);
        }

        public void Present()
        {
            // Set the current frame time.
            ThisTime = DeltaTimer.ElapsedMilliseconds;

            DeltaTime = (ThisTime - LastTime) / (double)1000;

            CurFPS = 1 / DeltaTime;

            SDL.SDL_RenderPresent(Settings.RendererHandle);
        }

    }
}
