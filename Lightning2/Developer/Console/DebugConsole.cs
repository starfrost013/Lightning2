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
        /// <summary>
        /// Rectangle UI used as part of the console.
        /// </summary>
        private Rectangle? Rectangle { get; set; }

        /// <summary>
        /// TextBox UI used as part of the console.
        /// </summary>
        private TextBox? TextBox { get; set; }

        /// <summary>
        /// Console header text.
        /// </summary>
        private TextBlock? ConsoleHeader { get; set; }
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanReceiveEventsWhileUnfocused => true;

        /// <summary>
        /// <inheritdoc/>. For DebugViewer, force to top EXCEPT for debugviewer.
        /// </summary>
        public override int ZIndex => 2147483646;

        /// <summary>
        /// Backing field for <see cref="Enabled"/>
        /// </summary>
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
                    && ConsoleHeader != null);

                _enabled = value;
                Rectangle.IsNotRendering = !value;
                TextBox.IsNotRendering = !value;
                ConsoleHeader.IsNotRendering = !value;
            }
        }

        // maybe make configurable?

        /// <summary>
        /// Default debug text colour.
        /// </summary>
        private readonly Color DEFAULT_DEBUG_TEXT_COLOR = Color.Black; 

        /// <summary>
        /// The default foreground colour for the debug screen.
        /// Maybe make configurable?
        /// </summary>
        private readonly Color DEFAULT_DEBUG_FOREGROUND_COLOR = Color.Black;

        /// <summary>
        /// Default "Console" debug text header colour.
        /// </summary>
        private readonly Color DEFAULT_DEBUG_HEADER_COLOR = Color.White;

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

        /// <summary>
        /// The default size of the debug console textbox border.
        /// </summary>
        private readonly Vector2 DEFAULT_DEBUG_BORDER_SIZE = new(5, 5);

        /// <summary>
        /// The default size of the debug console textbox.
        /// </summary>
        private readonly Vector2 DEFAULT_CONSOLE_TEXTBOX_SIZE = new(GlobalSettings.DebugConsoleSizeX, 30);

        /// <summary>
        /// The default position of the debug console header text.
        /// </summary>
        private readonly Vector2 DEFAULT_CONSOLE_HEADER_POSITION = new(0, 14);

        /// <summary>
        /// The default position of the console rectangle.
        /// </summary>
        private readonly Vector2 DEFAULT_RECTANGLE_POSITION = new(0, 0);

        /// <summary>
        /// The default position of the console textbox.
        /// </summary>
        private Vector2 DEFAULT_TEXTBOX_POSITION => new(0, GlobalSettings.DebugConsoleSizeY - DEFAULT_CONSOLE_TEXTBOX_SIZE.Y);

        /// <summary>
        /// The default size of the console textbox.
        /// </summary>
        private Vector2 DEFAULT_TEXTBOX_SIZE => new(0, GlobalSettings.DebugConsoleSizeY - DEFAULT_CONSOLE_TEXTBOX_SIZE.Y);

        /// <summary>
        /// The default name of the input binding used for triggering the console.
        /// </summary>
        private const string BINDING_TRIGGER_NAME = "DEBUGCONSOLETRIGGER";

        public DebugConsole(string name) : base(name) 
        {
            OnKeyDown += KeyPressed;
        }

        public override void Create()
        {
            Lightning.Renderer.AddRenderable(new Font("Arial.ttf", GlobalSettings.DebugFontSizeLarge, "DebugFontLarge"));

            Rectangle = Lightning.Renderer.AddRenderable(new Rectangle("ConsoleRectangle", new(0, 0), new(GlobalSettings.DebugConsoleSizeX, GlobalSettings.DebugConsoleSizeY),
                DEFAULT_DEBUG_FOREGROUND_COLOR, true, DEFAULT_DEBUG_BORDER, DEFAULT_DEBUG_BORDER_SIZE, true), this);
            TextBox = Lightning.Renderer.AddRenderable(new TextBox("ConsoleTextBox", 300, "DebugFont", DEFAULT_TEXTBOX_POSITION, 
                DEFAULT_CONSOLE_TEXTBOX_SIZE, DEFAULT_DEBUG_TEXT_COLOR, DEFAULT_DEBUG_TEXT_BOX_BACKGROUND, true, DEFAULT_DEBUG_BORDER, DEFAULT_DEBUG_BORDER_SIZE), this);
            ConsoleHeader = Lightning.Renderer.AddRenderable(new TextBlock("ConsoleHeader", "Console", "DebugFontLarge", DEFAULT_CONSOLE_HEADER_POSITION,
                 DEFAULT_DEBUG_HEADER_COLOR, Color.Transparent), this);

            ConsoleHeader.SnapToScreen = true;
            TextBox.SnapToScreen = true;
            ConsoleHeader.SnapToScreen = true;

            Rectangle.ZIndex = ZIndex + 1;
            TextBox.ZIndex = ZIndex + 1;
            ConsoleHeader.ZIndex = ZIndex + 1;

            // explicitly set to update ui state
            Enabled = false;

            TextBox.OnKeyDown += ConsoleBoxKeyPressed;
        }

        public override void Draw()
        {
            Debug.Assert(TextBox != null
                && Rectangle != null
                && ConsoleHeader != null);

            TextBox.Position = DEFAULT_TEXTBOX_SIZE;
            Rectangle.Position = DEFAULT_RECTANGLE_POSITION;
            ConsoleHeader.Position = DEFAULT_CONSOLE_HEADER_POSITION;
        }

        public void ConsoleBoxKeyPressed(InputBinding? binding, Key key)
        {
            Debug.Assert(TextBox != null);

            if (!Enabled
                || binding == null) return;

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
                Logger.LogError("Invalid console command entered (temporary)", 303, LoggerSeverity.Warning, null, true);
                return;
            }

            string[] commandComponents = textBoxText.Split(" ");

            // we already check for a length implicitly (with string.IsNullOrWhiteSpace, which also checks for empty)
            string commandType = commandComponents[0];

            bool succeeded = Enum.TryParse(commandType, true, out ConsoleCommands commandId);

            if (!succeeded)
            {
                Logger.Log($"Console command {commandType} not found!");
                return;
            }

            ConsoleCommand? command = ConsoleCommandFactory.GetCommand(commandId);

            // should not happen
            Debug.Assert(command != null);

            if (!command.Execute()) Logger.LogError("Command failed to execute!", 310, LoggerSeverity.Warning);
        }

        private void KeyPressed(InputBinding? binding, Key key)
        {
            if (binding == null) return;

            // case has to be a compile time constant so we do thos
            if (binding.Name == BINDING_TRIGGER_NAME) Enabled = !Enabled;
        }
    }
}
