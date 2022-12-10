global using static LightningGL.Lightning;
using LightningBase;
using System;
using System.Data.SqlTypes;

namespace LightningGL
{
    /// <summary>
    /// Defines a LightningGL Window. 
    /// </summary>
    public class SdlRenderer : Renderer
    {
        /// <summary>
        /// The last processed SDL event. Only valid if .Update() is called.
        /// </summary>
        public SDL_Event LastEvent { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override SdlRendererSettings Settings => (SdlRendererSettings)base.Settings;

        // no idea where to put this now (temporary SDL_gfx stuff)
        private const int AAbits = 8;

        private const int DEFAULT_ELLIPSE_OVERSCAN = 4;

        public SdlRenderer() : base()
        {
            FrameTimer = new Stopwatch();
            // Start the delta timer.
            FrameTimer.Start();
            ThisTime = 0;
            Settings = new SdlRendererSettings();
        }

        /// <summary>
        /// Starts this window.
        /// </summary>
        /// <param name="windowSettings">The window settings to use when starting this window - see <see cref="RendererSettings"/></param>
        internal override void Start()
        {
            base.Start();

            NCLogging.Log("Initialising SDL...");
            if (SDL_Init(SDL_InitFlags.SDL_INIT_EVERYTHING) < 0) NCError.ShowErrorBox($"Error initialising SDL2: {SDL_GetError()}", 200,
                "Failed to initialise SDL2 during SdlRenderer::Init", NCErrorSeverity.FatalError);

            NCLogging.Log("Initialising SDL_image...");
            if (IMG_Init(IMG_InitFlags.IMG_INIT_EVERYTHING) < 0) NCError.ShowErrorBox($"Error initialising SDL2_image: {SDL_GetError()}", 201,
                "Failed to initialise SDL2_image during SdlRenderer::Init", NCErrorSeverity.FatalError);

            NCLogging.Log("Initialising SDL_ttf...");
            if (TTF_Init() < 0) NCError.ShowErrorBox($"Error initialising SDL2_ttf: {SDL_GetError()}", 202,
                "Failed to initialise SDL2_ttf during SdlRenderer::Init", NCErrorSeverity.FatalError);

            NCLogging.Log("Initialising SDL_mixer...");
            if (Mix_Init(MIX_InitFlags.MIX_INIT_EVERYTHING) < 0) NCError.ShowErrorBox($"Error initialising SDL2_mixer: {SDL_GetError()}", 203,
                "Failed to initialise SDL2_mixer during SdlRenderer::Init", NCErrorSeverity.FatalError);

            NCLogging.Log($"Initialising audio device ({GlobalSettings.AudioDeviceHz}Hz, {GlobalSettings.AudioChannels} channels, format {GlobalSettings.AudioFormat}, chunk size {GlobalSettings.AudioChunkSize})...");
            if (Mix_OpenAudio(GlobalSettings.AudioDeviceHz, GlobalSettings.AudioFormat, GlobalSettings.AudioChannels, GlobalSettings.AudioChunkSize) < 0) NCError.ShowErrorBox(
                $"Error initialising audio device: {SDL_GetError()}", 56, "Failed to initialise audio device during SdlRenderer::Init", NCErrorSeverity.FatalError);

            // localise the window title
            Settings.Title = LocalisationManager.ProcessString(Settings.Title);

            // set the renderer if the user specified one
            string renderer = SDLu_GetRenderDriverName();

            if (GlobalSettings.GraphicsSdlRenderingBackend != default)
            {
                // set the renderer
                renderer = GlobalSettings.GraphicsSdlRenderingBackend.ToString().ToLowerInvariant(); // needs to be lowercase
                SDL_SetHintWithPriority("SDL_HINT_RENDER_DRIVER", renderer, SDL_HintPriority.SDL_HINT_OVERRIDE);
            }

            NCLogging.Log($"Using renderer: {renderer}");

            // Create the window,
            Settings.WindowHandle = SDL_CreateWindow(Settings.Title, (int)Settings.Position.X, (int)Settings.Position.Y, (int)Settings.Size.X, (int)Settings.Size.Y, Settings.WindowFlags);

            if (Settings.WindowHandle == IntPtr.Zero) NCError.ShowErrorBox($"Failed to create Window: {SDL_GetError()}", 8, 
                "Window::AddWindow - SDL_CreateWindow failed to create window", NCErrorSeverity.FatalError);

            // Create the renderer.
            Settings.RendererHandle = SDL_CreateRenderer(Settings.WindowHandle, -1, Settings.RenderFlags);

            // Get the renderer driver name using our unofficial SDL function
            string realRenderDriverName = SDLu_GetRenderDriverName();

            if (realRenderDriverName != renderer) NCError.ShowErrorBox($"Specified renderer {renderer} is not supported! Using {realRenderDriverName} instead!", 123, 
                "Renderer not supported in current environment", NCErrorSeverity.Warning, null, false);

            if (Settings.RendererHandle == IntPtr.Zero) NCError.ShowErrorBox($"Failed to create Renderer: {SDL_GetError()}", 9, 
                "Window::AddWindow - SDL_CreateRenderer failed to create renderer", NCErrorSeverity.FatalError);

            // Initialise the Light Manager.
            LightManager.Init();

            // maybe move this somewhere else
            if (!GlobalSettings.DebugDisabled) AddRenderable(new DebugViewer("DebugViewer"));
        }


        /// <summary>
        /// Runs the main loop at the start of each frame.
        /// </summary>
        /// <returns>A boolean determining if the window is to keep running or close.</returns>
        internal override bool Run()
        {
            // clear the renderet
            FrameTimer.Restart();
            SDL_RenderClear(Settings.RendererHandle);

            // Reset rendered this frame count
            RenderedLastFrame = 0;

            EventWaiting = (SDL_PollEvent(out var currentEvent) > 0);

            // default mainloop
            if (EventWaiting)
            {
                LastEvent = currentEvent;

                // default rendering loop.
                // Developers can choose to handle SDL events after this
                switch (currentEvent.type)
                {
                    case SDL_EventType.SDL_KEYDOWN:
                        EventManager.FireKeyPressed((Key)currentEvent.key);
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        EventManager.FireKeyReleased((Key)currentEvent.key);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN: // Mouse down event
                        EventManager.FireMousePressed((MouseButton)currentEvent.button);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP: // Mouse up event
                        EventManager.FireMouseReleased((MouseButton)currentEvent.button);
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION: // Mouse move event
                        EventManager.FireMouseMove((MouseButton)currentEvent.motion);
                        break;
                    case SDL_EventType.SDL_WINDOWEVENT: // Window Event - check subtypes
                        switch (currentEvent.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
                                EventManager.FireMouseEnter();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
                                EventManager.FireMouseLeave();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                                EventManager.FireFocusGained();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                                EventManager.FireFocusLost();
                                break;
                        }

                        break;
                    case SDL_EventType.SDL_QUIT: // User requested a quit, so shut down
                        Lightning.Shutdown();
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Manages the render loop.
        /// </summary>
        internal override void Render()
        {
            // only render if we have a scene
            Debug.Assert(CurrentScene != null);

            // this is actually fine for performance as it turns out (probably not a very big LINQ CALL)
            Renderables = Renderables.OrderBy(x => x.ZIndex).ToList();

            // Build a list of renderables to render from all asset managers.
            // Other stuff can be added "outside" so we simply remove and add to the list (todo: this isn't great)
            Cull();

            // Draw every object.
            RenderAll();

            // Update the primitive manager.
            PrimitiveManager.Update();

            // Render the lightmap.
            LightManager.Update();

            // Update audio.
            AudioManager.Update();

            // Update the text manager
            TextManager.Update();

            // Update camera (if it's not null)
            Settings.Camera?.Update();

            // Correctly draw the background
            SDL_SetRenderDrawColor(Settings.RendererHandle, Settings.BackgroundColor.R, Settings.BackgroundColor.G, Settings.BackgroundColor.B, Settings.BackgroundColor.A);

            SDL_RenderPresent(Settings.RendererHandle);

            int maxFps = GlobalSettings.GraphicsMaxFPS;

            // Delay for frame limiter
            if (maxFps > 0)
            {
                double targetFrameTime = 1000 / (double)maxFps;
                double actualFrameTime = DeltaTime;

                double delayTime = targetFrameTime - actualFrameTime;

                if (delayTime > 0) SDL_Delay((uint)delayTime);
            }

            // Update the internal FPS values.
            UpdateFps();
        }

        /// <summary>
        /// Clears the renderer and optionally sets the color to the Color <paramref name="clearColor"/>
        /// </summary>
        /// <param name="clearColor">The color to set the background to after clearing.</param>
        public override void Clear(Color clearColor = default)
        {
            // default(Color) is 0,0,0,0, no special case code needed
            SDL_SetRenderDrawColor(Settings.RendererHandle, clearColor.R, clearColor.G, clearColor.B, clearColor.A);
            SDL_RenderClear(Settings.RendererHandle);
            Settings.BackgroundColor = clearColor;
        }


        /// <summary>
        /// Sets the window to be fullscreen or windowed.
        /// </summary>
        /// <param name="fullscreen">A boolean determining if the window is fullscreen (TRUE) or windowed (FALSE)</param>
        public override void SetFullscreen(bool fullscreen) => SDL_SetWindowFullscreen(Settings.WindowHandle, fullscreen ? (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP : 0);

        internal override void Shutdown()
        {
            base.Shutdown();

            SDL_DestroyRenderer(Settings.RendererHandle);
            SDL_DestroyWindow(Settings.WindowHandle);
        }

        #region Backend-specific primitive code

        internal override void DrawPixel(int x, int y, byte r, byte g, byte b, byte a)
        {
            int result = 0;
            result |= SDL_SetRenderDrawBlendMode(Settings.RendererHandle, (a == 255) ? SDL_BlendMode.SDL_BLENDMODE_NONE : SDL_BlendMode.SDL_BLENDMODE_BLEND);
            result |= SDL_SetRenderDrawColor(Settings.RendererHandle, r, g, b, a);
            result |= SDL_RenderDrawPoint(Settings.RendererHandle, x, y);
            
            if (result != 0)
            {
                NCError.ShowErrorBox($"An error occurred while drawing a pixel in SDL", 213, $"An error occurred in SdlRenderer::DrawPixel - error code {result}. " +
                    $"The pixel will not be drawn!", NCErrorSeverity.Warning, null, true);
            }
        }

        /// <summary>
        /// Private weighted pixel method
        /// SDL2_gfx for C#
        /// </summary>
        /// <param name="x">The x position of the pixel to draw.</param>
        /// <param name="y">The y position of the pixel to draw.</param>
        /// <param name="r">The red component of the colour of the pixel to draw.</param>
        /// <param name="g">The green component of the colour of the pixel to draw.</param>
        /// <param name="b">The blue component of the colour of the pixel to draw.</param>
        /// <param name="a">The alpha component of the colour of the pixel to draw.</param>
        /// <param name="weight">The weight of the pixel to draw.</param>
        private void DrawPixelWeighted(int x, int y, byte r, byte g, byte b, byte a, int weight)
        {
            /*
	            * Modify Alpha by weight 
	        */
            long ax = a; // c# assumes long for bitshift
            ax = ((ax * weight) >> 8);
            if (ax > 255)
            {
                a = 255;
            }
            else
            {
                a = (byte)(ax & 0xff);
            }

            // this already checks for failure
            DrawPixel(x, y, r, g, b, a);
        }

        internal override void DrawLine(int x1, int y1, int x2, int y2, byte r, byte g, byte b, byte a, int thickness = 1)
        {
            if (thickness > 1)
            {
                int wh;
                double dx, dy, dx1, dy1, dx2, dy2;
                double l, wl2, nx, ny, ang, adj;
                int[] px = new int[4];
                int[] py = new int[4];


                /* Special case: thick "point" */
                if ((x1 == x2) && (y1 == y2))
                {
                    wh = thickness / 2;
                    DrawRectangle(x1 - wh, y1 - wh, x2 + thickness, y2 + thickness, r, g, b, a);
                    return;
                }

                /* Calculate offsets for sides */
                dx = (double)(x2 - x1);
                dy = (double)(y2 - y1);
                l = Math.Sqrt(dx * dx + dy * dy);
                ang = Math.Atan2(dx, dy);
                adj = 0.1 + 0.9 * Math.Abs(Math.Cos(2.0 * ang));
                wl2 = ((double)thickness - adj) / (2.0 * l);
                nx = dx * wl2;
                ny = dy * wl2;

                /* Build polygon */
                dx1 = x1;
                dy1 = y1;
                dx2 = x2;
                dy2 = y2;
                px[0] = (int)(dx1 + ny);
                px[1] = (int)(dx1 - ny);
                px[2] = (int)(dx2 - ny);
                px[3] = (int)(dx2 + ny);
                py[0] = (int)(dy1 - nx);
                py[1] = (int)(dy1 + nx);
                py[2] = (int)(dy2 + nx);
                py[3] = (int)(dy2 - nx);

                /* Draw polygon */
                DrawPolygon(px, py, r, g, b, a, true);
            }
            else
            {
                int xx0, yy0, xx1, yy1;
                int intshift, erracc, erradj;
                int erracctmp, wgt;
                int dx, dy, tmp, xdir, y0p1, x0pxdir;

                /*
                * Keep on working with 32bit numbers 
                */
                xx0 = x1;
                yy0 = y1;
                xx1 = x2;
                yy1 = y2;

                /*
                * Reorder points to make dy positive 
                */
                if (yy0 > yy1)
                {
                    tmp = yy0;
                    yy0 = yy1;
                    yy1 = tmp;
                    tmp = xx0;
                    xx0 = xx1;
                    xx1 = tmp;
                }

                /*
                * Calculate distance 
                */
                dx = xx1 - xx0;
                dy = yy1 - yy0;

                /*
                * Adjust for negative dx and set xdir 
                */
                if (dx >= 0)
                {
                    xdir = 1;
                }
                else
                {
                    xdir = -1;
                    dx = (-dx);
                }

                /*
                * Check for special cases 
                */
                if (dx == 0)
                {
                    DrawVLine(x1, y1, y2, r, g, b, a);

                    if (dy > 0)
                    {
                        DrawVLine(x1, yy0, yy0 + dy, r, g, b, a);
                    }
                    else
                    {
                        DrawPixel(x1, y1, r, g, b, a);
                    }

                    return;
                }
                else if (dy == 0)
                {
                    /*
                    * Horizontal line 
                    */
                    DrawHLine(x1, x2, y1, r, g, b, a);

                    if (dx > 0)
                    {
                        DrawHLine(xx0, xx0 + (xdir * dx), y1, r, g, b, a);
                    }
                    else
                    {
                        DrawPixel(x1, y1, r, g, b, a);
                    }

                    return;
                }
                else if ((dx == dy))
                {
                    /*
                    * Diagonal line (with endpoint)
                    */
                    DrawLine(x1, y1, x2, y2, r, g, b, a);
                    return;
                }

                /*
                * Zero accumulator 
                */
                erracc = 0;

                /*
                * # of bits by which to shift erracc to get intensity level 
                */
                intshift = 32 - AAbits;

                /*
                * Draw the initial pixel in the foreground color 
                */
                DrawPixel(x1, y1, r, g, b, a);

                /*
                * x-major or y-major? 
                */
                if (dy > dx)
                {

                    /*
                    * y-major.  Calculate 16-bit fixed point fractional part of a pixel that
                    * X advances every time Y advances 1 pixel, truncating the result so that
                    * we won't overrun the endpoint along the X axis 
                    */
                    /*
                    * Not-so-portable version: erradj = ((Uint64)dx << 32) / (Uint64)dy; 
                    */
                    erradj = ((dx << 16) / dy) << 16;

                    /*
                    * draw all pixels other than the first and last 
                    */
                    x0pxdir = xx0 + xdir;
                    while (dy > 0)
                    {
                        erracctmp = erracc;
                        erracc += erradj;
                        if (erracc <= erracctmp)
                        {
                            /*
                            * rollover in error accumulator, x coord advances 
                            */
                            xx0 = x0pxdir;
                            x0pxdir += xdir;
                        }
                        yy0++;      /* y-major so always advance Y */

                        /*
                        * the AAbits most significant bits of erracc give us the intensity
                        * weighting for this pixel, and the complement of the weighting for
                        * the paired pixel. 
                        */
                        wgt = (erracc >> intshift) & 255;
                        DrawPixelWeighted(xx0, yy0, r, g, b, a, 255 - wgt);
                        DrawPixelWeighted(x0pxdir, yy0, r, g, b, a, wgt);
                    }

                }
                else
                {

                    /*
                    * x-major line.  Calculate 16-bit fixed-point fractional part of a pixel
                    * that Y advances each time X advances 1 pixel, truncating the result so
                    * that we won't overrun the endpoint along the X axis. 
                    */
                    /*
                    * Not-so-portable version: erradj = ((Uint64)dy << 32) / (Uint64)dx; 
                    */
                    erradj = ((dy << 16) / dx) << 16;

                    /*
                    * draw all pixels other than the first and last 
                    */
                    y0p1 = yy0 + 1;
                    while (dx > 0) // check this
                    {

                        erracctmp = erracc;
                        erracc += erradj;
                        if (erracc <= erracctmp)
                        {
                            /*
                            * Accumulator turned over, advance y 
                            */
                            yy0 = y0p1;
                            y0p1++;
                        }
                        xx0 += xdir;    /* x-major so always advance X */
                        /*
                        * the AAbits most significant bits of erracc give us the intensity
                        * weighting for this pixel, and the complement of the weighting for
                        * the paired pixel. 
                        */
                        wgt = (erracc >> intshift) & 255;
                        DrawPixelWeighted(xx0, yy0, r, g, b, a, 255 - wgt);
                        DrawPixelWeighted(xx0, y0p1, r, g, b, a, wgt);
                    }
                }

                /*
                * Draw final pixel, always exactly intersected by the line and doesn't
                * need to be weighted. 
                */
                DrawPixel(x2, y2, r, g, b, a);
            }
            
        }

        private void DrawHLine(int x1, int x2, int y, byte r, byte g, byte b, byte a) => DrawLine(x1, y, x2, y, r, g, b, a);

        private void DrawVLine(int x, int y1, int y2, byte r, byte g, byte b, byte a) => DrawLine(x, y1, x, y2, r, g, b, a);

        internal override void DrawBezier(int[] vx, int[] vy, int s, byte r, byte g, byte b, byte a)
        {
            int i;
            double t, stepsize;
            double x1, y1, x2, y2;

            // use the x length
            int n = vx.Length;

            /*
            * Sanity check 
            */
            if (n < 3
                || s < 2
                || vx.Length != vy.Length)
            {
                // todo: error message :DDD
                return;
            }

            double[] x = new double[n];
            double[] y = new double[n];

            /*
            * Variable setup 
            */
            stepsize = (double)1.0 / s;

            for (i = 0; i < n; i++)
            {
                x[i] = (double)vx[i];
                y[i] = (double)vy[i];
            }

            /*
            * Draw 
            */
            t = 0.0;
            x1 = EvaluateBezier(x, t);
            y1 = EvaluateBezier(x, t);
            for (i = 0; i <= (n * s); i++)
            {
                t += stepsize;
                x2 = EvaluateBezier(x, t);
                y2 = EvaluateBezier(x, t);
                // keep as double until here, to prevent quantisation issues?
                DrawLine((int)x1, (int)y1, (int)x2, (int)y2, r, g, b, a);
                x1 = x2;
                y1 = y2;
            }
        }

        private double EvaluateBezier(double[] data, double t)
        {
            double mu, result;
            int n, k, kn, nn, nkn;
            double blend, muk, munk;

            /* Sanity check bounds */
            if (t < 0.0)
            {
                return data[0];
            }
            if (t >= data.Length)
            {
                return data[^1];
            }

            /* Adjust t to the range 0.0 to 1.0 */
            mu = t / data.Length;

            /* Calculate interpolate */
            n = data.Length - 1;
            result = 0.0;
            muk = 1;
            munk = Math.Pow(1 - mu, n);
            for (k = 0; k <= n; k++)
            {
                nn = n;
                kn = k;
                nkn = n - k;
                blend = muk * munk;
                muk *= mu;
                munk /= (1 - mu);
                while (nn >= 1)
                {
                    blend *= nn;
                    nn--;
                    if (kn > 1)
                    {
                        blend /= kn;
                        kn--;
                    }
                    if (nkn > 1)
                    {
                        blend /= nkn;
                        nkn--;
                    }
                }
                result += data[k] * blend;
            }

            return (result);
        }

        internal override void DrawEllipse(int x, int y, int rx, int ry, byte r, byte g, byte b, byte a, bool filled)
        {
            if (filled)
            {
                int rxi, ryi;
                int rx2, ry2, rx22, ry22;
                int error;
                int curX, curY, curXp1, curYm1;
                int scrX, scrY, oldX, oldY;
                int deltaX, deltaY;
                int ellipseOverscan;

                /*
                * Special cases for rx=0 and/or ry=0: draw a hline/vline/pixel 
                */
                if (rx == 0)
                {
                    if (ry == 0)
                    {
                        DrawPixel(x, y, r, g, b, a);
                    }
                    else
                    {
                        DrawVLine(x, y - ry, y + ry, r, g, b, a);
                    }

                    return;
                }
                else
                {
                    if (ry == 0)
                    {
                        DrawHLine(x - rx, x + rx, y, r, g, b, a);
                        return;
                    }
                }

                /*
                 * Adjust overscan 
                 */
                rxi = rx;
                ryi = ry;
                if (rxi >= 512 || ryi >= 512)
                {
                    ellipseOverscan = DEFAULT_ELLIPSE_OVERSCAN / 4;
                }
                else if (rxi >= 256 || ryi >= 256)
                {
                    ellipseOverscan = DEFAULT_ELLIPSE_OVERSCAN / 2;
                }
                else
                {
                    ellipseOverscan = DEFAULT_ELLIPSE_OVERSCAN / 1;
                }

                /*
                 * Top/bottom center points.
                 */
                oldX = scrX = 0;
                oldY = scrY = ryi;
                DrawQuadrant(x, y, 0, ry, filled, r, g, b, a);

                /* Midpoint ellipse algorithm with overdraw */
                rxi *= ellipseOverscan;
                ryi *= ellipseOverscan;
                rx2 = rxi * rxi;
                rx22 = rx2 + rx2;
                ry2 = ryi * ryi;
                ry22 = ry2 + ry2;
                curX = 0;
                curY = ryi;
                deltaX = 0;
                deltaY = rx22 * curY;

                /* Points in segment 1 */
                error = ry2 - rx2 * ryi + rx2 / 4;
                while (deltaX <= deltaY)
                {
                    curX++;
                    deltaX += ry22;

                    error += deltaX + ry2;
                    if (error >= 0)
                    {
                        curY--;
                        deltaY -= rx22;
                        error -= deltaY;
                    }

                    scrX = curX / ellipseOverscan;
                    scrY = curY / ellipseOverscan;
                    if ((scrX != oldX && scrY == oldY) || (scrX != oldX && scrY != oldY))
                    {
                        DrawQuadrant(x, y, scrX, scrY, filled, r, g, b, a);
                        oldX = scrX;
                        oldY = scrY;
                    }
                }

                /* Points in segment 2 */
                if (curY > 0)
                {
                    curXp1 = curX + 1;
                    curYm1 = curY - 1;
                    error = ry2 * curX * curXp1 + ((ry2 + 3) / 4) + rx2 * curYm1 * curYm1 - rx2 * ry2;
                    while (curY > 0)
                    {
                        curY--;
                        deltaY -= rx22;

                        error += rx2;
                        error -= deltaY;

                        if (error <= 0)
                        {
                            curX++;
                            deltaX += ry22;
                            error += deltaX;
                        }

                        scrX = curX / ellipseOverscan;
                        scrY = curY / ellipseOverscan;
                        if ((scrX != oldX && scrY == oldY) || (scrX != oldX && scrY != oldY))
                        {
                            oldY--;
                            for (; oldY >= scrY; oldY--)
                            {
                                DrawQuadrant(x, y, scrX, oldY, filled, r, g, b, a);
                                /* prevent overdraw */
                                oldY = scrY - 1;
                            }
                            oldX = scrX;
                            oldY = scrY;
                        }
                    }
                }
            }
            else
            {
                int i;
                int a2, b2, ds, dt, dxt, t, s, d;
                int xp, yp, xs, ys, dyt, od, xx, yy, xc2, yc2;
                float cp;
                double sab;
                byte weight, iweight;

                /*
                * Special cases for rx=0 and/or ry=0: draw a hline/vline/pixel 
                */
                if (rx == 0)
                {
                    if (ry == 0)
                    {
                        DrawPixel(x, y, r, g, b, a);
                    }
                    else
                    {
                        DrawVLine(x, y - ry, y + ry, r, g, b, a);
                    }

                    return;
                }
                else
                {
                    if (ry == 0)
                    {
                        DrawHLine(x - rx, x + rx, y, r, g, b, a);
                        return;
                    }
                }

                /* Variable setup */
                a2 = rx * rx;
                b2 = ry * ry;

                ds = 2 * a2;
                dt = 2 * b2;

                xc2 = 2 * x;
                yc2 = 2 * y;

                sab = Math.Sqrt((double)(a2 + b2));
                od = (int)(sab * 0.01) + 1; /* introduce some overdraw */
                dxt = Convert.ToInt32((double)a2 / sab) + od;

                t = 0;
                s = -2 * a2 * ry;
                d = 0;

                xp = x;
                yp = y - ry;


                SDL_SetRenderDrawBlendMode(Settings.RendererHandle, (a == 255) ? SDL_BlendMode.SDL_BLENDMODE_NONE : SDL_BlendMode.SDL_BLENDMODE_BLEND);

                /* "End points" */
                DrawPixel(xp, yp, r, g, b, a);
                DrawPixel(xc2 - xp, yp, r, g, b, a);
                DrawPixel(xp, yc2 - yp, r, g, b, a);
                DrawPixel(xc2 - xp, yc2 - yp, r, g, b, a);

                for (i = 1; i <= dxt; i++)
                {
                    xp--;
                    d += t - b2;

                    if (d >= 0)
                    {
                        ys = yp - 1;
                    }
                    else if ((d - s - a2) > 0)
                    {
                        if ((2 * d - s - a2) >= 0)
                        {
                            ys = yp + 1;
                        }
                        else
                        {
                            ys = yp;
                            yp++;
                            d -= s + a2;
                            s += ds;
                        }
                    }
                    else
                    {
                        yp++;
                        ys = yp + 1;
                        d -= s + a2;
                        s += ds;
                    }

                    t -= dt;

                    /* Calculate alpha */
                    if (s != 0)
                    {
                        cp = (float)Math.Abs(d) / (float)Math.Abs(s);
                        if (cp > 1.0)
                        {
                            cp = 1.0f;
                        }
                    }
                    else
                    {
                        cp = 1.0f;
                    }

                    /* Calculate weights */
                    weight = (byte)(cp * 255);
                    iweight = (byte)(255 - weight);

                    /* Upper half */
                    xx = xc2 - xp;
                    DrawPixelWeighted(xp, yp, r, g, b, a, iweight);
                    DrawPixelWeighted(xx, yp, r, g, b, a, iweight);

                    DrawPixelWeighted(xp, ys, r, g, b, a, weight);
                    DrawPixelWeighted(xx, ys, r, g, b, a, weight);

                    /* Lower half */
                    yy = yc2 - yp;
                    DrawPixelWeighted(xp, yy, r, g, b, a, iweight);
                    DrawPixelWeighted(xx, yy, r, g, b, a, iweight);

                    yy = yc2 - ys;
                    DrawPixelWeighted(xp, yy, r, g, b, a, weight);
                    DrawPixelWeighted(xx, yy, r, g, b, a, weight);
                }

                /* Replaces original approximation code dyt = abs(yp - yc); */
                dyt = (int)((double)b2 / sab) + od;

                for (i = 1; i <= dyt; i++)
                {
                    yp++;
                    d -= s + a2;

                    if (d <= 0)
                        xs = xp + 1;
                    else if ((d + t - b2) < 0)
                    {
                        if ((2 * d + t - b2) <= 0)
                            xs = xp - 1;
                        else
                        {
                            xs = xp;
                            xp--;
                            d += t - b2;
                            t -= dt;
                        }
                    }
                    else
                    {
                        xp--;
                        xs = xp - 1;
                        d += t - b2;
                        t -= dt;
                    }

                    s += ds;

                    /* Calculate alpha */
                    if (t != 0)
                    {
                        cp = (float)Math.Abs(d) / (float)Math.Abs(t);
                        if (cp > 1.0)
                        {
                            cp = 1.0f;
                        }
                    }
                    else
                    {
                        cp = 1.0f;
                    }

                    /* Calculate weight */
                    weight = (byte)(cp * 255);
                    iweight = (byte)(255 - weight);

                    /* Left half */
                    xx = xc2 - xp;
                    yy = yc2 - yp;
                    DrawPixelWeighted(xp, yp, r, g, b, a, iweight);
                    DrawPixelWeighted(xp, yy, r, g, b, a, iweight);

                    DrawPixelWeighted(xp, yy, r, g, b, a, iweight);
                    DrawPixelWeighted(xx, yy, r, g, b, a, iweight);

                    /* Right half */
                    xx = xc2 - xs;
                    DrawPixelWeighted(xs, yp, r, g, b, a, weight);
                    DrawPixelWeighted(xx, yp, r, g, b, a, weight);

                    DrawPixelWeighted(xs, yy, r, g, b, a, weight);
                    DrawPixelWeighted(xx, yy, r, g, b, a, weight);
                }
            }

        }

        private void DrawQuadrant(int x, int y, int dx, int dy, bool filled, byte r, byte g, byte b, byte a)
        {
            int xpdx, xmdx;
            int ypdy, ymdy;

            if (dx == 0)
            {
                if (dy == 0)
                {
                    DrawPixel(x, y, r, g, b, a);
                }
                else
                {
                    ypdy = y + dy;
                    ymdy = y - dy;
                    if (filled)
                    {
                        DrawVLine(x, ymdy, ypdy, r, g, b, a);
                    }
                    else
                    {
                        DrawPixel(x, ypdy, r, g, b, a);
                        DrawPixel(x, ymdy, r, g, b, a);
                    }
                }
            }
            else
            {
                xpdx = x + dx;
                xmdx = x - dx;
                ypdy = y + dy;
                ymdy = y - dy;
                if (filled)
                {
                    DrawVLine(xpdx, ymdy, ypdy, r, g, b, a);
                    DrawVLine(xmdx, ymdy, ypdy, r, g, b, a);
                }
                else
                {
                    DrawPixel(xpdx, ypdy, r, g, b, a);
                    DrawPixel(xmdx, ypdy, r, g, b, a);
                    DrawPixel(xpdx, ymdy, r, g, b, a);
                    DrawPixel(xmdx, ymdy, r, g, b, a);
                }
            }
        }

        internal override void DrawRectangle(int x1, int x2, int y1, int y2, byte r, byte g, byte b, byte a, bool filled = false)
        {
            int result = 0;
            int tmp;
            SDL_Rect rect;

            /*
            * Test for special cases of straight lines or single point 
            */
            if (x1 == x2)
            {
                if (y1 == y2)
                {
                    DrawPixel(x1, y1, r, g, b, a);
                    return;
                }
                else
                {
                    DrawVLine(x1, y1, y2, r, g, b, a);
                    return;
                }
            }
            else
            {
                if (y1 == y2)
                {
                    DrawHLine(x1, x2, y1, r, g, b, a);
                    return;
                }
            }

            /*
            * Swap x1, x2 if required 
            */
            if (x1 > x2)
            {
                tmp = x1;
                x1 = x2;
                x2 = tmp;
            }

            /*
            * Swap y1, y2 if required 
            */
            if (y1 > y2)
            {
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            /* 
            * Create destination rect
            */
            rect.x = x1;
            rect.y = y1;
            rect.w = x2 - x1;
            rect.h = y2 - y1;

            /*
            * Draw
            */

            result |= SDL_SetRenderDrawBlendMode(Settings.RendererHandle, (a == 255) ? SDL_BlendMode.SDL_BLENDMODE_NONE : SDL_BlendMode.SDL_BLENDMODE_BLEND);
            result |= SDL_SetRenderDrawColor(Settings.RendererHandle, r, g, b, a);

            if (filled)
            {
                result |= SDL_RenderFillRect(Settings.RendererHandle, ref rect);
                
            }
            else
            {
                result |= SDL_RenderDrawRect(Settings.RendererHandle, ref rect);
            }

            if (result > 0)
            {
                NCError.ShowErrorBox($"An error occurred while drawing a line in SDL", 215, $"An error occurred in SdlRenderer::DrawRectangle - error code {result}. " +
                $"The rectangle will not be drawn!", NCErrorSeverity.Warning, null, true);
            }
        }

        internal override void DrawRoundedRectangle(int x1, int y1, int x2, int y2, int rad, byte r, byte g, byte b, byte a, bool filled)
        {
            int w, h, r2, tmp;
            int cx = 0;
            int cy = rad;
            int ocx = 0xffff;
            int ocy = 0xffff;
            int df = 1 - rad;
            int d_e = 3;
            int d_se = -2 * rad + 5;
            int xpcx, xmcx, xpcy, xmcy;
            int ypcy, ymcy, ypcx, ymcx;
            int x, y, dx, dy;

            /*
            * Check radius vor valid range
            */
            if (rad < 0)
            {
                return;
            }
            else if (rad <= 1)
            {
                /*
                * Special case - no rounding
                */
                DrawRectangle(x1, y1, x2, y2, r, g, b, a, true);
                return;
            }

            /*
            * Test for special cases of straight lines or single point 
            */
            if (x1 == x2)
            {
                if (y1 == y2)
                {
                    DrawPixel(x1, y1, r, g, b, a);
                    return;
                }
                else
                {
                    DrawVLine(x1, y1, y2, r, g, b, a);
                    return;
                }
            }
            else
            {
                if (y1 == y2)
                {
                    DrawHLine(x1, x2, y1, r, g, b, a);
                    return;
                }
            }

            /*
* Swap x1, x2 if required 
*/
            if (x1 > x2)
            {
                tmp = x1;
                x1 = x2;
                x2 = tmp;
            }

            /*
            * Swap y1, y2 if required 
            */
            if (y1 > y2)
            {
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            /*
            * Calculate width&height 
            */
            w = x2 - x1 + 1;
            h = y2 - y1 + 1;

            /*
            * Maybe adjust radius
            */
            r2 = rad + rad;
            if (r2 > w)
            {
                rad = w / 2;
                r2 = rad + rad;
            }
            if (r2 > h)
            {
                rad = h / 2;
            }

            /* Setup filled circle drawing for corners */
            x = x1 + rad;
            y = y1 + rad;
            dx = x2 - x1 - rad - rad;
            dy = y2 - y1 - rad - rad;

            /*
            * Draw corners
            */
            do
            {
                xpcx = x + cx;
                xmcx = x - cx;
                xpcy = x + cy;
                xmcy = x - cy;
                if (ocy != cy)
                {
                    if (cy > 0)
                    {
                        ypcy = y + cy;
                        ymcy = y - cy;
                        DrawHLine(xmcx, xpcx + dx, ypcy + dy, r, g, b, a);
                        DrawHLine(xmcx, xpcx + dx, ymcy, r, g, b, a);
                    }
                    else
                    {
                        DrawHLine(xmcx, xpcx + dx, y, r, g, b, a);
                    }
                    ocy = cy;
                }
                if (ocx != cx)
                {
                    if (cx != cy)
                    {
                        if (cx > 0)
                        {
                            ypcx = y + cx;
                            ymcx = y - cx;
                            DrawHLine(xmcy, xpcy + dx, ymcx, r, g, b, a);
                            DrawHLine(xmcy, xpcy + dx, ypcx + dy, r, g, b, a);
                        }
                        else
                        {
                            DrawHLine(xmcy, xpcy + dx, y, r, g, b, a);
                        }
                    }
                    ocx = cx;
                }

                /*
                * Update 
                */
                if (df < 0)
                {
                    df += d_e;
                    d_e += 2;
                    d_se += 2;
                }
                else
                {
                    df += d_se;
                    d_e += 2;
                    d_se += 4;
                    cy--;
                }
                cx++;
            } while (cx <= cy);

            /* Inside */
            if (dx > 0 && dy > 0) DrawRectangle(x1, y1 + rad + 1, x2, y2 - rad, r, g, b, a, filled);
        }

        internal override void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, byte r, byte g, byte b, byte a, bool filled)
        {
            int[] vx = new int[3];
            int[] vy = new int[3];

            vx[0] = x1;
            vx[1] = x2;
            vx[2] = x3;
            vy[0] = y1;
            vy[1] = y2;
            vy[2] = y3;

            DrawPolygon(vx, vy, r, g, b, a, filled);
        }

        internal override void DrawPolygon(int[] vx, int[] vy, byte r, byte g, byte b, byte a, bool filled)
        {
            // use x l length (we check it later)
            int n = vx.Length;

            /*
            * Vertex array NULL check 
            */
            if (vx.Length == 0
                || vy.Length == 0
                || vx.Length != vy.Length)
            {
                return;
            }
            /*
            * Sanity check 
            */
            if (n < 3)
            {
                // TODO: error message
                return;
            }

            for (int i = 0; (i < n - 1); i++)
            {
                DrawLine(vx[i], vy[i], vx[i + 1], vy[i + 1], r, g, b, a);
            }

            // this bit was written by me (because the original code is insanely optimised to use presorting and various other memory allocation tricks
            // and i cant be assed to port it to c#

            if (filled)
            {
                // This algorithm exploits the fact we know where the coordinates are.
                // We predict where the line is and then fill in.

                for (int polyNum = 0; polyNum < (n - 1); polyNum++)
                {
                    int diffY = 0;
                    int startX = 0, endX = 0;
                    int startY = 0, endY = 0;

                    // calculate x coord to fill
                    int curX = vx[polyNum];
                    int curY = vy[polyNum];

                    int nextX = vx[polyNum + 1];
                    int nextY = vy[polyNum + 1];

                    // calculate the start and end points
                    if (curX > nextX)
                    {
                        startX = nextX;
                        endX = curX;
                    }
                    else
                    {
                        startX = curX;
                        endX = nextX;
                    }

                    if (curX > nextX)
                    {
                        diffY = curY - nextY;
                        startY = nextY;
                        endY = curY;
                    }
                    else
                    {
                        diffY = nextY - curY;
                        startX = curY;
                        endY = nextY;
                    }

                    for (int y = startY; y <= endY; y++)
                    {
                        // calculate where the line ends for the current scanline
                        int realEndX = Convert.ToInt32(startX + ((endX - startX) * Convert.ToDouble(diffY / (y - startY))));

                        for (int x = startX; x <= realEndX; x++)
                        {
                            DrawPixel(x, y, r, g, b, a);
                        }
                    }
                }
            }

        }

        #endregion
    }
}