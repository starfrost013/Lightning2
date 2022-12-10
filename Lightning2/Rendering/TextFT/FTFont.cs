namespace LightningGL
{
    /// <summary>
    /// Font
    /// 
    /// Defines a font (FreeType2).
    /// </summary>
    public class FTFont : Renderable
    {
        /// <summary>
        /// The size of this font.
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// Private: Pointer to the unmanaged TTF_Font containing this font.
        /// </summary>
        public nint Handle { get; private set; }

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

        public FTFont(string name, int size, string friendlyName, string? path = null, int index = 0) : base(friendlyName)
        {
            FontName = name;
            Name = friendlyName;
            FontSize = size;
            Index = index;

            if (path == null) // default to system load path 
            {
                Path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Fonts)}\{name}.ttf";
            }
            else
            {
                Path = $"{path}.ttf";
            }
        }

        /// <summary>
        /// Internal: Loads this font.
        /// </summary>
        internal override void Create()
        {
            if (!File.Exists(Path))
            {
                NCError.ShowErrorBox($"Error loading font: Attempted to load nonexistent font at {Path}", 34, "Font::Path does not exist", NCErrorSeverity.Error);
                return;
            }

            if (!Path.Contains(".ttf", StringComparison.InvariantCultureIgnoreCase))
            {
                NCError.ShowErrorBox($"Error loading font: Only TTF fonts are supported!", 36, "Font::Path is not a TrueType font", NCErrorSeverity.Error);
                return;
            }

            if (FontSize < 1) NCError.ShowErrorBox($"Error loading font: Invalid font size {Size}, must be at least 1!", 37, 
                "size parameter to Font::Load is not a valid font size!", NCErrorSeverity.Error);

            
            Handle = TTF_OpenFontIndex(Path, FontSize, Index);

            if (Handle == nint.Zero) NCError.ShowErrorBox($"Error loading font at {Path}: {TTF_GetError()}", 38, 
                "An SDL error occurred during font loading from Font::Load!", NCErrorSeverity.Error);

            NCLogging.Log($"Loaded font {Name}, size {FontSize} at {Path}");

            Loaded = true;
        }

        /// <summary>
        /// Unloads this font. Should ONLY be called if this font is about to be removed
        /// </summary>
        internal void Unload()
        {
            TTF_CloseFont(Handle);
            Handle = nint.Zero;
            NCLogging.Log($"Unloaded font {Name}, size {FontSize}");
        }
    }
}
