using System;
using System.Drawing;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// Light
    /// 
    /// April 7, 2022
    /// 
    /// Defines a light. 
    /// </summary>
    public class Light
    {
        public Vector2 Position { get; set; }

        public uint Brightness { get; set; }

        public double Range { get; set; }

        public bool SnapToScreen { get; set; }

        public Light()
        {
            Brightness = 255;
            Range = 10;
        }

        public void RenderToTexture(Window win)
        {
            // Code nicked and modified from
            // https://stackoverflow.com/questions/10878209/midpoint-circle-algorithm-for-filled-circles

            // This here is sin(45) but i just hard-coded it.
            const float sinus = 0.70710678118f;
            // This is the distance on the axis from sin(90) to sin(45). 

            int range = (int)((Range * 40) * sinus);

            int x = (int)Position.X;
            int y = (int)Position.Y;

            float max_size_x = win.Settings.Size.X;
            float max_size_y = win.Settings.Size.Y;

            Color transparent = Color.FromArgb(0, LightManager.EnvironmentalLight.R, LightManager.EnvironmentalLight.G, LightManager.EnvironmentalLight.B);

            // calculate magnitude of vector so that the alpha can be calculated

            Vector2 init = new Vector2(x, y);
            for (int cur_x = x - range + 1; cur_x < x + range; cur_x++)
            {
                for (int cur_y = y - range + 1; cur_y < y + range; cur_y++)
                {
                    // don't draw offscreen pixels
                    if (cur_x >= 0 && cur_y >= 0 && cur_x < max_size_x && cur_y < max_size_y)
                    {
                        Vector2 final = new Vector2(cur_x, cur_y);

                        double new_dist = Vector2.Distance(init, final);

                        double transparency = 0;

                        if (new_dist > 0) transparency = (double)(new_dist * (10 / Range));

                        if (transparency > LightManager.EnvironmentalLight.A) transparency = LightManager.EnvironmentalLight.A;

                        if (transparency < (255 - Brightness)) transparency = (255 - Brightness);

                        // optimisation: don't bother setting pixels we don't need to set
                        if (transparency < LightManager.EnvironmentalLight.A)
                        {
                            // math.clamp has aggressive inlining and is therefore a bit faster
                            transparency = Math.Clamp(transparency, 0, LightManager.EnvironmentalLight.A);

                            transparent = Color.FromArgb((byte)transparency, transparent.R, transparent.G, transparent.B);

                            LightManager.SSMapTexture.SetPixel(cur_x, cur_y, transparent);
                        }
                    }
                }
            }
        }
    }
}