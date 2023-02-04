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

        private readonly Color DEFAULT_DEBUG_TEXT_COLOR = Color.Black; 

        /// <summary>
        /// The default foreground colour for the debug screen.
        /// Maybe make configurable?
        /// </summary>
        private readonly Color DEFAULT_DEBUG_FOREGROUND_COLOR = Color.Black;

        /// <summary>
        /// The default background colour for the text box of the console.
        /// </summary>
        private readonly Color DEFAULT_DEBUG_TEXT_BOX_BACKGROUND = Color.LightBlue;

        /// <summary>
        /// The default border colour for the debug screen.
        /// Maybe make configurable?
        /// </summary>
        private readonly Color DEFAULT_DEBUG_BORDER = Color.FromArgb(0, Color.White);

        // todo: make these configurable AFTER GLOBALSETTINGS REWRITE

        private readonly Vector2 DEFAULT_DEBUG_BORDER_SIZE = new(5, 5);

        private readonly Vector2 DEFAULT_CONSOLE_TEXTBOX_SIZE = new(GlobalSettings.DebugConsoleSizeX, 30);

        private readonly Vector2 DEFAULT_CONSOLE_HEADER_POSITION = new(0, 28);

        private readonly Vector2 DEFAULT_CONSOLE_TEXT_POSITION = new(0, 64);

        private readonly Vector2 DEFAULT_RECTANGLE_POSITION = new(0, 0);

        private Vector2 DEFAULT_TEXTBOX_POSITION => new(0, GlobalSettings.DebugConsoleSizeY - DEFAULT_CONSOLE_TEXTBOX_SIZE.Y);
        private Vector2 DEFAULT_TEXTBOX_SIZE => new(0, GlobalSettings.DebugConsoleSizeY - DEFAULT_CONSOLE_TEXTBOX_SIZE.Y);

        public DebugConsole(string name) : base(name) 
        {
            OnKeyPressed += KeyPressed;
        }

        public override void Create()
        {
            Lightning.Renderer.AddRenderable(new Font("Arial.ttf", GlobalSettings.DebugFontSizeLarge, "DebugFontLarge"));

            Rectangle = Lightning.Renderer.AddRenderable(new Rectangle("ConsoleRectangle", new(0, 0), new(GlobalSettings.DebugConsoleSizeX, GlobalSettings.DebugConsoleSizeY),
                DEFAULT_DEBUG_FOREGROUND_COLOR, true, DEFAULT_DEBUG_BORDER, DEFAULT_DEBUG_BORDER_SIZE, true), this);
            TextBox = Lightning.Renderer.AddRenderable(new TextBox("ConsoleTextBox", 300, "DebugFont", DEFAULT_TEXTBOX_POSITION, 
                DEFAULT_CONSOLE_TEXTBOX_SIZE, DEFAULT_DEBUG_TEXT_COLOR, DEFAULT_DEBUG_TEXT_BOX_BACKGROUND, true, DEFAULT_DEBUG_BORDER, DEFAULT_DEBUG_BORDER_SIZE), this);
            ConsoleHeader = Lightning.Renderer.AddRenderable(new TextBlock("ConsoleHeader", "Console", "DebugFontLarge", DEFAULT_CONSOLE_HEADER_POSITION,
                DEFAULT_DEBUG_TEXT_COLOR, Color.Transparent), this);
            ConsoleText = Lightning.Renderer.AddRenderable(new TextBlock("ConsoleText", "(PLACEHOLDER)", "DebugFont", DEFAULT_CONSOLE_TEXT_POSITION,
                DEFAULT_DEBUG_TEXT_COLOR), this);

            ConsoleHeader.SnapToScreen = true;
            ConsoleText.SnapToScreen = true;
            TextBox.SnapToScreen = true;
            ConsoleHeader.SnapToScreen = true;

            Rectangle.ZIndex = ZIndex + 1;
            TextBox.ZIndex = ZIndex + 1;
            ConsoleHeader.ZIndex = ZIndex + 1;
            ConsoleText.ZIndex = ZIndex + 1;

            // explicitly set to update ui state
            Enabled = false;

            TextBox.OnKeyPressed += ConsoleBoxKeyPressed;
        }

        public override void Draw()
        {
            Debug.Assert(TextBox != null
                && Rectangle != null
                && ConsoleHeader != null
                && ConsoleText != null);

            TextBox.Position = DEFAULT_TEXTBOX_SIZE;
            Rectangle.Position = DEFAULT_RECTANGLE_POSITION;
            ConsoleHeader.Position = DEFAULT_CONSOLE_HEADER_POSITION;
            ConsoleText.Position = DEFAULT_CONSOLE_TEXT_POSITION;
        }

        public void ConsoleBoxKeyPressed(Keyboard key)
        {
            Debug.Assert(TextBox != null);

            if (!Enabled) return;

            // go to base
            //TextBox.KeyPressed(key); 

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
            Debug.Assert(TextBox != null);
            string? textBoxText = TextBox.Text;

            // clear textbox text
            TextBox.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(textBoxText))
            {
                NCLogging.LogError("Temporary ERROR - PLEASE ENTER A FUCKING COMMAND!", 303, NCLoggingSeverity.Warning, null, true);
                return;
            }

            string[] commandComponents = textBoxText.Split(" ");

            // we already check for a length implicitly (with string.IsNullOrWhiteSpace, which also checks for empty)
            string commandType = commandComponents[0];

            ConsoleCommands commandId = default;

            bool succeeded = Enum.TryParse(commandType, true, out commandId);

            if (!succeeded)
            {
                NCLogging.Log($"Console command {commandType} not found!");
                return;
            }

            ConsoleCommand? command = ConsoleCommandFactory.GetCommand(commandId);

            // should not happen
            Debug.Assert(command != null);

            if (!command.Execute()) NCLogging.Log("Command failed to execute");
        }

        private void KeyPressed(Keyboard key)
        {
            string keyString = key.ToString();

            // case has to be a compile time constant so we do thos
            if (keyString == GlobalSettings.DebugConsoleKey) Enabled = !Enabled;
        }
    }
}
