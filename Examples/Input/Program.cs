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

string youPressedString = "Last keypress: ";
string youClickedString = "Last mouse click: ";
string lastMousePosString = "Last mouse motion: ";
while (window.Run())
{
    if (window.EventWaiting)
    {
        if (window.LastEvent.type == SDL_EventType.SDL_KEYDOWN)
        {
            Key key = (Key)window.LastEvent.key;
            youPressedString = $"Last keypress: {key}, modifiers: {key.Modifiers}, repeated: {key.Repeated}";
        }
        else if (window.LastEvent.type == SDL_EventType.SDL_MOUSEBUTTONDOWN)
        {
            MouseButton button = (MouseButton)window.LastEvent.button;
            youClickedString = $"Last mouse click: {button.Button}, position: {button.Position}";
        }
        else if (window.LastEvent.type == SDL_EventType.SDL_MOUSEMOTION)
        {
            MouseButton button = (MouseButton)window.LastEvent.motion;
            lastMousePosString = $"Last mouse motion: {button.Button}, position: {button.Position}, velocity: {button.Velocity}";
        }
    }

    PrimitiveRenderer.DrawText(window, "Input example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(window, youPressedString, new Vector2(100, 120), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(window, youClickedString, new Vector2(100, 140), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(window, lastMousePosString, new Vector2(100, 160), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}