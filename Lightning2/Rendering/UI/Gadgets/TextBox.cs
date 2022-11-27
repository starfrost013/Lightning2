﻿namespace LightningGL
{
    public class TextBox : Gadget
    {
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
        /// The thickness of the cursor in pixels.
        /// </summary>
        public short CursorThickness { get; set; }

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

        public TextBox(string name, int capacity, string font) : base(name, font)
        {
            Capacity = capacity;
            OnKeyPressed += KeyPressed;
            OnRender += Render;
            if (CursorThickness == 0) CursorThickness = 2;
            if (CursorBlinkFrequency == 0) CursorBlinkFrequency = 300;
            if (ForegroundColor == default) ForegroundColor = Color.White;
            if (CursorBlinkLength == 0) CursorBlinkLength = 50;

            // reasonable default
            NumberOfFramesUntilNextBlink = CursorBlinkFrequency;
        }

        internal override void Create()
        {
            Rectangle = PrimitiveManager.AddRectangle(Position, Size, CurBackgroundColor, true, BorderColor, BorderSize, SnapToScreen, this);
            Cursor = PrimitiveManager.AddLine(default, default, CursorThickness, ForegroundColor, true, false, this);
        }

        /// <summary>
        /// Keypress handler for TextBoxes.
        /// </summary>
        /// <param name="key">The key that has been pressed.</param>
        public void KeyPressed(Key key)
        {
            // reject if text is not set or text is longer than capacity
            if (Text == null) return;
            if (Text.Length > Capacity) return;

            SDL_Scancode keySym = key.KeySym.scancode;
            SDL_Keymod keyMod = key.KeySym.mod;

            // SDL_EnableUNICODE is not supported and also doesn't translate key release events
            // so let's roll our own...

            bool lowercase = (!keyMod.HasFlag(SDL_Keymod.KMOD_CAPS));

            // invert case on shift
            if (keyMod.HasFlag(SDL_Keymod.KMOD_LSHIFT) || keyMod.HasFlag(SDL_Keymod.KMOD_RSHIFT)) lowercase = !lowercase;

            // switch various selected keys
            switch (keySym)
            {
                // remove a character
                case SDL_Scancode.SDL_SCANCODE_BACKSPACE:
                case SDL_Scancode.SDL_SCANCODE_KP_BACKSPACE:
                    if (Text.Length > 0) Text = Text.Remove(Text.Length - 1, 1);
                    return;
                // do nothing
                case SDL_Scancode.SDL_SCANCODE_LEFT:
                case SDL_Scancode.SDL_SCANCODE_RIGHT:
                case SDL_Scancode.SDL_SCANCODE_UP:
                case SDL_Scancode.SDL_SCANCODE_DOWN:
                case SDL_Scancode.SDL_SCANCODE_LALT:
                case SDL_Scancode.SDL_SCANCODE_RALT:
                // already handled these three
                case SDL_Scancode.SDL_SCANCODE_LSHIFT:
                case SDL_Scancode.SDL_SCANCODE_RSHIFT:
                case SDL_Scancode.SDL_SCANCODE_CAPSLOCK:
                // some more:
                case SDL_Scancode.SDL_SCANCODE_LGUI:
                case SDL_Scancode.SDL_SCANCODE_RGUI:
                case SDL_Scancode.SDL_SCANCODE_LCTRL:
                case SDL_Scancode.SDL_SCANCODE_RCTRL:
                    return;
                case SDL_Scancode.SDL_SCANCODE_RETURN:
                case SDL_Scancode.SDL_SCANCODE_RETURN2:
                case SDL_Scancode.SDL_SCANCODE_KP_ENTER:
                    if (AllowMultiline) Text = $"{Text}\n";
                    return;
                default:
                    string keyStr = key.KeySym.ToString();

                    // check if lowercase
                    if (lowercase) keyStr = keyStr.ToLower();
                    Text = $"{Text}{keyStr}";
                    return;
            }


        }

        /// <summary>
        /// Renders this TextBox.
        /// </summary>
        /// <param name="Lightning.Renderer">The window to render this <see cref="TextBox"/> to.</param>
        public void Render()
        {
            Text ??= "";

            if (Font == null)
            {
                NCLogging.Log($"Cannot draw a TextBox when the value of its Font property is null!", ConsoleColor.Yellow);
                return;
            }

#pragma warning disable CS8602
            Rectangle.Color = CurBackgroundColor;

            TextManager.DrawText(Text, Font, Position, ForegroundColor);

            // slight hack
            if (CurBackgroundColor == default) CurBackgroundColor = BackgroundColor;

            if (!HideCursor)
            {
                Vector2 fontSize = FontManager.GetLargestTextSize(Font, Text);
                Vector2 cursorPosition = new(Position.X + fontSize.X, Position.Y);

                // actually blink it
                if (NumberOfFramesUntilNextBlink == 0)
                {
                    IsActive = !IsActive;
                    if (Lightning.Renderer.DeltaTime > 0) NumberOfFramesUntilNextBlink = Convert.ToInt32((CursorBlinkLength - 1) + (CursorBlinkFrequency / Lightning.Renderer.DeltaTime));
                }

                Cursor.Start = cursorPosition;
                Cursor.End = new(cursorPosition.X, cursorPosition.Y + Size.Y);
#pragma warning restore CS8602

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
