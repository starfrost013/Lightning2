using NuCore.SDL2; 
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    public class Font
    {
        public string Name { get; set; }

        public int Size { get; set; }

        public FontSmoothingType SmoothingType { get; set; }

        public IntPtr Handle { get; private set; }

        public static Font Load(string FriendlyName, int Size, string Path = null, int Index = -1) // probably static
        {
            Font temp_font = new Font();

            temp_font.Name = FriendlyName;
            temp_font.Size = Size;

            string temp_path = null;

            if (Path == null) // default to system load path 
            {
                temp_path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Fonts)}\{FriendlyName}.ttf";
            }
            else
            {
                temp_path = $"{Path}.ttf";
            }

            if (!File.Exists(temp_path)) throw new NCException($"Error loading font: Attempted to load nonexistent font at {temp_path}", 35, "Font.Load()", NCExceptionSeverity.Error);
            
            if (!temp_path.Contains(".ttf")) throw new NCException($"Error loading font: Only TTF fonts are supported!", 36, "Font.Load()", NCExceptionSeverity.Error);

            if (Size <= 0) throw new NCException($"Error loading font: Invalid font size {Size}, must be at least 1!", 37, "Font.Load()", NCExceptionSeverity.Error);

            if (Index == -1)
            {
                temp_font.Handle = SDL_ttf.TTF_OpenFont(temp_path, Size);
            }
            else
            {
                temp_font.Handle = SDL_ttf.TTF_OpenFontIndex(temp_path, Size, Index);
            }

            if (temp_font.Handle == IntPtr.Zero) throw new NCException($"Error loading font at {Path}: {SDL_ttf.TTF_GetError()}", 38, "Font.Load()", NCExceptionSeverity.Error);

            NCLogging.Log($"Loaded font {temp_font.Name}, size {temp_font.Size} at {temp_path}");
            return temp_font; 
        }
    }
}
