using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Font and text example
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

// if you do not provide a path it will load from the system font directory
// TrueType fonts only, you don't need to load bold/italic/etc fonts - drawtext has parameters for this
FontManager.LoadFont("Arial", 24, "Arial.24pt");
FontManager.LoadFont("comic", 24, "ComicSans.24pt"); // comic.ttf is the file name for comic sans ms in windows
FontManager.LoadFont("segoeui", 24, "SegoeUI.24pt"); //v segoeui.ttf is the filename for segoe ui
FontManager.LoadFont("consola", 18, "Consolas.18pt"); // same for consolas...
FontManager.LoadFont("consola", 36, "Consolas.36pt");

while (window.Run())
{
    // backgrounds etc supported
    PrimitiveRenderer.DrawText(window, "Font test example (primitive debug font)", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    FontManager.DrawText(window, "Arial 24pt", "Arial.24pt", new Vector2(100, 150), Color.White, Color.Red, NuCore.SDL2.SDL_ttf.TTF_FontStyle.Italic); // Arial.24pt, italic
    FontManager.DrawText(window, "Comic Sans 24pt", "ComicSans.24pt", new Vector2(100, 200), Color.Blue);
    FontManager.DrawText(window, "Segoe UI 24pt", "SegoeUI.24pt", new Vector2(100, 250), Color.White, Color.Yellow, NuCore.SDL2.SDL_ttf.TTF_FontStyle.Underline | NuCore.SDL2.SDL_ttf.TTF_FontStyle.Bold);
    FontManager.DrawText(window, "Consolas 18pt", "Consolas.18pt", new Vector2(100, 300), Color.White, Color.Red);
    FontManager.DrawText(window, "Consolas 36pt\nMultiline!", "Consolas.36pt", new Vector2(100, 350), Color.White, Color.Red);

    window.Render();
}