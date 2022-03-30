using NuCore.SDL2;
using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics; 
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    public class WindowSettings
    {
        /// <summary>
        /// The title of the window.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The position of this window.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The size of this window.
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// The SDL window flags of this flag.
        /// </summary>
        public SDL.SDL_WindowFlags WindowFlags { get; set; }
        
        /// <summary>
        /// The SDL renderer flags of this window.
        /// </summary>
        public SDL.SDL_RendererFlags RenderFlags { get; set; }  

        /// <summary>
        /// The SDL ID of this window 
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// The window handle for this window.
        /// </summary>
        public IntPtr WindowHandle { get; internal set; }

        /// <summary>
        /// The renderer handle for this window.
        /// </summary>
        public IntPtr RendererHandle { get; internal set; }

        public WindowSettings()
        {
            WindowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN;
        }
    }
}
