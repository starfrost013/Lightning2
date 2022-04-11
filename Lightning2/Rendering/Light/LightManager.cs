using NuCore.SDL2;
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

        static LightManager()
        {
            Lights = new List<Light>();
        }

        public static void Init(Window Win)
        {
            // move this if it is slower
            SSMapTexture = new Texture(Win, Win.Settings.Size.X, Win.Settings.Size.Y);
            SSMapTexture.Clear(new Color4(0, 0, 0, 255));
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

            SSMapTexture.Unlock();

            return SSMapTexture;
        }

        public static void RenderLightmap(Window Win)
        {
            if (SSMapTexture.TextureHandle == IntPtr.Zero) throw new NCException("You must initialise the Light Manager before using it!", 62, "LightManager.RenderLightmap called before LightManager.Init!", NCExceptionSeverity.FatalError);
            SSMapTexture.Draw(Win);
        }
    }
}
