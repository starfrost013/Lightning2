using System;
using System.Drawing;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// Light
    /// 
    /// April 7, 2022 (modified July 11, 2022)
    /// 
    /// Defines a light. 
    /// </summary>
    public class Light : Renderable
    {
        /// <summary>
        /// The Brightness of this light.
        /// </summary>
        public uint Brightness { get; set; }

        /// <summary>
        /// The Range of this light.
        /// </summary>
        public double Range { get; set; }

        public Light()
        {
            Brightness = 255;
            Range = 10;
        }

        /// <summary>
        /// Renders this Light to a texture
        /// </summary>
        /// <param name="cWindow">The window to render this Light to.</param>
        internal void RenderToTexture(Window cWindow)
        {
            // Code nicked and modified from
            // https://stackoverflow.com/questions/10878209/midpoint-circle-algorithm-for-filled-circles

            // This here is sin(45) but i just hard-coded it.
            const float sinus = 0.70710678118f;
            // This is the distance on the axis from sin(90) to sin(45). 

            int range = (int)((Range * 40) * sinus);

            int x = (int)Position.X;
            int y = (int)Position.Y;

            float maxSizeX = cWindow.Settings.Size.X;
            float maxSizeY = cWindow.Settings.Size.Y;

            Color transparent = Color.FromArgb(0, LightManager.EnvironmentalLight.R, LightManager.EnvironmentalLight.G, LightManager.EnvironmentalLight.B);

            // calculate magnitude of vector so that the alpha can be calculated

            Vector2 init = new Vector2(x, y);
            for (int curX = x - range + 1; curX < x + range; curX++)
            {
                for (int curY = y - range + 1; curY < y + range; curY++)
                {
                    // don't draw offscreen pixels
                    if (curX >= 0 && curY >= 0 && curX < maxSizeX && curY < maxSizeY)
                    {
                        Vector2 final = new Vector2(curX, curY);

                        double newDistance = Vector2.Distance(init, final);

                        double transparency = 0;

                        if (newDistance > 0) transparency = (double)(newDistance * (10 / Range));

                        if (transparency > LightManager.EnvironmentalLight.A) transparency = LightManager.EnvironmentalLight.A;

                        if (transparency < (255 - Brightness)) transparency = (255 - Brightness);

                        // optimisation: don't bother setting pixels we don't need to set
                        if (transparency < LightManager.EnvironmentalLight.A)
                        {
                            // math.clamp has aggressive inlining and is therefore a bit faster
                            transparency = Math.Clamp(transparency, 0, LightManager.EnvironmentalLight.A);

                            transparent = Color.FromArgb((byte)transparency, transparent.R, transparent.G, transparent.B);

                            LightManager.ScreenSpaceMap.SetPixel(curX, curY, transparent);
                        }
                    }
                }
            }
        }
    }
}