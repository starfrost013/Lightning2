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

        /// <summary>
        /// The real range of this light used when drawing
        /// </summary>
        private int RealRange => (int)((Range * 40) * sinus);

        /// <summary>
        /// The colour of this Light.
        /// The alpha value is ignored.
        /// </summary>
        public Color LightColor { get; set; }

        /// <summary>
        /// sin(45) but (slightly) faster
        /// </summary>
        const float sinus = 0.70710678118f;

        /// <summary>
        /// Constructor of the Light class. Sets the default range to 10 and the default brightness to 255.
        /// </summary>
        public Light(string name) : base(name)
        {
            Brightness = 255;
            Range = 10;
        }

        /// <summary>
        /// Renders this Light to a texture
        /// </summary>
        /// <param name="Lightning.Renderer">The window to render this Light to.</param>
        internal void RenderToTexture()
        {
            // Code nicked and modified from
            // https://stackoverflow.com/questions/10878209/midpoint-circle-algorithm-for-filled-circles

            // lights always act as if snaptoscreen is false
            int x = (int)Position.X;
            int y = (int)Position.Y;

            float maxSizeX = Lightning.Renderer.Settings.Size.X;
            float maxSizeY = Lightning.Renderer.Settings.Size.Y;

            Color transparentBaseColor;

            if (LightColor == default)
            {
                transparentBaseColor = Color.FromArgb(0, LightManager.EnvironmentalLight.R, LightManager.EnvironmentalLight.G, LightManager.EnvironmentalLight.B);
            }
            else
            {
                transparentBaseColor = Color.FromArgb(0, LightColor.R, LightColor.G, LightColor.B);
            }

            // calculate magnitude of vector so that the alpha can be calculated

            Vector2 initialPos = new(x, y);

            for (int curX = x - RealRange + 1; curX < x + RealRange; curX++)
            {
                for (int curY = y - RealRange + 1; curY < y + RealRange; curY++)
                {
                    // don't draw offscreen pixels
                    if (curX >= 0 && curY >= 0 && curX < maxSizeX && curY < maxSizeY)
                    {
                        Vector2 final = new(curX, curY);

                        double newDistance = Vector2.Distance(initialPos, final);

                        double opaqueness = 0;

                        if (LightColor == default)
                        {
                            // calculate alpha for white lights
                            if (newDistance > 0) opaqueness = (double)(newDistance * (10 / Range));

                            if (opaqueness > LightManager.EnvironmentalLight.A) opaqueness = LightManager.EnvironmentalLight.A;

                            if (opaqueness < (255 - Brightness)) opaqueness = (255 - Brightness);
                        }
                        else
                        {
                            if (newDistance > 0)
                            {
                                opaqueness = (double)(newDistance * (10 / Range));

                                // set per-pixel opaqueness
                                // this "inverts" the normal algorithm for masking out of a screenspace lightmap
                                if (opaqueness < 255) opaqueness = 255 - opaqueness;
                            }
                        }

                        // optimisation: don't bother setting pixels we don't need to set
                        if (opaqueness < LightManager.EnvironmentalLight.A)
                        {
                            // math.clamp has aggressive inlining and is therefore a bit faster
                            opaqueness = Math.Clamp(opaqueness, 0, LightManager.EnvironmentalLight.A);

                            if (LightColor == default)
                            {
                                transparentBaseColor = Color.FromArgb((byte)opaqueness, 
                                    transparentBaseColor.R, 
                                    transparentBaseColor.G, 
                                    transparentBaseColor.B);
                            }
                            else
                            {
                                // always use 255
                                double finalA = 255 * ((double)(255 - opaqueness) / 255);
                                double finalR = LightColor.R * ((double)opaqueness / 255);
                                double finalG = LightColor.G * ((double)opaqueness / 255);
                                double finalB = LightColor.B * ((double)opaqueness / 255);

                                // adjust for brightness
                                // increase alpha/opaqueness and multiply RGB
                                finalA += (255 - Brightness);
                                finalR *= ((double)Brightness / 255);
                                finalG *= ((double)Brightness / 255);
                                finalB *= ((double)Brightness / 255);
                                if (finalA > 255) finalA = 255;

                                transparentBaseColor = Color.FromArgb((byte)finalA,
                                    (byte)finalR,
                                    (byte)finalG,
                                    (byte)finalB);
                            }

                            LightManager.ScreenSpaceMap.SetPixel(curX, curY, transparentBaseColor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes this Light from the texture.
        /// </summary>
        /// <param name="Lightning.Renderer">The window to remove this light from.</param>
        internal void RemoveFromTexture()
        {
            // Code nicked and modified from
            // https://stackoverflow.com/questions/10878209/midpoint-circle-algorithm-for-filled-circles

            int x = (int)Position.X;
            int y = (int)Position.Y;

            float maxSizeX = Lightning.Renderer.Settings.Size.X;
            float maxSizeY = Lightning.Renderer.Settings.Size.Y;

            // calculate magnitude of vector so that the alpha can be calculated

            for (int curX = x - RealRange + 1; curX < x + RealRange; curX++)
            {
                for (int curY = y - RealRange + 1; curY < y + RealRange; curY++)
                {
                    // set all non-offscreen pixels of the light (we don't draw offscreen light pixels currently) to the environmental light colour
                    // don't draw offscreen pixels
                    if (curX >= 0 && curY >= 0 && curX < maxSizeX && curY < maxSizeY) LightManager.ScreenSpaceMap.SetPixel(x, curY, LightManager.EnvironmentalLight);
                }
            }
        }
    }
}