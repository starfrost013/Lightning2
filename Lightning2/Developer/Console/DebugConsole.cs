namespace LightningGL
{
    /// <summary>
    /// Console 
    /// 
    /// Defines the main class for the inengine debugging console.
    /// The console has the capability to set any arbitrary registered global settings,
    /// as well as execute commands that inherit from the <see cref="ConsoleCommand"/> class.
    /// </summary>
    public class DebugConsole : Renderable
    {
        private Rectangle? Rectangle { get; set; }

        private TextBox? TextBox { get; set; }

        private TextBlock? ConsoleHeader { get; set; }

        private TextBlock? ConsoleText { get; set; }

        public override bool CanReceiveEventsWhileUnfocused => true;

        /// <summary>
        /// <inheritdoc/>. For DebugViewer, force to top EXCEPT for debugviewer.
        /// </summary>
        public override int ZIndex => 2147483646;

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
                Debug.Assert(Rectangle != null
                    && TextBox != null
                    && ConsoleHeader != null
                    && ConsoleText != null);

                _enabled = value;
                Rectangle.IsNotRendering = !value;
                TextBox.IsNotRendering = !value;
                ConsoleHeader.IsNotRendering = !value;
                ConsoleText.IsNotRendering = !value;

            }
        }

        // maybe make configurable?

        private readonly Color DebugTextColor = Color.White; 

        /// <summary>
        /// The default foreground colour for the debug screen.
        /// Maybe make configurable?
        /// </summary>
        private readonly Color DebugForeground = Color.Black;

        /// <summary>
        /// The default border colour for the debug screen.
        /// Maybe make configurable?
        /// </summary>
        private readonly Color DebugBorder = Color.FromArgb(0, Color.White);

        // todo: make these configurable AFTER GLOBALSETTINGS REWRITE

        private readonly Vector2 DEFAULT_DEBUG_BORDER_SIZE = new(5, 5);

        private readonly Vector2 DEFAULT_CONSOLE_TEXTBOX_SIZE = new(GlobalSettings.DebugConsoleSizeX, 30);

        private readonly Vector2 DEFAULT_CONSOLE_HEADER_POSITION = new(0, 28);

        private readonly Vector2 DEFAULT_CONSOLE_TEXT_POSITION = new(0, 64);

        public DebugConsole(string name) : base(name) 
        {
            OnKeyPressed += KeyPressed;
        }

        public override void Create()
        {
            Lightning.Renderer.AddRenderable(new Font("Arial.ttf", GlobalSettings.DebugFontSizeLarge, "DebugFontLarge"));

            Rectangle = Lightning.Renderer.AddRenderable(new Rectangle("ConsoleRectangle", new(0, 0), new(GlobalSettings.DebugConsoleSizeX, GlobalSettings.DebugConsoleSizeY),
                DebugForeground, true, DebugBorder, DEFAULT_DEBUG_BORDER_SIZE, true), this);
            TextBox = Lightning.Renderer.AddRenderable(new TextBox("ConsoleTextBox", 300, "DebugFont", new(0, GlobalSettings.DebugConsoleSizeY - DEFAULT_CONSOLE_TEXTBOX_SIZE.Y), DEFAULT_CONSOLE_TEXTBOX_SIZE,
                DebugTextColor, Color.Black, DEFAULT_DEBUG_BORDER_SIZE, false, true), this);
            ConsoleHeader = Lightning.Renderer.AddRenderable(new TextBlock("ConsoleHeader", "Console", "DebugFontLarge", DEFAULT_CONSOLE_HEADER_POSITION,
                DebugTextColor, Color.Transparent), this);
            ConsoleText = Lightning.Renderer.AddRenderable(new TextBlock("ConsoleText", "(PLACEHOLDER)", "DebugFont", DEFAULT_CONSOLE_TEXT_POSITION,
                DebugTextColor, Color.Transparent), this);

            ConsoleHeader.SnapToScreen = true;
            ConsoleText.SnapToScreen = true;

            Rectangle.ZIndex = ZIndex;
            TextBox.ZIndex = ZIndex;
            ConsoleHeader.ZIndex = ZIndex;
            ConsoleText.ZIndex = ZIndex;

            // explicitly set to update ui state
            Enabled = false;

            TextBox.OnKeyPressed += ConsoleBoxKeyPressed;
        }

        public void ConsoleBoxKeyPressed(Key key)
        {
            Debug.Assert(TextBox != null);

            if (!Enabled) return;

            // go to base
            TextBox.KeyPressed(key); 

            switch (key.KeySym.scancode)
            {
                case SDL_Scancode.SDL_SCANCODE_RETURN:
                case SDL_Scancode.SDL_SCANCODE_RETURN2:
                case SDL_Scancode.SDL_SCANCODE_KP_ENTER:
                    ProcessCommand(); 
                    break;
            }
        }

        private void ProcessCommand()
        {

        }

        private void KeyPressed(Key key)
        {
            string keyString = key.ToString();

            // case has to be a compile time constant so we do thos
            if (keyString == GlobalSettings.DebugConsoleKey) Enabled = !Enabled;
        }
    }
}
