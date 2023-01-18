namespace LightningGL
{
    /// <summary>
    /// DebugView
    /// 
    /// Detailed debugging information.
    /// </summary>
    internal class DebugViewer : Renderable
    {
        internal DebugViews CurrentDebugView { get; private set; }

        /// <summary>
        /// Private: Determines if the debug menu is enabled.
        /// </summary>
        private bool Enabled { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// 
        /// <para>Always true for DebugViewer</para>
        /// </summary>
        public override bool CanReceiveEventsWhileUnfocused => true;

        /// <summary>
        /// <inheritdoc/>
        /// 
        /// <para>Always true for DebugViewer</para>
        /// </summary>
        public override bool SnapToScreen => true;

        /// <summary>
        /// <inheritdoc/>
        /// 
        /// <para>Always true for DebugViewer</para>
        /// </summary>
        public override bool NotCullable => true;

        /// <summary>
        /// <inheritdoc/>. For DebugViewer, force to top.
        /// </summary>
        public override int ZIndex => 2147483647;

        // maybe make configurable?
        private readonly Color DebugForeground = Color.Blue;

        private readonly Color DebugBackground = Color.FromArgb(0, Color.White);

        /// <summary>
        /// The debug text block.
        /// </summary>
        private TextBlock? DebugText { get; set; }

        /// <summary>
        /// Maximum number of renderables for the renderables view.
        /// </summary>
        private const int MAX_RENDERABLES = 5000;

        /// <summary>
        /// Default padding for child renderables.
        /// </summary>
        private const int DEFAULT_PAD_LEFT = 8;

        public DebugViewer(string name) : base(name)
        {
            OnKeyPressed += KeyPressed;
        }

        public override void Create()
        {
            NCLogging.Log("Loading debug font...");
            // Load the debug font.
            // My lazy cleaning up hack makes me not put the debug font as a logical child of the debug viewer.
            Lightning.Renderer.AddRenderable(new Font("Arial.ttf", GlobalSettings.DebugFontSize, "DebugFont"));

            DebugText = Lightning.Renderer.AddRenderable(new TextBlock("DebugText", "(PLACEHOLDER)", "DebugFont",
                new(0, (float)GlobalSettings.GraphicsLineSpacing * 3), DebugForeground, DebugBackground));
            DebugText.SnapToScreen = true;
            DebugText.Localise = false; // dont localise
            DebugText.IsNotRendering = !Enabled; 
        }

        public override void Draw()
        {
            if (!Enabled) return;

            Debug.Assert(DebugText != null);

            // reset drawing
            // TODO: configurable colours

            int currentPage = (int)CurrentDebugView + 1;
            int maxPage = (int)DebugViews.MaxPage + 1;

            DebugText.Text = $"Lightning Debug v{LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING} " +
                $"(Debug page {currentPage}/{maxPage} - {CurrentDebugView})\n";

            switch (CurrentDebugView)
            {
                case DebugViews.BigPicture:
                    DrawBigPictureView();
                    break;
                case DebugViews.RenderableDetails:
                    DrawRenderableDetailsView();
                    break;
                case DebugViews.SystemInformation:
                    DrawSystemInformationView();
                    break;
                case DebugViews.Performance:
                    DrawPerformanceView();
                    break;
                case DebugViews.GlobalSettings:
                    DrawGlobalSettingsView();
                    break;
            }
        }

        private void DrawBigPictureView()
        {
            Debug.Assert(CurrentScene != null
                && DebugText != null);

            string debugText =
                $"FPS: {Lightning.Renderer.CurFPS:F1} ({Lightning.Renderer.DeltaTime:F2}ms)\n" +
                $"Renderer: {Lightning.Renderer.GetType().Name}\n" + 
                $"Frame #{Lightning.Renderer.FrameNumber}\n" +
                $"Number of renderables: {Lightning.Renderer.CountRenderables()}\n" + // uses recursion so we have to call a method
                $"Number of renderables on screen: {Lightning.Renderer.RenderedLastFrame}\n" +
                $"Delta time: {Lightning.Renderer.DeltaTime}\n" +
                $"Camera position: {Lightning.Renderer.Settings.Camera.Position}\n";

            DebugText.Text += debugText;

            DebugText.Text += $"Current scene: {CurrentScene.Name}\n";

            int maxFps = (GlobalSettings.GraphicsMaxFPS == 0) ? 60 : GlobalSettings.GraphicsMaxFPS;

            // draw indicator that we are under 60fps always under it
            if (Lightning.Renderer.CurFPS < maxFps)
            {
                DebugText.Text += $"Running under target FPS ({maxFps})!\n";
            }
        }

        private void DrawRenderableDetailsView()
        {
            Debug.Assert(DebugText != null);

            // bail too many renderables
            if (Lightning.Renderer.Renderables.Count > MAX_RENDERABLES)
            {
                DebugText.Text += "Something went wrong, renderable vomiting in progress (>5000 renderables in scene!!!!)\n";
            }
            else
            {
                DebugText.Text += $"Camera Position: {Lightning.Renderer.Settings.Camera.Position}\n";
        
                for (int renderableId = 0; renderableId < Lightning.Renderer.Renderables.Count; renderableId++)
                {
                    Renderable renderable = Lightning.Renderer.Renderables[renderableId];
                    DebugText.Text += $"{renderable.Name} ({renderable.GetType().Name}): position {renderable.Position}, " + $"size: {renderable.Size}, " +
                        $"render position: {renderable.RenderPosition}, on screen: {renderable.IsOnScreen}, z-index: {renderable.ZIndex}, " + $"is animating now: {renderable.IsAnimating}\n";

                    if (renderable.Children.Count > 0) DrawRenderableChildren(renderable);
                }
            }
        }

        private void DrawRenderableChildren(Renderable parent, int depth = 1)
        {
            Debug.Assert(DebugText != null);

            foreach (Renderable renderable in parent.Children)
            {
                string initialString = $"{renderable.Name} ({renderable.GetType().Name}): position {renderable.Position}, " +
                    $"size: {renderable.Size}, render position: {renderable.RenderPosition}, on screen: {renderable.IsOnScreen}, z-index: {renderable.ZIndex}, " +
                    $"is animating now: {renderable.IsAnimating} - parent {parent.Name}\n";

                // string::format requires constants so we need to pad to the left
                initialString = initialString.PadLeft(initialString.Length + (DEFAULT_PAD_LEFT * depth)); // todo: make this a setting with a defauilt value

                DebugText.Text += initialString; 
                
                if (renderable.Children.Count > 0) DrawRenderableChildren(renderable, depth++);
            }
        }

        private void DrawSystemInformationView()
        {
            Debug.Assert(DebugText != null);

            string debugText = $"Resolution: {SystemInfo.ScreenResolutionX},{SystemInfo.ScreenResolutionY} (window size: {GlobalSettings.GraphicsResolutionX},{GlobalSettings.GraphicsResolutionY}\n" +
                $"Window flags: {GlobalSettings.GraphicsWindowFlags}, render flags: {GlobalSettings.GraphicsRenderFlags})\n" +
                $"SDL {SDL_EXPECTED_MAJOR_VERSION}.{SDL_EXPECTED_MINOR_VERSION}.{SDL_EXPECTED_PATCHLEVEL}\n" +
                $"SDL_image {SDL_IMAGE_EXPECTED_MAJOR_VERSION}.{SDL_IMAGE_EXPECTED_MINOR_VERSION}.{SDL_IMAGE_EXPECTED_PATCHLEVEL}\n" +
                $"SDL_mixer {SDL_MIXER_EXPECTED_MAJOR_VERSION}.{SDL_MIXER_EXPECTED_MINOR_VERSION}.{SDL_MIXER_EXPECTED_PATCHLEVEL}\n" +
                $"{SystemInfo.Cpu.ProcessArchitecture} Lightning on {SystemInfo.Cpu.SystemArchitecture} {SystemInfo.CurOperatingSystem}\n" +
                $"CPU Capabilities: {SystemInfo.Cpu.Capabilities}\n" +
                $"Hardware Threads (NOT cores!): {SystemInfo.Cpu.Threads}\n" +
                $"Total System RAM: {SystemInfo.SystemRam}MiB\n";

            DebugText.Text += debugText;
        }

        private void DrawPerformanceView()
        {
            Debug.Assert(DebugText != null);

            if (!GlobalSettings.GeneralProfilePerformance)
            {
                DebugText.Text += "This page is disabled as the Performance Profiler is not enabled!\n";
            }
            else
            {
                string debugText = $"FPS: {Lightning.Renderer.CurFPS.ToString("F1")} ({Lightning.Renderer.DeltaTime:F2}ms)\n" +
                    $"0.1% High: {PerformanceProfiler.Current999thPercentile}\n" +
                    $"1% High: {PerformanceProfiler.Current99thPercentile}\n" +
                    $"5% High: {PerformanceProfiler.Current95thPercentile}\n" +
                    $"Average: {PerformanceProfiler.CurrentAverage}\n" +
                    $"5% Low: {PerformanceProfiler.Current5thPercentile}\n" +
                    $"1% Low: {PerformanceProfiler.Current1stPercentile}\n" +
                    $"0.1% Low: {PerformanceProfiler.Current01thPercentile}\n";

                DebugText.Text += debugText;
            }
        }

        private void DrawGlobalSettingsView()
        {
            Debug.Assert(DebugText != null);

            // this file must exist
            string[] lines = File.ReadAllLines(GlobalSettings.GLOBALSETTINGS_PATH);

            DebugText.Text += lines;
        }

        private void KeyPressed(Key key)
        {
            Debug.Assert(DebugText != null);

            string keyString = key.ToString();

            // case has to be a compile time constant so we do thos
            if (keyString == GlobalSettings.DebugKey)
            {
                Enabled = !Enabled;
                DebugText.IsNotRendering = !Enabled; 
            }

            switch (keyString)
            {
                case "PAGEUP":
                    CurrentDebugView--;
                    break;
                case "PAGEDOWN":
                    CurrentDebugView++;
                    break;
            }

            if (CurrentDebugView < 0) CurrentDebugView = DebugViews.MaxPage;
            if (CurrentDebugView > DebugViews.MaxPage) CurrentDebugView = 0; 
        }
    }
}