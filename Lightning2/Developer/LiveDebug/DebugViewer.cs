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

        public DebugViewer(string name) : base(name)
        {
            OnKeyPressed += KeyPressed;
        }

        internal override void Create()
        {
            NCLogging.Log("Loading debug font...");
            // Load the debug font.
            FontManager.AddAsset(new Font("Arial", 11, "DebugFont"));
        }

        internal override void Draw()
        {
            if (!Enabled) return;

            // reset drawing
            // TODO: configurable colours

            CurrentY = 0;

            int currentPage = (int)CurrentDebugView + 1;
            int maxPage = (int)DebugViews.MaxPage + 1;

            TextManager.DrawText($"Lightning Debug v{LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING} (Debug page {currentPage}/{maxPage} - {CurrentDebugView})", 
                "DebugFont", new(0,0), DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
            CurrentY += GlobalSettings.DebugLineDistance;

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
                TextManager.DrawText(line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
                CurrentY += GlobalSettings.DebugLineDistance;
            }

            TextManager.DrawText($"Current scene: {CurrentScene.Name}", "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, TTF_FontStyle.Normal,
                -1, -1, FontSmoothingType.Default, true);
            CurrentY += GlobalSettings.DebugLineDistance;

            // draw indicator that we are under 60fps always under it
            if (Lightning.Renderer.CurFPS < GlobalSettings.GraphicsMaxFPS)
            {
                CurrentY += GlobalSettings.DebugLineDistance;

                int maxFps = GlobalSettings.GraphicsMaxFPS;

                if (maxFps == 0) maxFps = 60;

                TextManager.DrawText($"Running under target FPS ({maxFps})!", "DebugFont", new Vector2(0, CurrentY), 
                    DebugForeground, DebugBackground, TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Default, true);
                CurrentY += GlobalSettings.DebugLineDistance;
            }

        }

        private void DrawRenderableDetailsView()
        {
            // bail too many renderables
            if (Lightning.Renderer.Renderables.Count > 5000)
            {
                TextManager.DrawText($"Something went wrong, renderable vomiting in progress (>5000 renderables in scene!!!!)", "DebugFont", new(0, CurrentY),
                    DebugForeground, DebugBackground, TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Default, true);
                CurrentY += GlobalSettings.DebugLineDistance;
            }
            else
            {
                TextManager.DrawText($"Camera Position: {Lightning.Renderer.Settings.Camera.Position}", "DebugFont", new Vector2(0, CurrentY),
                    DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
                CurrentY += GlobalSettings.DebugLineDistance;

                for (int renderableId = 0; renderableId < Lightning.Renderer.Renderables.Count; renderableId++)
                {
                    Renderable renderable = Lightning.Renderer.Renderables[renderableId];
                    TextManager.DrawText($"{renderable.Name} ({renderable.GetType().Name}): position {renderable.Position}, " +
                        $"size: {renderable.Size}, render position: {renderable.RenderPosition}, on screen: {renderable.IsOnScreen}, z-index: {renderable.ZIndex}, " +
                        $"is animating now: {renderable.IsAnimating}", "DebugFont", new Vector2(0, CurrentY),
                        DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
                    CurrentY += GlobalSettings.DebugLineDistance;

                    if (renderable.Children.Count > 0) DrawRenderableChildren(renderable);
                }
            }
        }

        private void DrawRenderableChildren(Renderable parent, int depth = 1)
        {
            foreach (Renderable renderable in parent.Children)
            {
                string initialString = $"{renderable.Name} ({renderable.GetType().Name}): position {renderable.Position}, " +
                    $"size: {renderable.Size}, render position: {renderable.RenderPosition}, on screen: {renderable.IsOnScreen}, z-index: {renderable.ZIndex}, " +
                    $"is animating now: {renderable.IsAnimating} - parent {parent.Name}";

                // string::format requires constants so we need to pad to the left
                initialString = initialString.PadLeft(initialString.Length + (8 * depth)); // todo: make this a setting with a defauilt value
                TextManager.DrawText(initialString, "DebugFont", new(0, CurrentY),
                    DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
                CurrentY += GlobalSettings.DebugLineDistance;
                
                if (renderable.Children.Count > 0) DrawRenderableChildren(renderable, depth++);
            }
        }

        private void DrawSystemInformationView()
        {
            string[] debugText =
            {
                $"Resolution: {SystemInfo.ScreenResolutionX},{SystemInfo.ScreenResolutionY} (window size: {GlobalSettings.GraphicsResolutionX},{GlobalSettings.GraphicsResolutionY} " +
                $"(SDL is using the {GlobalSettings.GraphicsSdlRenderingBackend} rendering backend, window flags: {GlobalSettings.GraphicsWindowFlags}, render flags: {GlobalSettings.GraphicsRenderFlags})",
                $"SDL {SDL_EXPECTED_MAJOR_VERSION}.{SDL_EXPECTED_MINOR_VERSION}.{SDL_EXPECTED_PATCHLEVEL}",
                $"SDL_image {SDL_IMAGE_EXPECTED_MAJOR_VERSION}.{SDL_IMAGE_EXPECTED_MINOR_VERSION}.{SDL_IMAGE_EXPECTED_PATCHLEVEL}",
                $"SDL_mixer {SDL_MIXER_EXPECTED_MAJOR_VERSION}.{SDL_MIXER_EXPECTED_MINOR_VERSION}.{SDL_MIXER_EXPECTED_PATCHLEVEL}",
                $"SDL_ttf {SDL_TTF_EXPECTED_MAJOR_VERSION}.{SDL_TTF_EXPECTED_MINOR_VERSION}.{SDL_TTF_EXPECTED_PATCHLEVEL}",
                $"{SystemInfo.Cpu.ProcessArchitecture} Lightning on {SystemInfo.Cpu.SystemArchitecture} {SystemInfo.CurOperatingSystem}",
                $"CPU Capabilities: {SystemInfo.Cpu.Capabilities}",
                $"Hardware Threads (NOT cores!): {SystemInfo.Cpu.Threads}",
                $"Total System RAM: {SystemInfo.SystemRam}MiB",
            };

            foreach (string line in debugText)
            {
                TextManager.DrawText(line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
                CurrentY += GlobalSettings.DebugLineDistance;
            }
        }

        private void DrawPerformanceView()
        {
            if (!GlobalSettings.GeneralProfilePerformance)
            {
                TextManager.DrawText("This page is disabled as the Performance Profiler is not enabled!", "DebugFont", new Vector2(0, CurrentY),
                    DebugForeground, DebugBackground, TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Default, true, false);
                CurrentY += GlobalSettings.DebugLineDistance;
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
                    TextManager.DrawText(line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
                    CurrentY += GlobalSettings.DebugLineDistance;
                }
            }
        }

        private void DrawGlobalSettingsView()
        {
            // this file must exist
            string[] lines = File.ReadAllLines(GlobalSettings.GLOBALSETTINGS_PATH);

            foreach (string line in lines)
            {
                TextManager.DrawText(line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, TTF_FontStyle.Normal, 
                    -1, -1, FontSmoothingType.Default, true, false);
                CurrentY += GlobalSettings.DebugLineDistance;
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