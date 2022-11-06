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
        // set in constructor (see constructor), no method called by the construct
#pragma warning disable CS8618
        /// <summary>
        /// Internal: Texture used for rendering lights
        /// </summary>
        internal Texture ScreenSpaceMap { get; private set; }
#pragma warning restore CS8618

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
        /// <param name="cRenderer"></param>
        internal void Init(Renderer cRenderer)
        {
            if (Initialised) return; // don't initialise twice

            // move this if it is slower
            ScreenSpaceMap = new(cRenderer, cRenderer.Settings.Size.X, cRenderer.Settings.Size.Y)
            {
                SnapToScreen = true
            };

            SetEnvironmentalLightBlendMode(SDL_BlendMode.SDL_BLENDMODE_BLEND);
            // This is used so we don't build lightmaps when LightManager.Init hasn't been called
            Initialised = true;
        }


        /// <summary>
        /// Adds a light.
        /// </summary>
        /// <param name="window">The window to add the light to.</param>
        /// <param name="asset">The <see cref="Light"/> object to add to the light manager.</param>
        public override Light AddAsset(Renderer window, Light asset)
        {
            if (ScreenSpaceMap.Handle == IntPtr.Zero) _ = new NCException("The Light Manager must be initialised before using it!", 61, "LightManager::AddLight called before LightManager::Init!", NCExceptionSeverity.FatalError);
            asset.RenderToTexture(window);
            Assets.Add(asset);
            return asset;
        }

        /// <summary>
        /// Removes a light.
        /// </summary>
        /// <param name="cRenderer"></param>
        /// <param name="asset"></param>
        public override void RemoveAsset(Renderer cRenderer,
            Light asset)
        {
            asset.RemoveFromTexture(cRenderer);
            Assets.Remove(asset);
        }

        /// <summary>
        /// Sets the environmental light color.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to set as the environmental light color.</param>
        public void SetEnvironmentalLight(Color color)
        {
            if (ScreenSpaceMap.Handle == IntPtr.Zero) _ = new NCException("The Light Manager must be initialised before using it!", 124, "LightManager::SetEnvironmentalLight called before LightManager::Init!", NCExceptionSeverity.FatalError);
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
            if (ScreenSpaceMap.Handle == IntPtr.Zero) _ = new NCException("The Light Manager must be initialised before using it!", 
                125, "LightManager::SetEnvironmentalLightBlendMode called before LightManager::Init!", NCExceptionSeverity.FatalError);
            SDL_SetTextureBlendMode(ScreenSpaceMap.Handle, blendMode);
        }

        /// <summary>
        /// Internal: Renders the current screen-space lightmap.
        /// </summary>
        /// <param name="cRenderer">The window to render the current screen-space light map to.</param>
        internal override void Update(Renderer cRenderer)
        {
            if (ScreenSpaceMap.Handle == IntPtr.Zero) _ = new NCException("The Light Manager must be initialised before using it!",
                62, "LightManager::RenderLightmap called before LightManager::Init!", NCExceptionSeverity.FatalError);
            ScreenSpaceMap.Draw(cRenderer);
        }

        /// <summary>
        /// Internal - used by LightningGL.Shutdown
        /// </summary>
        internal void Shutdown() => SDL_DestroyTexture(ScreenSpaceMap.Handle);
    }
}