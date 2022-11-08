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

        private bool Enabled { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// 
        /// <para>Always true for DebugViewer</para>
        /// </summary>
        public override bool CanReceiveEventsWhileUnfocused => true;

        /// <summary>
        /// Current Y for drawing
        /// </summary>
        private float CurrentY { get; set; }

        // maybe make configurable?
        private Color DebugForeground = Color.Blue;

        private Color DebugBackground = Color.FromArgb(0, Color.White);

        public DebugViewer(string name) : base(name)
        {
            OnKeyPressed += KeyPressed;
        }

        internal override void Create()
        {
            NCLogging.Log("Loading debug font...");
            // Load the debug font.
            FontManager.LoadFont("Arial", 11, "DebugFont");
        }

        internal override void Draw()
        {
            // reset drawing
            CurrentY = 0;
            if (!Enabled) return;
            
            // todo: configurable colours

            int currentPage = (int)CurrentDebugView + 1;
            int maxPage = (int)DebugViews.MaxPage + 1;

            TextManager.DrawText($"Lightning Debug v{LightningVersion.LIGHTNING_VERSION_EXTENDED_STRING} (Debug page {currentPage}/{maxPage} - {CurrentDebugView})", 
                "DebugFont", new(0,0), DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
            CurrentY += GlobalSettings.DEFAULT_DEBUG_LINE_DISTANCE;

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
            string[] debugText =
            {
                $"FPS: {Lightning.Renderer.CurFPS.ToString("F1")} ({Lightning.Renderer.DeltaTime.ToString("F2")}ms)",
                Lightning.Renderer.FrameNumber.ToString(),
                $"Number of renderables: {Lightning.Renderer.Renderables.Count}",
                $"Number of renderables on screen: {Lightning.Renderer.RenderedLastFrame}",
                $"Delta time: {Lightning.Renderer.DeltaTime}",
            };

            foreach (string line in debugText)
            {
                TextManager.DrawText(line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
                CurrentY += GlobalSettings.DebugLineDistance;
            }

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
                TextManager.DrawText($"Something went wrong, renderable vomiting in progress (>5000 renderables!!!!)", "DebugFont", new(0, CurrentY),
                    DebugForeground, DebugBackground, TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Default, true);
                CurrentY += GlobalSettings.DebugLineDistance;
            }
            else
            {
                for (int renderableId = 0; renderableId < Lightning.Renderer.Renderables.Count; renderableId++)
                {
                    Renderable renderable = Lightning.Renderer.Renderables[renderableId];
                    TextManager.DrawText($"{renderable.Name} ({renderable.GetType().Name}): position {renderable.Position}, " +
                        $"size: {renderable.Size}, render position: {renderable.RenderPosition}, on screen: {renderable.IsOnScreen}, is animating now: {renderable.AnimationRunning}", "DebugFont", new Vector2(0, CurrentY),
                        DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
                    CurrentY += GlobalSettings.DebugLineDistance;
                }
            }
        }

        private void DrawSystemInformationView()
        {
            string[] debugText =
            {
                $"Resolution: {SystemInfo.ScreenResolutionX},{SystemInfo.ScreenResolutionY} (window size: {GlobalSettings.GraphicsResolutionX},{GlobalSettings.GraphicsResolutionY} " +
                $"(SDL is using the {GlobalSettings.GraphicsRenderingBackend} rendering backend, window flags: {GlobalSettings.GraphicsWindowFlags}, render flags: {GlobalSettings.GraphicsRenderFlags})",
                $"SDL {SDL_EXPECTED_MAJOR_VERSION}.{SDL_EXPECTED_MINOR_VERSION}.{SDL_EXPECTED_PATCHLEVEL}",
                $"SDL_image {SDL_IMAGE_EXPECTED_MAJOR_VERSION}.{SDL_IMAGE_EXPECTED_MINOR_VERSION}.{SDL_IMAGE_EXPECTED_PATCHLEVEL}",
                $"SDL_mixer {SDL_MIXER_EXPECTED_MAJOR_VERSION}.{SDL_MIXER_EXPECTED_MINOR_VERSION}.{SDL_MIXER_EXPECTED_PATCHLEVEL}",
                $"SDL_ttf {SDL_TTF_EXPECTED_MAJOR_VERSION}.{SDL_TTF_EXPECTED_MINOR_VERSION}.{SDL_TTF_EXPECTED_PATCHLEVEL}",
                $"SDL_gfx {SDL_GFX_VERSION_MAJOR}.{SDL_GFX_VERSION_MINOR}.{SDL_GFX_VERSION_REVISION}",
                $"{SystemInfo.Cpu.ProcessArchitecture} Lightning on {SystemInfo.Cpu.SystemArchitecture} {SystemInfo.CurOperatingSystem}",
                $"CPU Capabilities: {SystemInfo.Cpu.Capabilities}",
                $"Hardware Threads (NOT cores!): {SystemInfo.Cpu.Threads}",
                $"Total System RAM - {SystemInfo.SystemRam}MiB",
            };

            foreach (string line in debugText)
            {
                TextManager.DrawText(line, "DebugFont", new Vector2(0, CurrentY), DebugForeground, DebugBackground, TTF_FontStyle.Normal, -1, -1, FontSmoothingType.Default, true);
                CurrentY += GlobalSettings.DebugLineDistance;
            }
        }

        private void DrawPerformanceView()
        {

        }

        private void KeyPressed(Key key)
        {
            switch (key.ToString())
            {
                case "F9":
                    Enabled = !Enabled;
                    break;
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

        private void DrawGlobalSettingsView()
        {

        }
    }
}
