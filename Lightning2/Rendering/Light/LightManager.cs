using static NuCore.SDL2.SDL;
using static NuCore.SDL2.SDL_gfx;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        internal static bool Initialised { get; private set; }

        static LightManager()
        {
            Lights = new List<Light>();
        }

        public static void Init(Window Win)
        {
            // move this if it is slower
            SSMapTexture = new Texture(Win, Win.Settings.Size.X, Win.Settings.Size.Y);
            SDL_SetTextureBlendMode(SSMapTexture.TextureHandle, SDL_BlendMode.SDL_BLENDMODE_BLEND);

            // This is used so we don't build lightmaps when LightManager.Init hasn't been called
            Initialised = true; 
        }

        public static void AddLight(Light Light)
        {
            if (SSMapTexture.TextureHandle == IntPtr.Zero) throw new NCException("You must initialise the Light Manager before using it!", 61, "LightManager.AddLight called before LightManager.Init!", NCExceptionSeverity.FatalError);
            Lights.Add(Light);
        }

        public static Texture BuildLightmap(Window Win)
        {
            if (SSMapTexture.TextureHandle == IntPtr.Zero) throw new NCException("You must initialise the Light Manager before using it!", 60, "LightManager.BuildLightmap called before LightManager.Init!", NCExceptionSeverity.FatalError); 

            SSMapTexture.Clear(EnvironmentalLight);

            Camera cur_camera = Win.Settings.Camera;

            Vector2 light_position = default(Vector2);

            foreach (Light light in Lights)
            {
                light_position = new Vector2(light.Position.X, light.Position.Y);

                if (!light.SnapToScreen)
                {
                    light_position.X -= cur_camera.Position.X;
                    light_position.Y -= cur_camera.Position.Y;
                }

                RenderLight(light, (int)(light.Brightness * 40));
            }

            return SSMapTexture;
        }

        private static void RenderLight(Light Light, int r)
        {
            // Code nicked and modified from
            // https://stackoverflow.com/questions/10878209/midpoint-circle-algorithm-for-filled-circles

            // This here is sin(45) but i just hard-coded it.
            float sinus = 0.70710678118f;
            // This is the distance on the axis from sin(90) to sin(45). 

            int range =  (int)(r * sinus);

            int x = (int)Light.Position.X;
            int y = (int)Light.Position.Y;

            float max_size_x = SSMapTexture.Size.X;
            float max_size_y = SSMapTexture.Size.Y;

            Color4 transparent = new Color4(EnvironmentalLight.R, EnvironmentalLight.G, EnvironmentalLight.B, 0);

            // To fill the circle we draw the circumscribed square.

            for (int cur_x = x - range + 1; cur_x < x + range; cur_x++)
            {
                for (int cur_y = y - range + 1; cur_y < y + range; cur_y++)
                {
                    // calculate magnitude of vector so that the alpha can be calculated
                    Vector2 init = new Vector2(x, y);
                    Vector2 final = new Vector2(cur_x, cur_y);

                    double new_dist = Vector2.Distance(init, final);

                    double transparency = 0;

                    if (new_dist > 0) transparency = (double)(new_dist * (10 / Light.Brightness));

                    if (transparency > EnvironmentalLight.A) transparency = EnvironmentalLight.A;

                    // optimisation: don't bother setting pixels we don't need to set
                    if (transparency < EnvironmentalLight.A) 
                    {
                        // math.clamp has aggressive inlining and is therefore a bit faster
                        transparency = Math.Clamp(transparency, 0, EnvironmentalLight.A);

                        transparent.A = (byte)transparency;

                        if (cur_x < 0 || cur_y < 0 || cur_x > max_size_x || cur_y > max_size_y) continue;
                        SSMapTexture.SetPixel(cur_x, cur_y, transparent);
                    }
                }
            }
        }

        public static void SetEnvironmentalLight(Color4 Colour)
        {
            EnvironmentalLight = Colour;

            if (EnvironmentalLight == null) EnvironmentalLight = new Color4(255, 255, 255, 255);

            // Set all pixels in the texture to the environmental light colour.
            SSMapTexture.Clear(EnvironmentalLight);
        }

        public static void SetEnvironmentalLightBlendMode(SDL_BlendMode BlendMode) => SDL_SetTextureBlendMode(SSMapTexture.TextureHandle, BlendMode);

        public static void RenderLightmap(Window Win)
        {
            if (SSMapTexture.TextureHandle == IntPtr.Zero) throw new NCException("You must initialise the Light Manager before using it!", 62, "LightManager.RenderLightmap called before LightManager.Init!", NCExceptionSeverity.FatalError);

            SSMapTexture.Unlock();
            SSMapTexture.Draw(Win);
        }
    }
}
