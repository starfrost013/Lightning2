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
        private int RealRange => (int)(Range * (40 * sinus));

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
        /// Default value for <see cref="Brightness"/>
        /// </summary>
        private const int DEFAULT_BRIGHTNESS = 255;

        /// <summary>
        /// Default value for <see cref="Range"/>
        /// </summary>
        private const int DEFAULT_RANGE = 10;

        /// <summary>
        /// Constructor of the Light class. Sets the default range to 10 and the default brightness to 255.
        /// </summary>
        public Light(string name) : base(name)
        {
            Brightness = DEFAULT_BRIGHTNESS;
            Range = DEFAULT_RANGE;
        }

        public override void Create()
        {
            RenderToTexture();
        }

        /// <summary>
        /// Renders this Light to a texture
        /// </summary>
        internal void RenderToTexture()
        {
            // Code nicked and modified from
            // https://stackoverflow.com/questions/10878209/midpoint-circle-algorithm-for-filled-circles

            Debug.Assert(LightManager.ScreenSpaceMap != null);

            // lights always act as if snaptoscreen is false
            int x = (int)Position.X;
            int y = (int)Position.Y;

            float maxSizeX = Lightning.Renderer.Settings.Size.X;
            float maxSizeY = Lightning.Renderer.Settings.Size.Y;

            Color finalColor;

            if (LightColor == default)
            {
                finalColor = Color.FromArgb(0, LightManager.EnvironmentalLight.R, LightManager.EnvironmentalLight.G, LightManager.EnvironmentalLight.B);
            }
            else
            {
                finalColor = Color.FromArgb(0, LightColor.R, LightColor.G, LightColor.B);
            }

            // calculate magnitude of vector so that the alpha can be calculated

            Vector2 initialPos = new(x, y);

            for (int curX = x - RealRange + 1; curX < x + RealRange; curX++)
            {
                for (int curY = y - RealRange + 1; curY < y + RealRange; curY++)
                {
                    // don't draw offscreen pixels
                    if (curX >= 0 
                        && curY >= 0 
                        && curX < maxSizeX 
                        && curY < maxSizeY)
                    {
                        Vector2 final = new(curX, curY);

                        double newDistance = Vector2.Distance(initialPos, final);

                        double opacity = 0;

                        if (LightColor == default)
                        {
                            // calculate alpha for white lights
                            if (newDistance > 0) opacity = (double)(newDistance * (10 / Range));

                            if (opacity > LightManager.EnvironmentalLight.A) opacity = LightManager.EnvironmentalLight.A;

                            if (opacity < (255 - Brightness)) opacity = (255 - Brightness);
                        }
                        else
                        {
                            if (newDistance > 0)
                            {
                                opacity = (newDistance * (10d / Range));

                                // set per-pixel opaqueness
                                // this "inverts" the normal algorithm for masking out of a screenspace lightmap
                                if (opacity < 255) opacity = 255 - opacity;
                            }
                        }

                        // optimisation: don't bother setting pixels we don't need to set
                        if (opacity < LightManager.EnvironmentalLight.A)
                        {
                            // math.clamp has aggressive inlining and is therefore a bit faster
                            opacity = Math.Clamp(opacity, 0, LightManager.EnvironmentalLight.A);

                            if (LightColor == default)
                            {
                                finalColor = Color.FromArgb((byte)opacity, 
                                    finalColor.R, 
                                    finalColor.G, 
                                    finalColor.B);
                            }
                            else
                            {
                                // always use 255
                                double finalA = 255 * ((double)(255 - opacity) / 255);
                                double finalR = LightColor.R * ((double)opacity / 255);
                                double finalG = LightColor.G * ((double)opacity / 255);
                                double finalB = LightColor.B * ((double)opacity / 255);

                                // adjust for brightness
                                // increase alpha/opaqueness and multiply RGB
                                finalA += (255 - Brightness);
                                finalR *= ((double)Brightness / 255);
                                finalG *= ((double)Brightness / 255);
                                finalB *= ((double)Brightness / 255);
                                if (finalA > 255) finalA = 255;

                                finalColor = Color.FromArgb((byte)finalA,
                                    (byte)finalR,
                                    (byte)finalG,
                                    (byte)finalB);
                            }

                            // now we calculated the final colour.
                            // we simply add it on if there is already a pixel there

                            Color pixelColour = LightManager.ScreenSpaceMap.GetPixel(curX, curY);

                            // pixel colour is not opaque and we have colored lighting
                            if (LightColor != default
                                && pixelColour != LightManager.EnvironmentalLight) 
                            {
                                int additiveFinalA = pixelColour.A + finalColor.A;
                                int additiveFinalR = pixelColour.R + finalColor.R;
                                int additiveFinalG = pixelColour.G + finalColor.G;
                                int additiveFinalB = pixelColour.B + finalColor.B;

                                // cap at 255
                                if (additiveFinalA > 255) additiveFinalA = 255;
                                if (additiveFinalR > 255) additiveFinalR = 255;
                                if (additiveFinalG > 255) additiveFinalG = 255;
                                if (additiveFinalB > 255) additiveFinalB = 255;

                                finalColor = Color.FromArgb(additiveFinalA,
                                    (byte)additiveFinalR,
                                    (byte)additiveFinalG, 
                                    (byte)additiveFinalB);

                                LightManager.ScreenSpaceMap.SetPixel(curX, curY, finalColor);
                            }
                            else
                            {
                                LightManager.ScreenSpaceMap.SetPixel(curX, curY, finalColor);
                            }
                            
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes this Light from the texture.
        /// </summary>

        internal void RemoveFromTexture()
        {
            Debug.Assert(LightManager.ScreenSpaceMap != null);

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
                    if (curX >= 0 
                        && curY >= 0
                        && curX < maxSizeX 
                        && curY < maxSizeY) LightManager.ScreenSpaceMap.SetPixel(x, curY, LightManager.EnvironmentalLight);
                }
            }
        }

        public override void Destroy()
        {
            RemoveFromTexture();
        }
    }
}