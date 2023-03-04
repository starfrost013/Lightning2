namespace LightningGL
{
    /// <summary>
    /// Font
    /// 
    /// Defines a font (FreeType2).
    /// </summary>
    public class Font : Renderable
    {
        /// <summary>
        /// The size of this font.
        /// </summary>
        public int FontSize { get; set; }

        public int FontSizePixels
        {
            get
            {
                // SDL is allowed outside of SDL Renderer
                float ddpi;

                if (SDL_GetDisplayDPI(0, out ddpi, out _, out _) != 0)
                {
                    Logger.LogError($"Failed to get display DPI!: {SDL_GetError()}", 333, LoggerSeverity.FatalError);
                }

                return (int)(FontSize * (ddpi / 72));
            }

        }
        /// <summary>
        /// Private: Pointer to the unmanaged TTF_Font containing this font.
        /// </summary>
        public FreeTypeFaceFacade? Handle { get; private set; }

        /// <summary>
        /// The name of this font on the system.
        /// </summary>
        public string FontName { get; internal set; }

        /// <summary>
        /// The path of this Font.
        /// NULL if this font was loaded from the system font directory.
        /// </summary>
        public string? Path { get; internal set; }

        /// <summary>
        /// The index of this font.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// The line gap used for this font.
        /// </summary>
        public int LineGap { get; internal set; }

        public Font(string name, int size, string friendlyName, string? path = null, int index = 0) : base(friendlyName)
        {
            FontName = name;
            Name = friendlyName;
            FontSize = size;
            Index = index;

            if (path == null) // default to system load path 
            {
                Path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Fonts)}\{name}";
            }
            else
            {
                Path = $"{path}";
            }
        }

        /// <summary>
        /// Internal: Loads this font.
        /// </summary>
        public unsafe override void Create()
        {
            Debug.Assert(Lightning.Renderer.FreeTypeLibrary != null);

            if (!File.Exists(Path))
            {
                Logger.LogError($"Error loading font: Attempted to load nonexistent font at {Path}", 34, LoggerSeverity.Error);
                return;
            }

            bool isValidFontType = false;

            for (int fontType = 0; fontType < (int)FontFormat.MAX_FONT; fontType++)
            {
                string extension = $".{(FontFormat)fontType}";

                // if we have a valid extension judge the font as valid
                if (Path.Contains(extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    isValidFontType = true;
                }
            }
           
            if (!isValidFontType)
            {
                Logger.LogError($"Error loading font: Attempted to load an invalid font format. The supported font formats are:\n\n" +
                    $"- TrueType and TrueType collections (.ttf/ttc)\n" +
                    $"- OpenType and OpenType collections (.otf/otc)\n" +
                    $"- OpenType CFF\n" +
                    $"- WOFF\n" +
                    $"- Adobe Type 1\n" +
                    $"- CID\n" +
                    $"- SFNT\n" +
                    $"- X11 PCF\n" +
                    $"- Windows legacy FNT\n" +
                    $"- BDF\n" +
                    $"- PFR\n" +
                    $"- PostScript Type 42 (limited support)" +
                    $"- ", 36, LoggerSeverity.Error);
                return;
            }

            if (FontSize < 1) Logger.LogError($"Error loading font: Invalid font size {Size}, must be at least 1!", 37, 
                 LoggerSeverity.Error);

            FT_Error error = FT_New_Face(Lightning.Renderer.FreeTypeLibrary.Native, Path, Index, out var newHandle);

            if (error != FT_Error.FT_Err_Ok)
            {
                Logger.LogError($"A fatal FreeType error occurred loading the font: {error}", 236, 
                    LoggerSeverity.Error);
                return;
            }

            Handle = new FreeTypeFaceFacade(Lightning.Renderer.FreeTypeLibrary, newHandle);

            //todo: kerning 

            try
            {
                Handle.SelectCharSize(FontSize, 0, 0);
            }
            catch
            {
                Logger.LogError($"A fatal FreeType error occurred setting the font size: {error}", 236, LoggerSeverity.Error);
                return;
            }

            LineGap = (int)(Handle.FaceRec->size->metrics.height >> 6);

            
            Logger.Log($"Loaded font {Name}, size {FontSize} at {Path}");
            
            Loaded = true;
        }

        public override void Destroy()
        {
            Handle?.Unload();
            Handle = null;
            Loaded = false; 
            Logger.Log($"Unloaded font {Name}, size {FontSize}");
        }
    }
}
