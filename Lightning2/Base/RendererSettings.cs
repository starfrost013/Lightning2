namespace LightningGL
{
    /// <summary>
    /// RendererSettings
    /// 
    /// Defines settings for a <see cref="Renderer"/>.
    /// </summary>
    public class RendererSettings
    {
        private int MINIMUM_WINDOW_SIZE_X = 192;
        private int MINIMUM_WINDOW_SIZE_Y = 48;

        /// <summary>
        /// Backing field for <see cref="Title"/>.
        /// </summary>
        private string _title;

        /// <summary>
        /// The title of the window.
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;

                if (WindowHandle != default) SDL_SetWindowTitle(WindowHandle, _title);
            }
        }

        /// <summary>
        /// Backing field for <see cref="Position"/>.
        /// </summary>
        private Vector2 _position;

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

                if (_position.X < MINIMUM_WINDOW_SIZE_X
                    || _position.Y < MINIMUM_WINDOW_SIZE_Y
                    || _position.X > SystemInfo.ScreenResolutionX
                    || _position.Y > SystemInfo.ScreenResolutionY) _ = new NCException($"Attempted to change window to illegal position ({_position.X},{_position.Y}!). Range is 0,0 to {SystemInfo.ScreenResolutionX},{SystemInfo.ScreenResolutionY}", 118, "Set accessor of WindowSettings::Size detected an attempt to resize to an invalid window size", NCExceptionSeverity.FatalError);

                if (WindowHandle != default)
                {
                    int intPosX = Convert.ToInt32(_position.X),
                        intPosY = Convert.ToInt32(_position.Y);

                    GlobalSettings.ResolutionX = Convert.ToInt32(intPosX);
                    GlobalSettings.ResolutionY = Convert.ToInt32(intPosY);

                    SDL_SetWindowPosition(WindowHandle, Convert.ToInt32(intPosX), Convert.ToInt32(intPosY));
                }
            }
        }

        /// <summary>
        /// Backing field for <see cref="Size"/>. 
        /// </summary>
        private Vector2 _size;

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
                    || _size.Y > SystemInfo.ScreenResolutionY) _ = new NCException($"Attempted to change window to illegal resolution ({_size.X},{_size.Y}!). Range is 192,48 to {SystemInfo.ScreenResolutionX},{SystemInfo.ScreenResolutionY}", 117, "Set accessor of WindowSettings::Size detected an attempt to resize to an invalid window size", NCExceptionSeverity.FatalError);

                if (WindowHandle != default)
                {
                    int intSizeX = Convert.ToInt32(_size.X),
                        intSizeY = Convert.ToInt32(_size.Y);

                    GlobalSettings.ResolutionX = Convert.ToInt32(intSizeX);
                    GlobalSettings.ResolutionY = Convert.ToInt32(intSizeY);

                    SDL_SetWindowSize(WindowHandle, Convert.ToInt32(intSizeX), Convert.ToInt32(intSizeY));
                }
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
        /// The background color of this window.
        /// </summary>
        public Color BackgroundColor { get; internal set; }

        public RendererSettings()
        {
            // Show the window and set its color to RGBA 0,0,0,255 (solid black) by default
            WindowFlags = GlobalSettings.WindowFlags;
            Size = new Vector2(GlobalSettings.ResolutionX, GlobalSettings.ResolutionY);

            // Set a few default values.
            if (WindowFlags == 0) WindowFlags = SDL_WindowFlags.SDL_WINDOW_SHOWN;
            if (Size == default(Vector2)) Size = new Vector2(960, 640);
            if (Position == default(Vector2)) Position = new Vector2(GlobalSettings.PositionX, GlobalSettings.PositionY);
            if (Title == null) Title = GlobalSettings.WindowTitle;
            BackgroundColor = Color.FromArgb(255, 0, 0, 0);
        }
    }
}
