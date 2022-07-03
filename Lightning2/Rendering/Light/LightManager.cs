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
    /// April 8, 2022 (modified April 15, 2022)
    /// 
    /// Static class that manages lights and generates a screen-space lightmap.
    /// </summary>
    public static class LightManager
    {
        /// <summary>
        /// The lights that have been loaded.
        /// </summary>
        private static List<Light> Lights { get; set; }

        internal static Texture SSMapTexture { get; private set; }

        /// <summary>
        /// The default colour of the environment.
        /// </summary>
        public static Color EnvironmentalLight { get; private set; }

        /// <summary>
        /// Internal: determines if the light manager is initialised
        /// </summary>
        internal static bool Initialised { get; private set; }

        // SIN(45)
        private const float sinus = 0.70710678118f;

        static LightManager()
        {
            Lights = new List<Light>();
        }

        public static void Init(Window Win)
        {
            // move this if it is slower
            SSMapTexture = new Texture(Win, Win.Settings.Size);
            SetEnvironmentalLightBlendMode(SDL_BlendMode.SDL_BLENDMODE_BLEND);
            // This is used so we don't build lightmaps when LightManager.Init hasn't been called
            Initialised = true;
        }

        public static void AddLight(Window win, Light Light)
        {
            if (SSMapTexture.Handle == IntPtr.Zero) new NCException("You must initialise the Light Manager before using it!", 61, "LightManager.AddLight called before LightManager.Init!", NCExceptionSeverity.FatalError);
            Light.RenderToTexture(win);
            Lights.Add(Light);
        }
        public static void SetEnvironmentalLight(Color Colour)
        {
            EnvironmentalLight = Colour;

            if (EnvironmentalLight == default(Color)) EnvironmentalLight = Color.FromArgb(255, 255, 255, 255);

            // Set all pixels in the texture to the environmental light colour.
            SSMapTexture.Clear(EnvironmentalLight);
        }

        public static void SetEnvironmentalLightBlendMode(SDL_BlendMode BlendMode) => SDL_SetTextureBlendMode(SSMapTexture.Handle, BlendMode);

        public static void RenderLightmap(Window win)
        {
            if (SSMapTexture.Handle == IntPtr.Zero) new NCException("You must initialise the Light Manager before using it!", 62, "LightManager.RenderLightmap called before LightManager.Init!", NCExceptionSeverity.FatalError);

            SSMapTexture.Unlock();
            SSMapTexture.Draw(win);
        }

        /// <summary>
        /// Internal - used by LightningGL.Shutdown
        /// </summary>
        internal static void Shutdown()
        {
            SDL_DestroyTexture(SSMapTexture.Handle);
        }
    }
}