using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using static NuCore.SDL2.SDL;

namespace LightningGL
{
    /// <summary>
    /// LightManager
    /// 
    /// April 8, 2022 (modified August 10, 2022)
    /// 
    /// Static class that manages lights and generates a screen-space lightmap.
    /// </summary>
    public static class LightManager
    {
        /// <summary>
        /// The lights that have been loaded.
        /// </summary>
        private static List<Light> Lights { get; set; }

        /// <summary>
        /// Internal: Texture used for rendering lights
        /// </summary>
        internal static Texture ScreenSpaceMap { get; private set; }

        /// <summary>
        /// The default colour of the environment.
        /// </summary>
        public static Color EnvironmentalLight { get; private set; }

        /// <summary>
        /// Internal: determines if the light manager is initialised
        /// </summary>
        internal static bool Initialised { get; private set; }

        /// <summary>
        /// Constructor for the Light Manager.
        /// </summary>
        static LightManager()
        {
            Lights = new List<Light>();
        }

        /// <summary>
        /// Initialises the Light Manager.
        /// </summary>
        /// <param name="cWindow"></param>
        internal static void Init(Window cWindow)
        {
            if (Initialised) return; // don't initialise twice
            // move this if it is slower
            ScreenSpaceMap = new Texture(cWindow, cWindow.Settings.Size.X, cWindow.Settings.Size.Y);
            SetEnvironmentalLightBlendMode(SDL_BlendMode.SDL_BLENDMODE_BLEND);
            // This is used so we don't build lightmaps when LightManager.Init hasn't been called
            Initialised = true;
        }

        /// <summary>
        /// Adds a light.
        /// </summary>
        /// <param name="window">The window to add the light to.</param>
        /// <param name="light">The <see cref="Light"/> object to add to the light manager.</param>
        public static void AddLight(Window window, Light light)
        {
            if (ScreenSpaceMap.Handle == IntPtr.Zero) _ = new NCException("The Light Manager must be initialised before using it!", 61, "LightManager::AddLight called before LightManager::Init!", NCExceptionSeverity.FatalError);
            light.RenderToTexture(window);
            Lights.Add(light);
        }

        /// <summary>
        /// Sets the environmental light colour.
        /// </summary>
        /// <param name="colour">The <see cref="Color"/> to set as the environmental light colour.</param>
        public static void SetEnvironmentalLight(Color colour)
        {
            if (ScreenSpaceMap.Handle == IntPtr.Zero) _ = new NCException("The Light Manager must be initialised before using it!", 124, "LightManager::SetEnvironmentalLight called before LightManager::Init!", NCExceptionSeverity.FatalError);
            EnvironmentalLight = colour;

            if (EnvironmentalLight == default(Color)) EnvironmentalLight = Color.FromArgb(255, 255, 255, 255);

            // Set all pixels in the texture to the environmental light colour.
            ScreenSpaceMap.Clear(EnvironmentalLight);
        }

        /// <summary>
        /// Sets the blend mode of the environmental light.
        /// </summary>
        /// <param name="blendMode">The <see cref="SDL_BlendMode"/> of the environmental light texture to set,</param>
        public static void SetEnvironmentalLightBlendMode(SDL_BlendMode blendMode)
        {
            if (ScreenSpaceMap.Handle == IntPtr.Zero) _ = new NCException("The Light Manager must be initialised before using it!", 125, "LightManager::SetEnvironmentalLightBlendMode called before LightManager::Init!", NCExceptionSeverity.FatalError);
            SDL_SetTextureBlendMode(ScreenSpaceMap.Handle, blendMode);
        }

        /// <summary>
        /// Internal: Renders the current screen-space lightmap.
        /// </summary>
        /// <param name="cWindow">The window to render the current screen-space light map to.</param>
        internal static void Render(Window cWindow)
        {
            if (ScreenSpaceMap.Handle == IntPtr.Zero) _ = new NCException("The Light Manager must be initialised before using it!", 62, "LightManager::RenderLightmap called before LightManager::Init!", NCExceptionSeverity.FatalError);

            ScreenSpaceMap.Unlock();
            ScreenSpaceMap.Draw(cWindow);
        }

        /// <summary>
        /// Internal - used by LightningGL.Shutdown
        /// </summary>
        internal static void Shutdown() => SDL_DestroyTexture(ScreenSpaceMap.Handle);
    }
}