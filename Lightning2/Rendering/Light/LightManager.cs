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
    /// <summary>
    /// LightManager
    /// 
    /// April 8, 2022 (modified April 10, 2022)
    /// 
    /// Static class that manages lights and generates a screen-space lightmap.
    /// </summary>
    public static class LightManager
    {
        /// <summary>
        /// The lights that have been loaded.
        /// </summary>
        private static List<Light> Lights { get; set; } 

        private static Texture SSMapTexture { get; set; }
        
        /// <summary>
        /// The default colour of the environment.
        /// </summary>
        public static Color4 EnvironmentalLight { get; private set; }

        static LightManager()
        {
            Lights = new List<Light>();
        }

        public static void Init(Window Win)
        {
            // move this if it is slower
            SSMapTexture = new Texture(Win, Win.Settings.Size.X, Win.Settings.Size.Y);
            SDL_SetTextureBlendMode(SSMapTexture.TextureHandle, SDL_BlendMode.SDL_BLENDMODE_BLEND);
        }

        public static void AddLight(Light Light)
        {
            if (SSMapTexture.TextureHandle == IntPtr.Zero) throw new NCException("You must initialise the Light Manager before using it!", 61, "LightManager.AddLight called before LightManager.Init!", NCExceptionSeverity.FatalError);
            Lights.Add(Light);
        }


        public static Texture BuildLightmap(Window Win)
        {
            if (SSMapTexture.TextureHandle == IntPtr.Zero) throw new NCException("You must initialise the Light Manager before using it!", 60, "LightManager.BuildLightmap called before LightManager.Init!", NCExceptionSeverity.FatalError);

            Camera cur_camera = Win.Settings.Camera;

            return SSMapTexture;
        }

        public static void SetEnvironmentalLight(Window Win, Color4 Colour)
        {
            EnvironmentalLight = Colour;

            if (EnvironmentalLight == null) EnvironmentalLight = new Color4(255, 255, 255, 255);

            SSMapTexture.Clear(EnvironmentalLight);
        }

        public static void RenderLightmap(Window Win)
        {
            if (SSMapTexture.TextureHandle == IntPtr.Zero) throw new NCException("You must initialise the Light Manager before using it!", 62, "LightManager.RenderLightmap called before LightManager.Init!", NCExceptionSeverity.FatalError);

            SSMapTexture.Unlock();
            SSMapTexture.Draw(Win);
        }
    }
}
