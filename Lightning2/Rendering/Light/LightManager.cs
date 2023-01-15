namespace LightningGL
{
    /// <summary>
    /// LightManager
    /// 
    /// April 8, 2022 (modified September 4, 2022)
    /// 
    /// Static class that manages lights and generates a screen-space lightmap.
    /// </summary>
    public class LightAssetManager : AssetManager<Light>
    {
        /// <summary>
        /// Internal: Texture used for rendering lights
        /// </summary>
        internal Texture? ScreenSpaceMap { get; private set; }

        /// <summary>
        /// The default color of the environment.
        /// </summary>
        public Color EnvironmentalLight { get; private set; }

        /// <summary>
        /// Internal: determines if the light manager is initialised
        /// </summary>
        internal bool Initialised { get; private set; }


        /// <summary>
        /// Initialises the Light Manager.
        /// </summary>
        /// <param name="Lightning.Renderer"></param>
        internal void Init()
        {
            if (Initialised) return; // don't initialise twice

            // move this if it is slower
            ScreenSpaceMap = new("ScreenSpaceMap", Lightning.Renderer.Settings.Size.X, Lightning.Renderer.Settings.Size.Y)
            {
                SnapToScreen = true,
                NotCullable = true,
            };

            SetEnvironmentalLightBlendMode(SDL_BlendMode.SDL_BLENDMODE_BLEND);
            // This is used so we don't build lightmaps when LightManager.Init hasn't been called
            Initialised = true;
        }

        /// <summary>
        /// Sets the environmental light color.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to set as the environmental light color.</param>
        public void SetEnvironmentalLight(Color color)
        {
            Debug.Assert(ScreenSpaceMap != null);

            if (ScreenSpaceMap.Handle == nint.Zero) NCError.ShowErrorBox("The Light Manager must be initialised before using it!", 124, NCErrorSeverity.FatalError);
            EnvironmentalLight = color;

            if (EnvironmentalLight == default(Color)) EnvironmentalLight = Color.FromArgb(255, 255, 255, 255);

            // Set all pixels in the texture to the environmental light color.
            ScreenSpaceMap.Clear(EnvironmentalLight);
        }

        /// <summary>
        /// Sets the blend mode of the environmental light.
        /// </summary>
        /// <param name="blendMode">The <see cref="SDL_BlendMode"/> of the environmental light texture to set,</param>
        public void SetEnvironmentalLightBlendMode(SDL_BlendMode blendMode)
        {
            Debug.Assert(ScreenSpaceMap != null);

            if (ScreenSpaceMap.Handle == nint.Zero) NCError.ShowErrorBox("The Light Manager must be initialised before using it!", 
                125, NCErrorSeverity.FatalError);
            SDL_SetTextureBlendMode(ScreenSpaceMap.Handle, blendMode);
        }

        /// <summary>
        /// Internal: Renders the current screen-space lightmap.
        /// </summary>
        /// <param name="Lightning.Renderer">The window to render the current screen-space light map to.</param>
        internal override void Update()
        {
            Debug.Assert(ScreenSpaceMap != null);

            if (ScreenSpaceMap.Handle == nint.Zero) NCError.ShowErrorBox("The Light Manager must be initialised before using it!",
                62, NCErrorSeverity.FatalError);
            ScreenSpaceMap.Draw();
        }

        /// <summary>
        /// Internal - used by LightningGL.Shutdown
        /// </summary>
        internal void Shutdown()
        {
            Debug.Assert(ScreenSpaceMap != null);
            SDL_DestroyTexture(ScreenSpaceMap.Handle);
        } 
    }
}