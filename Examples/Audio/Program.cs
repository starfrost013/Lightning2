using LightningGL; // use lightninggl
using static NuCore.SDL2.SDL; // import for sdl
using System.Drawing; // for color
using System.Numerics;

//Lightning Input Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

while (window.Run())
{
    if (window.EventWaiting)
    {
        switch (window.LastEvent.type)
        {
            case SDL_EventType.SDL_MOUSEBUTTONDOWN:

                MouseButton button = (MouseButton)window.LastEvent.button;

                switch (button.Button)
                {
                    case SDL_MouseButton.Left:
                        break;
                }

                break;
        }
    }

    PrimitiveRenderer.DrawText(window, "Audio example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}