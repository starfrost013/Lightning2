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

        private bool _enabled;

        /// <summary>
        /// Determines if this console is enabled.
        /// </summary>
        private bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                Debug.Assert(DebugText != null
                    && DebugRectangle != null);

                _enabled = value;
                DebugText.IsNotRendering = !value;
                DebugRectangle.IsNotRendering = !value;
            }
        }

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
        /// <summary>
        /// The default foreground colour for the debug screen.
        /// Maybe make configurable?
        /// </summary>
        private readonly Color DebugForeground = Color.Black;

        /// <summary>
        /// The default background colour for the debug screen.
        /// Maybe make configurable?
        /// </summary>
        private readonly Color DebugBackground = Color.FromArgb(127, Color.White);

        /// <summary>
        /// The debug text block.
        /// </summary>
        private TextBlock? DebugText { get; set; }

        /// <summary>
        /// Used to make the debug text easier to see.
        /// </summary>
        private Rectangle? DebugRectangle { get; set; }

        /// <summary>
        /// Maximum number of renderables for the renderables view.
        /// </summary>
        private const int MAX_RENDERABLES = 5000;

        /// <summary>
        /// Default padding for child renderables.
        /// </summary>
        private const int DEFAULT_PAD_LEFT = 8;

        /// <summary>
        /// Default name for input binding to toggle the debug viewer.
        /// </summary>
        private const string BINDING_TRIGGER_NAME = "DEBUGDISPLAYTRIGGER";

        /// <summary>
        /// Default name for input binding to go up one page in the debug viewer.
        /// </summary>
        private const string BINDING_PAGE_UP_NAME = "DEBUGDISPLAYPAGEUP";

        /// <summary>
        /// Default name for input binding to go down one page in the debug viewer.
        /// </summary>
        private const string BINDING_PAGE_DOWN_NAME = "DEBUGDISPLAYPAGEDOWN";


        public DebugViewer(string name) : base(name)
        {
            OnKeyDown += KeyDown;
        }

        public override void Create()
        {
            Logger.Log("Loading debug font...");
            // Load the debug font.
            // My lazy cleaning up hack makes me not put the debug font as a logical child of the debug viewer.
            Lightning.Tree.AddRenderable(new Font("Arial.ttf", GlobalSettings.DebugFontSize, "DebugFont"));

            DebugText = Lightning.Tree.AddRenderable(new TextBlock("DebugText", "(PLACEHOLDER)", "DebugFont",
                new((float)GlobalSettings.DebugPositionX, (float)GlobalSettings.DebugPositionY), DebugForeground));
            DebugText.SnapToScreen = true;
            DebugText.Localise = false; // dont localise
            DebugText.ZIndex = ZIndex;
            DebugRectangle = Lightning.Tree.AddRenderable(new Rectangle("DebugRectangle", new((float)GlobalSettings.DebugPositionX, (float)GlobalSettings.DebugPositionY),
                new(GlobalSettings.GraphicsResolutionX, GlobalSettings.GraphicsResolutionY), DebugBackground, true, default, default, true));
            DebugRectangle.ZIndex = ZIndex - 1;
            Enabled = false; // explicitly set to turn off localise text
        }

        public override void Draw()
        {
            if (!Enabled) return;

            Debug.Assert(DebugText != null);

            // reset drawing
            // TODO: configurable colours

            int currentPage = (int)CurrentDebugView + 1;
            int maxPage = (int)DebugViews.MaxPage + 1;

            DebugText.Text = $"[Debug Menu]\nEngine version {LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING} " +
                $"(Page {currentPage}/{maxPage} - {CurrentDebugView})\n\n";

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

            string bigPictureText =
                $"FPS: {Lightning.Renderer.CurFPS:F1} ({Lightning.Renderer.DeltaTime:F2}ms)\n" +
                $"Renderer: {Lightning.Renderer.GetType().Name}\n" + 
                $"Frame #{Lightning.Renderer.FrameNumber}\n" +
                $"Number of renderables: {Lightning.Tree.CountRenderables()}\n" + // uses recursion so we have to call a method
                $"Number of renderables currently being rendered: {Lightning.Renderer.RenderedLastFrame}\n" +
                $"Delta time: {Lightning.Renderer.DeltaTime}\n" +

                // these 3 are same line 
               $"Current camera: {Lightning.Renderer.Settings.Camera.Type} @ {Lightning.Renderer.Settings.Camera.Position} " +
                $"(focus delta: {Lightning.Renderer.Settings.Camera.FocusDelta}, shake: {Lightning.Renderer.Settings.Camera.ShakeAmount}" +
                $"velocity {Lightning.Renderer.Settings.Camera.Velocity}, can move while shaking: {Lightning.Renderer.Settings.Camera.AllowCameraMoveOnShake})\n" +

                $"Current scene: {CurrentScene.Name}\n";

            DebugText.Text += bigPictureText;
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
                    DebugText.Text += $"{renderable.Name} ({renderable.GetType().Name}): position {renderable.Position:F2}, " + $"size: {renderable.Size:F2}, " +
                        $"render position: {renderable.RenderPosition:F2}, on screen: {renderable.IsOnScreen}, z-index: {renderable.ZIndex}, " + $"is animating now: {renderable.IsAnimating}\n";

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

            string sysInfoText = $"Resolution: {SystemInfo.ScreenResolutionX},{SystemInfo.ScreenResolutionY} (window size: {GlobalSettings.GraphicsResolutionX},{GlobalSettings.GraphicsResolutionY}\n" +
                $"Window flags: {GlobalSettings.GraphicsWindowFlags}, render flags: {GlobalSettings.GraphicsRenderFlags})\n" +
                $"SDL {SDL_EXPECTED_MAJOR_VERSION}.{SDL_EXPECTED_MINOR_VERSION}.{SDL_EXPECTED_PATCHLEVEL}\n" +
                $"SDL_image {SDL_IMAGE_EXPECTED_MAJOR_VERSION}.{SDL_IMAGE_EXPECTED_MINOR_VERSION}.{SDL_IMAGE_EXPECTED_PATCHLEVEL}\n" +
                $"SDL_mixer {SDL_MIXER_EXPECTED_MAJOR_VERSION}.{SDL_MIXER_EXPECTED_MINOR_VERSION}.{SDL_MIXER_EXPECTED_PATCHLEVEL}\n" +
                $"{SystemInfo.Cpu.ProcessArchitecture} Lightning on {SystemInfo.Cpu.SystemArchitecture} {SystemInfo.CurOperatingSystem}\n" +
                $"CPU Capabilities: {SystemInfo.Cpu.Capabilities}\n" +
                $"Hardware Threads (NOT cores!): {SystemInfo.Cpu.Threads}\n" +
                $"Total System RAM: {SystemInfo.SystemRam}MiB\n";

            DebugText.Text += sysInfoText;
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
                string debugText = $"FPS: {Lightning.Renderer.CurFPS:F1} ({Lightning.Renderer.DeltaTime:F2}ms)\n" +
                    $"0.1% High: {PerformanceProfiler.Current999thPercentile}\n" +
                    $"1% High: {PerformanceProfiler.Current99thPercentile}\n" +
                    $"5% High: {PerformanceProfiler.Current95thPercentile}\n" +
                    $"Average: {PerformanceProfiler.CurrentAverage:F1}\n" +
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

            foreach (string line in lines)
            {
                DebugText.Text += $"{line}\n";
            }
        }

        private void KeyDown(InputBinding? binding, Key key)
        {
            Debug.Assert(DebugText != null);

            if (binding == null) return;

            switch (binding.Name)
            {
                case BINDING_TRIGGER_NAME:
                    Enabled = !Enabled;
                    break;
                case BINDING_PAGE_UP_NAME:
                    CurrentDebugView--;
                    break;
                case BINDING_PAGE_DOWN_NAME:
                    CurrentDebugView++;
                    break;
            }

            if (CurrentDebugView < 0) CurrentDebugView = DebugViews.MaxPage;
            if (CurrentDebugView > DebugViews.MaxPage) CurrentDebugView = 0; 
        }
    }
}