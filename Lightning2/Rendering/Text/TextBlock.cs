using System.Reflection.PortableExecutable;

namespace LightningGL
{
    /// <summary>
    /// TextBlock
    /// 
    /// Defines a block of text.
    /// </summary>
    public class TextBlock : Renderable
    {
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
        /// The style of the text to draw - see <see cref="FontStyle"/>
        /// </summary>
        public FontStyle Style { get; set; }

        /// <summary>
        /// The size of the text's outline.
        /// </summary>
        public int OutlineSize { get; set; }

        /// <summary>
        /// The smoothing type of this font - see <see cref="FontSmoothingType"/>,
        /// </summary>
        public FontSmoothingType SmoothingType { get; set; }

        /// <summary>
        /// Determines if this text will be localised.
        /// </summary>
        public bool Localise { get; set; }

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
        /// The text orientation (reading order) of this text.
        /// </summary>
        public Orientation Orientation { get; set; }

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
            FontStyle style = default, int outlineSize = 0, FontSmoothingType smoothingType = FontSmoothingType.Default, bool snapToScreen = false, bool localise = true) : base(name)
        {
            Text = text;
            Font = font;
            Position = position;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            Style = style;
            OutlineSize = outlineSize;
            SmoothingType = smoothingType;
            SnapToScreen = snapToScreen;
            Localise = localise; 
        }

        public override void Create()
        {
            RelativeZIndex = DEFAULT_RELATIVE_Z_INDEX; 
        }

        public override void Draw()
        {
            if (string.IsNullOrWhiteSpace(Font))
            {
                NCError.ShowErrorBox($"Tried to draw a text with no font!", 256, NCErrorSeverity.FatalError);
                return;
            }

            // variable to store localised text
            string? text = Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                NCError.ShowErrorBox($"Tried to draw text that is null, empty, or pure whitespace. Ignoring", 274, NCErrorSeverity.Warning, null, true);
                return; 
            }

            if (Font == null)
            {
                NCError.ShowErrorBox($"Tried to draw a text with no font!", 256, NCErrorSeverity.FatalError);
                return;
            }

            // Localise the string using Localisation Manager.
            if (Localise) text = LocalisationManager.ProcessString(text);

            // Get the font and throw an error if it's invalid
            // we only use this for minimum character spacing
            Font? font = (Font?)Lightning.Renderer.GetRenderableByName(Font);

            if (font == null
                || font.Handle == default)
            {
                NCError.ShowErrorBox($"Attempted to acquire invalid font with name {Font}", 39, NCErrorSeverity.FatalError);
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

                // first character position
                Vector2 firstCharPosition = new();

                foreach (char character in line)
                {
                    Glyph? glyph = GlyphCache.QueryCache(Font, character, ForegroundColor, Style, SmoothingType);

                    if (glyph != null)
                    {
                        glyph.UsedThisFrame = true;

                        // this just prettifies the text a bit
                        // some font characters are a bit too small so freetype's advance isn't long enough to give them a proper amount of spacing, so they look weird
                        // this is only a problem in the X direction
                        // we just fix that here
                        double minimumSpacing = (font.FontSize * GlobalSettings.GraphicsMinimumCharacterSpacing);

                        int pushRight = (int)(minimumSpacing - glyph.Advance.X) + 1;

                        // ignore negative values
                        if (pushRight < 0) pushRight = 0;

                        // maxverticallinesize
                        if (glyph.Size.Y > maxVerticalLineSize) maxVerticalLineSize = (int)glyph.Size.Y;

                        // change line length
                        lineLength += (int)(glyph.Advance.X);
                        
                        // syntax note: new() does not work here!!! you must provide a type name
                        switch (Orientation)
                        {
                            case Orientation.LeftToRight:
                                currentPosition += new Vector2(glyph.Advance.X, 0);
                                // just grab the texture and draw it again
                                // these should definitely be in the hierarchy...hmm...
                                glyph.Position = new(currentPosition.X + glyph.Offset.X + pushRight,
                                    currentPosition.Y - glyph.Offset.Y);
                                break;
                            case Orientation.RightToLeft:
                                currentPosition += new Vector2(-glyph.Advance.X, 0);
                                glyph.Position = new(currentPosition.X - glyph.Offset.X - pushRight,
                                    currentPosition.Y - glyph.Offset.Y);
                                break;
                            // use X here so the horizontal spacing is used vertically for these reading orders
                            case Orientation.TopToBottom:
                                currentPosition += new Vector2(0, glyph.Advance.X);
                                glyph.Position = new(currentPosition.X + glyph.Offset.X,
                                    currentPosition.Y - glyph.Offset.Y + pushRight);
                                break;
                            case Orientation.BottomToTop:
                                currentPosition += new Vector2(0, -glyph.Advance.X);
                                glyph.Position = new(currentPosition.X + glyph.Offset.X,
                                    currentPosition.Y - glyph.Offset.Y - pushRight);
                                break;
                        }

                        //if the line length is zero set the first character position
                        //*****STUPID HACK*****
                        if (lineLength == glyph.Advance.X) firstCharPosition = currentPosition; 

                        // the glyph is empty so just push forward by the size of the glyph
                        // (tabs, spaces, etc)
                        if (!glyph.IsEmpty) glyph.Draw();
                    }
                }

                int lineSpacing = (int)(font.FontSize * GlobalSettings.GraphicsLineSpacing);

                // line is done, draw underline or strikeout
                if (Style.HasFlag(FontStyle.Underline))
                {
                    // create the underline line
                    UnderlineLine ??= Lightning.Renderer.AddRenderable(new Rectangle("TextUnderlineLine",
                        default, default, ForegroundColor, true), this);

                    if (UnderlineLine != null)
                    {
                        UnderlineLine.Position = new(firstCharPosition.X, firstCharPosition.Y + maxVerticalLineSize);
                        UnderlineLine.Size = new(lineLength, GlobalSettings.GraphicsUnderlineThickness);
                        UnderlineLine.Color = ForegroundColor; // update colour
                    }
                }

                if (Style.HasFlag(FontStyle.Strikeout))
                {
                    // create the strikeout line
                    StrikeoutLine ??= Lightning.Renderer.AddRenderable(new Rectangle("TextStrikeoutLine",
                        default, default, ForegroundColor, true), this);

                    if (StrikeoutLine != null)
                    {
                        StrikeoutLine.Position = new(firstCharPosition.X, firstCharPosition.Y + (maxVerticalLineSize / 2) + 1);
                        StrikeoutLine.Size = new(lineLength, GlobalSettings.GraphicsStrikeoutThickness);
                        StrikeoutLine.Color = ForegroundColor; // update colour
                    }
                }

                // line is done
                // move down a line
                switch (Orientation)
                {
                    case Orientation.LeftToRight:
                    case Orientation.RightToLeft:
                        currentPosition += new Vector2(-lineLength, lineSpacing);
                        break;
                    case Orientation.TopToBottom:
                        currentPosition += new Vector2(lineSpacing, -lineLength);
                        break;
                    case Orientation.BottomToTop:
                        currentPosition += new Vector2(lineSpacing, lineLength);
                        break;
                }
            }
        }
    }
}
