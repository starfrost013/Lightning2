global using static LightningGL.Lightning;

namespace LightningGL
{
    /// <summary>
    /// Defines a LightningGL Window. 
    /// </summary>
    public class SdlRenderer : Renderer
    {
        [Obsolete("Use Lightning events instead, this property will be removed in Version 2.5.0")]
        /// <summary>
        /// The last processed SDL event. Only valid if .Update() is called.
        /// </summary>
        public SDL_Event LastEvent { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override SdlRendererSettings Settings => (SdlRendererSettings)base.Settings;

        // no idea where to put this now (temporary SDL_gfx stuff)
        private const int DEFAULT_AA_BITS = 8;

        private const int DEFAULT_ELLIPSE_OVERSCAN = 4;

        public SdlRenderer() : base()
        {

            Settings = new SdlRendererSettings();
        }

        /// <summary>
        /// Starts this window.
        /// </summary>
        /// <param name="windowSettings">The window settings to use when starting this window - see <see cref="RendererSettings"/></param>
        internal override void Start()
        {
            base.Start();

            Logger.Log("Initialising SDL_image...");
            if (IMG_Init(IMG_InitFlags.IMG_INIT_EVERYTHING) < 0) Logger.LogError($"Error initialising SDL2_image: {SDL_GetError()}", 201, LoggerSeverity.FatalError);

            Logger.Log("Initialising SDL_mixer...");
            if (Mix_Init(MIX_InitFlags.MIX_INIT_EVERYTHING) < 0) Logger.LogError($"Error initialising SDL2_mixer: {SDL_GetError()}", 203, LoggerSeverity.FatalError);

            Logger.Log($"Initialising audio device ({GlobalSettings.AudioDeviceHz}Hz, {GlobalSettings.AudioChannels} channels, format {GlobalSettings.AudioFormat}, chunk size {GlobalSettings.AudioChunkSize})...");
            if (Mix_OpenAudio(GlobalSettings.AudioDeviceHz, GlobalSettings.AudioFormat, GlobalSettings.AudioChannels, GlobalSettings.AudioChunkSize) < 0) Logger.LogError(
                $"Error initialising audio device: {SDL_GetError()}", 56, LoggerSeverity.FatalError);

            Logger.Log($"Allocating {GlobalSettings.AudioMaxSimultaneousAudioFiles} audio tracks");
            Mix_AllocateChannels(GlobalSettings.AudioMaxSimultaneousAudioFiles);

            // localise the window title
            Settings.Title = LocalisationManager.ProcessString(Settings.Title);

            // Create the window,
            Settings.WindowHandle = SDL_CreateWindow(Settings.Title, (int)Settings.Position.X, (int)Settings.Position.Y, (int)Settings.Size.X, (int)Settings.Size.Y, Settings.WindowFlags);

            if (Settings.WindowHandle == nint.Zero) Logger.LogError($"Failed to create Window: {SDL_GetError()}", 8, LoggerSeverity.FatalError);

            // Create the renderer.
            Settings.RendererHandle = SDL_CreateRenderer(Settings.WindowHandle, -1, Settings.RenderFlags);

            if (Settings.RendererHandle == nint.Zero) Logger.LogError($"Failed to create Renderer: {SDL_GetError()}", 9, LoggerSeverity.FatalError);

            // Initialise the Light Manager.
            LightManager.Init();

            // maybe move this somewhere else
            if (GlobalSettings.DebugState.HasFlag(DebugState.DebugViewer)) AddRenderable(new DebugViewer("DebugViewer"));
            if (GlobalSettings.DebugState.HasFlag(DebugState.DebugConsole)) AddRenderable(new DebugConsole("DebugConsole"));

            EventsRunning = true;
        }

        /// <summary>
        /// Runs the main loop at the start of each frame.
        /// </summary>
        /// <returns>A boolean determining if the window is to keep running or close.</returns>
        internal override bool Run()
        {
#if PROFILING
            ProfilingTimers.Clear.Start();
#endif
            // clear the renderet
            FrameTimer.Restart();
            SDL_RenderClear(Settings.RendererHandle);

#if PROFILING
            ProfilingTimers.Clear.Stop();
            ProfilingTimers.EventHandling.Start();
#endif
            // Reset rendered this frame count
            RenderedLastFrame = 0;

            EventWaiting = (SDL_PollEvent(out var currentEvent) > 0);

            // default mainloop
            if (EventWaiting
                && EventsRunning)
            {
                LastEvent = currentEvent;

                // default rendering loop.
                // Developers can choose to handle SDL events after this
                switch (currentEvent.type)
                {
                    case SDL_EventType.SDL_KEYDOWN:
                        EventManager.FireOnKeyDown((Key)currentEvent.key);
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        EventManager.FireOnKeyUp((Key)currentEvent.key);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN: // Mouse down event
                        EventManager.FireOnMousePressed((MouseButton)currentEvent.button);
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP: // Mouse up event
                        EventManager.FireOnMouseReleased((MouseButton)currentEvent.button);
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION: // Mouse move event
                        EventManager.FireOnMouseMove((MouseButton)currentEvent.motion);
                        break;
                    case SDL_EventType.SDL_MOUSEWHEEL:
                        EventManager.FireOnMouseWheel((MouseButton)currentEvent.wheel);
                        break;
                    case SDL_EventType.SDL_CONTROLLERBUTTONDOWN:
                        EventManager.FireOnControllerButtonDown((ControllerButton)currentEvent.cbutton);
                        break;
                    case SDL_EventType.SDL_CONTROLLERBUTTONUP:
                        EventManager.FireOnControllerButtonUp((ControllerButton)currentEvent.cbutton);
                        break;
                    case SDL_EventType.SDL_CONTROLLERAXISMOTION:
                        EventManager.FireOnControllerButtonDown((ControllerButton)currentEvent.caxis);
                        break;
                    case SDL_EventType.SDL_CONTROLLERDEVICEADDED:
                    case SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
                        Logger.Log($"Controller device changed, rescanning input methods");
                        InputMethodManager.ScanAvailableInputMethods();
                        break;
                    case SDL_EventType.SDL_WINDOWEVENT: // Window Event - check subtypes
                        switch (currentEvent.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
                                EventManager.FireOnMouseEnter();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
                                EventManager.FireOnMouseLeave();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                                EventManager.FireOnFocusGained();
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                                EventManager.FireOnFocusLost();
                                break;
                        }

                        break;
                    case SDL_EventType.SDL_QUIT: // User requested a quit, so shut down
                        Lightning.Shutdown();
                        return false;
                }
            }

#if PROFILING
            ProfilingTimers.EventHandling.Stop();
            ProfilingTimers.RunScene.Start();
#endif
            return true;
        }

        /// <summary>
        /// Manages the render loop.
        /// </summary>
        internal override void Render()
        {
#if PROFILING
            ProfilingTimers.RunScene.Stop();
            ProfilingTimers.Order.Start();
#endif
            // only render if we have a scene
            Debug.Assert(CurrentScene != null);

            base.Render();

#if PROFILING
            ProfilingTimers.UpdateCamera.Stop();
            ProfilingTimers.Present.Start();
#endif
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

#if PROFILING
            ProfilingTimers.Present.Stop();
            ProfilingTimers.EndOfFrame();
#endif
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

            Logger.Log("Destroying renderer...");
            SDL_DestroyRenderer(Settings.RendererHandle);
            SDL_DestroyWindow(Settings.WindowHandle);
            Logger.Log("Shutting down SDL_mixer...");
            Mix_Quit();

            Logger.Log("Shutting down SDL_image...");
            IMG_Quit();
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
                Logger.LogError($"An error occurred while drawing a pixel in SDL", 213, LoggerSeverity.Warning, null, true);
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

        internal override void DrawLine(int x1, int y1, int x2, int y2, byte r, byte g, byte b, byte a)
        {
                /*
                * Draw
                */
                int result = 0;

                result |= SDL_SetRenderDrawBlendMode(Settings.RendererHandle, (a == 255) ? SDL_BlendMode.SDL_BLENDMODE_NONE : SDL_BlendMode.SDL_BLENDMODE_BLEND);
                result |= SDL_SetRenderDrawColor(Settings.RendererHandle, r, g, b, a);
                result |= SDL_RenderDrawLine(Settings.RendererHandle, x1, y1, x2, y2);
                result |= SDL_SetRenderDrawColor(Settings.RendererHandle, 0, 0, 0, 255);

                if (result != 0)
                {
                    Logger.LogError($"An error occurred while drawing a line in SDL. The pixel will not be drawn!", 233, LoggerSeverity.Warning, null, true);
                }
            
        }

        private void DrawHLine(int x1, int x2, int y, byte r, byte g, byte b, byte a) => DrawLine(x1, y, x2, y, r, g, b, a);

        private void DrawVLine(int x, int y1, int y2, byte r, byte g, byte b, byte a) => DrawLine(x, y1, x, y2, r, g, b, a);

        internal override void DrawEllipse(int x, int y, int rx, int ry, byte r, byte g, byte b, byte a, bool filled)
        {
            // Honestly, I don't know what this code does, really.
            // It's some public domain code manually transpiled it from C to C#, it makes about as much sense to me as it does to you.

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

        internal override void DrawRectangle(Vector2 position, Vector2 size, byte r, byte g, byte b, byte a, bool filled = false)
        {
            int result = 0;
            SDL_Rect rect;
            /* 
            * Create destination rect
            */
            rect.x = (int)position.X;
            rect.y = (int)position.Y;
            rect.w = (int)size.X;
            rect.h = (int)size.Y;

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
                Logger.LogError($"An error occurred while drawing a rectangle in SDL. he rectangle will not be drawn!", 215,LoggerSeverity.Warning, null, true);
            }
        }



        #endregion

        #region Backend-specific texture code

        internal override nint CreateTexture(int sizeX, int sizeY, bool isTarget = false)
        {
            nint handle = SDL_CreateTexture(Settings.RendererHandle, SDL_PIXELFORMAT_ARGB8888,
            isTarget ? SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET : SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, sizeX, sizeY);

            if (handle == nint.Zero)
            {
                Logger.LogError($"Somehow failed to create a texture (call to SdlRenderer::CreateTexture failed): {SDL.SDL_GetError()}", 274, LoggerSeverity.FatalError);
                // fatal and would return a nullptr in all cases
            }

            return handle;
        }

        internal override nint AllocTextureFormat()
        {
            uint currentFormat = SDL_GetWindowPixelFormat(Settings.WindowHandle);

            nint handle = SDL_AllocFormat(currentFormat);

            // probably not the best to allocate formats like this (once for each texture)
            if (handle == nint.Zero) Logger.LogError($"Error allocating texture format for texture: {SDL_GetError()}",
                13, LoggerSeverity.FatalError);

            return handle;
        }

        internal override unsafe nint LoadTexture(string path)
        {
            nint handle = IMG_LoadTexture(Settings.RendererHandle, path);

            // this is here for now

            if (handle == nint.Zero)
            {
                // Print an error message and load a 'default' texture (see MissingTexture) as of 12/31/22
                Logger.LogError($"Failed to load texture at {path} - {SDL_GetError()}", 10, LoggerSeverity.Error, null, true);

                fixed (byte* missingTexturePtr = MissingTexture.Data)
                {
                    nint missingTexture = (nint)missingTexturePtr;
                    nint missingTextureRw = SDL_RWFromMem(missingTexture, MissingTexture.Data.Length);

                    handle = IMG_LoadTexture_RW(Settings.RendererHandle, missingTextureRw, true);
                }
               
                // we have to check again to see if the missing texture texture failed to load
                if (handle == nint.Zero)
                {
                    Logger.LogError($"Failed to load texture at {path} - {SDL_GetError()} AND failed to load the missing texture texture. " +
                        $"THIS IS AN ENGINE BUG", 271, LoggerSeverity.FatalError);
                }
            }

            return handle;
        }

        internal unsafe override Texture? TextureFromFreetypeBitmap(FT_Bitmap bitmap, Texture texture, Color foregroundColor)
        {
            if (!texture.Loaded)
            {
                Logger.LogError($"Passed an unloaded texture to SdlRenderer::TextureFromFreeTypeBitmap", 256, LoggerSeverity.FatalError);
                return null;
            }

            byte* bufferPtr = (byte*)bitmap.buffer.ToPointer();

            // we have to use SDL_BLENDMODE_BLEND to render text
            // this has to be in a better place
            texture.SetBlendMode(SDL_BlendMode.SDL_BLENDMODE_BLEND);

            // When FreeType renders a glyph, it actually creates an "alpha map" of the alpha values of the glyph that we have rendered in INDEX8 format.
            // Lightning uses ARGB8888, so we need to convert to ARGB8888 using the colour the user specified.
            // and then plot the pixels of the *RGB* component of the surface, the alpha already having been set for us by freetype.
            // convert to a pointer so we can get the alpha

            texture.Lock();

            // Now we actually set the pixels to the colour required
            // convert to ARGB

            int pixelId = 0;

            for (int y = 0; y < texture.Size.Y; y++)
            {
                for (int x = 0; x < texture.Size.X; x++)
                {
                    pixelId = (bitmap.pitch * y) + x;
                    // get the alpha
                    byte alpha = bufferPtr[pixelId];

                    // use the font colour
                    texture.SetPixel(x, y, Color.FromArgb(alpha, foregroundColor.R, foregroundColor.G, foregroundColor.B));
                }
            }

            texture.Unlock();
            return texture; 
        }

        internal override void LockTexture(nint handle, Vector2 start, Vector2 size, out nint pixels, out int pitch)
        {
            if (handle == nint.Zero)
            {
                Logger.LogError("Attempted to lock an invalid texture!", 226, LoggerSeverity.FatalError);
                pixels = default;
                pitch = 0;
            }
            else
            {
                SDL_Rect rect = new((int)start.X, (int)start.Y, (int)size.X, (int)size.Y);
                
                if (SDL_LockTexture(handle, ref rect, out pixels, out pitch) != 0)
                {
                    Logger.LogError("Failed to lock texture!", 228, LoggerSeverity.FatalError);
                    pixels = default;
                    pitch = 0;
                }
            }
        }

        internal override void UnlockTexture(nint handle)
        {
            if (handle == nint.Zero)
            {
                Logger.LogError("Attempted to unlock an invalid texture!", 227, LoggerSeverity.FatalError);
            }
            else
            {
                SDL_UnlockTexture(handle);
            }
        }

        internal override void DrawTexture(params object[] args)
        {
            // Parameters:
            // Argument 0 - Vector2 - viewport start
            // Argument 1 - Vector2 - viewport end
            // Argument 2 - Vector2 - render position
            // Argument 3 - Vector2 - size
            // Argument 4 - nint - Handle
            // Argument 5 - Vector2 - Repeat
            
            int numberOfArgs = 6;

            if (args.Length != numberOfArgs
                || args[0] is not Vector2
                || args[1] is not Vector2
                || args[2] is not Vector2
                || args[3] is not Vector2
                || args[4] is not nint
                || args[5] is not Vector2)
            {
                Logger.LogError($"CODE IS BORKED! Incorrect parameter types or invalid number of parameters to SdlRenderer::DrawTexture!\n\n" +
                    $"THIS IS AN ENGINE BUG PLEASE FILE A BUG REPORT!", 229, LoggerSeverity.FatalError);
                return;
            }

            Vector2 viewportStart = (Vector2)args[0];
            Vector2 viewportEnd = (Vector2)args[1];
            Vector2 renderPosition = (Vector2)args[2];
            Vector2 size = (Vector2)args[3];
            nint handle = (nint)args[4];
            Vector2 repeat = (Vector2)args[5];

            SDL_Rect sourceRect = new();
            SDL_FRect destinationRect = new();

            // Draw to the viewpoint
            if (viewportStart == default
                && viewportEnd == default)
            {
                sourceRect.x = 0;
                sourceRect.y = 0;
                sourceRect.w = (int)size.X;
                sourceRect.h = (int)size.Y;

                destinationRect.x = renderPosition.X;
                destinationRect.y = renderPosition.Y;
                destinationRect.w = size.X;
                destinationRect.h = size.Y;
            }
            else
            {
                sourceRect.x = (int)viewportStart.X;
                sourceRect.y = (int)viewportStart.Y;
                sourceRect.w = (int)(viewportEnd.X - viewportStart.X);
                sourceRect.h = (int)(viewportEnd.Y - viewportStart.Y);

                destinationRect.x = renderPosition.X;
                destinationRect.y = renderPosition.Y;
                destinationRect.w = viewportEnd.X - viewportStart.X;
                destinationRect.h = viewportEnd.Y - viewportStart.Y;
            }

            if (repeat == default)
            {
                // call to SDL - we are simply drawing it once.
                SDL_RenderCopyF(Settings.RendererHandle, handle, ref sourceRect, ref destinationRect);
            }
            else
            {
                SDL_FRect newRect = new(destinationRect.x, destinationRect.y, destinationRect.w, destinationRect.h);

                // Draws a tiled texture.
                for (int y = 0; y < repeat.Y; y++)
                {
                    SDL_RenderCopyF(Settings.RendererHandle, handle, ref sourceRect, ref newRect);

                    for (int x = 0; x < repeat.X; x++)
                    {
                        SDL_RenderCopyF(Settings.RendererHandle, handle, ref sourceRect, ref newRect);

                        newRect.x += destinationRect.w;
                    }

                    newRect.y += destinationRect.h; // we already set it up
                    newRect.x = destinationRect.x;
                }
            }
        }

        internal override void SetTextureBlendMode(params object[] args)
        {
            // Parameters:
            // Argument 0 - nint - Handle
            // Argument 1 - SDL_BlendMode - BlendMode 

            int numOfArgs = 2;

            if (args.Length != numOfArgs
                || args[0] is not nint
                || args[1] is not SDL_BlendMode)
            {
                Logger.LogError($"CODE IS BORKED! Incorrect parameter types or invalid number of parameters to SdlRenderer::SetTextureBlendMode!\n\n" +
                    $"THIS IS AN ENGINE BUG, PLEASE FILE A BUG REPORT!", 230, LoggerSeverity.FatalError);
                return;
            }

            nint handle = (nint)args[0];
            SDL_BlendMode blendMode = (SDL_BlendMode)args[1];

            if (SDL_SetTextureBlendMode(handle, blendMode) != 0)
            {
                Logger.LogError("Failed to set texture blend mode!", 231, LoggerSeverity.FatalError);
            }
        }

        internal override nint DestroyTexture(nint handle)
        {
            SDL_DestroyTexture(handle);
            handle = nint.Zero;
            return handle; 
        }

        #endregion
    }
}