using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.IO;

namespace LightningGL
{
    public class Font
    {
        public string FriendlyName { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public FontSmoothingType SmoothingType { get; set; }

        public IntPtr Handle { get; private set; }

        public static Font Load(string name, int size, string path = null, string friendlyName = null, int index = -1) // probably static
        {
            Font temp_font = new Font();

            temp_font.Name = name;

            if (friendlyName == null)
            {
                temp_font.FriendlyName = name;
            }
            else
            {
                temp_font.FriendlyName = friendlyName;
            }

            temp_font.Size = size;

            string temp_path = null;

            if (path == null) // default to system load path 
            {
                temp_path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Fonts)}\{name}.ttf";
            }
            else
            {
                temp_path = $"{path}.ttf";
            }

            if (!File.Exists(temp_path)) _ = new NCException($"Error loading font: Attempted to load nonexistent font at {temp_path}", 35, "Font.Load()", NCExceptionSeverity.Error);

            if (!temp_path.Contains(".ttf")) _ = new NCException($"Error loading font: Only TTF fonts are supported!", 36, "Font.Load()", NCExceptionSeverity.Error);

            if (size <= 0) _ = new NCException($"Error loading font: Invalid font size {size}, must be at least 1!", 37, "Font.Load()", NCExceptionSeverity.Error);

            if (index == -1)
            {
                temp_font.Handle = SDL_ttf.TTF_OpenFont(temp_path, size);
            }
            else
            {
                temp_font.Handle = SDL_ttf.TTF_OpenFontIndex(temp_path, size, index);
            }

            if (temp_font.Handle == IntPtr.Zero) _ = new NCException($"Error loading font at {path}: {SDL_ttf.TTF_GetError()}", 38, "Font.Load()", NCExceptionSeverity.Error);

            NCLogging.Log($"Loaded font {temp_font.Name}, size {temp_font.Size} at {temp_path}");
            return temp_font;
        }

        public void Unload()
        {
            SDL_ttf.TTF_CloseFont(Handle);
            NCLogging.Log($"Unloaded font {Name}, size {Size}");
        }

    }
}
