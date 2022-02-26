using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// Defines a Lightning2 Window. 
    /// </summary>
    public class Window
    {
        public WindowSettings Settings { get; set; }

        public void AddWindow(WindowSettings Ws)
        {
            if (Ws == null) throw new NCException("Passed null WindowSettings to Window init method!", 7, "Window.AddWindow", NCExceptionSeverity.FatalError);

            Settings = Ws;

            Settings.WindowHandle = SDL.SDL_CreateWindow(Settings.Title, Settings.Position.X, Settings.Position.Y, Settings.Size.X, Settings.Size.Y, Settings.Flags);
            
            if (Settings.WindowHandle == IntPtr.Zero) throw new NCException($"Failed to create Window: {SDL.SDL_GetError}", 8, "Window.AddWindow", NCExceptionSeverity.FatalError);

            Settings.RendererHandle = SDL.SDL_CreateRenderer(Settings.WindowHandle, Settings.ID, Settings.RenderFlags);

            if (Settings.RendererHandle == IntPtr.Zero) throw new NCException($"Failed to create Renderer: {SDL.SDL_GetError}", 9, "Window.AddWindow", NCExceptionSeverity.FatalError);


        }

        public void Update() => SDL.SDL_RenderClear(Settings.RendererHandle);

        public void Present() => SDL.SDL_RenderPresent(Settings.RendererHandle);
    }
}
