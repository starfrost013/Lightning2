namespace LightningGL
{
    public class TextBox : Gadget
    {
        /// <summary>
        /// The text of this textbox.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// A boolean determining if the cursor is hidden or not.
        /// </summary>
        public bool HideCursor { get; set; }

        /// <summary>
        /// The capacity of the textbox.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// The number of milliseconds between cursor blinks.
        /// </summary>
        public int CursorBlinkFrequency { get; set; }

        /// <summary>
        /// The length of time the cursor will blink.
        /// </summary>
        public int CursorBlinkLength { get; set; }

        /// <summary>
        /// Determines if multiple lines are allowed in this text box.
        /// </summary>
        public bool AllowMultiline { get; set; }

        /// <summary>
        /// The number of frames until the next cursor blink.
        /// </summary>
        private int NumberOfFramesUntilNextBlink { get; set; }

        /// <summary>
        /// Determines if the text box is blinking
        /// </summary>
        private bool IsActive { get; set; }

        /// <summary>
        /// UI rectangle used for drawing this textbox.
        /// </summary>
        private Rectangle? Rectangle { get; set; }

        /// <summary>
        /// UI line used for drawing the cursor.
        /// </summary>
        private Line? Cursor { get; set; }

        /// <summary>
        /// UI text used for drawing the text.
        /// </summary>
        private TextBlock? TextBoxText { get; set; }

        /// <summary>
        /// Default value for <see cref="CursorBlinkLength"/>.
        /// </summary>
        private const int DEFAULT_CURSOR_BLINK_LENGTH = 50;

        /// <summary>
        /// Default value for <see cref="ForegroundColor"/>.
        /// </summary>
        private readonly Color DEFAULT_FOREGROUND_COLOR = Color.White;

        /// <summary>
        /// Default value for <see cref="CursorBlinkFrequency"/>.
        /// </summary>
        private const int DEFAULT_CURSOR_BLINK_FREQUENCY = 300;

        /// <summary>
        /// Default value for <see cref="NumberOfFramesUntilNextBlink"/>.
        /// </summary>
        private const int DEFAULT_NUMBER_OF_FRAMES_UNTIL_NEXT_BLINK = DEFAULT_CURSOR_BLINK_FREQUENCY; // reasonable default

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool SnapToScreen
        { 
            get => base.SnapToScreen; 
            set
            {
                base.SnapToScreen = value;
                if (TextBoxText != null) TextBoxText.SnapToScreen = value;
                if (Cursor != null) Cursor.SnapToScreen = value;
                if (Rectangle != null) Rectangle.SnapToScreen = value;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool IsNotRendering
        { 
            get => base.IsNotRendering; 
            set
            {
                base.IsNotRendering = value;
                if (Rectangle != null) Rectangle.IsNotRendering = value;
                if (Cursor != null) Cursor.IsNotRendering = value;
                if (TextBoxText != null) TextBoxText.IsNotRendering = value;
            }
        }

        public TextBox(string name, int capacity, string font) : base(name, font)
        {
            Capacity = capacity;
            OnKeyDown += KeyDown;
            if (CursorBlinkFrequency <= 0) CursorBlinkFrequency = DEFAULT_CURSOR_BLINK_FREQUENCY;
            if (ForegroundColor == default) ForegroundColor = DEFAULT_FOREGROUND_COLOR;
            if (CursorBlinkLength <= 0) CursorBlinkLength = DEFAULT_CURSOR_BLINK_LENGTH;

            NumberOfFramesUntilNextBlink = DEFAULT_NUMBER_OF_FRAMES_UNTIL_NEXT_BLINK;
        }

        public TextBox(string name, int capacity, string font, Vector2 position, Vector2 size, Color foregroundColor, Color backgroundColor = default,
             bool filled = false, Color borderColor = default, Vector2 borderSize = default, bool snapToScreen = false) : base(name, font)
        { 
            Capacity = capacity;
            OnKeyDown += KeyDown;
            Position = position;
            Size = size;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;  
            Filled = filled;
            BorderColor = borderColor;
            BorderSize = borderSize;
            SnapToScreen = snapToScreen;

            if (CursorBlinkFrequency <= 0) CursorBlinkFrequency = DEFAULT_CURSOR_BLINK_FREQUENCY;
            if (ForegroundColor == default) ForegroundColor = DEFAULT_FOREGROUND_COLOR;
            if (CursorBlinkLength <= 0) CursorBlinkLength = DEFAULT_CURSOR_BLINK_LENGTH;

            NumberOfFramesUntilNextBlink = DEFAULT_NUMBER_OF_FRAMES_UNTIL_NEXT_BLINK;
        }

        public override void Create()
        {
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

            Text ??= string.Empty;
            Rectangle = Lightning.Renderer.AddRenderable(new Rectangle("TextBoxRectangle", Position, Size, CurBackgroundColor, true, BorderColor, BorderSize, SnapToScreen), this);
            Cursor = Lightning.Renderer.AddRenderable(new Line("TextBoxLine", default, default, ForegroundColor, SnapToScreen), this);
            TextBoxText = Lightning.Renderer.AddRenderable(new TextBlock("TextBoxText", Text, Font, Position, ForegroundColor, default), this);
            TextBoxText.SnapToScreen = SnapToScreen;    
            Cursor.SnapToScreen = SnapToScreen;
        }

        /// <summary>
        /// Keypress handler for TextBoxes.
        /// </summary>
        /// <param name="key">The key that has been pressed.</param>
        private void KeyDown(InputBinding? binding, Key key)
        {
            // reject if text is not set or text is longer than capacity
            if (Text == null
                || Text.Length > Capacity) return;

            SDL_Scancode keySym = key.KeySym.scancode;
            SDL_Keymod keyMod = key.KeySym.mod;

            // SDL_EnableUNICODE is not supported and also doesn't translate key release events
            // so let's roll our own...

            bool isUppercase = Key.GetCapsLockCurrentState(); 

            // invert case on shift
            if (keyMod.HasFlag(SDL_Keymod.KMOD_LSHIFT) 
                || keyMod.HasFlag(SDL_Keymod.KMOD_RSHIFT)) isUppercase = !isUppercase;

            // switch various selected keys
            switch (keySym)
            {
                // remove a character
                case SDL_Scancode.SDL_SCANCODE_BACKSPACE:
                case SDL_Scancode.SDL_SCANCODE_KP_BACKSPACE:
                    if (Text.Length > 0) Text = Text.Remove(Text.Length - 1, 1);
                    break;
                case SDL_Scancode.SDL_SCANCODE_RETURN:
                case SDL_Scancode.SDL_SCANCODE_RETURN2:
                case SDL_Scancode.SDL_SCANCODE_KP_ENTER:
                    if (AllowMultiline) Text = $"{Text}\n";
                    break;
                // already handled these three
                case SDL_Scancode.SDL_SCANCODE_LSHIFT:
                case SDL_Scancode.SDL_SCANCODE_RSHIFT:
                case SDL_Scancode.SDL_SCANCODE_CAPSLOCK:
                // block everything in a few different ranges
                // TODO: MOVEABLE CURSOR
                case >= SDL_Scancode.SDL_SCANCODE_PRINTSCREEN and <= SDL_Scancode.SDL_SCANCODE_UP: 
                case >= SDL_Scancode.SDL_SCANCODE_APPLICATION and <= SDL_Scancode.SDL_SCANCODE_PRIOR:
                case >= SDL_Scancode.SDL_SCANCODE_SEPARATOR and <= SDL_Scancode.SDL_SCANCODE_AUDIOFASTFORWARD:
                    break;
                default:
                    string keyStr = key.KeySym.ToString();

                    // check if lowercase
                    // (Key::ToString converts to uppercase by default so we only need to handle one case here)
                    if (!isUppercase) keyStr = keyStr.ToLower();
                    Text = $"{Text}{keyStr}";
                    break;
            }
        }

        /// <summary>
        /// Renders this TextBox.
        /// </summary>
        public override void Draw()
        {
            Debug.Assert(TextBoxText != null
                && Rectangle != null
                && Cursor != null);

            if (!string.IsNullOrWhiteSpace(Text))
            {
                TextBoxText.Text = Text; // maybe put this in the getter

                if (Font == null)
                {
                    Logger.Log($"Cannot draw a TextBox when the value of its Font property is null!", ConsoleColor.Yellow);
                    return;
                }

                Rectangle.Color = CurBackgroundColor;

                if (!HideCursor)
                {
                    Vector2 fontSize = TextUtils.GetLargestTextSize(Font, Text, ForegroundColor);
                    Cursor.Position = new(Position.X + fontSize.X, Position.Y);
                    Cursor.Size = new(1, fontSize.Y); //1px for now
                   
                    // actually blink it
                    if (NumberOfFramesUntilNextBlink == 0)
                    {
                        IsActive = !IsActive;
                        if (Lightning.Renderer.DeltaTime > 0) NumberOfFramesUntilNextBlink = Convert.ToInt32(CursorBlinkLength - 1 + 
                            (CursorBlinkFrequency / Lightning.Renderer.DeltaTime));
                    }

                    // if it's active, draw the line
                    if (IsActive)
                    {
                        Cursor.Color = Color.FromArgb(0, ForegroundColor);
                    }
                    else
                    {
                        Cursor.Color = ForegroundColor;
                    }

                    NumberOfFramesUntilNextBlink--;
                }
            }
        }
    }
}