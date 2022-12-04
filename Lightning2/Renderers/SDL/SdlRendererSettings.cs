
namespace LightningGL
{
    /// <summary>
    /// SdlRendererSettings
    /// 
    /// Renderer settings for the SDL renderer.
    /// </summary>
    public class SdlRendererSettings : RendererSettings
    {

        /// <summary>
        /// Backing field for <see cref="Size"/>. 
        /// </summary>
        private Vector2 _size;

        /// <summary>
        /// The size of this window.
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;

                if (WindowHandle != default)
                {
                    int intSizeX = Convert.ToInt32(_size.X),
                        intSizeY = Convert.ToInt32(_size.Y);

                    GlobalSettings.GraphicsResolutionX = Convert.ToInt32(intSizeX);
                    GlobalSettings.GraphicsResolutionY = Convert.ToInt32(intSizeY);

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
        /// The native window handle for this renderer.
        /// </summary>
        public IntPtr WindowHandle { get; internal set; }

        /// <summary>
        /// The native renderer handle for this renderer.
        /// </summary>
        public IntPtr RendererHandle { get; internal set; }

        /// <summary>
        /// Backing field for <see cref="Title"/>
        /// </summary>
        public string _title;

        /// <summary>
        /// The title of the window.
        /// </summary>
        public override string Title
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

        public SdlRendererSettings() : base()
        {
            // Show the window and set its color to RGBA 0,0,0,255 (solid black) by default
            WindowFlags = GlobalSettings.GraphicsWindowFlags;
            // Set a few default values.
            if (WindowFlags == 0) WindowFlags = SDL_WindowFlags.SDL_WINDOW_SHOWN;
        }
    }
}
