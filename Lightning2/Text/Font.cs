namespace LightningGL
{
    /// <summary>
    /// Font
    /// 
    /// Defines a font.
    /// </summary>
    public class Font
    {
        /// <summary>
        /// A name used to describe this font in rendering operations.
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// The name of this font.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The size of this font.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Private: Pointer to the unmanaged TTF_Font containing this font.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// The path of this Font.
        /// NULL if this font was loaded from the system font directory.
        /// </summary>
        public string Path { get; internal set; }

        /// <summary>
        /// The index of this font.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Internal: Loads this font.
        /// </summary>
        /// <param name="name">The name of the font to load.</param>
        /// <param name="size">The size of the font to load.</param>
        /// <param name="friendlyName">The friendly name of the font to load.</param>
        /// <param name="path">The path to this font. If it is null, it will be loaded from the system font directory.</param>
        /// <param name="index">Index of the font in the font file to load. Will default to 0.</param>
        internal static Font Load(string name, int size, string friendlyName, string path = null, int index = 0)
        {
            Font font = new Font();

            font.Name = name;
            font.FriendlyName = friendlyName;
            font.Size = size;
            font.Index = index;

            if (path == null) // default to system load path 
            {
                path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Fonts)}\{name}.ttf";
            }
            else
            {
                path = $"{path}.ttf";
            }

            if (!File.Exists(path)) _ = new NCException($"Error loading font: Attempted to load nonexistent font at {path}", 34, "Font.:Path does not exist", NCExceptionSeverity.Error);

            if (!path.Contains(".ttf")) _ = new NCException($"Error loading font: Only TTF fonts are supported!", 36, "Font::Path is not a TrueType font", NCExceptionSeverity.Error);

            // set the font after it's been processed
            font.Path = path;

            if (size < 1) _ = new NCException($"Error loading font: Invalid font size {size}, must be at least 1!", 37, "size parameter to Font::Load is not a valid font size!", NCExceptionSeverity.Error);

            font.Handle = TTF_OpenFontIndex(path, size, index);

            if (font.Handle == IntPtr.Zero) _ = new NCException($"Error loading font at {path}: {SDL_ttf.TTF_GetError()}", 38, "An SDL error occurred during font loading from Font::Load!", NCExceptionSeverity.Error);

            NCLogging.Log($"Loaded font {font.Name}, size {font.Size} at {path}");
            return font;
        }

        /// <summary>
        /// Unloads this font. Should ONLY be called if this font is about to be removed
        /// </summary>
        internal void Unload()
        {
            TTF_CloseFont(Handle);
            Handle = IntPtr.Zero;
            NCLogging.Log($"Unloaded font {FriendlyName}, size {Size}");
        }
    }
}
