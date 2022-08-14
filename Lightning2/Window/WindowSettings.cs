using System;
using System.Drawing;
using System.Numerics;
using static NuCore.SDL2.SDL;
using NuCore.Utilities;

namespace LightningGL
{
    /// <summary>
    /// WindowSettings
    /// 
    /// Defines settings for a <see cref="Window"/>.
    /// </summary>
    public class WindowSettings
    {
        /// <summary>
        /// The title of the window.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Backing field for <see cref="Position"/>.
        /// </summary>
        private Vector2 _position { get; set; }

        /// <summary>
        /// The position of this window.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;   
            }
            set
            {
                _position = value;

                if (_position.X < 0
                    || _position.Y < 0
                    || _position.X > SystemInfo.ScreenResolutionX
                    || _position.Y > SystemInfo.ScreenResolutionY) _ = new NCException($"Attempted to change window to illegal position ({_position.X},{_position.Y}!). Range is 0,0 to {SystemInfo.ScreenResolutionX},{SystemInfo.ScreenResolutionY}", 118, "Set accessor of WindowSettings::Size detected an attempt to resize to an invalid window size", NCExceptionSeverity.FatalError);

                SDL_SetWindowPosition(WindowHandle, Convert.ToInt32(_position.X), Convert.ToInt32(_position.Y));
            }
        }

        /// <summary>
        /// Backing field for <see cref="Size"/>. 
        /// </summary>
        private Vector2 _size { get; set; }

        /// <summary>
        /// The size of this window.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                // UWP but should work
                // https://stackoverflow.com/questions/42932983/minimum-size-of-a-window
                if (_size.X < 192
                    || _size.Y < 48
                    || _size.X > SystemInfo.ScreenResolutionX
                    || _size.Y > SystemInfo.ScreenResolutionY) _ = new NCException($"Attempted to change window to illegal resolution ({_size.X},{_size.Y}!). Range is 1,1 to {SystemInfo.ScreenResolutionX},{SystemInfo.ScreenResolutionY}", 117, "Set accessor of WindowSettings::Size detected an attempt to resize to an invalid window size", NCExceptionSeverity.FatalError);

                SDL_SetWindowSize(WindowHandle, Convert.ToInt32(_size.X), Convert.ToInt32(_size.Y));
            }
        }

        /// <summary>
        /// The SDL window flags of this flag.
        /// </summary>
        public SDL_WindowFlags WindowFlags { get; set; }

        /// <summary>
        /// The SDL renderer flags of this window.
        /// </summary>
        public SDL_RendererFlags RenderFlags { get; set; }

        /// <summary>
        /// The SDL window ID of this window.
        /// </summary>
        public uint ID { get; internal set; }

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
        public Color BackgroundColour { get; internal set; }

        public WindowSettings()
        {
            // Show the window by default, set its colour to RGBA 0,0,0,255 (solid black), and set the rendering blend mode to SDL_BLENDMODE_BLEND.
            WindowFlags = GlobalSettings.WindowFlags;
            Size = new Vector2(GlobalSettings.ResolutionX, GlobalSettings.ResolutionY);

            // Set a few default values.
            if (WindowFlags == 0) WindowFlags = SDL_WindowFlags.SDL_WINDOW_SHOWN;
            if (Size == default(Vector2)) Size = new Vector2(960, 640);
            if (Position == default(Vector2)) Position = new Vector2(GlobalSettings.PositionX, GlobalSettings.PositionY);
            if (Title == null) Title = GlobalSettings.WindowTitle;
            BackgroundColour = Color.FromArgb(255, 0, 0, 0);
        }
    }
}
