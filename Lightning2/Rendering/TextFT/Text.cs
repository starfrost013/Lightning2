﻿using System.Reflection.PortableExecutable;

namespace LightningGL
{
    /// <summary>
    /// FTTextAssetManager
    /// 
    /// Text asset manager (FreeType Version)
    /// </summary>
    public class Text : Renderable
    {
        /// <summary>
        /// The text to draw.
        /// </summary>
        public string? Content { get; set; }

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

        public Text(string name) : base(name) { }

        public Text(string name, string text, string font, Vector2 position, Color foregroundColor, Color backgroundColor = default,
            FontStyle style = default, int outlineSize = 0, FontSmoothingType smoothingType = FontSmoothingType.Default, bool snapToScreen = false, bool localise = true) : base(name)
        {
            Content = text;
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
                NCError.ShowErrorBox($"Tried to draw a text with no font!", 256,
                    "Text::Draw - Font property is null!", NCErrorSeverity.FatalError);
                return;
            }

            // variable to store localised text
            string? text = Content;

            // just ignore it if there is still text to draw
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (Font == null)
                {
                    NCError.ShowErrorBox($"Tried to draw a text with no font!", 256,
                        "Text::Draw - Font property is null!", NCErrorSeverity.FatalError);
                    return;
                }

                // Localise the string using Localisation Manager.
                if (Localise) text = LocalisationManager.ProcessString(text);

                // Get the font and throw an error if it's invalid
                Font? curFont = FontManager.GetFont(Font);

                if (curFont == null
                    || curFont.Handle == default)
                {
                    NCError.ShowErrorBox($"Attempted to acquire invalid font with name {Font}", 39,
                        "Text::Draw font parameter is not a loaded font!", NCErrorSeverity.FatalError);
                    return;
                }

                Vector2 currentPosition = RenderPosition;

                // cache everything
                foreach (char character in text)
                {
                    Glyph? glyph = GlyphCache.QueryCache(Font, character, ForegroundColor, SmoothingType);

                    if (glyph != null)
                    {
                        glyph.UsedThisFrame = true;

                        // syntax note: new() does not work here!!! you must provide a type name
                        switch (Orientation)
                        {
                            case Orientation.LeftToRight:
                                currentPosition += new Vector2(glyph.Advance.X, 0);
                                break;
                            case Orientation.RightToLeft:
                                currentPosition += new Vector2(-glyph.Advance.X, 0);
                                break;
                            case Orientation.TopToBottom:
                                currentPosition += new Vector2(0, glyph.Advance.Y);
                                break;
                            case Orientation.BottomToTop:
                                currentPosition += new Vector2(0, -glyph.Advance.Y);
                                break;
                        }


                        // the glyph is empty so just push forward by the size of the glyph
                        // (tabs, spaces, etc)
                        if (!glyph.IsEmpty)
                        {
                            // just grab the texture and draw it again
                            // these should definitely be in the hierarchy...hmm...
                            glyph.Position = new(currentPosition.X + glyph.Offset.X,
                                currentPosition.Y - glyph.Offset.Y);
                            glyph.Draw();
                        }
                    }
                }
            }
            else
            {
                NCError.ShowErrorBox($"Tried to draw text that is null, empty, or pure whitespace. Ignoring", 274, 
                    "string.IsNullOrWhiteSpace(Content) returned TRUE in Text::Draw", NCErrorSeverity.Warning, null, true);
            }
            
        }
    }
}
