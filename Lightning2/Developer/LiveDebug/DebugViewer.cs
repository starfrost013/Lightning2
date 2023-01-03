using LightningBase;

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
        /// <inheritdoc/>
        /// </summary>
        public override int ZIndex => 2147483647;

        /// <summary>
        /// Current Y for drawing
        /// </summary>
        private float CurrentY { get; set; }

        // maybe make configurable?
        private readonly Color DebugForeground = Color.Blue;

        private readonly Color DebugBackground = Color.FromArgb(0, Color.White);

        private int LineSpacing => (int)(GlobalSettings.DebugFontSize * GlobalSettings.GraphicsLineSpacing);

        /// <summary>
        /// Maximum number of renderables for the renderables view.
        /// </summary>
        private const int MAX_RENDERABLES = 5000;

        public DebugViewer(string name) : base(name)
        {
            OnKeyPressed += KeyPressed;
        }

        public override void Create()
        {
            NCLogging.Log("Loading debug font...");
            // Load the debug font.
            // My lazy cleaning up hack makes me not put the debug font as a child of the debug viewer.
            Lightning.Renderer.AddRenderable(new Font("Arial.ttf", GlobalSettings.DebugFontSize, "DebugFont"));
        }

        public override void Draw()
        {
            if (!Enabled) return;

            // reset drawing
            // TODO: configurable colours

            CurrentY = 0;

            int currentPage = (int)CurrentDebugView + 1;
            int maxPage = (int)DebugViews.MaxPage + 1;

            Lightning.Renderer.AddRenderable(new TextBlock("DebugText1", $"Lightning Debug v{LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING} (Debug page {currentPage}/{maxPage} - {CurrentDebugView})", 
                "DebugFont", new(0, CurrentY), DebugForeground, DebugBackground, FontStyle.Normal, -1, FontSmoothingType.Default, true), this);

            CurrentY += LineSpacing;

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

            // lazy hack to get over the fact i cant be bothered to rewrite this to use the new text APIs
            Lightning.Renderer.RemoveAllChildren(this);
        }

        private void DrawBigPictureView()
        {
            Debug.Assert(CurrentScene != null);

            string[] debugText =
            {
                $"FPS: {Lightning.Renderer.CurFPS:F1} ({Lightning.Renderer.DeltaTime:F2}ms)",
                $"Renderer: {Lightning.Renderer.GetType().Name}",
                $"Frame #{Lightning.Renderer.FrameNumber}",
                $"Number of renderables: {Lightning.Renderer.CountRenderables()}", // uses recursion so we have to call a method
                $"Number of renderables on screen: {Lightning.Renderer.RenderedLastFrame}",
                $"Delta time: {Lightning.Renderer.DeltaTime}",
                $"Camera position: {Lightning.Renderer.Settings.Camera.Position}",
            };

            foreach (string line in debugText)
            {
                Lightning.Renderer.AddRenderable(new TextBlock("DebugText2", line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, FontStyle.Normal, 
                    -1, FontSmoothingType.Default, true), this);
                CurrentY += LineSpacing;
            }

            Lightning.Renderer.AddRenderable(new TextBlock("DebugText3", $"Current scene: {CurrentScene.Name}", "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, 
                FontStyle.Normal, -1, FontSmoothingType.Default, true), this);

            CurrentY += LineSpacing;

            // draw indicator that we are under 60fps always under it
            if (Lightning.Renderer.CurFPS < GlobalSettings.GraphicsMaxFPS)
            {
                CurrentY += LineSpacing;

                int maxFps = GlobalSettings.GraphicsMaxFPS;

                if (maxFps == 0) maxFps = 60;

                Lightning.Renderer.AddRenderable(new TextBlock("DebugText4", $"Running under target FPS ({maxFps})!", "DebugFont", new Vector2(0, CurrentY), 
                    DebugForeground, DebugBackground, FontStyle.Bold, -1, FontSmoothingType.Default, true), this);
                CurrentY += LineSpacing;
            }

        }

        private void DrawRenderableDetailsView()
        {
            // bail too many renderables
            if (Lightning.Renderer.Renderables.Count > MAX_RENDERABLES)
            {
                Lightning.Renderer.AddRenderable(new TextBlock("DebugText5", $"Something went wrong, renderable vomiting in progress (>5000 renderables in scene!!!!)", "DebugFont", new(0, CurrentY),
                    DebugForeground, DebugBackground, FontStyle.Bold, -1, FontSmoothingType.Default, true), this);
                CurrentY += LineSpacing;
            }
            else
            {
                Lightning.Renderer.AddRenderable(new TextBlock("DebugText6", $"Camera Position: {Lightning.Renderer.Settings.Camera.Position}", "DebugFont", new Vector2(0, CurrentY), DebugForeground, 
                    DebugBackground, FontStyle.Normal, -1, FontSmoothingType.Default, true), this);
                CurrentY += LineSpacing;

                for (int renderableId = 0; renderableId < Lightning.Renderer.Renderables.Count; renderableId++)
                {
                    Renderable renderable = Lightning.Renderer.Renderables[renderableId];
                    Lightning.Renderer.AddRenderable(new TextBlock("DebugText7", $"{renderable.Name} ({renderable.GetType().Name}): position {renderable.Position}, " + $"size: {renderable.Size}, " +
                        $"render position: {renderable.RenderPosition}, on screen: {renderable.IsOnScreen}, z-index: {renderable.ZIndex}, " + $"is animating now: {renderable.IsAnimating}", 
                        "DebugFont", new(0, CurrentY), DebugForeground, DebugBackground, FontStyle.Normal, -1, FontSmoothingType.Default, true), this);
                    CurrentY += LineSpacing;

                    if (renderable.Children.Count > 0) DrawRenderableChildren(renderable);
                }
            }
        }

        private void DrawRenderableChildren(Renderable parent, int depth = 1)
        {
            foreach (Renderable renderable in parent.Children)
            {
                //Renderable renderable = parent.Children[renderableId];

                string initialString = $"{renderable.Name} ({renderable.GetType().Name}): position {renderable.Position}, " +
                    $"size: {renderable.Size}, render position: {renderable.RenderPosition}, on screen: {renderable.IsOnScreen}, z-index: {renderable.ZIndex}, " +
                    $"is animating now: {renderable.IsAnimating} - parent {parent.Name}";

                // string::format requires constants so we need to pad to the left
                initialString = initialString.PadLeft(initialString.Length + (8 * depth)); // todo: make this a setting with a defauilt value

                Lightning.Renderer.AddRenderable(new TextBlock("DebugText8", initialString, "DebugFont", new(0, CurrentY),
                    DebugForeground, DebugBackground, FontStyle.Normal, -1, FontSmoothingType.Default, true));
                CurrentY += LineSpacing;
                
                if (renderable.Children.Count > 0) DrawRenderableChildren(renderable, depth++);
            }
        }

        private void DrawSystemInformationView()
        {
            string[] debugText =
            {
                $"Resolution: {SystemInfo.ScreenResolutionX},{SystemInfo.ScreenResolutionY} (window size: {GlobalSettings.GraphicsResolutionX},{GlobalSettings.GraphicsResolutionY} " +
                $"Window flags: {GlobalSettings.GraphicsWindowFlags}, render flags: {GlobalSettings.GraphicsRenderFlags})",
                $"SDL {SDL_EXPECTED_MAJOR_VERSION}.{SDL_EXPECTED_MINOR_VERSION}.{SDL_EXPECTED_PATCHLEVEL}",
                $"SDL_image {SDL_IMAGE_EXPECTED_MAJOR_VERSION}.{SDL_IMAGE_EXPECTED_MINOR_VERSION}.{SDL_IMAGE_EXPECTED_PATCHLEVEL}",
                $"SDL_mixer {SDL_MIXER_EXPECTED_MAJOR_VERSION}.{SDL_MIXER_EXPECTED_MINOR_VERSION}.{SDL_MIXER_EXPECTED_PATCHLEVEL}",
                $"{SystemInfo.Cpu.ProcessArchitecture} Lightning on {SystemInfo.Cpu.SystemArchitecture} {SystemInfo.CurOperatingSystem}",
                $"CPU Capabilities: {SystemInfo.Cpu.Capabilities}",
                $"Hardware Threads (NOT cores!): {SystemInfo.Cpu.Threads}",
                $"Total System RAM: {SystemInfo.SystemRam}MiB",
            };

            foreach (string line in debugText)
            {
                Lightning.Renderer.AddRenderable(new TextBlock("DebugText9", line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, 
                    DebugBackground, FontStyle.Normal, -1, FontSmoothingType.Default, true), this);
                CurrentY += LineSpacing;
            }
        }

        private void DrawPerformanceView()
        {
            if (!GlobalSettings.GeneralProfilePerformance)
            {
                Lightning.Renderer.AddRenderable(new TextBlock("DebugText10", "This page is disabled as the Performance Profiler is not enabled!", "DebugFont", new Vector2(0, CurrentY),
                    DebugForeground, DebugBackground, FontStyle.Bold, -1, FontSmoothingType.Default, true, false), this);
                CurrentY += LineSpacing;
            }
            else
            {
                string[] debugText =
                {
                    $"FPS: {Lightning.Renderer.CurFPS:F1)} ({Lightning.Renderer.DeltaTime:F2}ms)",
                    $"0.1% High: {PerformanceProfiler.Current999thPercentile}",
                    $"1% High: {PerformanceProfiler.Current99thPercentile}",
                    $"5% High: {PerformanceProfiler.Current95thPercentile}",
                    $"Average: {PerformanceProfiler.CurrentAverage}",
                    $"5% Low: {PerformanceProfiler.Current5thPercentile}",
                    $"1% Low: {PerformanceProfiler.Current1stPercentile}",
                    $"0.1% Low: {PerformanceProfiler.Current01thPercentile}",
                };

                foreach (string line in debugText)
                {
                    Lightning.Renderer.AddRenderable(new TextBlock("DebugText11", line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, 
                        FontStyle.Normal, -1, FontSmoothingType.Default, true), this);
                    CurrentY += LineSpacing;
                }
            }
        }

        private void DrawGlobalSettingsView()
        {
            // this file must exist
            string[] lines = File.ReadAllLines(GlobalSettings.GLOBALSETTINGS_PATH);

            foreach (string line in lines)
            {
                Lightning.Renderer.AddRenderable(new TextBlock("DebugText12", line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, FontStyle.Normal, 
                    -1, FontSmoothingType.Default, true, false), this);
                CurrentY += LineSpacing;
            }
        }

        private void KeyPressed(Key key)
        {
            string keyString = key.ToString();

            // case has to be a compile time constant so we do thos
            if (keyString == GlobalSettings.DebugKey) Enabled = !Enabled;

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