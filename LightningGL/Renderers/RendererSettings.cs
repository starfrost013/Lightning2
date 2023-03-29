namespace LightningGL
{
    /// <summary>
    /// RendererSettings
    /// 
    /// Defines settings for a <see cref="SdlRenderer"/>.
    /// </summary>
    public class RendererSettings
    {
        /// <summary>
        /// The title of the window.
        /// </summary>
        public virtual string Title { get; set; }


        /// <summary>
        /// The position of this window.
        /// </summary>
        public virtual Vector2 Position { get; set; }

        /// <summary>
        /// The size of this window.
        /// </summary>
        public virtual Vector2 Size { get; set; }

        /// <summary>
        /// The freetype flags of this window.
        /// </summary>
        public FreeTypeLibrary FreeType { get; set; }

        /// <summary>
        /// The current <see cref="Camera"/> used.
        /// </summary>
        public Camera Camera { get; internal set; }

        /// <summary>
        /// The background color of this window.
        /// </summary>
        public Color BackgroundColor { get; internal set; }

// _title is always not null because it's set in the constructor of Title and is a backing field,
// but VS doesn't like it so we just turn it off
#pragma warning disable CS8618
        public RendererSettings()
        {
            Size = new Vector2(GlobalSettings.GraphicsResolutionX, GlobalSettings.GraphicsResolutionY);
            if (Size == default) Size = new Vector2(960, 640);
            if (Position == default) Position = new Vector2(GlobalSettings.GraphicsPositionX, GlobalSettings.GraphicsPositionY);
            Title ??= GlobalSettings.GraphicsWindowTitle; // assign if null
            BackgroundColor = Color.FromArgb(255, 0, 0, 0);

            // Create a default camera.
            Camera = new Camera(CameraType.Follow); 
        }
#pragma warning restore CS8618

    }
}
