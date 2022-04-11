using static NuCore.SDL2.SDL;
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
        public SDL_WindowFlags WindowFlags { get; set; }
        
        /// <summary>
        /// The SDL renderer flags of this window.
        /// </summary>
        public SDL_RendererFlags RenderFlags { get; set; }  

        /// <summary>
        /// The SDL ID of this window 
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// The native window handle for this window.
        /// </summary>
        public IntPtr WindowHandle { get; internal set; }

        /// <summary>
        /// The native renderer handle for this window.
        /// </summary>
        public IntPtr RendererHandle { get; internal set; }

        /// <summary>
        /// The current <see cref="Camera"/> used.
        /// </summary>
        public Camera Camera { get; internal set; }

        /// <summary>
        /// The background colour of this window.
        /// </summary>
        public Color4 Background { get; internal set; }

        public WindowSettings()
        {
            // Show the window by default, set its colour to RGBA 0,0,0,255 (solid black), and set the rendering blend mode to SDL_BLENDMODE_BLEND.
            WindowFlags = SDL_WindowFlags.SDL_WINDOW_SHOWN;
            Background = new Color4(0, 0, 0, 255);
        }
    }
}
