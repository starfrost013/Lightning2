namespace LightningGL
{
    /// <summary>
    /// TextBlock
    /// 
    /// Defines a block of text.
    /// </summary>
    public class TextBlock : Renderable
    {
        public override Vector2 Size
        {
            get
            {
                // return (0,0) if there is no text...duh
                if (Font == null
                    || Text == null) return default;

                // it's too slow to use getlargesttextsize hundreds of times per frame
                // so let's just approximate it...
                // if we don't override it doesn't cull right
                return new(Text.Length * 4); 
            }
        }
        /// <summary>
        /// The text to draw.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// The font of the text to draw.
        /// </summary>
        public string? Font { get; set; }

        /// <summary>
        /// The foreground colour of the text to draw.
        /// </summary>
        public Color ForegroundColor { get; set; }

        /// <summary>
        /// The background colour of the text to draw.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// The size of the text's outline.
        /// </summary>
        public int OutlineSize { get; set; }

        /// <summary>
        /// Determines if this text will be localised.
        /// </summary>
        public bool Localise { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// 
        /// <para>In this class: stupid kludge that deserves to die</para>
        /// </summary>
        public override int ZIndex
        {
            get
            {
                if (ZIndexNotRelativeToParent
                    || Parent == null)
                {
                    return base.ZIndex;
                }
                else
                {
                    return base.ZIndex + RelativeZIndex;
                }
            }
            set
            {
                if (ZIndexNotRelativeToParent
                    || Parent == null)
                {
                    base.ZIndex = value;
                }
                else
                {
                    base.ZIndex = Parent.ZIndex + RelativeZIndex;
                }
            }
        }


        /// <summary>
        /// The relative zindex value for this text.
        /// 
        /// Ignored if <see cref="ZIndexNotRelativeToParent"/> is set to true.
        /// </summary>
        public int RelativeZIndex { get; set; }

        /// <summary>
        /// Determines if, if this text has a parent,
        /// if its z-index will be interpreted relative to it using the <see cref="RelativeZIndex"/> property or not.
        /// </summary>
        public bool ZIndexNotRelativeToParent { get; set; }

        /// <summary>
        /// Default value for <see cref="RelativeZIndex"/>..
        /// </summary>
        private const int DEFAULT_RELATIVE_Z_INDEX = 1;

        /// <summary>
        /// Line used for rendering underline.
        /// </summary>
        private Rectangle? UnderlineLine { get; set; }

        /// <summary>
        /// Line used for rendering strikeout.
        /// </summary>
        private Rectangle? StrikeoutLine { get; set; }

        public TextBlock(string name, string text, string font) : base(name)
        {
            Text = text;
            Font = font;

            // default values for localise are true so set localisation
            Localise = true;
        }

        public TextBlock(string name, string text, string font, Vector2 position, Color foregroundColor) : base(name)
        {
            Text = text;
            Font = font;
            Position = position;
            ForegroundColor = foregroundColor;

            // default values for localise are true so set localisation
            Localise = true;
        }

        public TextBlock(string name, string text, string font, Vector2 position, Color foregroundColor, Color backgroundColor = default,
            int outlineSize = 0, bool snapToScreen = false, bool localise = true, 
            int relativeZIndex = DEFAULT_RELATIVE_Z_INDEX) : base(name)
        {
            Text = text;
            Font = font;
            Position = position;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            OutlineSize = outlineSize;
            SnapToScreen = snapToScreen;
            Localise = localise;
            RelativeZIndex = relativeZIndex;
        }

        public override void Draw()
        {
            if (string.IsNullOrWhiteSpace(Font))
            {
                Logger.LogError($"Tried to draw a text with no font! The text will not be drawn.", 256, LoggerSeverity.Error, null, true);
                return;
            }

            // variable to store localised text
            string? text = Text;

            if (string.IsNullOrWhiteSpace(text)) return;

            // Localise the string using Localisation Manager.
            if (Localise) text = LocalisationManager.ProcessString(text);

            // Get the font and throw an error if it's invalid
            // we only use this for minimum character spacing
            Font? font = (Font?)Lightning.Renderer.GetRenderableByName(Font);

            if (font == null
                || font.Handle == default)
            {
                Logger.LogError($"Attempted to acquire invalid font with name {Font}", 39, LoggerSeverity.FatalError);
                return;
            }

            Vector2 currentPosition = RenderPosition;

            // split text by lines
            string[] linesArray = text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (string line in linesArray)
            {
                // reset line length
                int lineLength = 0;
                // offset from first y position for drawing underline/strikethrough line. start at 1 pixel below lin
                int maxVerticalLineSize = 1;

                foreach (char character in line)
                {
                    Glyph? glyph = GlyphCache.QueryCache(Font, character, ForegroundColor);

                    if (glyph != null)
                    {
                        // mark it as used so it will not be purged
                        glyph.UsedThisFrame = true;

                        // maxverticallinesize
                        if (glyph.Size.Y > maxVerticalLineSize) maxVerticalLineSize = (int)glyph.Size.Y;

                        float oldY = Position.Y;

                        // just grab the texture and draw it again
                        // these should definitely be in the hierarchy...hmm...
                        glyph.Position = new(currentPosition.X + glyph.Offset.X,
                            currentPosition.Y - glyph.Offset.Y + font.FontSizePixels);
                       
                        // change line length
                        lineLength += (int)glyph.Advance.X;

                        currentPosition.X += glyph.Advance.X;

                        if (!glyph.IsEmpty) glyph.Draw();
                    }
                }

                // line is done
                // move down a line

                currentPosition += new Vector2(-lineLength, font.LineGap);

            }
        }
    }
}
